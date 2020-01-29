using UnityEngine;

public class PerksInventory : Inventory
{
	PerksData _PerksData;

	protected override void OnEnable()
	{
		if (!HasBeenInitialized)
		{
			_PerksData = TheRunGameManager.Instance.PerksData;
			InstantiateCells(_PerksData.Data.Length);
		}

		base.OnEnable();
	}

	public override void LoadInventory()
	{
		base.LoadInventory();

		for (int c = 0; c < Cells.Length; c++)
		{
			Cells[c].SetSprite(_PerksData.Data[c].ItemSprite);
			Cells[c].SetQuantity(TheRunGameManager.Instance.GameData.Data.Profile.PerksQuantities[c]);
		}
	}
}