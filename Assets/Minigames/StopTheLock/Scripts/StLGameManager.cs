
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/





using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AppAdvisory.StopTheLock
{
	public class StLGameManager : MonoBehaviourHelper
	{
		[Header("Internal References")]
		[SerializeField]
		Text levelCenterScreen = null;
		[SerializeField] Text levelTopScreen = null;
		[SerializeField] RectTransform lockRect = null;
		[SerializeField] Transform theGame = null;

		[Header("Continue Window")]
		[SerializeField]
		GameObject ContinueWindow = null;
		[SerializeField] Text ContinueText = null;

		[Header("Game Over Window")]
		[SerializeField]
		GameObject GameOverWindow = null;
		[SerializeField] Text GameOverText = null;

		[Header("Reward Window")]
		[SerializeField]
		GameObject RewardWindow = null;
		[SerializeField] Text RewardText = null;
		[SerializeField] Image RewardImage = null;
		[SerializeField] Text PerkNameText = null;

		[Header("Trophy Window")]
		[SerializeField]
		GameObject TrophyWindow = null;
		[SerializeField] Image TrophyRewardImage = null;

		[Header("Error Window")]
		[SerializeField]
		StLErrorWindow ErrorWindow = null;

		[Header("Misc")]
		public bool gameIsStarted = false;


		CanvasScaler canvasScaler;

		public bool isGameOver { get; private set; }

		public bool isSuccess
		{
			get
			{
				bool success = numTotalOfMove <= 0;
				return success;
			}
		}

		RectTransform _rectTransform;
		public RectTransform rectTransform
		{
			get
			{
				if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
				return _rectTransform;
			}
		}

		StLGameState State;
		PerksData PerksData;
		TrophiesData TrophiesData;

		void Awake()
		{
			PerksData = TheRunGameManager.Instance.PerksData;
			TrophiesData = TheRunGameManager.Instance.TrophiesData;
		}

		void Start()
		{
			State = new StLGameState();

			ResetGame();
		}

		public void ResetGame()
		{
			double ElapsedTime = (DateTime.Now - TheRunGameManager.Instance.GameData.Data.Profile.StopTheLockData.LastTimePlayed).TotalMinutes;
			if (ElapsedTime >= 60)
			{
				TheRunGameManager.Instance.GameData.Data.Profile.StopTheLockData.MatchesPlayedWithinLimit = 0;
				TheRunGameManager.Instance.GameData.Data.Profile.StopTheLockData.LastTimePlayed = DateTime.Now;
			}
			else
			{
				if (TheRunGameManager.Instance.GameData.Data.Profile.StopTheLockData.MatchesPlayedWithinLimit >= 5)
				{
					ErrorWindow.Show(ElapsedTime);
					return;
				}
			}

			TheRunGameManager.Instance.GameData.Data.Profile.StopTheLockData.MatchesPlayedWithinLimit++;
			TheRunGameManager.Instance.GameData.Save();

			StartLevel();
			gameIsStarted = true;

			//ShowContinueWindow(false);
			if (State.Lives <= 0)
			{
				ShowGameOverWindow(true);
				isGameOver = true;
			}
		}

		void StartLevel()
		{
			isGameOver = false;

			levelCenterScreen.text = GetCurrentLevel().ToString();

			levelTopScreen.text = "LEVEL: " + GetCurrentLevel().ToString();

			numTotalOfMove = GetCurrentLevel();

			gameIsStarted = false;

			lockRect.eulerAngles = Vector3.zero;

			player.transform.eulerAngles = Vector3.zero;

			dotPosition.DoPosition();
		}

		IEnumerator StartNewLevel()
		{
			StartLevel();

			StartCoroutine(screenMove.Move(theGame.GetComponent<RectTransform>(), true));

			while (screenMove.isMoving)
			{
				yield return 0;
			}

			gameIsStarted = true;
		}

		private int numTotalOfMove = 0;

		public void MoveDone()
		{
			numTotalOfMove--;

			levelCenterScreen.text = numTotalOfMove.ToString();

			bool success = numTotalOfMove <= 0;

			if (success)
				LevelCleared();
			else
				soundManager.PlayTouch();
		}

		#region Game Over

		public void GameOver()
		{
			soundManager.PlayFail();
			isGameOver = true;
			StopAllCoroutines();
			StartCoroutine(_GameOver());
		}

		IEnumerator _GameOver()
		{
			StartCoroutine(ScreenShake.Shake(theGame, 0.10f));

			while (ScreenShake.isShaking)
			{
				yield return 0;
			}

			State.Lives--;

			if (State.Lives > 0)
				ShowContinueWindow(true);
			else
				ShowGameOverWindow(true);
		}

		void ShowContinueWindow(bool Value)
		{
			if (Value)
			{
				if (State.Lives == 1)
					ContinueText.text = "You only have 1 life left.\n\nDo you want to continue?";
				else
					ContinueText.text = string.Format("You only have {0} lives left.\n\nDo you want to continue?", State.Lives);
			}

			ContinueWindow.SetActive(Value);
		}

		void ShowGameOverWindow(bool Value)
		{
			if (Value)
				GameOverText.text = "You don't have any lives left.\n\nDo you want to purchase more?";

			GameOverWindow.SetActive(Value);
		}

		public void Quit()
		{
			Loading.LoadScene("Main Menu");
		}

		public void Continue()
		{
			ShowContinueWindow(false);
			StartCoroutine(StartNewLevel());
		}

		public void Purchase()
		{

		}

		#endregion

		#region Rewards

		bool CheckIfDeservesReward()
		{
			bool Result = false;

			for (int n = 0; n < PerksData.Data.Length; n++)
			{
				if (PerksData.Data[n].StLRewardLevel == GetCurrentLevel() - 1)
				{
					ShowRewardsWindow(n);
					Result = true;
					break;
				}
			}

			return Result;
		}

		void ShowRewardsWindow(int PerkIndex)
		{
			RewardText.text = "You've got a new reward!";
			RewardImage.sprite = PerksData.Data[PerkIndex].ItemSprite;
			PerkNameText.text = PerksData.Data[PerkIndex].ItemName;

			TheRunGameManager.Instance.GameData.Data.Profile.PerksQuantities[PerkIndex]++;
			TheRunGameManager.Instance.GameData.Save();

			RewardWindow.SetActive(true);
		}

		void HideRewardsWindow()
		{
			RewardWindow.SetActive(false);

			if (GetCurrentLevel() - 1 == 40)
			{
				ShowTrophyWindow();
			}
			else
			{
				WaitingToCloseRewardWindow = false;
			}
		}

		public void CloseRewardsWindow()
		{
			HideRewardsWindow();
		}

		void ShowTrophyWindow()
		{
			TrophyRewardImage.sprite = TrophiesData.Data[5].ItemSprite;
			TheRunGameManager.Instance.GameData.Data.Profile.TrophiesQuantities[5]++;
			TheRunGameManager.Instance.GameData.Save();

			TrophyWindow.SetActive(true);
		}

		public void CloseTrophyWindow()
		{
			TrophyWindow.SetActive(false);
			WaitingToCloseRewardWindow = false;
		}

		#endregion

		#region Level Cleared

		bool WaitingToCloseRewardWindow;

		public void LevelCleared()
		{
			soundManager.PlaySuccess();

			int current = GetCurrentLevel();
			current++;
			SetCurrentLevel(current);

			SetMaxLevel(current);

			StartCoroutine(_LevelCleared());

			//		if (current%3 == 0)
			colorManager.ChangeColor();
		}

		IEnumerator _LevelCleared()
		{
			float t0 = 0f;
			float t1 = -30f;
			float timer = 0;
			float time = 0.5f;

			while (timer <= time)
			{
				timer += Time.deltaTime;

				float f = Mathf.Lerp(t0, t1, timer / time);

				Vector3 rot = Vector3.forward * f;

				lockRect.eulerAngles = rot;

				yield return 0;
			}

			yield return new WaitForSeconds(0.2f);

			if (CheckIfDeservesReward())
			{
				WaitingToCloseRewardWindow = true;

				while (WaitingToCloseRewardWindow)
				{
					yield return null;
				}
			}

			StartCoroutine(screenMove.Move(theGame.GetComponent<RectTransform>(), false));

			while (screenMove.isMoving)
			{
				yield return 0;
			}

			//		SceneManager.LoadScene (0);

			StartCoroutine(StartNewLevel());
		}

		#endregion

		public int GetMaxLevel()
		{
			return State.MaxLevel;
		}

		public void SetMaxLevel(int level)
		{
			if (State.MaxLevel < level)
			{
				State.MaxLevel = level;
			}
		}

		public int GetCurrentLevel()
		{
			State.CurrentLevel = Mathf.Max(State.CurrentLevel, 1);
			return State.CurrentLevel;
		}

		public void SetCurrentLevel(int level)
		{
			State.CurrentLevel = level;
		}
	}

	public class StLGameState
	{
		public int MaxLevel;
		public int CurrentLevel;
		public int GameOverCount;
		public int Lives;

		public StLGameState()
		{
			MaxLevel = 1;
			CurrentLevel = 1;
			GameOverCount = 0;
			Lives = 3;
		}
	}
}