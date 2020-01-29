using UnityEngine;

public class BettingPanelManager : MonoBehaviour
{
	[SerializeField]
	Animator AnimatorComponent = null;
	[SerializeField]
	BettingWindow BettingWindow = null;

	[Space]
	[SerializeField]
	bool IsShownByDefault = false;

	enum BettingGames { BowAndArrow, Polygonix, DiceGame }
	BettingGames SelectedGame;

	UMP_Manager MenuManager;

	void Start()
	{
		if (IsShownByDefault) Show(true);

		GameObject GO = GameObject.FindWithTag("MainMenuCanvas");
		MenuManager = GO.GetComponent<UMP_Manager>();
	}

	public void ShowGame()
	{
		TheRunGameManager.Instance.GameData.Data.Profile.PurpleHearts -= 2;
		TheRunGameManager.Instance.GameData.Save();

		switch (SelectedGame)
		{
			case BettingGames.BowAndArrow:

				Loading.LoadScene("BowAndArrow");
				break;

			case BettingGames.Polygonix:

				Loading.LoadScene("Polygonix");
				break;

			case BettingGames.DiceGame:

				Loading.LoadScene("DiceGame");
				break;

			default:

				break;
		}
	}

	#region Betting Window

	public void ShowBettingWindow(int GameIndex)
	{
		if (TheRunGameManager.Instance.GameData.Data.Profile.PurpleHearts < 2)
		{
			MenuManager.NotEnoughPurpleHeartsWindow.SetActive(true);
		}
		else
		{
			SelectedGame = (BettingGames)GameIndex;
			BettingWindow.Show();
		}
	}

	#endregion

	#region Transitions

	int ShowID = Animator.StringToHash("Show");
	int HideID = Animator.StringToHash("Hide");

	public void Show(bool Value)
	{
		if (Value)
			AnimatorComponent.Play(ShowID);
		else
			AnimatorComponent.Play(HideID);
	}

	#endregion
}