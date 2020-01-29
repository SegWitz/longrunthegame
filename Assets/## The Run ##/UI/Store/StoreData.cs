using UnityEngine;

[CreateAssetMenu(fileName = "Store Data", menuName = "Store Data")]
public class StoreData : ScriptableObject
{
	[SerializeField]
	StorePowerupInfo[] storePowerups = null;

	public StorePowerupInfo[] StorePowerups { get { return storePowerups; } }
}

[System.Serializable]
public class StoreItemInfo
{
	public int Price;
}

[System.Serializable]
public class StorePowerupInfo : StoreItemInfo
{
	public PowerupsInfo.PowerupsIDs ItemID;
}