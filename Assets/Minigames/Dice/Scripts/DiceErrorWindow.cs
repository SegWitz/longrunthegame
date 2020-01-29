using System;
using UnityEngine;
using UnityEngine.UI;

public class DiceErrorWindow : MonoBehaviour
{
	[SerializeField]
	DiceGameManager Manager = null;
	[SerializeField]
	Text MessageText = null;
	[SerializeField]
	Text TimeText = null;

	public void Show(double ElapsedTime)
	{
		MessageText.text = "You can only play 3 matches within an hour. If you want to keep playing, please wait a little bit or watch an ad.\n\nTime remaining:";
		TimeText.text = (60 - Math.Floor(ElapsedTime)) + " minutes.";

		Manager.MinigameCommon.ShowBackToMenuButton(false);

		gameObject.SetActive(true);
	}

	public void Hide()
	{
		Manager.DiceGameReset();
		Manager.MinigameCommon.ShowBackToMenuButton(true);
		gameObject.SetActive(false);
	}

	public void WatchAd()
	{

	}

	public void ExitDiceGame()
	{
		Loading.LoadScene("Main Menu");
	}
}