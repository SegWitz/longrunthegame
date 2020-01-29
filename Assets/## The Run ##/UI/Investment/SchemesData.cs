using UnityEngine;

[CreateAssetMenu(fileName = "Schemes Data", menuName = "Data/Schemes Data")]
public class SchemesData : ScriptableObject
{
	[SerializeField]
	SchemeInfo[] _SchemesData = null;
	[SerializeField]
	RiskInfo[] _RisksData = null;

	public SchemeInfo[] Data { get { return _SchemesData; } }
	public RiskInfo[] RisksData { get { return _RisksData; } }
}

[System.Serializable]
public class SchemeInfo
{
	public string SchemeName;
	public float Roi1Day;
	public float Roi3Days;
	public float Roi6Days;
	public float Roi14Days;
	public RiskTypes InvestmentRisk;

	public enum RiskTypes { Lowest, Low, LowerMedium, UpperMedium, High, Highest }
}

[System.Serializable]
public class RiskInfo
{
	public float Probability;
}