using UnityEngine;

public class DeathPlane : MonoBehaviour
{
	public float moveDownDistance;

	public void MoveDown()
	{
		transform.Translate(Vector3.down * moveDownDistance);
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
			EmojiGameManager.Instance.playerController.Die();
		}
	}
}