using System;
using UnityEngine;

namespace SgLib
{
	public class ScoreManager : MonoBehaviour
	{
		static ScoreManager _Instance;
		public static ScoreManager Instance
		{
			get
			{
				if (_Instance == null) _Instance = FindObjectOfType<ScoreManager>();
				return _Instance;
			}
		}

		public int Score { get; private set; }

		public int HighScore { get; private set; }

		public bool HasNewHighScore { get; private set; }

		public static event Action<int> ScoreUpdated = delegate { };
		public static event Action<int> HighscoreUpdated = delegate { };

		void Start()
		{
			Reset();
		}

		public void Reset()
		{
			// Initialize score
			Score = 0;

			// Initialize highscore
			HighScore = TheRunGameManager.Instance.GameData.Data.Profile.EmojiData.HighScore;
			HasNewHighScore = false;
		}

		public void AddScore(int amount)
		{
			Score += amount;

			// Fire event
			if (ScoreUpdated != null) ScoreUpdated(Score);

			if (Score > HighScore)
			{
				UpdateHighScore(Score);
				HasNewHighScore = true;
			}
			else
			{
				HasNewHighScore = false;
			}
		}

		public void UpdateHighScore(int newHighScore)
		{
			// Update highscore if player has made a new one
			if (newHighScore > HighScore)
			{
				HighScore = newHighScore;

				TheRunGameManager.Instance.GameData.Data.Profile.EmojiData.HighScore = HighScore;
				TheRunGameManager.Instance.GameData.Save();

				if (HighscoreUpdated != null) HighscoreUpdated(HighScore);
			}
		}
	}
}