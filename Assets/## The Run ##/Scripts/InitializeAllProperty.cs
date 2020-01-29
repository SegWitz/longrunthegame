using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeAllProperty : MonoBehaviour {

	public CheckOwnedPropertQuantity[] checkOwnedPropertQuantity;

	void OnEnable()
	{
		for (int i = 0; i < checkOwnedPropertQuantity.Length; i++) 
		{
			checkOwnedPropertQuantity [i].enabled = true;
			checkOwnedPropertQuantity [i].Check ();
		}
	}

	void OnDisable()
	{
		for (int i = 0; i < checkOwnedPropertQuantity.Length; i++) 
		{
			checkOwnedPropertQuantity [i].enabled = false;
		}
	}
}
