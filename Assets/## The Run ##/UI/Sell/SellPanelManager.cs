using UnityEngine;

public class SellPanelManager : MonoBehaviour
{
	[SerializeField]
	protected Transform CellsParent = null;
	[SerializeField]
	protected GameObject CellPrefab = null;
	[SerializeField]
	Animator AnimatorComponent = null;
	[SerializeField]
	SellWindow SellWindow = null;

	[Space]
	[SerializeField]
	bool IsShownByDefault = false;

	protected SellPanelCell[] Cells;

	int CrystalsLength;
	int PerksLength;

	bool HasBeenInitialized;

	void OnEnable()
	{
		if (!HasBeenInitialized)
		{
			if (IsShownByDefault) Show(true);

			CrystalsLength = TheRunGameManager.Instance.CrystalsData.Data.Length;
			PerksLength = TheRunGameManager.Instance.PerksData.Data.Length;

			InstantiateCells(CrystalsLength + PerksLength);

			HasBeenInitialized = true;
		}

		LoadInventory();
	}

	void InstantiateCells(int Instances)
	{
		Cells = new SellPanelCell[Instances];

		for (int c = 0; c < Instances; c++)
		{
			Cells[c] = Instantiate(CellPrefab).GetComponent<SellPanelCell>();
			Cells[c].transform.SetParent(CellsParent, false);
		}
	}

	public void LoadInventory()
	{
		for (int c = 0; c < CrystalsLength; c++)
		{
			Cells[c].Initialize(this, SellPanelCell.SellableItemType.Crystal, c, TheRunGameManager.Instance.CrystalsData.Data[c].ItemSprite, TheRunGameManager.Instance.GameData.Data.Profile.CrystalsQuantities[c]);
		}

		for (int c = CrystalsLength; c < CrystalsLength + PerksLength; c++)
		{
			Cells[c].Initialize(this, SellPanelCell.SellableItemType.Perk, c - CrystalsLength, TheRunGameManager.Instance.PerksData.Data[c - CrystalsLength].ItemSprite, TheRunGameManager.Instance.GameData.Data.Profile.PerksQuantities[c - CrystalsLength]);
		}
	}

	#region Sell Window

	public void ShowSellItemWindow(SellPanelCell Cell, SellPanelCell.SellableItemType Type, int ItemIndex)
	{
		SellWindow.Show(Cell, Type, ItemIndex);
	}

	#endregion

	#region Transitions

	int ShowID = Animator.StringToHash("Show");
	int HideID = Animator.StringToHash("Hide");

	public void Show(bool Value)
	{
		if (Value)
			AnimatorComponent.Play(ShowID);
		else
			AnimatorComponent.Play(HideID);
	}

	#endregion
}