using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leftJoystick : MonoBehaviour {
	public float temp;
	public string movement;
	public static leftJoystick ins;
	//--if upDown is true, joystick is moved upward, if it is false joystick is idle or downward.--
	public bool upOrDown;
	//-- if runOrJog is true, player runs else jogs.--
	public bool runOrJog;
	void Awake(){
		ins = this;
	}
 	// Use this for initialization
	void Start () {
 		//temp = UltimateJoystick.ins.tempValueY;
		upOrDown = false;
		runOrJog = false;
 
	}
	
	// Update is called once per frame
	void Update () {
		//temp = UltimateJoystick.ins.tempValueY;

		if (temp <= 0) {
			upOrDown = false;
//			print (upOrDown);
//			print ("idle");
			movement="idle";
 		}

		else if (temp > 0 && temp <= 0.5f) {
			upOrDown = true;
			runOrJog = false;
//			print (upOrDown);
//			print (runOrJog);
//			print ("up");
			movement="jog";
  		}

		else if (temp > 0.5) {
			runOrJog = true;
			upOrDown = true;
//			print (runOrJog);
//			print (upOrDown);
//			print ("up and run");
			movement="run";
		}

 	}
}
