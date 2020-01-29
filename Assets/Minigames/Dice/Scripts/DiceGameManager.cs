/**
 * Copyright (c) 2010-2015, WyrmTale Games and Game Components
 * All rights reserved.
 * http://www.wyrmtale.com
 *
 * THIS SOFTWARE IS PROVIDED BY WYRMTALE GAMES AND GAME COMPONENTS 'AS IS' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL WYRMTALE GAMES AND GAME COMPONENTS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DiceGameManager : MonoBehaviour
{
	[SerializeField]
	Transform spawnPoint = null;
	[SerializeField]
	Material[] DiceMaterials = null;

	[Space]
	[SerializeField]
	Text ResultsText = null;
	[SerializeField]
	Button ThrowButton = null;
	[SerializeField]
	DiceResultsWindow ResultsWindow = null;
	[SerializeField]
	DiceErrorWindow ErrorWindow = null;

	[Space]
	[SerializeField]
	float ThrowForce = 1f;
	[SerializeField]
	float RollWaitTime = 2;
	[SerializeField]
	int ThrowsPerMatch = 10;

	Dice dice;

	public MinigameCommonManager MinigameCommon { get; private set; }

	int CurrentDiceRoll;
	bool IsRolling;
	float RollWaitTimer;
	int[] Results;

	int FinalScore;

	bool HasPlacedBet;

	void Awake()
	{
		dice = GetComponent<Dice>();
		MinigameCommon = FindObjectOfType<MinigameCommonManager>();
	}

	void Start()
	{
		HasPlacedBet = false;
		DiceGameReset();
	}

	void Update()
	{
		ResultsText.text = dice.GetResultsString();

		if (IsRolling)
		{
			RollWaitTimer -= Time.deltaTime;
			if (RollWaitTimer <= 0)
			{
				IsRolling = false;
				ThrowButton.gameObject.SetActive(true);
				Results[CurrentDiceRoll] = Dice.Value();
				CurrentDiceRoll++;
				if (CurrentDiceRoll >= ThrowsPerMatch)
				{
					ShowFinalResultsWindow();
				}
			}
		}
	}

	public void DiceGameReset()
	{
		double ElapsedTime = (DateTime.Now - TheRunGameManager.Instance.GameData.Data.Profile.DiceData.LastTimePlayed).TotalMinutes;
		if (ElapsedTime >= 60)
		{
			TheRunGameManager.Instance.GameData.Data.Profile.DiceData.MatchesPlayedWithinLimit = 0;
			TheRunGameManager.Instance.GameData.Data.Profile.DiceData.LastTimePlayed = DateTime.Now;
		}
		else
		{
			if (TheRunGameManager.Instance.GameData.Data.Profile.DiceData.MatchesPlayedWithinLimit >= 3)
			{
				//TheRunGameManager.Instance.GameData.Data.Profile.DiceGame.MatchesPlayedWithinLimit = 3;
				ErrorWindow.Show(ElapsedTime);
				return;
			}
		}

		if (!HasPlacedBet)
		{
			TheRunGameManager.Instance.GameData.Data.Profile.SubtractMoney(TheRunGameManager.Instance.BetAmount);
			HasPlacedBet = true;
		}

		TheRunGameManager.Instance.GameData.Data.Profile.DiceData.MatchesPlayedWithinLimit++;
		TheRunGameManager.Instance.GameData.Save();

		CurrentDiceRoll = 0;
		IsRolling = false;
		Results = new int[ThrowsPerMatch];
		ThrowButton.gameObject.SetActive(true);
	}

	Vector3 Force()
	{
		Vector3 rollTarget = new Vector3(2 + 7 * Random.value, .5F + 4 * Random.value, -2 - 3 * Random.value);
		return rollTarget.normalized * (-35 - Random.value * 20) * ThrowForce;
	}

	void ShowFinalResultsWindow()
	{
		MinigameCommon.ShowBackToMenuButton(false);
		CalculateFinalScore();
		ResultsWindow.Show(FinalScore);
	}

	void CalculateFinalScore()
	{
		FinalScore = 0;

		for (int n = 0; n < Results.Length; n++)
		{
			if (Results[n] >= 35) FinalScore++;
		}
	}

	public void ThrowDice()
	{
		if (IsRolling) return;

		dice.Clear();

		dice.Roll(DiceMaterials[0], spawnPoint.position, Force());
		dice.Roll(DiceMaterials[1], spawnPoint.position, Force());
		dice.Roll(DiceMaterials[2], spawnPoint.position, Force());
		dice.Roll(DiceMaterials[3], spawnPoint.position, Force());
		dice.Roll(DiceMaterials[4], spawnPoint.position, Force());
		dice.Roll(DiceMaterials[5], spawnPoint.position, Force());
		dice.Roll(DiceMaterials[6], spawnPoint.position, Force());
		dice.Roll(DiceMaterials[7], spawnPoint.position, Force());

		IsRolling = true;
		ThrowButton.gameObject.SetActive(false);
		RollWaitTimer = RollWaitTime;
	}
}