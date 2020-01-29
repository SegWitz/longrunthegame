using UnityEngine;

[CreateAssetMenu(fileName = "Trophies Data", menuName = "Data/Trophies Data")]
public class TrophiesData : ScriptableObject
{
	[SerializeField]
	TrophyInfo[] _TrophiesData = null;

	public TrophyInfo[] Data { get { return _TrophiesData; } }
}

[System.Serializable]
public class TrophyInfo
{
	public Trophies ItemID;
	public string ItemName;
	public Sprite ItemSprite;

	public enum Trophies { Trophy_01, Trophy_02, Trophy_03, Trophy_04, Trophy_05, Trophy_06, Trophy_07, Trophy_08 }
}