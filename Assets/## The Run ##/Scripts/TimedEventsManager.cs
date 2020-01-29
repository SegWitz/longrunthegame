using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TimedEventsManager : MonoBehaviour
{
	[SerializeField]
	UMP_Manager Manager = null;

	[Header("Reward Window")]
	[SerializeField]
	GameObject RewardWindowGO = null;
	[SerializeField]
	Image RewardItemImage = null;

	[Header("Claim Window")]
	[SerializeField]
	GameObject ClaimWindowGO = null;
	[SerializeField]
	Text ClaimItemText = null;
	[SerializeField]
	Image ClaimItemImage = null;

	[Header("Quote Window")]
	[SerializeField]
	GameObject QuoteWindowGO = null;
	[SerializeField]
	Text QuoteText = null;
	[SerializeField]
	Text QuoteAuthorText = null;
	[SerializeField]
	QuotesData Quotes = null;

	RewardsData Rewards;
	CrystalsData Crystals;
	PerksData Perks;
	MoneyData Money;
	PowerupsData Powerups;

	void Start()
	{
		Rewards = TheRunGameManager.Instance.RewardsData;
		Crystals = TheRunGameManager.Instance.CrystalsData;
		Perks = TheRunGameManager.Instance.PerksData;
		Money = TheRunGameManager.Instance.MoneyData;
		Powerups = TheRunGameManager.Instance.PowerupsData;
	}

	void Update()
	{
		if (CheckRewardTime()) return;
		if (CheckQuoteTime()) return;
		if (CheckShieldTime()) return;
	}

	#region Rewards

	int RewardIndex;

	bool CheckRewardTime()
	{
		if (!TheRunGameManager.Instance.GameData.Data.Profile.LastRewardTime.HasTimeFinished()) return false;

		enabled = false;

		int BoxesAmount = Rewards.Data.Length;

		//If the player has a total play time of less thant half an hour, avoid letting him get a card.
		//Since 10000JGD * 0 would be zero. It would be weird to show a message for winning nothing.
		if (TheRunGameManager.Instance.GameData.Data.Profile.TotalPlayTime < 1800) BoxesAmount = 2;

		RewardIndex = Random.Range(0, BoxesAmount);

		RewardItemImage.sprite = Rewards.Data[RewardIndex].ItemSprite;

		RewardWindowGO.SetActive(true);

		return true;
	}

	public void ClaimButtonClick()
	{
		GetReward();

		RewardWindowGO.SetActive(false);
		ClaimWindowGO.SetActive(true);
	}

	public void CloseClaimWindowButtonClick()
	{
		ClaimWindowGO.SetActive(false);

		enabled = true;
	}

	void GetReward()
	{
		Sprite RewardSprite = null; ;
		string RewardText = string.Empty;

		switch (RewardIndex)
		{
			case 0: //Box01
			case 1: //Box02

				int Type = Random.Range(0, 3);

				switch (Type)
				{
					case 0: //Random Crystal

						int CrystalIndex = Random.Range(0, Crystals.Data.Length);
						RewardSprite = Crystals.Data[CrystalIndex].ItemSprite;
						RewardText = Crystals.Data[CrystalIndex].ItemName;
						TheRunGameManager.Instance.GameData.Data.Profile.CrystalsQuantities[CrystalIndex]++;

						break;

					case 1: //Random Perk

						int PerkIndex = Random.Range(0, Perks.Data.Length);
						RewardSprite = Perks.Data[PerkIndex].ItemSprite;
						RewardText = Perks.Data[PerkIndex].ItemName;
						TheRunGameManager.Instance.GameData.Data.Profile.PerksQuantities[PerkIndex]++;

						break;

					case 2: //Random Money

						int MoneyIndex = Random.Range(0, Money.Data.Length);
						RewardSprite = Money.Data[MoneyIndex].ItemSprite;
						RewardText = Money.Data[MoneyIndex].ItemName;
						TheRunGameManager.Instance.GameData.Data.Profile.AddMoney(Money.Data[MoneyIndex].ItemValue);

						break;
				}

				break;

			case 2: //Card01
			case 3: //Card03

				uint TotalMinutes = (uint)(TheRunGameManager.Instance.GameData.Data.Profile.TotalPlayTime / 60f);
				long MoneyAmount = 10000 * (TotalMinutes / 30);

				RewardSprite = Money.Data[0].ItemSprite;
				RewardText = Globals.GetFormattedCurrency(MoneyAmount, true);
				TheRunGameManager.Instance.GameData.Data.Profile.AddMoney(MoneyAmount);

				break;
		}

		TheRunGameManager.Instance.GameData.Data.Profile.LastRewardTime.SetTimeFromNow(TimeSpan.FromHours(24));
		TheRunGameManager.Instance.GameData.Save();

		Manager.UpdateMoneyText();

		ClaimItemText.text = RewardText;
		ClaimItemImage.sprite = RewardSprite;
	}

	#endregion

	#region Quotes

	bool CheckQuoteTime()
	{
		if (!TheRunGameManager.Instance.GameData.Data.Profile.LastQuoteTime.HasTimeFinished()) return false;

		enabled = false;

		ShowQuoteWindow();

		return true;
	}

	void ShowQuoteWindow()
	{
		Quote quote = Quotes.Quotes[Random.Range(0, Quotes.Quotes.Length)];
		QuoteText.text = quote.QuoteText;
		QuoteAuthorText.text = "- " + quote.AuthorName;
		QuoteWindowGO.SetActive(true);
	}

	public void CloseQuoteWindowButtonClick()
	{
		QuoteWindowGO.SetActive(false);
		TheRunGameManager.Instance.GameData.Data.Profile.LastQuoteTime.SetTimeFromNow(TimeSpan.FromHours(24));
		TheRunGameManager.Instance.GameData.Save();

		enabled = true;
	}

	#endregion

	#region Shield

	bool CheckShieldTime()
	{
		if (!TheRunGameManager.Instance.GameData.Data.Profile.LastShieldGiftTime.HasTimeFinished()) return false;

		enabled = false;

		TheRunGameManager.Instance.GameData.Data.Profile.PowerupsQuantities[(int)PowerupsInfo.PowerupsIDs.Shield]++;
		TheRunGameManager.Instance.GameData.Data.Profile.LastShieldGiftTime.SetTimeFromNow(TimeSpan.FromHours(48));
		TheRunGameManager.Instance.GameData.Save();

		ClaimItemText.text = Powerups.Data[(int)PowerupsInfo.PowerupsIDs.Shield].ItemName;
		ClaimItemImage.sprite = Powerups.Data[(int)PowerupsInfo.PowerupsIDs.Shield].ItemSprite;
		ClaimWindowGO.SetActive(true);

		return true;
	}

	#endregion
}