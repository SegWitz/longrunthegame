using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPropertyWindow : MonoBehaviour {

	[SerializeField]
	GameObject ElementPrefab;
	[SerializeField]
	Transform ElementsParent;

	public Text totalSellingPriceText;

	CurrentPropertyElement[] PropertyElements;

	public void Show()
	{
		List<PropertyOwned> Property = TheRunGameManager.Instance.GameData.Data.Profile.PropertyOwned;

		PropertyElements = new CurrentPropertyElement[Property.Count];

		for (int i = 0; i < PropertyElements.Length; i++)
		{
			if(Property[i].propertyQuantity > 0)
			{
				long propertyPrice = (long)TheRunGameManager.Instance.StoreDefaultProperty.GetPropertyDefaultDataPriceRecord (Property[i].propertyName);
				long currentSellPrice = propertyPrice;
				GameObject GO = Instantiate(ElementPrefab);
				GO.transform.SetParent(ElementsParent, false);
				PropertyElements[i] = GO.GetComponent<CurrentPropertyElement>();
				PropertyElements [i].SetData (Property [i].CurrentBuyPrice, currentSellPrice, Property [i].propertyName, Property [i].propertyQuantity);
			}
		}
		Debug.Log (PropertyElements.Length);

		TheRunGameManager.Instance.GameData.Data.Profile.TotalSellingPriceProperty = TheRunGameManager.Instance.GameData.Data.Profile.GetTotalSellingPriceProperty ();

		long total = TheRunGameManager.Instance.GameData.Data.Profile.TotalSellingPriceProperty;

		totalSellingPriceText.text = Globals.GetFormattedCurrency (total,true);

		gameObject.SetActive(true);
	}

	public void Close()
	{
		for (int i = 0; i < PropertyElements.Length; i++)
		{
			if(PropertyElements[i] != null)
			{
				Destroy(PropertyElements[i].gameObject);
			}
		}
		gameObject.SetActive(false);
	}
}
