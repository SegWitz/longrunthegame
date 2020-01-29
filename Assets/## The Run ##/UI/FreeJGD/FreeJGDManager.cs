using ChartboostSDK;
using UnityEngine;
using UnityEngine.UI;

public class FreeJGDManager : MonoBehaviour
{
    [SerializeField]
    Animator AnimatorComponent = null;

    [Space]
    [SerializeField]
    bool IsShownByDefault = false;
    public static FreeJGDManager instance;
    public Button ShowAdvertisementButton;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (IsShownByDefault) Show(true);
    }

    /// <summary>
    /// Plays video ad in the game
    /// </summary>
    public void showVideoAd()
    {
        ChartBoostImplimentation.ShowRewardVideo();
        ShowAdvertisementButton.enabled = false;
    }

    /// <summary>
    /// Reward the user with the coins on ad view completed
    /// </summary>
    public void onAdCompletedReward()
    {
        Debug.Log("Reward user with coins here");
    }

    #region Transitions

    int ShowID = Animator.StringToHash("Show");
    int HideID = Animator.StringToHash("Hide");

    public void Show(bool Value)
    {
        if (Value)
            AnimatorComponent.Play(ShowID);
        else
            AnimatorComponent.Play(HideID);
       

            ShowAdvertisementButton.enabled = ChartBoostImplimentation.IsRewardVideoAvailableToShow;
    }

    #endregion
}