using UnityEngine;
using UnityEngine.UI;

public class SchemeToggle : MonoBehaviour
{
	[SerializeField]
	Text NameText = null;
	[SerializeField]
	Toggle Toggle;

	InvestmentPanelManager Manager;
	int SchemeIndex;

	public void SetData(InvestmentPanelManager Manager, int SchemeIndex, string Name, ToggleGroup Group, bool IsOn)
	{
		this.Manager = Manager;
		this.SchemeIndex = SchemeIndex;
		NameText.text = Name;
		Toggle.group = Group;
		Toggle.isOn = IsOn;
	}

	public void SchemeSelect(bool Value)
	{
		if (Value)
		{
			Manager.ChangeScheme(SchemeIndex);
		}
	}
}