using UnityEngine;
using System.Collections;
using SgLib;

public class CloudController : MonoBehaviour
{
    public GameObject spriteObject;
    public bool noObstacle;
    public float obstacleDistance;
    public GameObject obstacle;
    public GameObject coin;
    public GameObject cloudEffect;
    public float distanceToDisable;

    public bool contactFlag;

    [Header("Animation")]
    public CloudAnim anim;

    public void Start()
    {
        bool hasObstacle = Random.Range(0f, 1f) < EmojiGameManager.Instance.obstacleFrequency && !noObstacle ? true : false;
        obstacle.SetActive(hasObstacle);
        obstacle.transform.Translate(0, obstacleDistance, 0, Space.Self);
    }

    public void Update()
    {
        Rotate();
        CheckDisable();
    }

    public void Rotate()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, EmojiGameManager.Instance.rotationDirection * Mathf.Abs(EmojiGameManager.Instance.maxRotatingAngle));
        transform.root.rotation = Quaternion.RotateTowards(transform.root.rotation, targetRotation, EmojiGameManager.Instance.rotationDelta);
    }

    public void CheckDisable()
    {
        Vector3 playerPos = EmojiGameManager.Instance.playerController.transform.position;
        float sqrDistance = (transform.position - playerPos).sqrMagnitude;
        if (EmojiGameManager.Instance.GameState == GameState.Playing && sqrDistance >= distanceToDisable * distanceToDisable && transform.position.y > playerPos.y)
        {
            Spawner.Instance.RemoveClouds(gameObject);
            Destroy(transform.root.gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Player" && contactFlag == false)
        {
            contactFlag = true;
            cloudEffect.transform.position = col.contacts[0].point;
            cloudEffect.SetActive(true);

            //add score, camera shake, sound...
            anim.Bounce();
			EmojiGameManager.Instance.playerController.anim.Squeeze();
            Spawner.Instance.BunkSpawn(Random.Range(1, 3));
            ScoreManager.Instance.AddScore(1);
            CameraController.Instance.ShakeCamera();
            SoundManager.Instance.PlaySound(SoundManager.Instance.cloudHit);
        }
    }

    public void SetActiveCoin(bool active)
    {
        coin.gameObject.SetActive(active);
    }
}
