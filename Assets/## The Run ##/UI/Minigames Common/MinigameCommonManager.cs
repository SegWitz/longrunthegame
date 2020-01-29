using UnityEngine;
using UnityEngine.UI;

public class MinigameCommonManager : MonoBehaviour
{
	[SerializeField]
	GameObject ExitConfirmationWindow = null;
	[SerializeField]
	Button BackToMenuButton = null;

	void Start()
	{
		ExitConfirmationWindow.SetActive(false);
	}

	public void BackToMenu()
	{
		ExitConfirmationWindow.SetActive(true);
	}

	public void ShowBackToMenuButton(bool Value)
	{
		BackToMenuButton.gameObject.SetActive(Value);
	}

	public void ExitConfirmationYes()
	{
		Loading.LoadScene("Main Menu");
	}

	public void ExitConfirmationNo()
	{
		ExitConfirmationWindow.SetActive(false);
	}
}