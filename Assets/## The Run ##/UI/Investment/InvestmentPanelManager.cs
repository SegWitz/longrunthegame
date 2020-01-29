using UnityEngine;
using UnityEngine.UI;

public class InvestmentPanelManager : MonoBehaviour
{
	[SerializeField]
	UMP_Manager Manager = null;

	[Space]
	[SerializeField]
	Animator AnimatorComponent = null;
	[SerializeField]
	Transform SchemesTogglesParent = null;
	[SerializeField]
	ToggleGroup SchemesTogglesGroup = null;

	[Space]
	[SerializeField]
	SchemesData SchemesData = null;
	[SerializeField]
	GameObject SchemeTogglePrefab = null;

	[Space]
	[SerializeField]
	Text BalanceValueText = null;
	[SerializeField]
	Text SelectedSchemeText = null;
	[SerializeField]
	Text TotalReturnText = null;
	[SerializeField]
	InputField InvestValueInput = null;
	[SerializeField]
	Text RoiText = null;
	[SerializeField]
	Text RiskText = null;

	[Space]
	[SerializeField]
	bool IsShownByDefault = false;

	[Space]
	[Header("Investment Error Window")]
	[SerializeField]
	GameObject ErrorWindow = null;
	[SerializeField]
	Text ErrorWindowMessageText = null;

	[Header("Investment Confirmation Window")]
	[SerializeField]
	GameObject ConfirmWindow = null;

	SchemeToggle[] SchemesToggles;

	InvestmentsManager Investments;

	int SelectedSchemeIndex;
	int SelectedDaysIndex;

	void Awake()
	{
		Investments = FindObjectOfType<InvestmentsManager>();
	}

	void Start()
	{
		if (IsShownByDefault) Show(true);

		int Instances = SchemesData.Data.Length;

		SchemesToggles = new SchemeToggle[Instances];

		for (int s = 0; s < Instances; s++)
		{
			SchemesToggles[s] = Instantiate(SchemeTogglePrefab).GetComponent<SchemeToggle>();
			SchemesToggles[s].transform.SetParent(SchemesTogglesParent, false);
			SchemesToggles[s].SetData(this, s, SchemesData.Data[s].SchemeName, SchemesTogglesGroup, s == 0);
		}
	}

	public void ChangeScheme(int Index)
	{
		SelectedSchemeIndex = Index;
		SelectedSchemeText.text = SchemesData.Data[Index].SchemeName;
		RoiText.text = string.Format("{0}% - {1}%", SchemesData.Data[Index].Roi1Day, SchemesData.Data[Index].Roi14Days);
		RiskText.text = SchemesData.Data[Index].InvestmentRisk.ToString();
		UpdateValues();
	}

	public void ChangeDays(int Index)
	{
		SelectedDaysIndex = Index;
		UpdateValues();
	}

	public void Invest()
	{
		long InvestValue = ProcessInvestValueString();

		if (InvestValue > 0 && InvestValue <= TheRunGameManager.Instance.GameData.Data.Profile.Money)
		{
			ShowConfirmWindow();
		}
		else
		{
			ShowErrorWindow(TheRunGameManager.Instance.GameData.Data.Profile.Money);
		}
	}

	void PlaceInvestment()
	{
		int Days;

		switch (SelectedDaysIndex)
		{
			default:
			case 0:
				Days = 1;
				break;
			case 1:
				Days = 3;
				break;
			case 2:
				Days = 6;
				break;
			case 3:
				Days = 14;
				break;
		}

		Investments.PlaceInvestment(ProcessInvestValueString(), CalculateReturn(), Days, SchemesData.Data[SelectedSchemeIndex].InvestmentRisk);
		Manager.UpdateMoneyText();
	}

	public void InvestValueChanged(string Value)
	{
		UpdateValues();
	}

	void UpdateValues()
	{
		BalanceValueText.text = Globals.GetFormattedCurrency(TheRunGameManager.Instance.GameData.Data.Profile.Money, false);
		TotalReturnText.text = Globals.GetFormattedCurrency(CalculateReturn(), false);
	}

	long CalculateReturn()
	{
		float Percentage;

		switch (SelectedDaysIndex)
		{
			default:
			case 0:
				Percentage = SchemesData.Data[SelectedSchemeIndex].Roi1Day;
				break;
			case 1:
				Percentage = SchemesData.Data[SelectedSchemeIndex].Roi3Days;
				break;
			case 2:
				Percentage = SchemesData.Data[SelectedSchemeIndex].Roi6Days;
				break;
			case 3:
				Percentage = SchemesData.Data[SelectedSchemeIndex].Roi14Days;
				break;
		}

		long InvestValue = ProcessInvestValueString();

		return InvestValue + (long)(InvestValue * Percentage / 100f);
	}

	long ProcessInvestValueString()
	{
		long InvestValue;

		if (InvestValueInput.text.Length == 0)
			InvestValue = 0;
		else
			InvestValue = long.Parse(InvestValueInput.text);

		return InvestValue;
	}

	#region Pop-up windows

	public void ShowErrorWindow(long MaxValue)
	{
		ErrorWindow.SetActive(true);
		ErrorWindowMessageText.text = string.Format("You must enter an investment value between 1 and {0}.", Globals.GetFormattedCurrency(MaxValue, false));
	}

	public void HideErrorWindow()
	{
		ErrorWindow.SetActive(false);
	}

	public void ShowConfirmWindow()
	{
		ConfirmWindow.SetActive(true);
	}

	public void HideConfirmWindow()
	{
		ConfirmWindow.SetActive(false);
	}

	public void ConfirmConfirmWindow()
	{
		ConfirmWindow.SetActive(false);
		PlaceInvestment();
	}

	#endregion

	#region Transitions

	int ShowID = Animator.StringToHash("Show");
	int HideID = Animator.StringToHash("Hide");

	public void Show(bool Value)
	{
		if (Value)
		{
			AnimatorComponent.Play(ShowID);
			UpdateValues();
		}
		else
		{
			AnimatorComponent.Play(HideID);
		}
	}

	#endregion
}