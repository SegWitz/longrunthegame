using UnityEngine;

[CreateAssetMenu(fileName = "Power-ups Data", menuName = "Data/Power-ups Data")]
public class PowerupsData : ScriptableObject
{
	[SerializeField]
	PowerupsInfo[] _PowerupsData = null;

	public PowerupsInfo[] Data { get { return _PowerupsData; } }
}

[System.Serializable]
public class PowerupsInfo
{
	public PowerupsIDs ItemID;
	public string ItemName;
	//public int ItemValue;
	public Sprite ItemSprite;
	public GameObject PickupPrefab;

	public enum PowerupsIDs
	{
		GreenArrows, Magnet, PurpleHeart, PurplePoison, RedBomb, RedMedipack, RedSkull, RedStar, Shield
	}
}