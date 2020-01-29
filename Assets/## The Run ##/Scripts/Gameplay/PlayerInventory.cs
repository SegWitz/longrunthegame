using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	[SerializeField]
	PlayerInventorySlot[] Slots = null;

	void Start()
	{
		InitializeSlots();
	}

	void InitializeSlots()
	{
		PickupManager Manager = FindObjectOfType<PickupManager>();

		for (int s = 0; s < Slots.Length; s++)
		{
			Slots[s].Initialize(this, Manager);
		}
	}

	public void AddItem(PowerupsInfo.PowerupsIDs ID)
	{
		for (int s = 0; s < Slots.Length; s++)
		{
			if (Slots[s].ItemID == ID)
			{
				Slots[s].AddItem();
				break;
			}
		}
	}
}