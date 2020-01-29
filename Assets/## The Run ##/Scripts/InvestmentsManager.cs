using System;
using UnityEngine;
using UnityEngine.UI;

public class InvestmentsManager : MonoBehaviour
{
	[SerializeField]
	GameObject FinishInvestmentWindow = null;
	[SerializeField]
	Text FinishInvestmentText = null;

	[Space]
	[SerializeField]
	SchemesData SchemesData = null;

	public bool AreInvestmentsChecksEnabled { get; set; }

	public event Action<bool> OnFinishedInvestment;

	float InvestmentCheckTimer;

	void Start()
	{
		AreInvestmentsChecksEnabled = true;
	}

	void Update()
	{
		if (!AreInvestmentsChecksEnabled) return;

		InvestmentCheckTimer += Time.deltaTime;

		if (InvestmentCheckTimer >= 1)
		{
			InvestmentCheckTimer = 0;

			CheckInvestments();
		}
	}

	public void PlaceInvestment(long InvestmentAmount, long ReturnAmount, int DaysDuration, SchemeInfo.RiskTypes Risk)
	{
		TheRunGameManager.Instance.GameData.Data.Profile.SubtractMoney(InvestmentAmount);

		InvestmentData Data = new InvestmentData();

		Data.FinishTime = DateTime.Now + TimeSpan.FromDays(DaysDuration);
		Data.InvestmentAmount = InvestmentAmount;
		Data.ReturnAmount = ReturnAmount;
		Data.Risk = Risk;

		TheRunGameManager.Instance.GameData.Data.Profile.Investments.Add(Data);

		TheRunGameManager.Instance.GameData.Save();
	}

	void CheckInvestments()
	{
		int Count = TheRunGameManager.Instance.GameData.Data.Profile.Investments.Count;

		for (int i = 0; i < Count; i++)
		{
			if (DateTime.Now >= TheRunGameManager.Instance.GameData.Data.Profile.Investments[i].FinishTime)
			{
				FinishInvestment(i);
				break;
			}
		}
	}

	void FinishInvestment(int Index)
	{
		AreInvestmentsChecksEnabled = false;

		long InvestmentAmount = TheRunGameManager.Instance.GameData.Data.Profile.Investments[Index].InvestmentAmount;
		long ReturnAmount = TheRunGameManager.Instance.GameData.Data.Profile.Investments[Index].ReturnAmount;
		SchemeInfo.RiskTypes Risk = TheRunGameManager.Instance.GameData.Data.Profile.Investments[Index].Risk;

		float Probability = SchemesData.RisksData[(int)Risk].Probability;
		float Chance = UnityEngine.Random.Range(0f, 100f);

		if (Chance > Probability)
		{
			FinishInvestmentText.text = string.Format("<size=30>Investment Finished!</size>\n\nYou invested {0} JGD and you have won {1} JGD!\n\nCongratulations!", InvestmentAmount, ReturnAmount);
			TheRunGameManager.Instance.GameData.Data.Profile.AddMoney(ReturnAmount);
			if (OnFinishedInvestment != null) OnFinishedInvestment(true);
		}
		else
		{
			FinishInvestmentText.text = string.Format("<size=30>Investment Finished!</size>\n\nYou invested {0} JGD but you have lost.\n\nBetter luck next time!", InvestmentAmount);
			if (OnFinishedInvestment != null) OnFinishedInvestment(false);
		}

		TheRunGameManager.Instance.GameData.Data.Profile.Investments.RemoveAt(Index);
		TheRunGameManager.Instance.GameData.Save();

		FinishInvestmentWindow.SetActive(true);
	}

	public void CloseFinishInvestmentWindow()
	{
		FinishInvestmentWindow.SetActive(false);
		AreInvestmentsChecksEnabled = true;
	}
}