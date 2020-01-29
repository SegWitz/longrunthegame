using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Property Data", menuName = "Data/Property")]
public class StoreDefaultProperty : ScriptableObject {

	public List<PropertyDefaultData> propertyDefaultData = new List<PropertyDefaultData> ();
	public PropertyDefaultData GetPropertyDefaultDataRecord(string name)
	{
		PropertyDefaultData record = null;

		foreach (var property in propertyDefaultData) 
		{
			if (property.propertyName == name) {
				record = property;
			}
		}

		return record;
	}

	public float GetPropertyDefaultDataPriceRecord(string name)
	{
		PropertyDefaultData record = null;

		foreach (var property in propertyDefaultData) 
		{
			if (property.propertyName == name) {
				record = property;
			}
		}

		return record.currentPrice;
	}
}
