using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJM_changed : MonoBehaviour {
	public string oldMove, newMove;
	// Use this for initialization
	void Start () {
		oldMove="idle";
	}
	
	// Update is called once per frame
	void Update () {
		newMove = leftJoystick.ins.movement;
		if (oldMove == newMove)
			return;
		else if (newMove == "idle") {
			print ("Go idle");
			oldMove = newMove;
			StopAllCoroutines ();
			StartCoroutine ("idleState");
		}
		else if (newMove == "jog") {
			print (" start joging  ");
			oldMove = newMove;
			StopAllCoroutines ();
			StartCoroutine ("joggingStarted");
		}
		else if (newMove == "run") {
			print ("start runing");
			oldMove = newMove;
			StopAllCoroutines ();
			StartCoroutine ("runningStarted");
 		}
 	}

	public IEnumerator joggingStarted()
	{
		// do the animation stuff here--for Jogging--
		animation_leftJoystick.ins._jog();
		yield return null;
	}
	public IEnumerator runningStarted()
	{
		// do the animation stuff here--for Running--
		animation_leftJoystick.ins._run();

		yield return null;

	}
	public IEnumerator idleState()
	{
		// do the animation stuff here-- for Idle State--
		animation_leftJoystick.ins._idle();

		yield return null;

	}
}
