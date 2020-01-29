using UnityEngine;
using UnityEngine.UI;

public class UMP_LevelInfo : MonoBehaviour
{
	public Text Title;
	public Text Description;
	public Image Preview;

	//Name of scene of build setting
	string LevelName;

	UMP_Manager MenuManager;

	void Start()
	{
		GameObject GO = GameObject.FindWithTag("MainMenuCanvas");
		MenuManager = GO.GetComponent<UMP_Manager>();
	}

	/// <summary>
	/// Level Info
	/// </summary>
	/// <param name="title"></param>
	/// <param name="desc"></param>
	/// <param name="preview"></param>
	/// <param name="scene"></param>
	public void SetInfo(string title, string desc, Sprite preview, string scene)
	{
		Title.text = title;
		Description.text = desc;
		Preview.sprite = preview;

		LevelName = scene;
	}

	public void OpenLevel()
	{
		if (TheRunGameManager.Instance.GameData.Data.Profile.PurpleHearts < 2)
		{
			MenuManager.NotEnoughPurpleHeartsWindow.SetActive(true);
		}
		else
		{
			TheRunGameManager.Instance.GameData.Data.Profile.PurpleHearts -= 2;
			TheRunGameManager.Instance.GameData.Save();
			Loading.LoadScene(LevelName);
		}
	}
}