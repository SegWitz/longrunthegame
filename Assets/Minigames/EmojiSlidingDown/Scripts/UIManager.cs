using SgLib;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[Header("Object References")]
	public GameObject header;
	public GameObject title;
	public Text score;
	public Text bestScore;
	public Text coinText;
	public GameObject tapToStart;
	public GameObject RestartButton;
	public GameObject ExitButton;

	Animator scoreAnimator;

	void OnEnable()
	{
		EmojiGameManager.GameStateChanged += GameManager_GameStateChanged;
		ScoreManager.ScoreUpdated += OnScoreUpdated;
	}

	void OnDisable()
	{
		EmojiGameManager.GameStateChanged -= GameManager_GameStateChanged;
		ScoreManager.ScoreUpdated -= OnScoreUpdated;
	}

	// Use this for initialization
	void Start()
	{
		scoreAnimator = score.GetComponent<Animator>();
		Reset();
		ShowStartUI();
	}

	// Update is called once per frame
	void Update()
	{
		score.text = ScoreManager.Instance.Score.ToString();
		bestScore.text = ScoreManager.Instance.HighScore.ToString();
		coinText.text = CoinManager.Instance.Coins.ToString();
	}

	void GameManager_GameStateChanged(GameState newState, GameState oldState)
	{
		if (newState == GameState.Playing)
		{
			ShowGameUI();
		}
		else if (newState == GameState.PreGameOver)
		{
			// Before game over, i.e. game potentially will be recovered
		}
		else if (newState == GameState.GameOver)
		{
			Invoke("ShowGameOverUI", 1f);
		}
	}

	void OnScoreUpdated(int newScore)
	{
		scoreAnimator.Play("NewScore");
	}

	void Reset()
	{
		header.SetActive(false);
		title.SetActive(false);
		score.gameObject.SetActive(false);
		tapToStart.SetActive(false);
		RestartButton.SetActive(false);
		ExitButton.SetActive(false);
	}

	public void StartGame()
	{
		EmojiGameManager.Instance.StartGame();
	}

	public void EndGame()
	{
		EmojiGameManager.Instance.GameOver();
	}

	public void RestartGame()
	{
		EmojiGameManager.Instance.RestartGame(0.2f);
	}

	public void ExitGame()
	{
		Loading.LoadScene("Main Menu");
	}

	public void ShowStartUI()
	{
		header.SetActive(true);
		title.SetActive(true);
		tapToStart.SetActive(true);
	}

	public void ShowGameUI()
	{
		header.SetActive(true);
		title.SetActive(false);
		score.gameObject.SetActive(true);
		tapToStart.SetActive(false);
		RestartButton.SetActive(false);
		ExitButton.SetActive(false);
	}

	public void ShowGameOverUI()
	{
		header.SetActive(true);
		title.SetActive(false);
		score.gameObject.SetActive(true);
		tapToStart.SetActive(false);
		RestartButton.SetActive(true);
		ExitButton.SetActive(true);
	}

	public void ButtonClickSound()
	{
		Utilities.ButtonClickSound();
	}
}
