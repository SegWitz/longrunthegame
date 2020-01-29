using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ValintaMusicStreaming.SimpleJSON;
using System.Linq;

namespace Scripts.LeaderBoard
{
    /// <inheritdoc />
    /// <summary>
    /// Leader Board Details.
    /// </summary>
    public class LeaderBoard : MonoBehaviour
    {
        /// <summary>
        /// Leader Board Loading Panel .
        /// </summary>
        public GameObject LeaderBoardLoadinPanel;

        /// <summary>
        /// Leader Board Details panel.
        /// </summary>
        public GameObject LeaderBoardDetailsPanel;

        /// <summary>
        /// Leader Board Details .
        /// </summary>
		public GameObject LeaderBoardResult;

		/// <summary>
		/// Leader Board List
		/// </summary>
		private List<GameObject> leaderBoardResultList = new List<GameObject> ();

		private struct Detail
		{
			public string name;
			public long money;
			public string email;
		}

		private List<Detail> detailList = new List<Detail> ();

        /// <summary>
        /// On enable .
        /// </summary>
        private void OnEnable()
        {
            if (LeaderBoardLoadinPanel.activeInHierarchy) LeaderBoardLoadinPanel.SetActive(false);
            if (LeaderBoardDetailsPanel.activeInHierarchy) LeaderBoardDetailsPanel.SetActive(false);
            //if (!string.IsNullOrEmpty(LeaderBoardResult.text)) LeaderBoardResult.text = string.Empty;
        }

        #region For Getting Leader Board Details.

        /// <summary>
        /// Get LeaderBoard .
        /// </summary>
        /// <param name="leaderBoardName"></param>
        public void GetLeaderBoard(string leaderBoardName)
        {
            //Leader board routine .
            var leaderboardroutine = GetLeaderBoardWithName(leaderBoardName);
            //Start the routine .
            StartCoroutine(leaderboardroutine);
        }

        /// <summary>
        /// Get Leader Board Details with Name .
        /// </summary>
        /// <param name="leaderBoardName"></param>
        /// <returns></returns>
        public IEnumerator GetLeaderBoardWithName(string leaderBoardName)
        {
            //if result is active.
            if (LeaderBoardDetailsPanel.activeInHierarchy) LeaderBoardDetailsPanel.SetActive(false);
           // if (!string.IsNullOrEmpty(LeaderBoardResult.text)) LeaderBoardResult.text = string.Empty;
            //activate loding panel .
            LeaderBoardLoadinPanel.SetActive(true);
            //Url need to heat .
            const string url = "https://long-run.herokuapp.com/LeaderBoard";
            //var create json.
            var leaderBoardDetails = new LeaderBoardDetails(leaderBoardName);
            //convert to json.
            var json = JsonUtility.ToJson(leaderBoardDetails);
            //debug.
            Debug.Log("Print json" + json);
            //update json.
            json = json.Replace("'", "\"");
            //Encode the JSON string into a bytes
            var postData = System.Text.Encoding.UTF8.GetBytes(json);
            //create headers.
            //Add keys an values .
            var headers = new Dictionary<string, string> {{"Content-Type", "application/json"}};
            //Now call a new WWW request
            var www = new WWW(url, postData, headers);
            //Yield return www .
            yield return www;
            //deacctivate loding panel .
            LeaderBoardLoadinPanel.SetActive(false);

            //check for error.
            //Registered success fully.
            if (string.IsNullOrEmpty(www.error))
            {
                var receivedData = JSON.Parse(www.text)["User"];
                Debug.Log(receivedData);
                //var modifiledData = "Name" + "         " + "Joe Games Currency"+"            "+"Emailid";
               
               
				SetLeaderBoardResult(receivedData);
            }
            else
            {
                var receivedData = www.error;
                Debug.Log("Data" + receivedData);
                SetLeaderBoardResult(receivedData);
            }

            www.Dispose();
        }

        /// <summary>
        /// Set leaderboard Result.
        /// </summary>
		private void SetLeaderBoardResult(JSONNode data)
        {
			if (leaderBoardResultList.Count > 0) 
			{
				foreach (var list in leaderBoardResultList) 
				{
					Destroy (list);
				}

				leaderBoardResultList.Clear ();
				detailList.Clear ();
			}

            if (!LeaderBoardDetailsPanel.activeInHierarchy)
            {
                LeaderBoardDetailsPanel.SetActive(true);
            }

			var getnumberofentry = data.Count;

			for (var i = 0; i < getnumberofentry; i++)
			{
				var detail = new Detail ();
				detail.name = data[i]["User_Name"];
				detail.money = long.Parse(data [i] ["Joe Games Currency"]);
//				detail.email = data[i]["Email"];
				detailList.Add (detail);
				//modifiledData +="\n"+receivedData[i]["User_Name"] + "         " + receivedData[i]["Joe Games Currency"]+ "            "+ receivedData[i]["Email"];
			}

			detailList = detailList.OrderByDescending (x => x.money).ToList ();

			foreach (var value in detailList)
			{
				var detail = Instantiate (LeaderBoardResult, LeaderBoardDetailsPanel.transform.GetChild(0).transform);
				detail.transform.GetChild (0).GetComponent<Text> ().text = value.name;
				detail.transform.GetChild (1).GetComponent<Text> ().text = Globals.GetFormattedCurrency (value.money, true);
//				detail.transform.GetChild (2).GetComponent<Text> ().text = value.email;
				leaderBoardResultList.Add (detail);
				//modifiledData +="\n"+receivedData[i]["User_Name"] + "         " + receivedData[i]["Joe Games Currency"]+ "            "+ receivedData[i]["Email"];

			}
			//Debug.Log("Data" + receivedData);

			LeaderBoardDetailsPanel.transform.parent.GetComponent<ScrollRect> ().verticalNormalizedPosition = 1;
        }

        [Serializable]
        private class LeaderBoardDetails
        {
            //CAUTION:
            //NOTE:
            //DO NOT ALTER THE NAME OF PARAMETERS .
            //THE JSON WILL FAIL .

            /// <summary>
            /// Leader Board Name
            /// </summary>
            public string leader_board_name;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="leaderboardname"></param>
            public LeaderBoardDetails(string leaderboardname)
            {
                leader_board_name = leaderboardname;
            }
        }

        #endregion
    }
}