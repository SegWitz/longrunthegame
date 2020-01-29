// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : Reezoo
// Created          : 04-23-2018
//
// Last Modified By : Reezoo
// Last Modified On : 04-23-2018
// ***********************************************************************
// <copyright file="PlayfabRegistration.cs" author="Reezoo Bose">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Used for login an user in PlayFab Account</summary>
// ***********************************************************************

using System.Runtime.InteropServices;
using PlayFab.ClientModels;
using UnityEngine;

namespace PlayFab
{
    /// <summary>
    /// This class will login the user in play fab domain.
    /// </summary>
    public static class PlayfabLogin
    {
        #region Function

        /// <summary>
        /// Logins the specified with password.
        /// </summary>
        /// <param name="withPassword">With the password.</param>
        /// <param name="withEmailid">With the emailid.</param>
        public static void Login(string withPassword, string withEmailid)
        {
            //create player info request.
            var playerinfo = new GetPlayerCombinedInfoRequestParams
            {
                GetCharacterInventories = true,
                GetCharacterList = true,
                GetPlayerProfile = true,
                GetPlayerStatistics = true,
                GetTitleData = true,
                GetUserAccountInfo = true,
                GetUserData = true,
                GetUserInventory = true,
                GetUserReadOnlyData = true,
                GetUserVirtualCurrency = true,
                PlayerStatisticNames = null,
                TitleDataKeys = null,
                UserDataKeys = null,
                UserReadOnlyDataKeys = null
            };
            //create the request.
            var playfabLogin = new LoginWithEmailAddressRequest
            {
                Password = withPassword,
                Email = withEmailid,
                TitleId = PlayFabSettings.TitleId,
                LoginTitlePlayerAccountEntity = true,
                InfoRequestParameters = playerinfo
            };
            //fire the api.
            PlayFabClientAPI.LoginWithEmailAddress(playfabLogin, Onsuccess,
                OnFailure);
        }

        /// <summary>
        /// On successes of APi call the specified result is received.
        /// </summary>
        /// <param name="result">The result.</param>
        private static void Onsuccess(LoginResult result)
        {
            //TODO: stop the log for main build.
            Debug.Log("Result received");
            Debug.Log("Entity token ------ >" + result.EntityToken);
            Debug.Log("Playfab id ------ >" + result.PlayFabId);
            Debug.Log("Last login Time------>" + result.LastLoginTime.GetValueOrDefault());
            Debug.Log("Is Newly Created " + result.NewlyCreated);
            Debug.Log("session ticket-->" + result.SessionTicket);
            Debug.Log("Setting for user" + result.SettingsForUser);
            Debug.Log("Username--->" + result.InfoResultPayload.PlayerProfile.DisplayName);
            //set playfab data
            PlayfabData.PlayfabAccountLastLoginTime = result.LastLoginTime.GetValueOrDefault().ToString();
            PlayfabData.PlayfabSessionTicket = result.SessionTicket;
            PlayfabData.PlayfbEntityToken = result.EntityToken.ToString();
            PlayfabData.PlayfbUserName = result.InfoResultPayload.PlayerProfile.DisplayName;
            PlayfabData.IsPlayerRegisteredToPlayFab = true;
            Debug.Log("------------------"+PlayerPrefs.GetInt("PlayerRegistered", 0).ToString());
            //Registered success fully.
            UserFormController.Reference.ShowMessage("Login successfully", true);
           

        }

        /// <summary>
        /// Called when API failed to Login on playfab server.
        /// </summary>
        /// <param name="error">The error.</param>
        private static void OnFailure(PlayFabError error)
        {
            //TODO: stop the log for main build.
            Debug.Log("Message received" + error.ErrorMessage);
            Debug.Log("ErrorCode" + error.Error);
            Debug.Log("" + error.HttpCode);
            Debug.Log("Error details " + error.ErrorDetails);
            Debug.Log("Http Status " + error.HttpStatus);

            //Login failed
            UserFormController.Reference.ShowMessage(error.ErrorMessage, false);
        }

        #endregion
    }
}