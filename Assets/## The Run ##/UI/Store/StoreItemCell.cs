using UnityEngine;
using UnityEngine.UI;

public class StoreItemCell : MonoBehaviour
{
	[SerializeField]
	Image ItemImage = null;
	[SerializeField]
	Text PriceText = null;

	StoreManager Manager;
	StorePowerupInfo ItemInfo;

	void Start()
	{

	}

	public void Initialize(StoreManager Manager, StorePowerupInfo ItemInfo)
	{
		this.Manager = Manager;

		this.ItemInfo = ItemInfo;
		ItemImage.sprite = TheRunGameManager.Instance.PowerupsData.Data[(int)ItemInfo.ItemID].ItemSprite;
		PriceText.text = ItemInfo.Price.ToString();
	}


	public void SelectButtonClick()
	{
		Manager.SelectItem(ItemInfo);
	}
}