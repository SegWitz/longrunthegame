using System;
using UnityEngine;

namespace SgLib
{
	public class CoinManager : MonoBehaviour
	{
		static CoinManager _Instance;
		public static CoinManager Instance
		{
			get
			{
				if (_Instance == null) _Instance = FindObjectOfType<CoinManager>();
				return _Instance;
			}
		}

		int _Coins;
		public int Coins
		{
			get { return _Coins; }
			private set { _Coins = value; }
		}

		public static event Action<int> CoinsUpdated = delegate { };


		void Start()
		{
			Reset();
		}

		public void Reset()
		{
			Coins = 0;
		}

		public void AddCoins(int amount)
		{
			Coins += amount;

			// Fire event
			if (CoinsUpdated != null) CoinsUpdated(Coins);
		}

		public void RemoveCoins(int amount)
		{
			Coins -= amount;

			// Fire event
			if (CoinsUpdated != null) CoinsUpdated(Coins);
		}
	}
}