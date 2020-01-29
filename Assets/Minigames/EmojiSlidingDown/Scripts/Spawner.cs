using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	static Spawner _Instance;
	public static Spawner Instance
	{
		get
		{
			if (_Instance == null) _Instance = FindObjectOfType<Spawner>();
			return _Instance;
		}
	}

	List<GameObject> clouds;

	//-1 for left, 0 for center, 1 for right
	float sign;

	public void OnEnable()
	{
		EmojiGameManager.GameStateChanged += OnGameStateChanged;
	}

	public void OnDisable()
	{
		EmojiGameManager.GameStateChanged -= OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState newState, GameState oldState)
	{
		if (oldState == GameState.Prepare && newState == GameState.Playing)
		{
			clouds = new List<GameObject>();
			BunkSpawn(5);
		}
	}

	public void RemoveClouds(GameObject cloud)
	{
		clouds.Remove(cloud);
	}

	public void SpawnCloud()
	{
		Vector3 nextPos;
		bool contactFlag = false;
		if (clouds.Count == 0)
		{
			nextPos = new Vector3(0, EmojiGameManager.Instance.cloudInititalPosY, 0);
			sign = 0;
			contactFlag = true;
		}
		else
		{
			Vector3 lastPos = clouds[clouds.Count - 1].transform.position;
			nextPos = new Vector3(0, lastPos.y - EmojiGameManager.Instance.cloudVerticleOffset, 0);
			if (sign == -1 || sign == 1)
				sign = 0;
			else
				sign = Mathf.Sign(Random.Range(-10, 10));
		}
		GameObject g = Instantiate(EmojiGameManager.Instance.cloudPrefab.gameObject, nextPos, Quaternion.identity) as GameObject;

		g.transform.GetChild(0).Translate(new Vector3(sign * EmojiGameManager.Instance.cloudHorizontalOffset, 0, 0));
		CloudController controller = g.GetComponentInChildren<CloudController>();
		controller.contactFlag = contactFlag;
		bool hasCoin = Random.Range(0f, 1f) < EmojiGameManager.Instance.coinFrequency && !contactFlag ? true : false;
		controller.SetActiveCoin(hasCoin);
		if (clouds.Count == 0)
			controller.noObstacle = true;
		if (sign == 0)
		{
			Vector3 translation = new Vector3(Mathf.Sign(Random.Range(-10, 10)) * EmojiGameManager.Instance.cloudHorizontalOffset, 0, 0);
			controller.obstacle.transform.Translate(translation);

		}
		clouds.Add(g);

		if (clouds.Count == 2)
			EmojiGameManager.Instance.firstRotationDirection = -sign;
		EmojiGameManager.Instance.playerDeathPlane.MoveDown();

	}

	public void BunkSpawn(int quantity)
	{
		for (int i = 0; i < quantity; ++i)
		{
			SpawnCloud();
		}
	}

	public void OnDrawGizmos()
	{
		if (EmojiGameManager.Instance != null)
		{
			float camPosY = Camera.main.transform.position.y;
			float length = Camera.main.orthographicSize;
			Gizmos.DrawRay(new Vector3(-EmojiGameManager.Instance.cloudHorizontalOffset, camPosY, 0), Vector3.up * length);
			Gizmos.DrawRay(new Vector3(-EmojiGameManager.Instance.cloudHorizontalOffset, camPosY, 0), Vector3.down * length);

			Gizmos.DrawRay(new Vector3(0, camPosY, 0), Vector3.up * length);
			Gizmos.DrawRay(new Vector3(0, camPosY, 0), Vector3.down * length);

			Gizmos.DrawRay(new Vector3(EmojiGameManager.Instance.cloudHorizontalOffset, camPosY, 0), Vector3.up * length);
			Gizmos.DrawRay(new Vector3(EmojiGameManager.Instance.cloudHorizontalOffset, camPosY, 0), Vector3.down * length);
		}
	}

}