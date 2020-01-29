using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class AnimationBlending : MonoBehaviour
{
	//Serialized fields
	public Animator _blends;
	public UltimateJoystick leftJoyStick;
	public AnimationClip jumpStC, jumpLtC, jumpRtC, slideStC, slideLtC, slideRtC, avoidLtC, avoidRtC;
	public GameManager manager;

	bool jumpSt, jumpLt, jumpRt, slideSt, slideLt, slideRt, avoidLt, avoidRt, animInCall;

	int AnimatorVelocityX_ID, AnimatorVelocityZ_ID;

	public Vector3 MovementDirection { get; private set; }
	Vector3 MovementVelocity;
	public Vector3 ActionDirection { get; private set; }
	float ActionAngle;
	private bool canTurn;
	private bool isSwiping;
	private Vector2 startingPoint;
	private Rigidbody rigid;
	private Vector3 currentAcceleration, initialAcceleration;
	private float newRotation;
	[SerializeField] private float jumpForce = 0f;
	public Autorun autorun;
	private bool isRunning = true;
	public float speed = 0.0f;
	public bool isDeath {
		get {
			return GameManager.Instance.CurrentHealth <= 0;
		}
	}

    public float sensitivity { get; private set; }
    public float smooth { get; private set; }
	public static AnimationBlending Instance;

	private float smoothing = 1.0f;
	private Vector3 targetPos;

	public Transform[] path;

	private float percentPerSec = 0.2f;
	private float currentPathPercent = 0.0f;
	private Transform playerContainer;
	private bool _assist;

    void Awake()
	{
		AnimatorVelocityX_ID = Animator.StringToHash("velocityx");
		AnimatorVelocityZ_ID = Animator.StringToHash("velocityz");
	}

	void Start()
	{
		rigid = GetComponent<Rigidbody>();
		speed = autorun.speedRun;
		if (Instance == null) Instance = this;
//		PlayerPrefs.SetInt ("movementassist",0);
		if(!PlayerPrefs.HasKey(UMPKeys.Controller) || PlayerPrefs.GetInt(UMPKeys.Controller) == 0)
        {
			GameObject.Find ("Ultimate UI Canvas").SetActive (false);
			_blends.SetBool("assist", true);
			isRunning = true;
			_assist = true;
			_blends.applyRootMotion = false;
			GameObject[] col = GameObject.FindGameObjectsWithTag ("Trap");
			foreach (var _col in col) {
				var t = _col.GetComponent<BoxCollider> ();
				if (t != null)
					t.isTrigger = true;
			}
			GameObject[] col2 = GameObject.FindGameObjectsWithTag ("Enemy");
			foreach (var _col in col2) {
				var t = _col.GetComponent<BoxCollider> ();
				if (t != null)
					t.isTrigger = true;
			}

        }
		else if(PlayerPrefs.GetInt("movementassist") == 1)
        {
			GameObject.Find ("Ultimate UI Canvas").SetActive (true);
			_blends.SetBool("assist", false);
			isRunning = false;
			_assist = false;
			_blends.applyRootMotion = true;

        }


	}

	public void FixedUpdate()
	{
		if (_assist) {
			
			if (isRunning && !isDeath)
				autorun.TypeRun (this.gameObject.transform);
			
			if (!isRunning) {
				Mathf.Lerp (autorun.speedRun, 0, Time.deltaTime);

			} else if (isRunning) {
				//Mathf.Lerp (autorun.speedRun, autorun.maxSpeedRun, Time.deltaTime);
			}
			if (SceneManager.GetActiveScene ().name == "Medieval City") {
				FollowPath ();
			}
		} else {
//			Debug.Log ();
			//Get touch movement direction
			Vector3 TouchMovementDirection = new Vector3 (UltimateJoystick.GetPosition ("Movement").x, 0, UltimateJoystick.GetPosition ("Movement").y);

			//Get gamepad movement direction
			Vector3 GamepadMovementDirection = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0);

			//Decide touch or gamepad movement and smooth result
			if (GamepadMovementDirection.magnitude >= TouchMovementDirection.magnitude)
				MovementDirection = Vector3.SmoothDamp (MovementDirection, GamepadMovementDirection, ref MovementVelocity, .2f);
			else
				MovementDirection = Vector3.SmoothDamp (MovementDirection, TouchMovementDirection, ref MovementVelocity, .2f);

			//Get touch action direction
//		Vector3 TouchActionDirection = rightJoyStick.joyStick;

			//Get gamepad action direction
//		Vector3 GamepadActionDirection = new Vector3(Input.GetAxis("ActionHorizontal"), Input.GetAxis("ActionVertical"), 0);

			//Decide touch or gamepad action
//		if (GamepadActionDirection.magnitude >= TouchActionDirection.magnitude) ActionDirection = GamepadActionDirection;
//		else ActionDirection = TouchActionDirection;

//		ActionAngle = Mathf.Atan2(ActionDirection.y, ActionDirection.x) * Mathf.Rad2Deg;

			//print("ActionAngle: " + ActionAngle + " - Right Joy Stick: " + ActionDirection);

			_blends.SetFloat (AnimatorVelocityX_ID, MovementDirection.x);
			_blends.SetFloat (AnimatorVelocityZ_ID, MovementDirection.z);

			//if (animInCall || MovementDirection.y < .25f || ActionDirection.magnitude < .25f)
			//{
			//	return;
			//}

			//Skill();

			this.gameObject.transform.Translate (MovementDirection * Time.fixedDeltaTime);

		}


		if (transform.position.y <= -1)
			LevelManager.Instance.FinishLevel (LevelManager.FinishingLevelReason.Death);

		if (Input.GetKeyDown (KeyCode.JoystickButton0)) BtnJump1 (); //Button A
		else if (Input.GetKeyDown (KeyCode.JoystickButton2)) BtnSlide2 (); //Button X
	}

	void FollowPath()
	{
		if (playerContainer == null) GetComponentInParent<Transform> ();
		currentPathPercent += percentPerSec * Time.deltaTime;
//		iTween.PutOnPath (playerContainer, path, currentPathPercent);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.name == "CornerRight") {
			Turn (85, RotateMode.Fast);
		}

		if (other.name == "CornerLeft") {
			Turn (-85, RotateMode.FastBeyond360);
		}

		if ((other.CompareTag("Enemy") || other.CompareTag("Trap")) && _assist) {
			if(autorun.speedRun > 0)
			StartCoroutine (Slowing ());
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag("Trap") || other.transform.CompareTag("Enemy")) {
			GameManager.Instance.CauseDamage (GameManager.Instance.CurrentHealth, true);
		}
	}

	IEnumerator Slowing()
	{
		GameManager.Instance.CauseDamage (15f, false);
		autorun.speedRun -= 2f;
		manager.CanHurt = false;
		yield return new WaitForSeconds(1f);
		while(autorun.speedRun<autorun.maxSpeedRun)
		{
			var speed = autorun.speedRun;
			speed += Time.deltaTime;
			autorun.speedRun = Mathf.Clamp (speed, 0, autorun.maxSpeedRun);
		}
		manager.CanHurt = true;
	}

	void Turn(float angle,RotateMode mode)
	{
		var tempRot = transform.eulerAngles + new Vector3(0,angle,0);
		transform.DORotate (tempRot, 1.8f, mode);
	}

	public IEnumerator falseTheBool(string setBool, float time)
	{
		animInCall = true;
		yield return new WaitForSeconds(time);
		animInCall = false;
		if (setBool == "jumpSt")
		{
			isRunning = true;
			if (_assist) 
			{
				GetComponent<Animator>().applyRootMotion = false;
			}

//			GetComponent<CapsuleCollider> ().isTrigger = false;
			manager.CanHurt = true;
			jumpSt = false;
		}
		else if (setBool == "slideSt")
		{
			manager.CanHurt = true;
			slideSt = false;
		}
		else if (setBool == "jumpLt")
		{
			manager.CanHurt = true;
			jumpLt = false;
		}
		else if (setBool == "jumpRt")
		{
			manager.CanHurt = true;
			jumpRt = false;
		}
		else if (setBool == "slideLt")
		{
			isRunning = true;
			if (_assist) 
			{
				GetComponent<Animator>().applyRootMotion = false;
			}
						
			manager.CanHurt = true;
			slideLt = false;
		}
		else if (setBool == "slideRt")
		{
			manager.CanHurt = true;
			slideRt = false;
		}
		else if (setBool == "avoidLt")
		{
			manager.CanHurt = true;
			avoidLt = false;
		}
		else if (setBool == "avoidRt")
		{
			manager.CanHurt = true;
			avoidRt = false;
		}
	}

	public void BtnJump1()
	{
		if (SceneManager.GetActiveScene ().name != "Medieval City") {
			if (!jumpSt) {
				isRunning = false;
				//else if (Input.GetKeyDown (KeyCode.X) && !jumpSt && manager.canSkill == true) {
//			rigid.AddForce (new Vector3 (0, jumpForce, 0),ForceMode.Impulse);
//			rigid.velocity = new Vector3 (0, jumpForce, 0);
//			rigid.DOJump (transform.position+ new Vector3(0,0,6.5f), jumpForce, 1, jumpStC.length, false);
				GetComponent<Animator> ().applyRootMotion = true;
//			GetComponent<CapsuleCollider> ().isTrigger = true;
				manager.CanHurt = false;
				jumpSt = true;
				_blends.SetTrigger ("jumpSt");
				//rigid.velocity = Vector3.up * jumpForce;
				StartCoroutine (falseTheBool ("jumpSt", jumpStC.length));
			}
		}
	}

	//public void BtnJump2()
	//{
	//	if (!jumpLt)
	//	{
	//		manager.canHurt = false;
	//		jumpLt = true;
	//		_blends.SetTrigger("jumpLt");
	//		StartCoroutine(falseTheBool("jumpLt", jumpLtC.length));
	//	}
	//}

	//public void BtnJump3()
	//{
	//	if (!jumpRt)
	//	{
	//		manager.canHurt = false;
	//		jumpRt = true;
	//		_blends.SetTrigger("jumpRt");
	//		StartCoroutine(falseTheBool("jumpRt", jumpRtC.length));
	//	}
	//}

	//public void BtnSlide1()
	//{
	//	if (!slideSt)
	//	{
	//		manager.canHurt = false;
	//		slideSt = true;
	//		_blends.SetTrigger("slideSt");
	//		StartCoroutine(falseTheBool("slideSt", slideStC.length));
	//	}
	//}

	public void BtnSlide2()
	{
		if (SceneManager.GetActiveScene ().name != "Medieval City") {
			if (!slideLt)
 {		//			else if (Input.GetKeyDown (KeyCode.Z) && !slideLt && manager.canSkill == true) 
				isRunning = false;
				GetComponent<Animator> ().applyRootMotion = true;
				manager.CanHurt = false;
				slideLt = true;
				_blends.SetTrigger ("slideLt");
				StartCoroutine (falseTheBool ("slideLt", slideLtC.length));
			}
		}
	}

	//public void BtnSlide3()
	//{
	//	if (!slideRt)
	//	{
	//		manager.canHurt = false;
	//		slideRt = true;
	//		_blends.SetTrigger("slideRt");
	//		StartCoroutine(falseTheBool("slideRt", slideRtC.length));
	//	}
	//}

