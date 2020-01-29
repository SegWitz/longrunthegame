using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ValintaMusicStreaming.SimpleJSON;

namespace Scripts.UpdateMoney
{

    /// <inheritdoc />
    /// <summary>
    /// Update Money .
    /// </summary>
    public class UpdateMoney : MonoBehaviour
    {
        /// <summary>
        /// On update complete .
        /// </summary>
        public Action<string> OnUpdateComplete;

#if UNITY_EDITOR

        [Header("User Money That Will Set in data bease.")]
        public string UserUpdatedMoney;
        [Header("User corresponding email.")]
        public string UserRegisteredEmail;
        
#endif

        /// <summary>
        /// Awake the instance.
        /// </summary>
        private void Awake()
        {
            OnUpdateComplete += ShowOutPut;
        }

        /// <summary>
        /// Update User money .
        /// When user register from success message you can store user email.id .
        /// </summary>
        /// <param name="email"></param>
        /// <param name="money"></param>
        public void UpdateUserMoney(string email, string money)
        {
            //User update money.
            var usermoneyUpdateroutine = SetMoney(email, money, OnUpdateComplete);
            //start routine .
            StartCoroutine(usermoneyUpdateroutine);
        }

        /// <summary>
        /// Update Money.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="money"></param>
        /// <param name="onUpdateComplete"></param>
        /// <returns></returns>
        private IEnumerator SetMoney(string email, string money, Action<string> onUpdateComplete)
        {

            //Url need to heat .
            const string url = "https://long-run.herokuapp.com/SetMoney";
            //var create json.
            var usermoney = new UserDetails(email, money);
            //convert to json.
            var json = JsonUtility.ToJson(usermoney);
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
            //Received data .
            var receivedData = "";
            //check for error.
            //Registered success fully.
            if (string.IsNullOrEmpty(www.error))
            {
                 receivedData = JSON.Parse(www.text).ToString();
                

            }
            else
            {
                receivedData = www.error + www.responseHeaders;
                

            }


            OnUpdateComplete.Invoke(receivedData);

            www.Dispose();
        }

        /// <summary>
        /// Show out put .
        /// </summary>
        public void ShowOutPut(string output)
        {
            Debug.Log(output);
        }

    

    [Serializable]
    public class UserDetails
        {
            //CAUTION:
            //NOTE:
            //DO NOT ALTER THE NAME OF PARAMETERS .
            //THE JSON WILL FAIL .
            /// <summary>
            /// Email id of user.
            /// </summary>
            public string email_id;
            /// <summary>
            /// In game currency
            /// </summary>
            public string us_dollar;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="email">User email</param>
            /// <param name="money">User new Updated Money</param>
            public UserDetails(string email,string money)
            {
                email_id = email;
                us_dollar = money;
            }
        }


    }
}
