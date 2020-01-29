
using UnityEngine;
using System.Collections;

public class CameraMoveTempleRoadRace : MonoBehaviour {

	public float moveSpeed;
	public GameObject mainCamera;
	public bool bPlay;

	// Use this for initialization
	void Start () {
		bPlay = true;

		transform.position = new Vector3 (0, -0.8f, 0);
		mainCamera.transform.localPosition = new Vector3 ( 0, 3f, 0);
		mainCamera.transform.localRotation = Quaternion.Euler ( 10f, 0, 0 );
		moveSpeed = 6f;
	}
	
	// Update is called once per frame
	void Update () {

		
	}

	void FixedUpdate()
	{


		// Change View
		if (Input.GetKeyDown (KeyCode.Q)) {
			ChangeView01();
		}

		// Change View
		if (Input.GetKeyDown (KeyCode.W)) {
			ChangeView02();
		}


		// Cam Play
		if (Input.GetKeyDown (KeyCode.A)) {
			bPlay = true;		
		}

		// Cam Stop
		if (Input.GetKeyDown (KeyCode.S)) {
			bPlay = false;	
		}

		MoveObj ();
	}
	
	
	void MoveObj() {		
		if (bPlay == true) {
			float moveAmount = Time.smoothDeltaTime * moveSpeed;
			transform.Translate ( 0f, 0f, moveAmount );	
		}

	}



	void ChangeView01() {

		transform.position = new Vector3 (0, -0.8f, 0);
		mainCamera.transform.localPosition = new Vector3 ( 0, 3f, 0);
		mainCamera.transform.localRotation = Quaternion.Euler ( 10f, 0, 0 );
	

	}

	void ChangeView02() {
		transform.position = new Vector3 (0f, 5f, 0);
		mainCamera.transform.localPosition = new Vector3 ( 15f, 5f, 20f);
		mainCamera.transform.localRotation = Quaternion.Euler ( 20f, -90f, 0f );
	

	}
}























