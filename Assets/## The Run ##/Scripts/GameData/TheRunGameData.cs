using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public abstract class TheRunGameData
{
	protected GameData _Data;
	public GameData Data
	{
		get
		{
			if (_Data == null) Load();
			return _Data;
		}
	}


	public void Load()
	{
		if (CheckGameData())
		{
			Debug.Log("Game data found. Loading it...");
			LoadGameData();
		}
		else
		{
			Debug.Log("Game data not found. Creating new one...");
			CreateDefaultGameData();
		}
	}

	public void Save()
	{
		Debug.Log("Saving game data...");
		SaveGameData();
	}

	public void Delete()
	{

	}

	protected abstract bool CheckGameData();

	protected abstract void CreateDefaultGameData();

	protected virtual void LoadGameData()
	{

	}

	protected virtual void SaveGameData()
	{

	}
}

[Serializable]
public class GameData
{
	const int SAVE_DATA_VERSION = 1;

	public int Version;
	public UserProfile Profile;

	public bool IsSameVersion { get { return Version == SAVE_DATA_VERSION; } }

	public GameData()
	{
		Version = SAVE_DATA_VERSION;
		Profile = new UserProfile();
	}
}

[Serializable]
public class UserProfile
{
	public double TotalPlayTime;
	public uint Level { get { return (uint)(TotalPlayTime / 60) / 5; } }

	[SerializeField]
	long _Money;
	public long Money { get { return _Money; } }

	[SerializeField]
	long _TotalSellingPriceProperty;
	public long TotalSellingPriceProperty
	{ get { return _TotalSellingPriceProperty; } set { _TotalSellingPriceProperty = value; } }

	[SerializeField]
	ulong _TotalMoney;
	public ulong TotalMoney { get { return _TotalMoney; } }
	public int PurpleHearts;
	public int RedStars;

	public GameTimer LastRewardTime;
	public GameTimer LastQuoteTime;
	public GameTimer LastShieldGiftTime;

	public int[] CrystalsQuantities;
	public int[] PerksQuantities;
	public int[] PowerupsQuantities;
	public int[] TrophiesQuantities;

	public List<InvestmentData> Investments;
	public List<PropertyOwned> PropertyOwned;

	public StopTheLockGameData StopTheLockData;
	public PolygonixGameData PolygonixData;
	public DiceGameData DiceData;
	public EmojiGameData EmojiData;

	public UserProfile()
	{
		TotalPlayTime = 0;

		_Money = 0;
		PurpleHearts = 40;
		RedStars = 10;

		_TotalSellingPriceProperty = 0;

		LastRewardTime = new GameTimer();
		LastQuoteTime = new GameTimer();
		LastShieldGiftTime = new GameTimer();

		CrystalsQuantities = new int[TheRunGameManager.Instance.CrystalsData.Data.Length];
		PerksQuantities = new int[TheRunGameManager.Instance.PerksData.Data.Length];
		PowerupsQuantities = new int[TheRunGameManager.Instance.PowerupsData.Data.Length];
		TrophiesQuantities = new int[TheRunGameManager.Instance.TrophiesData.Data.Length];

		Investments = new List<InvestmentData>();
		PropertyOwned = new List<PropertyOwned> ();

		StopTheLockData = new StopTheLockGameData();
		PolygonixData = new PolygonixGameData();
		DiceData = new DiceGameData();
		EmojiData = new EmojiGameData();
	}

	public void AddMoney(long Value)
	{
		_Money = Globals.AddLongChecked(_Money, Value);
		_TotalMoney = Globals.AddULongChecked(_TotalMoney, (ulong)Value);
	}

	public void SubtractMoney(long Value)
	{
		_Money = Globals.SubtractLongChecked(_Money, Value);
	}

	public void AddProperty(string name, int quantity, long currentBuyPrice)
	{
		foreach (var property in PropertyOwned) 
		{
			if (property.propertyName == name) 
			{
				property.propertyQuantity += quantity;
				property.CurrentBuyPrice = currentBuyPrice;
				return;
			}
		}
		 
		PropertyOwned newProperty = new PropertyOwned ();
		newProperty.propertyName = name;
		newProperty.propertyQuantity = quantity;
		newProperty.CurrentBuyPrice = currentBuyPrice;
		PropertyOwned.Add (newProperty);

		TotalSellingPriceProperty = GetTotalSellingPriceProperty ();
	}

