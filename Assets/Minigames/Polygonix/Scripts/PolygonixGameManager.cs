using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolygonixGameManager : MonoBehaviour
{
	[Header("External References")]
	[SerializeField]
	GameObject comboTextPrefab = null;
	[SerializeField]
	GameObject mainCameraPrefab = null;
	[SerializeField]
	GameObject[] CollectablePrefabs = null;
	[SerializeField]
	Transform CollectablesParent = null;

	[Header("Internal References")]
	[SerializeField]
	Canvas canvas = null;
	[SerializeField]
	GameObject Menu = null;
	[SerializeField]
	GameObject HUD = null;
	[SerializeField]
	GameObject Instructions = null;
	[SerializeField]
	Text TimerText = null;
	[SerializeField]
	Text pointsText = null;
	[SerializeField]
	Text FailuresText = null;
	[SerializeField]
	PolygonPainter polygonPainter = null;

	[Header("Properties")]
	[SerializeField]
	int comboTimerSeconds = 1;  //if you kill the next enemy before this time elapsed you get a combo.
	[SerializeField]
	float MatchDuration = 60;
	[SerializeField]
	int MaxFails = 3;

	[Header("Results Window")]
	[SerializeField]
	GameObject ResultsWindow = null;
	[SerializeField]
	Text ResultsWindowTitleText = null;
	[SerializeField]
	Text ResultsWindowMessageText = null;
	[SerializeField]
	Text ResultsWindowButtonText = null;

	[Header("Trophy Window")]
	[SerializeField]
	GameObject TrophyWindow = null;
	[SerializeField] Image TrophyRewardImage = null;

	int CurrentFails;
	int CurrentLevel;

	private int points = 0;
	private float lastTimeCollected = 0f;
	private int comboMultiplier = 1;

	/// <summary>
	/// Gets the points.
	/// </summary>
	/// <value>The points.</value>
	public int Points
	{
		get { return points; }
	}

	//Collectable[] collectables;

	List<Collectable> Collectables = new List<Collectable>(9);

	enum GameStates { Menu, Playing, Finished }
	GameStates State;

	void Start()
	{
		State = GameStates.Menu;

		if (Camera.main == null)
		{
			Debug.Log("Found no Main Camera in the scene and added it automatically.");
			Instantiate(mainCameraPrefab);
		}

		Menu.SetActive(true);
		HUD.SetActive(false);
		polygonPainter.enabled = false;
		Instructions.SetActive(false);

		InitializeBet();

		CurrentLevel = 0;

		//collectables = new Collectable[MaxCollectables];
	}

	void Update()
	{
		if (State == GameStates.Playing)
		{
			ProcessTimer();

			if (LevelFinished())
			{
				FinishGame(false);
			}
		}
	}

	void FinishGame(bool IsGameOver)
	{
		State = GameStates.Finished;
		polygonPainter.enabled = false;

		for (int c = 0; c < Collectables.Count; c++)
		{
			Collectables[c].CollectableCollected -= OnCollectableCollected;
			Destroy(Collectables[c].gameObject);
		}
		Collectables.Clear();

		ShowResultsWindow(IsGameOver);
	}

	void InitializeBet()
	{
		TheRunGameManager.Instance.GameData.Data.Profile.SubtractMoney(TheRunGameManager.Instance.BetAmount);
		TheRunGameManager.Instance.GameData.Save();
	}

	#region Timer

	float Timer;
	int TimeRemaining;

	void ProcessTimer()
	{
		Timer -= Time.deltaTime;
		int t = Mathf.CeilToInt(Timer);
		if (t != TimeRemaining) TimerText.text = t.ToString();
		TimeRemaining = t;
	}

	#endregion

	/// <summary>
	/// Is raised by the enemy when it's killed.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
	void OnCollectableCollected(Collectable sender, CollectableEventArgs e)
	{
		if (e.Points < 0)
		{
			comboMultiplier = 1;
			CurrentFails++;

			FailuresText.text = "Failures: " + CurrentFails;

			if (CurrentFails >= MaxFails)
			{
				FinishGame(true);
				return;
			}
		}
		else
		{
			UpdateComboMultiplier(e);

			points += e.Points * comboMultiplier; //add the player points multiplied by the combo multiplier
			pointsText.text = "Points: " + points;

			lastTimeCollected = e.CollectedTime;
		}

		sender.CollectableCollected -= OnCollectableCollected;
		Collectables.Remove(sender);

		InstantiateCollectable(e.Index);
	}

	/// If the enemy is killed in combo time add a combo multiplier.
	/// The more enemies you kill in a combo the bigger the multiplier gets.
	void UpdateComboMultiplier(CollectableEventArgs e)
	{
		if (e.CollectedTime <= lastTimeCollected + comboTimerSeconds)
		{
			comboMultiplier++;
		}
		else
		{
			comboMultiplier = 1;
		}

		if (comboMultiplier > 1 && comboTextPrefab != null && canvas != null)
		{
			var comboText = Instantiate(comboTextPrefab);
			comboText.GetComponent<ComboText>().SetMultiplier(comboMultiplier);
			comboText.transform.position = Camera.main.WorldToScreenPoint(e.Position);

			comboText.transform.SetParent(canvas.transform);
		}
	}

	/// <summary>
	/// Returns true if the level is finished
	/// </summary>
	/// <returns><c>true</c>, if done was leveled, <c>false</c> otherwise.</returns>
	bool LevelFinished()
	{
		return Timer <= 0;
	}

	void InstantiateCollectable(int Index)
	{
		GameObject GO = Instantiate(CollectablePrefabs[Index]);
		GO.transform.SetParent(CollectablesParent, false);

		Collectable collectable = GO.GetComponent<Collectable>();
		collectable.CollectableCollected += OnCollectableCollected;
		collectable.Index = Index;

		Collectables.Add(collectable);
	}

	#region UI

	public void ShowInstructions(bool Value)
	{
		Instructions.SetActive(Value);
	}

	public void StartGame()
	{
		InstantiateCollectable(0);
		InstantiateCollectable(0);
		InstantiateCollectable(0);
		InstantiateCollectable(1);
		InstantiateCollectable(1);
		InstantiateCollectable(1);
		InstantiateCollectable(2);
		InstantiateCollectable(2);
		InstantiateCollectable(2);

		CurrentFails = 0;
		points = 0;
		Timer = MatchDuration;

		Menu.SetActive(false);
		HUD.SetActive(true);
		polygonPainter.enabled = true;
		Instructions.SetActive(false);

		State = GameStates.Playing;
	}

	#endregion

	#region Results

	bool IsGameOver;

	void ShowResultsWindow(bool IsGameOver)
	{
		if (!IsGameOver && points >= 500)
		{
			CurrentLevel++;

			ResultsWindowTitleText.text = "You have completed the level!";
			ResultsWindowMessageText.text = string.Format("You got 500 points or more, so you pass to the next level!\n\nYou have earned {0} JGD in total.", TheRunGameManager.Instance.BetAmount * CurrentLevel);
			ResultsWindowButtonText.text = "Go to next level!";

			TheRunGameManager.Instance.GameData.Data.Profile.PolygonixData.BetsWon++;
			TheRunGameManager.Instance.GameData.Data.Profile.AddMoney(TheRunGameManager.Instance.BetAmount);
			TheRunGameManager.Instance.GameData.Save();

			this.IsGameOver = false;
		}
		else
		{
			ResultsWindowTitleText.text = "You have lost...";
			ResultsWindowMessageText.text = string.Format("You haven't got 500 points or more, so you this is a game over.\n\nYou have earned {0} JGD in total.", TheRunGameManager.Instance.BetAmount * CurrentLevel);
			ResultsWindowButtonText.text = "Go back to menu";

			this.IsGameOver = true;
		}

		ResultsWindow.SetActive(true);
	}

	public void CloseResultsWindow()
	{
		if (IsGameOver)
		{
			Loading.LoadScene("Main Menu");
		}
		else
		{
			ResultsWindow.SetActive(false);

			if (TheRunGameManager.Instance.GameData.Data.Profile.PolygonixData.BetsWon >= 10)
			{
				TheRunGameManager.Instance.GameData.Data.Profile.PolygonixData.BetsWon = 0;
				TheRunGameManager.Instance.GameData.Save();

				ShowTrophyWindow();
			}
			else
			{
				StartGame();
			}
		}
	}

	#endregion

	#region Rewards

	void ShowTrophyWindow()
	{
		TrophyRewardImage.sprite = TheRunGameManager.Instance.TrophiesData.Data[5].ItemSprite;
		TheRunGameManager.Instance.GameData.Data.Profile.TrophiesQuantities[5]++;
		TheRunGameManager.Instance.GameData.Save();

		TrophyWindow.SetActive(true);
	}

	public void CloseTrophyWindow()
	{
		TrophyWindow.SetActive(false);
		StartGame();
	}

	#endregion
}