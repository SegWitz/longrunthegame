using UnityEngine;

public class PowerupPickup : Pickup
{
	[Header("Power-up Properties")]
	[SerializeField]
	PowerupsInfo.PowerupsIDs PowerupID = PowerupsInfo.PowerupsIDs.GreenArrows;

	PlayerInventory Inventory;

	protected override void OnStart()
	{
		base.OnStart();

		GameObject GO = GameObject.FindGameObjectWithTag("HUDCanvas");
		Inventory = GO.GetComponent<HUD>().Inventory;
	}

	protected override void Collect(GameManager PlayerGameManager)
	{
		switch (PowerupID)
		{
			case PowerupsInfo.PowerupsIDs.GreenArrows:

				Inventory.AddItem(PowerupsInfo.PowerupsIDs.GreenArrows);
				break;

			case PowerupsInfo.PowerupsIDs.Magnet:

				Inventory.AddItem(PowerupsInfo.PowerupsIDs.Magnet);
				break;

			case PowerupsInfo.PowerupsIDs.PurpleHeart:

				TheRunGameManager.Instance.GameData.Data.Profile.PurpleHearts++;
				break;

			case PowerupsInfo.PowerupsIDs.PurplePoison:

				PlayerGameManager.CauseDamage(75f, false);
				PlayerGameManager.CurrentMoney -= (int)(PlayerGameManager.CurrentMoney * 0.15f);
				break;

			case PowerupsInfo.PowerupsIDs.RedBomb:
			case PowerupsInfo.PowerupsIDs.RedSkull:

				PlayerGameManager.CauseDamage(50f, false);
				PlayerGameManager.CurrentMoney -= (int)(PlayerGameManager.CurrentMoney * 0.2f);
				break;

			case PowerupsInfo.PowerupsIDs.RedMedipack:

				Inventory.AddItem(PowerupsInfo.PowerupsIDs.RedMedipack);
				break;

			case PowerupsInfo.PowerupsIDs.RedStar:

				TheRunGameManager.Instance.GameData.Data.Profile.RedStars++;
				TheRunGameManager.Instance.GameData.Save();
				break;

			default:

				Debug.LogError("Power-up not implemented yet: " + PowerupID.ToString());
				break;
		}

		base.Collect(PlayerGameManager);
	}
}