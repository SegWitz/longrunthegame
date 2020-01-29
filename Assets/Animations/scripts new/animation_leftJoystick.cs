using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation_leftJoystick : MonoBehaviour {

	public Animator Anim;
	public static animation_leftJoystick ins;

	public void Awake(){
		ins = this;
	}
	public void _idle()
	{
		Anim.ResetTrigger ("JogForward");
		Anim.ResetTrigger ("RunForward");
		Anim.SetTrigger ("isDeactivated");
		Anim.SetTrigger ("IdleState");
//		Anim.ResetTrigger ("isDeactivated");

	}

	public void _jog(){  
		Anim.ResetTrigger ("RunForward");
		Anim.ResetTrigger ("IdleState");
		Anim.SetTrigger ("isDeactivated");
		Anim.SetTrigger ("JogForward");
//		Anim.ResetTrigger ("isDeactivated");
 	}

	public void _run(){
 		Anim.ResetTrigger ("JogForward");
		Anim.ResetTrigger ("IdleState");
 		Anim.SetTrigger ("isDeactivated");
		Anim.SetTrigger ("RunForward");
//		Anim.ResetTrigger ("isDeactivated");
 	}
	 
}
