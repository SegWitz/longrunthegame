using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	private Transform target = null;

	[SerializeField]
	private Vector3 offsetPosition = Vector3.zero;

	[SerializeField]
	private Space offsetPositionSpace = Space.Self;

	[SerializeField]
	private bool lookAt = true;

	Vector3 Position;
	Vector3 TargetPosition;

	Vector3 CurrentVelocity = Vector3.zero;
	Vector3 CurrentTargetVelocity = Vector3.zero;

	void Start()
	{
		TargetPosition = target.position;

		if (offsetPositionSpace == Space.Self)
			Position = target.TransformPoint(offsetPosition);
		else
			Position = target.position + offsetPosition;
	}

	void LateUpdate()
	{
		Refresh();
	}

	public void Refresh()
	{
		if (target == null)
		{
			Debug.LogWarning("Missing target.", this);
			return;
		}

		TargetPosition = Vector3.SmoothDamp(TargetPosition, target.position, ref CurrentTargetVelocity, 0.25f);

		if (offsetPositionSpace == Space.Self)
		{
			Position = Vector3.SmoothDamp(Position, target.TransformPoint(offsetPosition), ref CurrentVelocity, 0.25f);
		}
		else
		{
			Position = Vector3.SmoothDamp(Position, target.position + offsetPosition, ref CurrentVelocity, 0.25f);
		}

		transform.position = Position;

		if (lookAt)
		{
			transform.LookAt(TargetPosition);
		}
		else
		{
			transform.position = target.position;
		}
	}
}

/*public GameObject CameraGoalLookAt;
public GameObject CameraGoalPosition;
public Vector3 CameraCurrentLookAt;

public float MovementDividor;
public float LookAtDividor;

void LateUpdate() {
	// Camera Movement
	Vector3 movement = CameraGoalPosition - transform.position;
	movement = movement / MovementDividor;
	transform.position += movement;

	// Camera Look At
	Vector3 movementA = CameraGoalLookAt - CameraCurrentLookAt;
	movementA = movementA / LookAtDividor;
	CameraCurrentLookAt += movementA;
	transform.lookat(CameraCurrentLookAt);

	// If you dont' want the camera to have a smooth panning / turning one, then just replace the camera look at code with:
	//  transform.lookt(CameraGoalLookAt);
}*/

