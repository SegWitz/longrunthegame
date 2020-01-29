using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SellPanelCell : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	Image ItemImage = null;
	[SerializeField]
	Text QuantityText = null;

	SellPanelManager Manager;

	public enum SellableItemType { Crystal, Perk }

	SellableItemType Type;
	int ItemIndex;
	int ItemQuantity;

	public void Initialize(SellPanelManager Manager, SellableItemType Type, int ItemIndex, Sprite ItemSprite, int ItemQuantity)
	{
		this.Manager = Manager;
		this.Type = Type;
		this.ItemIndex = ItemIndex;
		ItemImage.sprite = ItemSprite;
		SetQuantity(ItemQuantity);
	}

	public void SetQuantity(int ItemQuantity)
	{
		this.ItemQuantity = ItemQuantity;
		QuantityText.text = ItemQuantity.ToString("00");
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (ItemQuantity > 0) Manager.ShowSellItemWindow(this, Type, ItemIndex);
	}
}