using UnityEngine;

public class Shield : MonoBehaviour
{
	[SerializeField]
	Transform ShieldsTransform = null;
	[SerializeField]
	float RotationSpeed = 90f;
	[SerializeField]
	float ShieldDuration = 60f;

	GameManager Owner;

	float Timer;

	void Update()
	{
		ShieldsTransform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
		transform.position = Owner.transform.position;
		ProcessTimer();
	}

	public void InitializeShield(GameManager Owner)
	{
		this.Owner = Owner;
		float CharacterScale = Owner.transform.localScale.y;
		transform.localScale = new Vector3(CharacterScale, CharacterScale, CharacterScale);
		Enable(false);
	}

	public void Enable(bool Value)
	{
		if (Value)
		{
			Owner.SetInvincibility(ShieldDuration);
			Timer = ShieldDuration;
		}

		gameObject.SetActive(Value);
	}

	void ProcessTimer()
	{
		if (Timer > 0)
		{
			Timer -= Time.deltaTime;
			if (Timer <= 0)
			{
				Timer = 0;
				Enable(false);
			}
		}
	}
}