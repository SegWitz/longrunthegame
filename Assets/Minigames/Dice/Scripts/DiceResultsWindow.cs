using UnityEngine;
using UnityEngine.UI;

public class DiceResultsWindow : MonoBehaviour
{
	[SerializeField]
	DiceGameManager Manager = null;
	[SerializeField]
	Text MessageText = null;
	[SerializeField]
	Text ScoreText = null;

	[Header("Trophy Window")]
	[SerializeField]
	GameObject TrophyWindow = null;
	[SerializeField] Image TrophyRewardImage = null;

	public void Show(int Result)
	{
		long Amount;

		if (Result > 0)
		{
			Amount = TheRunGameManager.Instance.BetAmount * Result;
			TheRunGameManager.Instance.GameData.Data.Profile.DiceData.BetsWon++;
			TheRunGameManager.Instance.GameData.Data.Profile.AddMoney(Amount);
			TheRunGameManager.Instance.GameData.Save();
			MessageText.text = string.Format("Congratulations! You've scored {0} times a value equal or greater than 35.\n\nYou have won:", Result);
			ScoreText.text = Globals.GetFormattedCurrency(Amount, true);
		}
		else
		{
			MessageText.text = string.Format("Too bad! You've scored {0} times a value equal or greater than 35.\n\nYou have lost:", Result);
			ScoreText.text = Globals.GetFormattedCurrency(TheRunGameManager.Instance.BetAmount, true);
		}

		gameObject.SetActive(true);
	}

	public void Hide()
	{
		if (TheRunGameManager.Instance.GameData.Data.Profile.DiceData.BetsWon >= 10)
		{
			TheRunGameManager.Instance.GameData.Data.Profile.DiceData.BetsWon = 0;
			TheRunGameManager.Instance.GameData.Save();

			ShowTrophyWindow();
		}
		else
		{
			Manager.DiceGameReset();
			Manager.MinigameCommon.ShowBackToMenuButton(true);
		}

		gameObject.SetActive(false);
	}

	public void Menu()
	{
		Loading.LoadScene("Main Menu");
	}

	#region Rewards

	void ShowTrophyWindow()
	{
		TrophyRewardImage.sprite = TheRunGameManager.Instance.TrophiesData.Data[5].ItemSprite;
		TheRunGameManager.Instance.GameData.Data.Profile.TrophiesQuantities[5]++;
		TheRunGameManager.Instance.GameData.Save();

		TrophyWindow.SetActive(true);
	}

	public void CloseTrophyWindow()
	{
		TrophyWindow.SetActive(false);
		Manager.DiceGameReset();
		Manager.MinigameCommon.ShowBackToMenuButton(true);
	}

	#endregion
}