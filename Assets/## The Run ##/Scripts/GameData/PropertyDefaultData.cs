using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PropertyDefaultData 
{
	public enum ChangePriceType
	{
		ChangeByRange,
		ChangeByPercent
	}
	public string propertyName;
	public long minPrice;
	public long maxPrice;
	public long currentPrice;
	public ChangePriceType changepriceType;
	public int hour = 0;
	public int minute = 0;
	public int second = 0;
	public float percentChange = 0.0f;

	private string time = null;
	public string Time
	{
		get { return time; }
		set { time = value; }
	}
}
