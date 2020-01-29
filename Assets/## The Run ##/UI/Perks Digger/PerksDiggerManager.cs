using UnityEngine;

public class PerksDiggerManager : MonoBehaviour
{
	[SerializeField]
	Animator AnimatorComponent = null;

	[Space]
	[SerializeField]
	bool IsShownByDefault = false;

	UMP_Manager MenuManager;

	void Start()
	{
		if (IsShownByDefault) Show(true);

		GameObject GO = GameObject.FindWithTag("MainMenuCanvas");
		MenuManager = GO.GetComponent<UMP_Manager>();
	}

	public void ShowStopTheLock()
	{
		if (!CheckPurpleHearts()) return;
		Loading.LoadScene("StopTheLock");
	}

	public void ShowEmojiGame()
	{
		if (!CheckPurpleHearts()) return;
		Loading.LoadScene("EmojiGame");
	}

	bool CheckPurpleHearts()
	{
		if (TheRunGameManager.Instance.GameData.Data.Profile.PurpleHearts < 2)
		{
			MenuManager.NotEnoughPurpleHeartsWindow.SetActive(true);
			return false;
		}
		else
		{
			TheRunGameManager.Instance.GameData.Data.Profile.PurpleHearts -= 2;
			TheRunGameManager.Instance.GameData.Save();
			return true;
		}
	}

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