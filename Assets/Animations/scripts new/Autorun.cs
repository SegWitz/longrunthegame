using UnityEngine;

[CreateAssetMenu (menuName = "RunningType/NoTilt")]
public class Autorun : ScriptableObject {

	public Vector3 currentAcceleration, initialAcceleration;
	public float sensitivity = 0f;
	public float smooth = 0.4f;
	private float newRotation;
	public float speedRun = 0;
	public float maxSpeedRun = 0;

	void OnEnable()
	{
		//if(speedRun > maxSpeedRun)
		speedRun = maxSpeedRun;
	}

	public virtual void TypeRun (Transform obj)
	{
		currentAcceleration = Vector3.Lerp (currentAcceleration, Input.acceleration - initialAcceleration, Time.deltaTime / smooth);
		newRotation = Mathf.Clamp (currentAcceleration.x * sensitivity, -1, 1);
		Vector3 newDir = new Vector3 (newRotation, obj.transform.position.y, speedRun);
		obj.transform.Translate (newDir * Time.fixedDeltaTime);
	}

}
