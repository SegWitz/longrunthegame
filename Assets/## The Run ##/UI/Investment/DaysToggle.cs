using UnityEngine;

public class DaysToggle : MonoBehaviour
{
	[SerializeField]
	InvestmentPanelManager Manager = null;
	[SerializeField]
	int Index = 0;

	public void DaysSelect(bool Value)
	{
		if (Value)
		{
			Manager.ChangeDays(Index);
		}
	}
}