	public void SubtractProperty(string name, int quantity)
	{
		foreach (var property in PropertyOwned) 
		{
			if (property.propertyName == name) 
			{
				property.propertyQuantity -= quantity;
			}
		}

		TotalSellingPriceProperty = GetTotalSellingPriceProperty ();
	}

	public int GetQuantityProperty(string name)
	{
		foreach (var property in PropertyOwned) 
		{
			if (property.propertyName == name) 
			{
				return property.propertyQuantity;
			}
		}

		return 0;
	}

	public long GetTotalSellingPriceProperty()
	{
		long current = 0;

		foreach (var property in PropertyOwned) 
		{
			if (property.propertyQuantity > 0) 
			{
				current += (long)TheRunGameManager.Instance.StoreDefaultProperty.GetPropertyDefaultDataPriceRecord (property.propertyName) * property.propertyQuantity;
			}
		}

		return current;
	}
}

[Serializable]
public class InvestmentData
{
	[SerializeField]
	string _FinishTime;
	public DateTime FinishTime
	{
		get { return Globals.StringToDateTime(_FinishTime); }
		set { _FinishTime = Globals.DateTimeToString(value); }
	}

	public long InvestmentAmount;
	public long ReturnAmount;
	public SchemeInfo.RiskTypes Risk;
}

[Serializable]
public class StopTheLockGameData
{
	[SerializeField]
	string _LastTimePlayed;
	public DateTime LastTimePlayed
	{
		get { return Globals.StringToDateTime(_LastTimePlayed); }
		set { _LastTimePlayed = Globals.DateTimeToString(value); }
	}
	public int MatchesPlayedWithinLimit;

	public StopTheLockGameData()
	{
		LastTimePlayed = default(DateTime);
		MatchesPlayedWithinLimit = 0;
	}
}

[Serializable]
public class PolygonixGameData
{
	public int BetsWon;

	public PolygonixGameData()
	{
		BetsWon = 0;
	}
}

[Serializable]
public class DiceGameData
{
	[SerializeField]
	string _LastTimePlayed;
	public DateTime LastTimePlayed
	{
		get { return Globals.StringToDateTime(_LastTimePlayed); }
		set { _LastTimePlayed = Globals.DateTimeToString(value); }
	}
	public int MatchesPlayedWithinLimit;
	public int BetsWon;

	public DiceGameData()
	{
		LastTimePlayed = default(DateTime);
		MatchesPlayedWithinLimit = 0;
		BetsWon = 0;
	}
}

[Serializable]
public class EmojiGameData
{
	[SerializeField]
	string _LastTimePlayed;
	public DateTime LastTimePlayed
	{
		get { return Globals.StringToDateTime(_LastTimePlayed); }
		set { _LastTimePlayed = Globals.DateTimeToString(value); }
	}
	public bool IsGameLocked;
	public int HighScore;

	public EmojiGameData()
	{
		LastTimePlayed = default(DateTime);
		IsGameLocked = false;
		HighScore = 0;
	}
}

[Serializable]
public class GameTimer
{
	[SerializeField]
	long time;

	public DateTime Time
	{
		get { return DateTime.FromBinary(time); }
		set { time = value.ToBinary(); }
	}

	public GameTimer()
	{
		Time = default(DateTime);
	}

	public void SetTimeFromNow(TimeSpan timeSpan)
	{
		Time = DateTime.Now + timeSpan;
	}

	public bool HasTimeFinished()
	{
		return DateTime.Now > Time;
	}
}

[Serializable]
public class PropertyOwned
{
	public string propertyName;
	public int propertyQuantity;

	[SerializeField]
	private long currentBuyPrice;
	public long CurrentBuyPrice
	{
		get{ return currentBuyPrice; }
		set{ currentBuyPrice = value; }
	}
}