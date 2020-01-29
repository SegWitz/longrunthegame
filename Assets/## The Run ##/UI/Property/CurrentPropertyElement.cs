using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPropertyElement : MonoBehaviour {

	[SerializeField]
	Text assetsText;
	[SerializeField]
	Text unitBoughtText;
	[SerializeField]
	Text initialBuyingPriceText;
	[SerializeField]
	Text currentSellingPriceText;

	public void SetData(long buyPrice, long sellPrice, string name, int quantity)
	{
		assetsText.text = name;
		unitBoughtText.text = quantity.ToString ();
		long initial = buyPrice * quantity;
		long sell = sellPrice * quantity;
		initialBuyingPriceText.text = Globals.GetFormattedCurrency(initial, true);
		currentSellingPriceText.text = Globals.GetFormattedCurrency (sell, true);
	}
}
