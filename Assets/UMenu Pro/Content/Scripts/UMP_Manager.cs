using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UMP_Manager : MonoBehaviour
{
    [Space(10f)]
    public int DefaultWindowIndex = 0;
    public List<GameObject> Windows = new List<GameObject>();
	[Space(10)]
	public List<LevelInfo> Levels = new List<LevelInfo>();
    
	public GameObject LevelPrefab;
	public Transform LevelPanel;
    public static UMP_Manager Reference;
	[Space]
	[SerializeField]
	Text MoneyText = null;
	[SerializeField]
	Text PurpleHeartsText = null;
	[SerializeField]
	Text RedStarText = null;

	[Space]
	public GameObject NotEnoughPurpleHeartsWindow = null;

	private int CurrentWindow = -1;

	InvestmentsManager Investments;

	void Awake()
	{
	    if (Reference == null)
	    {
	        Reference = this;
	    }
		InstanceLevels();

		Investments = FindObjectOfType<InvestmentsManager>();
		Investments.OnFinishedInvestment += Investments_OnFinishedInvestment;
        //call init
        Init();
	}

	void Start()
	{
		UpdateMoneyText();
		UpdatePurpleHeartsText();
		UpdateRedStarsText();
//		TheRunGameManager.Instance.GameData.Data.Profile.PurpleHearts = 40;
		PlayerPrefs.SetInt(UMPKeys.Controller, 1);
	}

	void OnDestroy()
	{
		Investments.OnFinishedInvestment -= Investments_OnFinishedInvestment;
	}

	/// <summary>
	/// 
	/// </summary>
	void InstanceLevels()
	{
		for (int i = 0; i < Levels.Count; i++)
		{
			GameObject l = Instantiate(LevelPrefab) as GameObject;

			UMP_LevelInfo li = l.GetComponent<UMP_LevelInfo>();
			li.SetInfo(Levels[i].Title, Levels[i].Description, Levels[i].Preview, Levels[i].LevelName);

			l.transform.SetParent(LevelPanel, false);
		}
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="id">window to active</param>
	/// <param name="disable">disabled currents window?</param>
	public void ChangeWindow(int id)
	{
		if (Time.timeScale != 1)
		{
			Time.timeScale = 1;
		}

		if (CurrentWindow == id) return;
		CurrentWindow = id;

		if (id != 2)
		{
			for (int i = 0; i < Windows.Count; i++)
			{
				Windows[i].SetActive(false);
			}

		}

		Windows[id].SetActive(true);

		if (id != 0)
		{
			Invoke("ShowBannerAd", 0.5f);
		}
	}
    /// <summary>
    /// Init the instance.
    /// </summary>
    public void Init()
    {
        //When the game start we will check that .
        //player has logged in or not .
        //if logged in we will direct him to home window .
        //else we will force him log in .
        //At the time of log in other tab will not work .
        //12 is user window form .
        ChangeWindow(!PlayFab.PlayfabData.IsPlayerRegisteredToPlayFab ? 12 : DefaultWindowIndex);

        
    }

	/// <summary>
	/// Open URL
	/// </summary>
	/// <param name="url"></param>
	public void SocialButton(string url) { Application.OpenURL(url); }

	/// <summary>
	/// Quit
	/// </summary>
	public void QuitApp()
	{
		Globals.ExitGame();
	}

	[System.Serializable]
	public class LevelInfo
	{
		/// <summary>
		/// Name of scene of build setting
		/// </summary>
		public string LevelName;
		[Space(5)]
		public string Title;
		public string Description;
		public Sprite Preview;
	}

	public void UpdateMoneyText()
	{
		MoneyText.text = Globals.GetFormattedCurrency(TheRunGameManager.Instance.GameData.Data.Profile.Money, true);
	}

	public void UpdatePurpleHeartsText()
	{
		PurpleHeartsText.text = TheRunGameManager.Instance.GameData.Data.Profile.PurpleHearts.ToString();
	}

	public void UpdateRedStarsText()
	{
		RedStarText.text = TheRunGameManager.Instance.GameData.Data.Profile.RedStars.ToString();
	}

	void Investments_OnFinishedInvestment(bool HasWon)
	{
		UpdateMoneyText();
	}

	public bool ValidEmail(UnityEngine.UI.InputField email)
	{
		if (email.text.Contains ("@")) 
		{
			return true;
		}
		return false;
	}
}