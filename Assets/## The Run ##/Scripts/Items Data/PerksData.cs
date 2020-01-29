using UnityEngine;

[CreateAssetMenu(fileName = "Perks Data", menuName = "Data/Perks Data")]
public class PerksData : ScriptableObject
{
	[SerializeField]
	PerkInfo[] _PerksData = null;

	public PerkInfo[] Data { get { return _PerksData; } }
}

[System.Serializable]
public class PerkInfo
{
	public PerksIDs ItemID;
	public string ItemName;
	public int ItemValue;
	public Sprite ItemSprite;
	public GameObject PickupPrefab;

	[Space]
	public int StLRewardLevel;
	public int EmojiRewardLevel;

	public enum PerksIDs
	{
		Plate_Biohazard, Plate_Coincidence, Plate_Fire, Plate_Lightning, Plate_Overkill, Plate_Protection, Plate_Radioactive
	}
}