using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//public float lostchances;
	//[SerializeField]
	//private Animation textAnim;
	//[SerializeField]
	//private Animation textAnim2;

	[SerializeField]
	float MaximumHealth = 100f;
	[SerializeField]
	private Stat1 chancesStat;

	bool _canHurt = true;
	public bool CanHurt
	{
		get { return _canHurt; }
		set { _canHurt = value; }
	}

	//[NonSerialized]
	//public bool isGrounded = false;

	private Transform spawn;
	private Animator anim;
	private Rigidbody rgd;

	private static GameManager instance;

	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<GameManager>();
			}
			return instance;
		}
	}

	void Awake()
	{

		InitializeHealth();
		chancesStat.Initialize();
	}

	void Start()
	{
		anim = GetComponent<Animator>();
		rgd = GetComponent<Rigidbody>();
//		spawn.position = rgd.gameObject.transform.position;
//		spawn.rotation = rgd.transform.rotation;
	}

	void Update()
	{
		ProcessInvincibility();
		//ProcessInvisibility();
	}

	/// <summary>
	/// Touch the enemy and the player taked damage. If player's health == 0, he dies.
	/// </summary>
	void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag("Enemy"))
		{
			CauseDamage(25, true);
		}
	}

	/// <summary>
	/// Make the player respawn.
	/// </summary>
	public void Respawn()
	{
		anim.SetTrigger ("Reset");
//		rgd.transform.position = spawn.position;
//		rgd.transform.rotation = spawn.rotation;
		InitializeHealth();
		chancesStat.CurrentVal = chancesStat.MaxVal;
	}

	#region Health

	private float _CurrentHealth;

	public float CurrentHealth
	{
		get
		{
			return _CurrentHealth;
		}
		set
		{
			_CurrentHealth = Mathf.Clamp(value, 0, MaxHealth);
			if (OnHealthChange != null) OnHealthChange(_CurrentHealth, _MaxHealth);
		}
	}

	private float _MaxHealth;

	public float MaxHealth
	{
		get
		{
			return _MaxHealth;
		}
		set
		{
			_MaxHealth = value;
			if (OnHealthChange != null) OnHealthChange(_CurrentHealth, _MaxHealth);
		}
	}

	void InitializeHealth()
	{
		MaxHealth = MaximumHealth;
		CurrentHealth = MaximumHealth;
	}

	#endregion

	#region Damage

	public event Action<float, float> OnHealthChange;

	public void CauseDamage(float Damage, bool StopCharacter)
	{
		if (!CanHurt || IsInvincible) return;

		CurrentHealth -= Damage;

		if (StopCharacter) rgd.velocity = Vector3.zero;

		if (CurrentHealth <= 0)
		{
			StartCoroutine(DieAnimation());
		}
		else
		{
			if (StopCharacter) StartCoroutine(TakeDamageAnimation());
		}
	}

	//  method player take damage
	IEnumerator TakeDamageAnimation()
	{
		anim.SetTrigger("damage");
		yield return new WaitForSeconds(3f);
	}

	//  method player die
	IEnumerator DieAnimation()
	{
		anim.SetTrigger("death2");
		yield return new WaitForSeconds(4f);
		LevelManager.Instance.FinishLevel(LevelManager.FinishingLevelReason.Death);
	}

	#endregion

	#region Money

	public event Action<int> OnMoneyValueChanged;

	int _CurrentMoney;
	public int CurrentMoney
	{
		get
		{
			return _CurrentMoney;
		}
		set
		{
			_CurrentMoney = value;
			if (OnMoneyValueChanged != null) OnMoneyValueChanged(_CurrentMoney);
		}
	}

	#endregion

	#region Invincibility

	float InvincibilityTimer;
	public bool IsInvincible { get { return InvincibilityTimer > 0; } }

	public void SetInvincibility(float Duration)
	{
		InvincibilityTimer = Duration;
	}

	void ProcessInvincibility()
	{
		if (IsInvincible)
		{
			InvincibilityTimer -= Time.deltaTime;
		}
	}

	#endregion

	#region Invisibility

	//float InvisibilityTimer;
	//public bool IsInvisibile { get { return InvisibilityTimer > 0; } }

	//public void SetInvisibility(float Duration)
	//{
	//	InvisibilityTimer = Duration;
	//}

	//void ProcessInvisibility()
	//{
	//	if (IsInvisibile)
	//	{
	//		InvisibilityTimer -= Time.deltaTime;
	//	}
	//}

	#endregion
}