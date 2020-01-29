using UnityEngine;

[CreateAssetMenu(fileName = "Money Data", menuName = "Data/Money Data")]
public class MoneyData : ScriptableObject
{
	[SerializeField]
	MoneyInfo[] _MoneyData = null;

	public MoneyInfo[] Data { get { return _MoneyData; } }

	public int GetIndexByID(MoneyInfo.MoneyIDs ID)
	{
		for (int i = 0; i < _MoneyData.Length; i++)
		{
			if (_MoneyData[i].ItemID == ID) return i;
		}

		return -1;
	}
}

[System.Serializable]
public class MoneyInfo
{
	public MoneyIDs ItemID;
	public string ItemName;
	public Sprite ItemSprite;
	public int ItemValue;
	public GameObject PickupPrefab;

	public enum MoneyIDs
	{
		JGD_100, JGD_400, JGD_1000
	}
}