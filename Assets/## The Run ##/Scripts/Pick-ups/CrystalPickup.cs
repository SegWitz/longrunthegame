using UnityEngine;

public class CrystalPickup : Pickup
{
	[Header("Crystal Properties")]
	[SerializeField]
	CrystalInfo.CrystalsIDs CrystalID = CrystalInfo.CrystalsIDs.Crystal_01_01;

	protected override void Collect(GameManager PlayerGameManager)
	{
		TheRunGameManager.Instance.GameData.Data.Profile.CrystalsQuantities[(int)CrystalID]++;

		base.Collect(PlayerGameManager);
	}
}