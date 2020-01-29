using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField]
	Sprite baseSlotSprite = null;
	[SerializeField]
	Sprite activeSlotSprite = null;
	[SerializeField]
	Image slotImage = null;
	[SerializeField]
	Image itemImage = null;
	[SerializeField]
	Text quantityText = null;
	[SerializeField]
	PowerupsInfo.PowerupsIDs _ItemID = PowerupsInfo.PowerupsIDs.GreenArrows;

	public PowerupsInfo.PowerupsIDs ItemID { get { return _ItemID; } }
	public int Quantity
	{
		get { return TheRunGameManager.Instance.GameData.Data.Profile.PowerupsQuantities[(int)_ItemID]; }
		set { TheRunGameManager.Instance.GameData.Data.Profile.PowerupsQuantities[(int)_ItemID] = value; }
	}

	//PlayerInventory Owner;
	PickupManager Manager;

	public void Initialize(PlayerInventory Owner, PickupManager Manager)
	{
		//this.Owner = Owner;
		this.Manager = Manager;
		itemImage.sprite = TheRunGameManager.Instance.PowerupsData.Data[(int)_ItemID].ItemSprite;
		UpdateSlotUI();
	}

	public void AddItem()
	{
		Quantity++;
		UpdateSlotUI();
	}

	public void RemoveItem()
	{
		Quantity--;
		if (Quantity < 0) Quantity = 0;
		UpdateSlotUI();
	}

	void UpdateSlotUI()
	{
		itemImage.color = Quantity > 0 ? Color.white : new Color(1f, 1f, 1f, 0.5f);
		quantityText.text = Quantity.ToString();
	}

	void ToggleSlot(bool active)
	{
		if (active)
			slotImage.sprite = activeSlotSprite;
		else
			slotImage.sprite = baseSlotSprite;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		ToggleSlot(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		ToggleSlot(false);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (Quantity == 0) return;

		switch (_ItemID)
		{
			case PowerupsInfo.PowerupsIDs.GreenArrows:

				LevelManager.Instance.RemainingTime += 10;
				RemoveItem();
				break;

			case PowerupsInfo.PowerupsIDs.Magnet:

				Manager.InitializeMagnet();
				RemoveItem();
				break;

			case PowerupsInfo.PowerupsIDs.RedMedipack:

				GameManager.Instance.CurrentHealth += 50f;
				RemoveItem();
				break;

			case PowerupsInfo.PowerupsIDs.Shield:

				Manager.ActivateShields();
				RemoveItem();
				break;

			//case PowerupsInfo.PowerupsIDs.Ghost:

			//	Manager.ActivateInvisibility();
			//	RemoveItem();
			//	break;

			default:

				Debug.LogError("Not usable power-up!");
				break;
		}
	}
}