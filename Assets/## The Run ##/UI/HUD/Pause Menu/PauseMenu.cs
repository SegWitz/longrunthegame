using RewardMobSDK;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	[SerializeField]
	GameObject PauseMenuGO = null;
	[SerializeField]
	GameObject EndGameMenuGO = null;
	[SerializeField]
	GameObject DeathBackgroundGO = null;
	[SerializeField]
	GameObject TimeUpBackgroundGO = null;
	[SerializeField]
	GameObject MusicMenuGO = null;
	[SerializeField]
	GameObject NotEnoughHeartGO = null;

	static PauseMenu _Instance;
	public static PauseMenu Instance
	{
		get
		{
			if (_Instance == null) _Instance = FindObjectOfType<PauseMenu>();
			return _Instance;
		}
	}

	#region End Game

	RewardMob RMInstance;

	public void ShowDeathMenu()
	{
		RMInstance = RewardMob.instance;

		EndGameMenuGO.SetActive(true);
		DeathBackgroundGO.SetActive(true);
		TimeUpBackgroundGO.SetActive(false);
		if (RMInstance != null) RMInstance.ShowButton();
		StartCoroutine(ShowAd());
	}

	public void ShowTimeUpMenu()
	{
		EndGameMenuGO.SetActive(true);
		DeathBackgroundGO.SetActive(false);
		TimeUpBackgroundGO.SetActive(true);
		if (RMInstance != null) RMInstance.ShowButton();
		StartCoroutine(ShowAd());
	}

	public void HideDeathMenu()
	{
		
		if (RMInstance != null) RMInstance.HideButton();
		EndGameMenuGO.SetActive(false);
		GameManager.Instance.Respawn();
		LevelManager.Instance.StartLevel();
	}

	public void GiveUp()
	{
		BackToMainMenu();
	}

	#endregion

	#region Pause

	public void ShowPauseMenu()
	{
		Time.timeScale = 0;
		LevelManager.Instance.IsPaused = true;
		PauseMenuGO.SetActive(true);
		StartCoroutine(ShowAd());
	}

	public void Resume()
	{
		HidePauseMenu();
	}

	public void Respawn()
	{
		//	GameManager.Instance.Respawn();
				HidePauseMenu();
		//		LevelManager.Instance.StartLevel();

		string scene = SceneManager.GetActiveScene().name;
		if (TheRunGameManager.Instance.GameData.Data.Profile.PurpleHearts < 2)
		{
			NotEnoughHeartGO.SetActive(true);
		}
		else
		{
			TheRunGameManager.Instance.GameData.Data.Profile.PurpleHearts -= 2;
			TheRunGameManager.Instance.GameData.Save();
			Loading.LoadScene(scene);
		}
	}

	public void QuitToMenu()
	{
		LevelManager.Instance.FinishLevel(LevelManager.FinishingLevelReason.PlayerDecision);
		BackToMainMenu();
	}

	void HidePauseMenu()
	{
		Time.timeScale = 1;
		LevelManager.Instance.IsPaused = false;
		PauseMenuGO.SetActive(false);
	}

	#endregion

	#region Music

	public void ShowMusicMenu()
	{
		MusicMenuGO.SetActive(true);
	}

	public void HideMusicMenu()
	{
		MusicMenuGO.SetActive(false);
	}

	#endregion

	void BackToMainMenu()
	{
		Time.timeScale = 1;
		Loading.LoadScene("Main Menu");
	}

	IEnumerator ShowAd()
	{
		yield return new WaitForSeconds(0.2f);
		
	}
}