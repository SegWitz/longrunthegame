using UnityEngine;

[CreateAssetMenu(fileName = "Crystals Data", menuName = "Data/Crystals Data")]
public class CrystalsData : ScriptableObject
{
	[SerializeField]
	CrystalInfo[] _CrystalsData = null;

	public CrystalInfo[] Data { get { return _CrystalsData; } }
}

[System.Serializable]
public class CrystalInfo
{
	public CrystalsIDs ItemID;
	public string ItemName;
	public int ItemValue;
	public Sprite ItemSprite;
	public GameObject PickupPrefab;

	public enum CrystalsIDs
	{
		Crystal_01_01, Crystal_01_02, Crystal_01_03,
		Crystal_02_01, Crystal_02_02, Crystal_02_03,
		Crystal_03_01, Crystal_03_02, Crystal_03_03,
		Crystal_04_01, Crystal_04_02, Crystal_04_03,
		Crystal_05_01, Crystal_05_02, Crystal_05_03,
		Crystal_06_01, Crystal_06_02, Crystal_06_03,
		Crystal_07_01, Crystal_07_02, Crystal_07_03,
		Crystal_08_01, Crystal_08_02, Crystal_08_03,
		Crystal_09_01, Crystal_09_02, Crystal_09_03,
		Crystal_10_01, Crystal_10_02, Crystal_10_03,
		Crystal_11_01, Crystal_11_02, Crystal_11_03,
		Crystal_12_01, Crystal_12_02, Crystal_12_03,
		Crystal_13_01, Crystal_13_02, Crystal_13_03,
		Crystal_14_01, Crystal_14_02, Crystal_14_03,
		Crystal_15_01, Crystal_15_02, Crystal_15_03
	}
}