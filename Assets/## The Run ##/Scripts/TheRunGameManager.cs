using UnityEngine;

public class TheRunGameManager : MonoBehaviour
{
	#region Singleton and constructor

	static TheRunGameManager _Instance;
	public static TheRunGameManager Instance
	{
		get
		{
			if (_Instance == null)
			{
				GameObject GO = Instantiate(Resources.Load<GameObject>("TheRunGameManager"));
				_Instance = GO.GetComponent<TheRunGameManager>();
				DontDestroyOnLoad(GO);
			}

			return _Instance;
		}
	}

	//Private constructor
	TheRunGameManager() { }

	#endregion

	[Header("Data References")]
	[SerializeField]
	CrystalsData _CrystalsData = null;
	[SerializeField]
	PerksData _PerksData = null;
	[SerializeField]
	PowerupsData _PowerupsData = null;
	[SerializeField]
	TrophiesData _TrophiesData = null;
	[SerializeField]
	MoneyData _MoneyData = null;
	[SerializeField]
	RewardsData _RewardsData = null;
	[SerializeField]
	StoreDefaultProperty _StoreDefaultProperty = null;

	public CrystalsData CrystalsData { get { return _CrystalsData; } }
	public PerksData PerksData { get { return _PerksData; } }
	public PowerupsData PowerupsData { get { return _PowerupsData; } }
	public TrophiesData TrophiesData { get { return _TrophiesData; } }
	public MoneyData MoneyData { get { return _MoneyData; } }
	public RewardsData RewardsData { get { return _RewardsData; } }
	public StoreDefaultProperty StoreDefaultProperty { get { return _StoreDefaultProperty; } set { _StoreDefaultProperty = value; } }

	void Awake()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		InitializeGameData();

		Debug.Log("GameManager Initialized.");
	}

	void Update()
	{
		GameData.Data.Profile.TotalPlayTime += Time.deltaTime;
	}

	void OnDestroy()
	{
		GameData.Save();
	}

	#region Persistent Stuff

	public long BetAmount { get; set; }

	#endregion

	#region Game Data

	public TheRunGameData GameData { get; private set; }

	void InitializeGameData()
	{
#if UNITY_ANDROID
		GameData = new TheRunMobileGameData();
#elif UNITY_IOS
		GameData = new TheRunMobileGameData();
#else
		GameData = new TheRunStandaloneGameData();
#endif
	}

	#endregion
}