using AppAdvisory.StopTheLock;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StLErrorWindow : MonoBehaviour
{
	[SerializeField]
	StLGameManager Manager = null;
	[SerializeField]
	Text MessageText = null;
	[SerializeField]
	Text TimeText = null;

	public void Show(double ElapsedTime)
	{
		MessageText.text = "You can only play 5 matches within an hour. If you want to keep playing, please wait a little bit or watch an ad.\n\nTime remaining:";
		TimeText.text = (60 - Math.Floor(ElapsedTime)) + " minutes.";

		gameObject.SetActive(true);
	}

	public void Hide()
	{
		Manager.ResetGame();
		gameObject.SetActive(false);
	}

	public void WatchAd()
	{

	}

	public void ExitGame()
	{
		Loading.LoadScene("Main Menu");
	}
}