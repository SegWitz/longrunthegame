using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu (menuName = "RunningType/With Tilt")]
public class RunningType : Autorun {

	private bool isSwiping;
	private Vector2 startingPoint;

	public override void TypeRun(Transform obj)
	{
		base.TypeRun (obj);
		if (Input.touchCount == 1) {
			if (isSwiping) {
				Vector2 diff = Input.GetTouch(0).position - startingPoint;
				diff = new Vector2 (diff.x / Screen.width, diff.y / Screen.width);

				if (diff.magnitude > 0.01f) {
					if (Mathf.Abs (diff.x) > Mathf.Abs	(diff.y)) {
						if (diff.x < 0) {
							Turn (TurnDirections.Left,obj);
						} else {
							Turn (TurnDirections.Right,obj);
						}
					}
					isSwiping = false;
				}
			}
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				startingPoint = Input.GetTouch (0).position;
				isSwiping = true;
			} else if (Input.GetTouch (0).phase == TouchPhase.Ended) {
				isSwiping = false;
			}
		}

	}

	enum TurnDirections { Left, Right }

	void Turn(TurnDirections dir, Transform obj)
	{
		if (SceneManager.GetActiveScene().name != "Medieval City")
		{
			if (dir == TurnDirections.Right)
			{
				var angles = obj.rotation.eulerAngles;
				angles.y += 90;
				obj.rotation = Quaternion.Euler(angles);
			}
			else if (dir == TurnDirections.Left)
			{
				var angles = obj.rotation.eulerAngles;
				angles.y += -90;
				obj.rotation = Quaternion.Euler(angles);
			}
		}
	}
}
