using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UMP_Property : MonoBehaviour {

	public Text title;
	public Text pricePerUnitText;
	public Text totalPriceText;
	public Text quantityText;


	[Space(5)]
	private int hour;
	private int minute;
	private int second;

	[Header("PurchaseWindow"), Space(5)]
	public GameObject confirmationWindow;
	public GameObject resultWindow;
	[SerializeField]
	Text confirmationText = null;
	[SerializeField]
	Text resultText = null;
	[SerializeField]
	Text buttonConfirmationText = null;

	public enum Transaction
	{
		Buy,
		Sell
	}
	public Transaction transaction;

	private long pricePerUnit;
	private long totalPrice;
	private int quantity = 1;
	private long pricePerUnitSell;

	private TimeSpan _startTime;
	private TimeSpan _endTime;

	public void ChangeState(int i)
	{
		if (i == 0) 
		{
			transaction = Transaction.Buy;
		} 
		else 
		{
			transaction = Transaction.Sell;
		}
	}

	public void ChangeQuantity(int value)
	{
		quantity += value;

		if (quantity <= 0) 
		{
			quantity = 1;
		}

		if(transaction == Transaction.Sell && quantity > TheRunGameManager.Instance.GameData.Data.Profile.GetQuantityProperty (title.text))
		{
			quantity = TheRunGameManager.Instance.GameData.Data.Profile.GetQuantityProperty (title.text);
		}

		InitializeText ();
	}

	

	public void PutInfo(string name)
	{
		title.text = name;
		quantity = 1;
		GeneratePrice ();

	}

	void InitializeText()
	{
		if (transaction == Transaction.Sell) 
		{
			pricePerUnitSell = pricePerUnit;
			totalPrice = pricePerUnitSell * quantity;
			pricePerUnitText.text = Globals.GetFormattedCurrency (pricePerUnitSell, true);
			totalPriceText.text = Globals.GetFormattedCurrency (totalPrice, true);
			quantityText.text = quantity.ToString ();
		} 
		else
		{
			totalPrice = pricePerUnit * quantity;
			pricePerUnitText.text = Globals.GetFormattedCurrency (pricePerUnit, true);
			totalPriceText.text = Globals.GetFormattedCurrency (totalPrice, true);
			quantityText.text = quantity.ToString ();
		}
	}

	void GeneratePrice()
	{
		for (int i = 0; i < TheRunGameManager.Instance.StoreDefaultProperty.propertyDefaultData.Count; i++) 
		{
			
			if (IsPassAnTime (TheRunGameManager.Instance.StoreDefaultProperty.propertyDefaultData[i].hour, 
				TheRunGameManager.Instance.StoreDefaultProperty.propertyDefaultData[i].minute,
				TheRunGameManager.Instance.StoreDefaultProperty.propertyDefaultData[i].second,
				i))
			{
				if(TheRunGameManager.Instance.StoreDefaultProperty.propertyDefaultData[i].changepriceType == PropertyDefaultData.ChangePriceType.ChangeByRange)
				{
					var property = TheRunGameManager.Instance.StoreDefaultProperty.propertyDefaultData [i];
					property.currentPrice = (long)UnityEngine.Random.Range (property.minPrice, property.maxPrice);

					pricePerUnit = GetPrice (title.text);
				}
				else
				{
					var property = TheRunGameManager.Instance.StoreDefaultProperty.propertyDefaultData [i];

					if(property.currentPrice <= property.minPrice)
					{
						property.currentPrice = property.minPrice;
					}
					else
					{

						property.currentPrice += (long)(property.percentChange / 100 * property.maxPrice); 
						
						if (property.currentPrice > property.maxPrice) 
						{
							property.currentPrice = property.minPrice;
						}
					}
					pricePerUnit = GetPrice (title.text);
				}

			} 
			else 
			{
				pricePerUnit = GetPrice (title.text);

			}
		}
		InitializeText ();
	}

	long GetPrice(string name)
	{
		var property = TheRunGameManager.Instance.StoreDefaultProperty.GetPropertyDefaultDataRecord (name);

		if (property == null) return 0;

		return property.currentPrice;
	}

	bool IsPassAnTime(int _hour, int _minute, int _second, int i)
	{
		string time = TheRunGameManager.Instance.StoreDefaultProperty.propertyDefaultData[i].Time;

		if (string.IsNullOrEmpty(time)) 
		{
			time = GetCurrentTime ();
			TheRunGameManager.Instance.StoreDefaultProperty.propertyDefaultData[i].Time = time;
			return true;
		}

		hour = _hour;
		minute = _minute;
		second = _second;

		var currentTime = GetCurrentTime ();
		_startTime = TimeSpan.Parse (time);
		_endTime = TimeSpan.Parse (hour + ":" + minute + ":" + second);
		TimeSpan temp = TimeSpan.Parse (currentTime);
		TimeSpan diff = temp.Subtract (_startTime);
		var _remainingTime = _endTime.Subtract (diff);

		if(diff >= _endTime)
		{
			TheRunGameManager.Instance.StoreDefaultProperty.propertyDefaultData[i].Time = GetCurrentTime ();
			return true;
		} else
		{
			return false;
		}

	}

	string GetCurrentTime()
	{
		var _currentDate = DateTime.Now.ToString("d");
		var date = DateTime.Now.ToShortTimeString();
		DateTime temp;
		DateTime.TryParse (date, out temp);
		var _currentTime = temp.ToString ("HH:mm:ss");
		return _currentTime;
	}

	public void BuyOrSell()
	{
		if (transaction == Transaction.Buy) 
		{
			if (TheRunGameManager.Instance.GameData.Data.Profile.Money >= totalPrice) 
			{
				TheRunGameManager.Instance.GameData.Data.Profile.SubtractMoney (totalPrice);
				TheRunGameManager.Instance.GameData.Data.Profile.AddProperty (title.text, quantity, (long)totalPrice / quantity);
				UMP_Manager.Reference.UpdateMoneyText ();
				Result (true);
			}
			else
			{
				Result (false);
			}
		}
		else 
		{
			TheRunGameManager.Instance.GameData.Data.Profile.AddMoney (totalPrice);
			TheRunGameManager.Instance.GameData.Data.Profile.SubtractProperty (title.text, quantity);
			UMP_Manager.Reference.UpdateMoneyText ();
			Result (true);
		}
	}

	public void Confirmation()
	{
		if (transaction == Transaction.Buy) 
		{
			confirmationText.text = string.Format ("Are you sure of buying {0} unit(s) of {1} ?", quantity, title.text);
			buttonConfirmationText.text = "Buy";
		}
		else
		{
			confirmationText.text = string.Format ("Are you sure of selling {0} unit(s) of {1} ?", quantity, title.text);
			buttonConfirmationText.text = "Sell";
		}

		confirmationWindow.SetActive (true);
	}

	public void CloseWindow()
	{
		confirmationWindow.SetActive (false);
		resultWindow.SetActive (false);
	}

	void Result(bool success)
	{
		if(transaction == Transaction.Buy)
		{
			if(success)
			{
				resultText.text = "Purchase successful.";
			}
			else
			{
				resultText.text = "Purchase unsuccessful. You do not have enough cash";
			}
			resultWindow.SetActive (true);
		}
		else
		{
			resultText.text = "Successfully sold.";
			resultWindow.SetActive (true);
		}
	}


}
