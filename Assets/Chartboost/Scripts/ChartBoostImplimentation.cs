using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using UnityEngine;

namespace ChartboostSDK
{
    public class ChartBoostImplimentation : MonoBehaviour
    {
        /// <summary>
        /// Auto Cache for Advertisement .
        /// </summary>
        public bool AutoCache = false;

        /// <summary>
        /// Dubug will be printed over console .
        /// </summary>
        public bool DebugAllowed = true;

        /// <summary>
        /// Check reward video is available or not 
        /// </summary>
        private static bool _isRewardVideoAvailble = false;

        /// <summary>
        /// Checking That return Reward video is available or not .
        /// Double check implemented.
        /// </summary>
        public static bool IsRewardVideoAvailableToShow
        {
            get { return _isRewardVideoAvailble && Chartboost.hasRewardedVideo(CBLocation.Default); }
        }

        #region Unity method

        /// <summary>
        /// OnEnable the script.
        /// </summary>
        private void OnEnable()
        {
            SetupDelegatesForRewardVideo();


        }

        /// <summary>
        /// Start the instance.
        /// </summary>
        private void Start()
        {
            Init();
        }

        #endregion
        /// <summary>
        /// Initialize the chart boost process .
        /// </summary>
        public void Init()
        {
            
            //implimenting Auto cache .
            Chartboost.setAutoCacheAds(AutoCache);
            //create chart boost
            Chartboost.CreateWithAppId("5aba8b5df7c1590bb81e831f", "ac5a8aec6f8222760cd0a01e0533d29cb19ab8e6");
            //Initialize chart boost
            Print("Is chart boost initialized "+Chartboost.isInitialized());

        }

        public void SetupDelegatesForRewardVideo()
        {
            //CALLBACK 1.

            //If chart boost is initialized successfully then we will start a call for Caching else we will request for initialization.
           Chartboost.didInitialize += b =>
            {
                //If chart boost is initialized .
                if (b)
                {
                    //If autocache is deactivated .
                    //We have to handle the cache functionality .
                    if (!AutoCache)
                    {
                        //Start  for Caching .
                        Chartboost.cacheRewardedVideo(CBLocation.Default);
                    }
                }
                else
                {
                    //create the chart boost .
                    Chartboost.Create();
                    //print charboost is initiated.
                    Print("Creating chart boost");
                    /*
		            // Sample to create Chartboost gameobject from code overriding editor AppId and AppSignature
		            // Remove the Chartboost gameobject from the sample first
		            #if UNITY_IPHONE
		            Chartboost.CreateWithAppId("4f21c409cd1cb2fb7000001b", "92e2de2fd7070327bdeb54c15a5295309c6fcd2d");
		            #elif UNITY_ANDROID
		            Chartboost.CreateWithAppId("4f7b433509b6025804000002", "dd2d41b69ac01b80f443f5b6cf06096d457f82bd");
		            #endif
		            */
                }
            };
            //CALL bACK 2

            Chartboost.didFailToLoadRewardedVideo += (CBLocation location, CBImpressionError error) =>
            {
                Print("Failed to load the reward video");
                Print("Location", location);
                Print("Error" + error);
                //if the error is internal we will ignore the error.
                switch (error)
                {
                    case CBImpressionError.Internal:
                        Print("ERROR IS INTERNAL UNABLE TO HANDLE THIS");
                        break;
                    case CBImpressionError.InternetUnavailable:
                    {
                        Print("No INTERNET IS AVAILABLE .");
                        var noInternetIenumerator = WaitForInternetToCome();
                        StartCoroutine(noInternetIenumerator);
                        return;
                    }
                    case CBImpressionError.TooManyConnections:
                        Print("SO MANY REQUEST RECEIVED WITHIN A SHORT PERION OF TIME");
                        break;
                    case CBImpressionError.WrongOrientation:
                        Print("NOT IN PROPER ORIENTATION");
                        break;
                    case CBImpressionError.FirstSessionInterstitialsDisabled:
                        Print("FIRST SESSION FOR INTERSTITIAL IN DIABLED");
                        break;
                    case CBImpressionError.NetworkFailure:
                    {
                        Print("NETWORK FAIlURE");
                        var noInternetIenumerator = WaitForInternetToCome();
                        StartCoroutine(noInternetIenumerator);
                        return;
                    }
                    case CBImpressionError.NoAdFound:
                        Print("NO AD FOUND FOR YOU");
                        break;
                    case CBImpressionError.SessionNotStarted:
                        Print("NOT A VALID SESSION ");
                        break;
                    case CBImpressionError.ImpressionAlreadyVisible:
                        Print("YOU ARE WATCHING THE VIDEO");
                        return;
                    case CBImpressionError.NoHostActivity:
                        Print("NO HOST ACTIVE");
                        return;
                    case CBImpressionError.UserCancellation:
                        Print("FUCKING USER CANCELL MY VIDEO");
                        return;
                    case CBImpressionError.InvalidLocation:
                        Print("CHECK YOUR LOCATION ");
                        return;
                    case CBImpressionError.VideoUnAvailable:
                        Print("VIDEO NOT AVAILABLE IN CACHE SO I AM STARTING CACHING ONE MORE TIME");
                        break;
                    case CBImpressionError.VideoIdMissing:
                        Print("VIDEO URL IS NOT PROPER");
                        break;
                    case CBImpressionError.ErrorPlayingVideo:
                        Print("ERROR IN PLAYING VIDEO");
                        break;
                    case CBImpressionError.InvalidResponse:
                        Print("INVALID RESPONCE");
                        return;
                    case CBImpressionError.AssetsDownloadFailure:
                        Print("ASSET MISSING");
                        break;
                    case CBImpressionError.ErrorCreatingView:
                        Print("ERROR IN MAKING A VIEW");
                        break;
                    case CBImpressionError.ErrorDisplayingView:
                        Print("ERROR IN SHOWING A VIEW");
                        break;
                    case CBImpressionError.PrefetchingIncomplete:
                        Print("ERROR IN PREFETCHING");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("error", error, null);

                }

                //Start  for Caching when auto caching is off
                if (!AutoCache)
                {
                    Chartboost.cacheRewardedVideo(CBLocation.Default);
                }
                //Reward video is not avilable.
                _isRewardVideoAvailble = false;
            };
            //CALL bACK 3

            Chartboost.didDismissRewardedVideo += (CBLocation location) =>
            {
                Print("DISSMISS REWARD VIDEO");
                Print("Location Where Video is cancelled" + location);

                //Prompt user 
                //You missed 20 Purple Heart .
            };
            //CALL BACK 4

            Chartboost.didCloseRewardedVideo += (CBLocation location) =>
            {
                Print("CLOSED REWARD VIDEO");
                Print("Location Where Video is cancelled" + location);
                //When reward video is cloed give a call for next time to cache video.
                if (!AutoCache)
                {
                    Chartboost.cacheRewardedVideo(CBLocation.Default);
                }
            };
            //CALL BACK 5

            Chartboost.didClickRewardedVideo += (CBLocation location) =>
            {
                Print("CLICKED REWARD VIDEO");
                Print("Location Where Video is cancelled" + location);
                //Prompt user 
                //are you want to leave the game.
            };
            //CALL BACK 6

            Chartboost.didCacheRewardedVideo += (CBLocation location) =>
            {
                Print("REWARD VIDEO CACHED");
                Print("Location Whete Video is cancelled" + location);
                _isRewardVideoAvailble = true;
            };
            //CALL BACK 7

            Chartboost.shouldDisplayRewardedVideo += ShouldDisplayRewardedVideo;
            //CALL BACK 8

            Chartboost.didCompleteRewardedVideo += (CBLocation location, int reward) =>
            {
                Print("Give some gift to user");
                Print("GIFT AMOUNT" + reward);
                //provide gift to the user .
            };
            //CALL BACK 9

            Chartboost.willDisplayVideo += (CBLocation location) =>
            {
                Print("REWARD VIDEO WILL BE DISPLAYED ");
                Print("Location Whete Video is cancelled" + location);
            };
        }

