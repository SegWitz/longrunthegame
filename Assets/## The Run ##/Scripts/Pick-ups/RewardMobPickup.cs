using RewardMobSDK;

public class RewardMobPickup : Pickup
{
	protected override void Collect(GameManager PlayerGameManager)
	{
		RewardMob RMInstance = RewardMob.instance;
		if (RMInstance != null) RMInstance.SendReward("This is your reward!", 1);

		base.Collect(PlayerGameManager);
	}
}