//	void _AutoRun()
//	{
//		currentAcceleration = Vector3.Lerp (currentAcceleration, Input.acceleration - initialAcceleration, Time.deltaTime / smooth);
//		newRotation = Mathf.Clamp (currentAcceleration.x * sensitivity, -1, 1);
//		Vector3 newDir = new Vector3 (newRotation, transform.position.y, 3);
//		transform.Translate (newDir * Time.deltaTime);
//
//		if (Input.touchCount == 1) {
//			if (isSwiping) {
//				Vector2 diff = Input.GetTouch(0).position - startingPoint;
//				diff = new Vector2 (diff.x / Screen.width, diff.y / Screen.width);
//
//				if (diff.magnitude > 0.01f) {
//					if (Mathf.Abs (diff.x) > Mathf.Abs	(diff.y)) {
//						if (diff.x < 0) {
//							Turn (TurnDirections.Left);
//						} else {
//							Turn (TurnDirections.Right);
//						}
//					}
//					isSwiping = false;
//				}
//			}
//
//			if (Input.GetTouch (0).phase == TouchPhase.Began) {
//				startingPoint = Input.GetTouch (0).position;
//				isSwiping = true;
//			} else if (Input.GetTouch (0).phase == TouchPhase.Ended) {
//				isSwiping = false;
//			}
//		}
//	}
//
//	enum TurnDirections { Left, Right }
//
//	void Turn(TurnDirections dir)
//	{
//		if (SceneManager.GetActiveScene().name != "Medieval City")
//		{
//			if (dir == TurnDirections.Right)
//			{
//				var angles = transform.rotation.eulerAngles;
//				angles.y += 90;
//				transform.rotation = Quaternion.Euler(angles);
//			}
//			else if (dir == TurnDirections.Left)
//			{
//				var angles = transform.rotation.eulerAngles;
//				angles.y += -90;
//				transform.rotation = Quaternion.Euler(angles);
//			}
//		}
//	}
}