        /// <summary>
        /// Print the out put in to the console .
        /// </summary>
        /// <param name="correspondingkey"></param>
        /// <param name="value"></param>
        private void Print(string correspondingkey = null, object value = null)
        {
            //if debug not allowed just return .
            if (!DebugAllowed) return;
            if (correspondingkey != null && value != null) Debug.Log(correspondingkey + value);
            if (correspondingkey != null && value == null) Debug.Log(correspondingkey);
            if (correspondingkey == null && value != null) Debug.Log("Default Value printed : " + value);
            if (correspondingkey == null && value == null) return;
        }

        /// <summary>
        /// When called before Show reward video is called .
        /// WE can use this as a Authentication from user that he wants to watch the video or not .
        /// </summary>
        /// <param name="location"></param>
        /// <returns>Always true</returns>
        private bool ShouldDisplayRewardedVideo(CBLocation location)
        {
            return true;
        }
        /// <summary>
        /// Show Reward Video.
        /// </summary>
        public static void ShowRewardVideo()
        {
            //if chart boost is initialized.
            if (Chartboost.isInitialized())
            {
               
                Chartboost.showRewardedVideo(CBLocation.Default);
            }
            else
            {
              Debug.Log("not yet initialized");
            }

            
           
        }
        /// <summary>
        /// Wait for Internet to appear.
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForInternetToCome()
        {
            //when auto caching is off and no internet.
            while (Application.internetReachability == NetworkReachability.NotReachable && !AutoCache)
            {
                yield return new WaitForSecondsRealtime(60f);
                Print("NETWORK NOT AVAILABLE.....");
            }

            Print("NETWORK  AVAILABLE...READY TO CACHE ADD..");
            Chartboost.cacheRewardedVideo(CBLocation.Default);
            
        }

        
    }
}