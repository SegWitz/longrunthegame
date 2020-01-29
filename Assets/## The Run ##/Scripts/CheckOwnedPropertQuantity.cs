using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckOwnedPropertQuantity : MonoBehaviour {

	private UMP_Property property;

	void Awake()
	{
		property = FindObjectOfType<UMP_Property> ();
	}
	

	 public void Check ()
	{
		if (property.transaction == UMP_Property.Transaction.Sell && !HasProperty ()) 
		{
			GetComponent<Button> ().interactable = false;
		}
		else if (property.transaction == UMP_Property.Transaction.Buy)
		{
			GetComponent<Button> ().interactable = true;
		}
		Debug.Log (property.transaction);
	}

	bool HasProperty()
	{
		string name = gameObject.name;

		Debug.Log (gameObject.name);

		foreach (var property in TheRunGameManager.Instance.GameData.Data.Profile.PropertyOwned) 
		{
			if (property.propertyName == name && property.propertyQuantity > 0) 
			{
				return true;
			}
		}

		return false;
	}
}
