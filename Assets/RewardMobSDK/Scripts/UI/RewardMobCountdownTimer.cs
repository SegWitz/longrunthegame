using RewardMobSDK;
using RewardMobSDK.Networking.WebRequests;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RewardMobSDK
{
    public class RewardMobCountdownTimer : MonoBehaviour
    {
        public float secondsRemaining = 0f;
        private Text textToUpdate;

        public static RewardMobCountdownTimer instance;

        private void Awake()
        {
            instance = this;
        }

        // Use this for initialization
        void Start()
        {
            textToUpdate = gameObject.GetComponentInChildren<Text>();
            StartCoroutine(GetTimeLeftAndUpdate());
        }

        //REFACTOR
        public IEnumerator UpdateSeconds()
        {
            var gameID = Resources.LoadAll<RewardMobData>("")[0].GameId;
            var webRequest = new WWW(RewardMobEndpoints.GetTournamentTimeRemainingEndpoint() + gameID);

            //halt execution until request returns
            yield return webRequest;

            float.TryParse(webRequest.text, out secondsRemaining);
        }

        private IEnumerator GetTimeLeftAndUpdate()
        {
            //grab game id, and prepare web request
            var gameID = Resources.LoadAll<RewardMobData>("")[0].GameId;
            var webRequest = new WWW(RewardMobEndpoints.GetTournamentTimeRemainingEndpoint() + gameID);

            //halt execution until request returns
            yield return webRequest;

            float.TryParse(webRequest.text, out secondsRemaining);

            //invoke time update
            InvokeRepeating("UpdateTime", 0f, 1.0f);
        }

        private void UpdateTime()
        {
            float delta = secondsRemaining--;

            if (secondsRemaining >= 0)
            {
                // calculate (and subtract) whole days
                var days = Mathf.Floor(delta / 86400);
                delta -= days * 86400;

                // calculate (and subtract) whole hours
                var hours = Mathf.Floor(delta / 3600) % 24;
                delta -= hours * 3600;

                // calculate (and subtract) whole minutes
                var minutes = Mathf.Floor(delta / 60) % 60;
                delta -= minutes * 60;

                // what's left is seconds
                var seconds = Mathf.Floor(delta % 60);

                textToUpdate.text = (days + "D  " + hours + "H  " + minutes + "M  " + seconds + "S");
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}