using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using SgLib;
using UnityEngine.UI;
using System;

public enum GameState
{
	Prepare,
	Playing,
	Paused,
	PreGameOver,
	GameOver
}

public class EmojiGameManager : MonoBehaviour
{
	public static EmojiGameManager Instance { get; private set; }

	public static event System.Action<GameState, GameState> GameStateChanged = delegate { };

	public GameState GameState
	{
		get
		{
			return _gameState;
		}
		private set
		{
			if (value != _gameState)
			{
				GameState oldState = _gameState;
				_gameState = value;

				GameStateChanged(_gameState, oldState);
			}
		}
	}

	[SerializeField]
	private GameState _gameState = GameState.Prepare;

	public static int GameCount
	{
		get { return _gameCount; }
		private set { _gameCount = value; }
	}

	private static int _gameCount = 0;

	// List of public variable for gameplay tweaking
	[Header("Gameplay Config")]

	[Range(0f, 1f)]
	public float coinFrequency = 0.1f;
	[Range(0f, 1f)]
	public float obstacleFrequency = 0.1f;
	public float playerGravityScale;
	public float cloudVerticleOffset;
	public float cloudHorizontalOffset;
	public float cloudInititalPosY;
	public int maxRotatingAngle = 20;
	public float rotationDelta = 10;
	[HideInInspector]
	public float rotationDirection = 0;
	[HideInInspector]
	public float firstRotationDirection;
	// List of public variables referencing other objects
	[Header("Object References")]
	public PlayerController playerController;
	public Rigidbody2D playerRigidbody;
	public DeathPlane playerDeathPlane;
	public GameObject cloudPrefab;

	[Header("Reward Window")]
	[SerializeField]
	GameObject RewardWindow = null;
	[SerializeField] Text RewardText = null;
	[SerializeField] Image[] RewardImages = null;

	[Header("Trophy Window")]
	[SerializeField]
	GameObject TrophyWindow = null;
	[SerializeField] Image TrophyRewardImage = null;

	[Header("Ads Window")]
	[SerializeField]
	GameObject AdsWindow = null;
	[SerializeField]
	Text AdsMessageText = null;
	[SerializeField]
	Text AdsTimeText = null;

	void OnEnable()
	{
		PlayerController.PlayerDied += PlayerController_PlayerDied;
	}

	void OnDisable()
	{
		PlayerController.PlayerDied -= PlayerController_PlayerDied;
	}

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			DestroyImmediate(Instance.gameObject);
			Instance = this;
		}

		PerksData = TheRunGameManager.Instance.PerksData;
		TrophiesData = TheRunGameManager.Instance.TrophiesData;
	}

	void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	// Use this for initialization
	void Start()
	{
		// Initial setup

		ScoreManager.Instance.Reset();

		PrepareGame();

		ShowAdsWindow();
	}

	// Listens to the event when player dies and call GameOver
	void PlayerController_PlayerDied()
	{
		GameOver();
	}

	// Make initial setup and preparations before the game can be played
	public void PrepareGame()
	{
		GameState = GameState.Prepare;
	}

	// A new game officially starts
	public void StartGame()
	{
		GameState = GameState.Playing;
		if (SoundManager.Instance.background != null)
		{
			SoundManager.Instance.PlayMusic(SoundManager.Instance.background);
		}
	}

	// Called when the player died
	public void GameOver()
	{
		if (SoundManager.Instance.background != null)
		{
			SoundManager.Instance.StopMusic();
		}

		SoundManager.Instance.PlaySound(SoundManager.Instance.gameOver);
		GameState = GameState.GameOver;
		GameCount++;

		CheckIfDeservesReward();

		// Add other game over actions here if necessary
	}

	// Start a new game
	public void RestartGame(float delay = 0)
	{
		StartCoroutine(CRRestartGame(delay));
	}

	IEnumerator CRRestartGame(float delay = 0)
	{
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	#region Rewards

	PerksData PerksData;
	TrophiesData TrophiesData;

	bool CheckIfDeservesReward()
	{
		bool Result = false;
		int MaxReward = -1;

		for (int n = 0; n < PerksData.Data.Length; n++)
		{
			if (ScoreManager.Instance.Score >= PerksData.Data[n].EmojiRewardLevel)
			{
				MaxReward = n;
			}
			else
			{
				break;
			}
		}

		if (MaxReward >= 0)
		{
			ShowRewardsWindow(MaxReward);
		}

		return Result;
	}

	void ShowRewardsWindow(int MaxReward)
	{
		if (MaxReward == 0)
			RewardText.text = "You've got a new reward!";
		else
			RewardText.text = "You've got new rewards!";

		for (int r = 0; r < RewardImages.Length; r++)
		{
			if (r <= MaxReward)
			{
				RewardImages[r].gameObject.SetActive(true);
				RewardImages[r].sprite = PerksData.Data[r].ItemSprite;
				TheRunGameManager.Instance.GameData.Data.Profile.PerksQuantities[r]++;
			}
			else
			{
				RewardImages[r].gameObject.SetActive(false);
			}
		}

		//PerkNameText.text = PerksData.Data[MaxReward].ItemName;

		TheRunGameManager.Instance.GameData.Save();

		RewardWindow.SetActive(true);
	}

	void HideRewardsWindow()
	{
		RewardWindow.SetActive(false);

		if (ScoreManager.Instance.Score >= 300)
		{
			ShowTrophyWindow();
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

		TheRunGameManager.Instance.GameData.Data.Profile.EmojiData.IsGameLocked = true;
		TheRunGameManager.Instance.GameData.Data.Profile.EmojiData.LastTimePlayed = DateTime.Now;
		TheRunGameManager.Instance.GameData.Save();

		ShowAdsWindow();
	}

	#endregion

	#region Ads

	void ShowAdsWindow()
	{
		if (TheRunGameManager.Instance.GameData.Data.Profile.EmojiData.IsGameLocked)
		{
			double ElapsedTime = (DateTime.Now - TheRunGameManager.Instance.GameData.Data.Profile.EmojiData.LastTimePlayed).TotalMinutes;

			if (ElapsedTime >= 60)
			{
				TheRunGameManager.Instance.GameData.Data.Profile.EmojiData.IsGameLocked = false;
				TheRunGameManager.Instance.GameData.Data.Profile.EmojiData.LastTimePlayed = default(DateTime);
				TheRunGameManager.Instance.GameData.Save();
			}
			else
			{
				AdsMessageText.text = "If you want to keep playing, please wait a little bit or watch an ad.\n\nTime remaining:";
				AdsTimeText.text = (60 - Math.Floor(ElapsedTime)) + " minutes.";

				AdsWindow.SetActive(true);
			}
		}
	}

	public void WatchAdButtonClick()
	{

	}

	public void AdsExitButtonClick()
	{
		Loading.LoadScene("Main Menu");
	}

	#endregion
}
