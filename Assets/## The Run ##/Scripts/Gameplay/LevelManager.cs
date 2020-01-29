using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[Header("Level Properties")]
	[SerializeField]
	bool _DoesItHaveTime = true;
	[SerializeField]
	float InitialTime = 400;

	public enum LevelStates { WaitingToStart, InProgress, Finished }
	public LevelStates LevelState { get; private set; }

	public enum FinishingLevelReason { Death, TimeUp, PlayerDecision }

	public float RemainingTime { get; set; }
	public bool DoesItHaveTime { get { return _DoesItHaveTime; } }

	#region Singleton and constructor

	static LevelManager _Instance;
	public static LevelManager Instance
	{
		get
		{
			if (_Instance == null) _Instance = FindObjectOfType<LevelManager>();
			return _Instance;
		}
	}

	//Private constructor
	LevelManager() { }

	#endregion

	void Awake()
	{
		LevelState = LevelStates.WaitingToStart;
	}

	void Start()
	{
		StartLevel();
	}

	/// <summary>
	/// Start the level.
	/// </summary>
	public virtual void StartLevel()
	{
		LevelState = LevelStates.InProgress;
		if (_DoesItHaveTime) RemainingTime = InitialTime;
	}

	/// <summary>
	/// Finish the level.
	/// </summary>
	public virtual void FinishLevel(FinishingLevelReason Reason)
	{
		LevelState = LevelStates.Finished;

		//Save the game when the level finishes whatever the reason.
		TheRunGameManager.Instance.GameData.Data.Profile.AddMoney(GameManager.Instance.CurrentMoney);
		TheRunGameManager.Instance.GameData.Save();

		switch (Reason)
		{
			case FinishingLevelReason.Death:

				ShowDeathMenu();
				break;

			case FinishingLevelReason.TimeUp:

				ShowTimeUpMenu();
				break;

			case FinishingLevelReason.PlayerDecision:

				//Do nothing special
				break;
		}
	}

	/// <summary>
	/// This method is updated every frame through the level manager.
	/// </summary>
	public virtual void Update()
	{
		if (LevelState == LevelStates.InProgress)
		{
			if (_DoesItHaveTime)
			{
				RemainingTime -= Time.deltaTime;
				if (RemainingTime <= 0)
				{
					RemainingTime = 0;
					FinishLevel(FinishingLevelReason.TimeUp);
				}
			}
		}
	}

	#region Pause Menu

	public bool IsPaused { get; set; }

	void ShowDeathMenu()
	{
		PauseMenu.Instance.ShowDeathMenu();
	}

	void ShowTimeUpMenu()
	{
		PauseMenu.Instance.ShowTimeUpMenu();
	}

	public void Pause()
	{
		if (!IsPaused)
		{
			PauseMenu.Instance.ShowPauseMenu();
		}
	}

	#endregion
}