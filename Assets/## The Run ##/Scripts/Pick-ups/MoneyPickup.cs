using UnityEngine;

public class MoneyPickup : Pickup
{
	[Header("Money Properties")]
	[SerializeField]
	int Value = 0;

	Transform PlayerTransform;

	Vector3 CurrentPosition;
	Vector3 MovementVelocity;
	float OriginalYPosition;

	protected override void OnStart()
	{
		base.OnStart();

		CurrentPosition = transform.position;
		OriginalYPosition = CurrentPosition.y;
		PlayerTransform = GameManager.Instance.transform;
	}

	protected override void Collect(GameManager PlayerGameManager)
	{
		PlayerGameManager.CurrentMoney += Value;

		base.Collect(PlayerGameManager);
	}

	protected override void Process()
	{
		base.Process();

		if (Manager.IsMagnetEnabled)
		{
			if (Vector3.Distance(PlayerTransform.position, CurrentPosition) < 8 * Manager.CharacterScale)
			{
				CurrentPosition = Vector3.SmoothDamp(CurrentPosition, PlayerTransform.position, ref MovementVelocity, 0.2f, 20f);
				transform.position = new Vector3(CurrentPosition.x, OriginalYPosition, CurrentPosition.z);
			}
		}
	}
}