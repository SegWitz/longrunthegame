
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/




using UnityEngine;
using System.Collections;

namespace AppAdvisory.StopTheLock
{
	public class Player : MonoBehaviourHelper
	{
		public float speedStart = 5f;

		public bool firstMove;

		float time = 2.7f;

		int direction = 1;

		Vector2 currentDotPosition
		{
			get 
			{
				return dotPosition.GetPosition ();
			}
		}

		[SerializeField] private Transform playerTransform;

		public Transform GetTransform()
		{
			return playerTransform;
		}

		public float GetRotation()
		{
			return transform.eulerAngles.z;
		}

		void Awake()
		{
			firstMove = true;
			direction = 1;
		}

		void Update()
		{
			if (!gameManager.gameIsStarted || gameManager.isGameOver || gameManager.isSuccess) 
			{
				firstMove = true;
				StopAllCoroutines ();
				return;
			}

			if (Input.GetMouseButtonDown (0) && !gameManager.isGameOver && !gameManager.isSuccess) 
			{
				if (Input.mousePosition.y > Screen.height * 0.9)
					return;


				if (firstMove) 
				{
					if (dotPosition.isLeftOfScreen())
						direction = -1;
					else
						direction = +1;

					StartTheMove ();

					firstMove = false;
					return;
				}

				Vector2 myPos = playerTransform.position;

				float dist = Vector2.Distance (currentDotPosition, myPos);

				if (dist <= dotPosition.GetDotSize()) //32
				{
					gameManager.MoveDone ();

					dotPosition.GetDotTransform ().localScale = Vector2.zero;

					if (gameManager.isSuccess)
						return;

					dotPosition.DoPosition ();

					StartTheMove ();
				} 
				else 
				{
					gameManager.GameOver ();
				}
			}
		}

		void StartTheMove()
		{
			if (gameManager.isSuccess) 
			{
				StopAllCoroutines ();
				return;
			}

			direction *= -1;

			StopAllCoroutines ();
			StartCoroutine (_StartTheMove());
		}

		IEnumerator _StartTheMove()
		{
			bool isAfter = false;

			while (!gameManager.isGameOver) 
			{
				float t0 = transform.rotation.eulerAngles.z;
				float t1 = transform.rotation.eulerAngles.z + direction * 360f;
				float timer = 0;

				while (timer <= time)
				{
					timer += Time.deltaTime;

					float f = Mathf.Lerp (t0, t1, timer / time);

					Vector3 rot = Vector3.forward * f;

					transform.eulerAngles = rot;

					Vector2 myPos = playerTransform.position;

					if (!isAfter) 
					{
						float dist = Vector2.Distance (currentDotPosition, myPos);
						if (dist <= dotPosition.GetDotSize()) //10
						{
							isAfter = true;
						}
					}

					if (isAfter) 
					{
						float dist = Vector2.Distance (currentDotPosition, myPos);

						if(dist > dotPosition.GetDotSize()) //20
						{
							if (!gameManager.isSuccess) 
							{
								gameManager.GameOver ();
							}
							break;
						}
					}

					if (gameManager.isGameOver)
						break;

					yield return null;
				}
			}
		}
	}
}