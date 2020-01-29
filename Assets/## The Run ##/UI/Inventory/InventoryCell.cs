using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
	[SerializeField]
	Image ItemImage = null;
	[SerializeField]
	Text QuantityText = null;

	public void SetSprite(Sprite sprite)
	{
		ItemImage.sprite = sprite;
	}

	public void SetQuantity(int quantity)
	{
		QuantityText.text = quantity.ToString("00");
	}
}