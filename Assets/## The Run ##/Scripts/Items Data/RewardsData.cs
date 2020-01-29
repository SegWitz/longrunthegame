using UnityEngine;

[CreateAssetMenu(fileName = "Rewards Data", menuName = "Data/Rewards Data")]
public class RewardsData : ScriptableObject
{
	[SerializeField]
	RewardsInfo[] _RewardsInfo = null;

	public RewardsInfo[] Data { get { return _RewardsInfo; } }
}

[System.Serializable]
public class RewardsInfo
{
	public Rewards ItemID;
	public string ItemName;
	public Sprite ItemSprite;

	public enum Rewards { Box_01, Box_02, Card_01, Card_03 }
}