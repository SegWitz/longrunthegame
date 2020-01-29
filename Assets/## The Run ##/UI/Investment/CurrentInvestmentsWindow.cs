using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrentInvestmentsWindow : MonoBehaviour
{
	[SerializeField]
	GameObject ElementPrefab = null;
	[SerializeField]
	Transform ElementsParent = null;

	CurrentInvestmentElement[] InvestmentElements;

	public void Show()
	{
		List<InvestmentData> Investments = TheRunGameManager.Instance.GameData.Data.Profile.Investments;
		InvestmentElements = new CurrentInvestmentElement[Investments.Count];

		for (int i = 0; i < InvestmentElements.Length; i++)
		{
			GameObject GO = Instantiate(ElementPrefab);
			GO.transform.SetParent(ElementsParent, false);
			InvestmentElements[i] = GO.GetComponent<CurrentInvestmentElement>();
			InvestmentElements[i].SetData(Investments[i].InvestmentAmount, Investments[i].Risk, Investments[i].FinishTime - DateTime.Now);
		}

		gameObject.SetActive(true);
	}

	public void Close()
	{
		for (int i = 0; i < InvestmentElements.Length; i++)
		{
			Destroy(InvestmentElements[i].gameObject);
		}

		gameObject.SetActive(false);
	}
}