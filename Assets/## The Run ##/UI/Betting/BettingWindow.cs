using UnityEngine;
using UnityEngine.UI;

public class BettingWindow : MonoBehaviour
{
	[SerializeField]
	BettingPanelManager BettingManager = null;
	[SerializeField]
	GameObject ErrorWindow = null;
	[SerializeField]
	Text ErrorText = null;
	[SerializeField]
	InputField ValueInput = null;

	int AmountToBet;

	public void Show()
	{
		ValueInput.text = string.Empty;
		gameObject.SetActive(true);
		ValueInput.Select();
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}

	public void Bet()
	{
		AmountToBet = int.Parse(ValueInput.text);

		if (AmountToBet <= 0 || AmountToBet > TheRunGameManager.Instance.GameData.Data.Profile.Money)
		{
			ShowErrorWindow();
			return;
		}

		TheRunGameManager.Instance.BetAmount = AmountToBet;

		BettingManager.ShowGame();

		Close();
	}

	void ShowErrorWindow()
	{
		ErrorWindow.SetActive(true);

		if (TheRunGameManager.Instance.GameData.Data.Profile.Money == 0)
			ErrorText.text = "You don't have any money to bet.";
		else
			ErrorText.text = string.Format("Please enter a value between 1 and {0}.", Globals.GetFormattedCurrency(TheRunGameManager.Instance.GameData.Data.Profile.Money, true));
	}

	public void CloseErrorWindow()
	{
		ErrorWindow.SetActive(false);
	}
}