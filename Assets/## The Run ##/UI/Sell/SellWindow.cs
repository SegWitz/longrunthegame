using UnityEngine;
using UnityEngine.UI;

public class SellWindow : MonoBehaviour
{
	[SerializeField]
	UMP_Manager MenuManager = null;

	[Space]

	[SerializeField]
	Image ItemImage = null;
	[SerializeField]
	Text ValueText = null;

	SellPanelCell Cell;
	SellPanelCell.SellableItemType Type;
	int ItemIndex;
	long Total;

	public void Show(SellPanelCell Cell, SellPanelCell.SellableItemType Type, int ItemIndex)
	{
		this.Cell = Cell;
		this.Type = Type;
		this.ItemIndex = ItemIndex;

		if (Type == SellPanelCell.SellableItemType.Crystal)
		{
			int Quantity = TheRunGameManager.Instance.GameData.Data.Profile.CrystalsQuantities[ItemIndex];
			int Value = TheRunGameManager.Instance.CrystalsData.Data[ItemIndex].ItemValue;
			Total = Quantity * Value;

			ItemImage.sprite = TheRunGameManager.Instance.CrystalsData.Data[ItemIndex].ItemSprite;
			ValueText.text = string.Format("{0} x {1} = {2} JGD", Quantity, Globals.GetFormattedCurrency(Value, false), Globals.GetFormattedCurrency(Total, false));
		}
		else
		{
			int Quantity = TheRunGameManager.Instance.GameData.Data.Profile.PerksQuantities[ItemIndex];
			int Value = TheRunGameManager.Instance.PerksData.Data[ItemIndex].ItemValue;
			Total = Quantity * Value;

			ItemImage.sprite = TheRunGameManager.Instance.PerksData.Data[ItemIndex].ItemSprite;
			ValueText.text = string.Format("{0} x {1} = {2} JGD", Quantity, Globals.GetFormattedCurrency(Value, false), Globals.GetFormattedCurrency(Total, false));
		}

		gameObject.SetActive(true);
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}

	public void Sell()
	{
		if (Type == SellPanelCell.SellableItemType.Crystal)
		{
			TheRunGameManager.Instance.GameData.Data.Profile.CrystalsQuantities[ItemIndex] = 0;
		}
		else
		{
			TheRunGameManager.Instance.GameData.Data.Profile.PerksQuantities[ItemIndex] = 0;
		}

		Cell.SetQuantity(0);

		TheRunGameManager.Instance.GameData.Data.Profile.AddMoney(Total);
		TheRunGameManager.Instance.GameData.Save();

		MenuManager.UpdateMoneyText();

		Close();
	}
}