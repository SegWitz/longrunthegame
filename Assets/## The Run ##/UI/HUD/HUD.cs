using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	[SerializeField]
	healthbar HealthBar = null;
	[SerializeField]
	Text MoneyText = null;
	[SerializeField]
	Text TimeText = null;
	[SerializeField]
	PlayerInventory _Inventory = null;

	public PlayerInventory Inventory { get { return _Inventory; } }

	GameManager PlayerGameManager;
	AnimationBlending PlayerAnimationBlending;

	void Awake()
	{
		PlayerGameManager = GameManager.Instance;
		PlayerAnimationBlending = PlayerGameManager.GetComponent<AnimationBlending>();

		PlayerGameManager.OnMoneyValueChanged += SetMoneyText;
		PlayerGameManager.OnHealthChange += PlayerGameManager_OnCauseDamage;

		TimeText.enabled = LevelManager.Instance.DoesItHaveTime;
	}

	void OnDestroy()
	{
		PlayerGameManager.OnMoneyValueChanged -= SetMoneyText;
		PlayerGameManager.OnHealthChange -= PlayerGameManager_OnCauseDamage;
	}

	void Update()
	{
		ProcessTime();
	}

	public void Pause()
	{
		LevelManager.Instance.Pause();
	}

	public void SetMoneyText(int Money)
	{
		MoneyText.text = Globals.GetFormattedCurrency(Money, true);
	}

	public void SetTimeText(int Seconds)
	{
		if (!LevelManager.Instance.DoesItHaveTime) return;
		TimeText.text = Seconds.ToString();
	}

	void PlayerGameManager_OnCauseDamage(float CurrentHealth, float MaxHealth)
	{
		HealthBar.Value = CurrentHealth;
		HealthBar.MaxValue = MaxHealth;
	}

	public void PerformAction(int ActionIndex)
	{
		switch (ActionIndex)
		{
			case 0:
				PlayerAnimationBlending.BtnJump1();
				break;
			case 1:
				//PlayerAnimationBlending.BtnJump3();
				break;
			case 2:
				//PlayerAnimationBlending.BtnSlide1();
				break;
			case 3:
				PlayerAnimationBlending.BtnSlide2();
				break;
			case 4:
				//PlayerAnimationBlending.BtnSlide3();
				break;
		}
	}

	#region Time

	int PreviousTime;

	void ProcessTime()
	{
		if (!LevelManager.Instance.DoesItHaveTime) return;

		int CurrentTime = Mathf.FloorToInt(LevelManager.Instance.RemainingTime);
		if (CurrentTime != PreviousTime)
		{
			SetTimeText(CurrentTime);
			PreviousTime = CurrentTime;
		}
	}

	#endregion
}