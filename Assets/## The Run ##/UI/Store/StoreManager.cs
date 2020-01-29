using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
	[SerializeField]
	StoreData Data = null;
	[SerializeField]
	GameObject ItemPrefab = null;
	[SerializeField]
	Transform ItemsParent = null;

	[Header("Confirmation Window")]
	[SerializeField]
	GameObject ConfirmationWindow = null;
	[SerializeField]
	Text ConfirmationText = null;

	[Header("Error Window")]
	[SerializeField]
	GameObject ErrorWindow = null;
	[SerializeField]
	Text ErrorText = null;

	UMP_Manager MenuManager;

	StorePowerupInfo ItemToBuy;

	void Start()
	{
		GameObject GO = GameObject.FindWithTag("MainMenuCanvas");
		MenuManager = GO.GetComponent<UMP_Manager>();

		InstantiateItemsPrefabs();
	}

	void InstantiateItemsPrefabs()
	{
		//Instantiate power-ups
		for (int p = 0; p < Data.StorePowerups.Length; p++)
		{
			GameObject GO = Instantiate(ItemPrefab);
			GO.transform.SetParent(ItemsParent, false);
			StoreItemCell Cell = GO.GetComponent<StoreItemCell>();
			Cell.Initialize(this, Data.StorePowerups[p]);
		}
	}

	public void SelectItem(StorePowerupInfo ItemInfo)
	{
		Debug.Log (ItemInfo.ItemID);
		if (TheRunGameManager.Instance.GameData.Data.Profile.RedStars < ItemInfo.Price)
		{
			Debug.Log ("cANT BUY");
			ErrorText.text = string.Format("You need at least {0} Red Stars in order to purchase this item.", ItemInfo.Price);
			ShowErrorWindowButtonClick(true);
		}
		else
		{
			Debug.Log ("CAN BUY");
			ItemToBuy = ItemInfo;

			ConfirmationText.text = string.Format("Are you sure you want to buy a \"{0}\" for {1} Red Stars?", TheRunGameManager.Instance.PowerupsData.Data[(int)ItemInfo.ItemID].ItemName, ItemInfo.Price);
			ShowConfirmationWindowButtonClick(true);
		}
	}

	public void ShowConfirmationWindowButtonClick(bool Show)
	{
		Debug.Log ("sHOWwINDOW");
		ConfirmationWindow.transform.parent.gameObject.SetActive (Show);
		ConfirmationWindow.SetActive(Show);
	}

	public void ShowErrorWindowButtonClick(bool Show)
	{
		ErrorWindow.transform.parent.gameObject.SetActive (Show);
		ErrorWindow.SetActive(Show);
	}

	public void PurchaseButtonClick()
	{
		TheRunGameManager.Instance.GameData.Data.Profile.RedStars -= ItemToBuy.Price;
		TheRunGameManager.Instance.GameData.Data.Profile.PowerupsQuantities[(int)ItemToBuy.ItemID]++;
		TheRunGameManager.Instance.GameData.Save();

		MenuManager.UpdateRedStarsText();

		ConfirmationWindow.transform.parent.gameObject.SetActive (false);
		ConfirmationWindow.SetActive(false);
	}
}