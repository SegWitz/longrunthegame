using System.Collections;
using UnityEngine;

namespace SgLib
{
	public static class Utilities
	{
		public static IEnumerator CRWaitForRealSeconds(float time)
		{
			float start = Time.realtimeSinceStartup;

			while (Time.realtimeSinceStartup < start + time)
			{
				yield return null;
			}
		}

		public static void ButtonClickSound()
		{
			SoundManager.Instance.PlaySound(SoundManager.Instance.button);
		}

		public static int[] GenerateShuffleIndices(int length)
		{
			int[] array = new int[length];

			// Populate array
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = i;
			}

			// Shuffle
			for (int j = 0; j < array.Length; j++)
			{
				int tmp = array[j];
				int randomPos = Random.Range(j, array.Length);
				array[j] = array[randomPos];
				array[randomPos] = tmp;
			}

			return array;
		}
	}
}