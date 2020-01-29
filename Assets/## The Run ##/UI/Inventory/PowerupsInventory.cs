using UnityEngine;

public class PowerupsInventory : Inventory
{
	PowerupsData _PowerupsData;

	protected override void OnEnable()
	{
		if (!HasBeenInitialized)
		{
			_PowerupsData = TheRunGameManager.Instance.PowerupsData;
			InstantiateCells(_PowerupsData.Data.Length);
		}

		base.OnEnable();
	}

	public override void LoadInventory()
	{
		base.LoadInventory();

		for (int c = 0; c < Cells.Length; c++)
		{
			Cells[c].SetSprite(_PowerupsData.Data[c].ItemSprite);
			Cells[c].SetQuantity(TheRunGameManager.Instance.GameData.Data.Profile.PowerupsQuantities[c]);
		}
	}
}