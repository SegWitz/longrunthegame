using System;
using UnityEngine;
using UnityEngine.UI;

public class CurrentInvestmentElement : MonoBehaviour
{
	[SerializeField]
	Text AmountText = null;
	[SerializeField]
	Text RiskText = null;
	[SerializeField]
	Text TimeText = null;

	public void SetData(long Amount, SchemeInfo.RiskTypes Risk, TimeSpan Time)
	{
		AmountText.text = Globals.GetFormattedCurrency(Amount, false);
		RiskText.text = Risk.ToString();
		TimeText.text = string.Format("{0:D2} days, {1:D2}:{2:D2}:{3:D2}", Time.Days, Time.Hours, Time.Minutes, Time.Seconds);
	}
}