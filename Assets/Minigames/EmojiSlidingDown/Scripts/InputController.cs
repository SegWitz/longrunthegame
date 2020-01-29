using UnityEngine;

public class InputController : MonoBehaviour
{
	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (EmojiGameManager.Instance.GameState == GameState.Playing)
				EmojiGameManager.Instance.rotationDirection = EmojiGameManager.Instance.rotationDirection == 0 ? EmojiGameManager.Instance.firstRotationDirection : -EmojiGameManager.Instance.rotationDirection;
		}
	}

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
			EmojiGameManager.Instance.rotationDirection = 0;
		}
	}
}