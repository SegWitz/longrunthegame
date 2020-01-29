using UnityEngine;
using System.Collections;
using SgLib;

public class PlayerController : MonoBehaviour
{
    public PlayerAnim anim;

    public static event System.Action PlayerDied;

    void OnEnable()
    {
		EmojiGameManager.GameStateChanged += OnGameStateChanged;
    }

    void OnDisable()
    {
		EmojiGameManager.GameStateChanged -= OnGameStateChanged;
    }

    void Start()
    {
		//TODO: Set sprite
		//GetComponentInChildren<SpriteRenderer>().sprite = selectedCharacter.sprite;
    }

    // Listens to changes in game state
    void OnGameStateChanged(GameState newState, GameState oldState)
    {
        if (newState == GameState.Playing)
        {
            GetComponentInChildren<SpriteRenderer>().enabled = true;
            GetComponentInChildren<Rigidbody2D>().gravityScale = EmojiGameManager.Instance.playerGravityScale;
        }
    }

    // Calls this when the player dies and game over
    public void Die()
    {
        // Fire event
        PlayerDied();
    }
}
