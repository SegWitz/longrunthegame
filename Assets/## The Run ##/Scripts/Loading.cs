using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
	[SerializeField]
	Image LoadingBarFGImage = null;
	[SerializeField]
	RawImage BGRawImage = null;
	[SerializeField]
	Texture2D[] Backgrounds = null;

	AsyncOperation async;

	float MaxBarLength;

	void Awake()
	{
		MaxBarLength = LoadingBarFGImage.rectTransform.rect.width;

		SetBG();
		
	}

	IEnumerator Start()
	{
		Application.backgroundLoadingPriority = ThreadPriority.Low;

		async = SceneManager.LoadSceneAsync(NextScene);

		yield return async;
	}



	void Update()
	{
		if (async != null) LoadingBarFGImage.rectTransform.sizeDelta = new Vector2(async.progress * MaxBarLength, 0);
	}

	void SetBG()
	{
		switch (NextScene)
		{
			case "Desert Road Map":
				BGRawImage.texture = Backgrounds[0];
				break;
			case "Bridges":
				BGRawImage.texture = Backgrounds[1];
				break;
			case "Cartoon City Maps":
				BGRawImage.texture = Backgrounds[2];
				break;
			case "Medieval City":
				BGRawImage.texture = Backgrounds[3];
				break;
			case "Temple Road":
				BGRawImage.texture = Backgrounds[4];
				break;
			default:
				BGRawImage.enabled = false;
				break;
		}
	}
	#region Load Scene

	static string NextScene;

	public static void LoadScene(string SceneName)
	{
		NextScene = SceneName;
		SceneManager.LoadScene("Loading");
	}

	#endregion
}