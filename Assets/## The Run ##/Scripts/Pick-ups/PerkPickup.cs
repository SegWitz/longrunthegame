using UnityEngine;

public class PerkPickup : Pickup
{
	[Header("Perk Properties")]
	[SerializeField]
	PerkInfo.PerksIDs PerkID = PerkInfo.PerksIDs.Plate_Biohazard;

	protected override void Collect(GameManager PlayerGameManager)
	{
		TheRunGameManager.Instance.GameData.Data.Profile.PerksQuantities[(int)PerkID]++;

		base.Collect(PlayerGameManager);
	}
}