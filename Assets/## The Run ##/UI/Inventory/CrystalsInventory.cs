using UnityEngine;

public class CrystalsInventory : Inventory
{
	CrystalsData _CrystalsData;

	protected override void OnEnable()
	{
		if (!HasBeenInitialized)
		{
			_CrystalsData = TheRunGameManager.Instance.CrystalsData;
			InstantiateCells(_CrystalsData.Data.Length);
		}

		base.OnEnable();
	}

	public override void LoadInventory()
	{
		base.LoadInventory();

		for (int c = 0; c < Cells.Length; c++)
		{
			Cells[c].SetSprite(_CrystalsData.Data[c].ItemSprite);
			Cells[c].SetQuantity(TheRunGameManager.Instance.GameData.Data.Profile.CrystalsQuantities[c]);
		}
	}
}