using UnityEngine;

public class TrophiesInventory : Inventory
{
	TrophiesData _TrophiesData;

	protected override void OnEnable()
	{
		if (!HasBeenInitialized)
		{
			_TrophiesData = TheRunGameManager.Instance.TrophiesData;
			InstantiateCells(_TrophiesData.Data.Length);
		}

		base.OnEnable();
	}

	public override void LoadInventory()
	{
		base.LoadInventory();

		for (int c = 0; c < Cells.Length; c++)
		{
			Cells[c].SetSprite(_TrophiesData.Data[c].ItemSprite);
			Cells[c].SetQuantity(TheRunGameManager.Instance.GameData.Data.Profile.TrophiesQuantities[c]);
		}
	}
}