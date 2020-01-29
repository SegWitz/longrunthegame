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
// <summary>Used for register an user in PlayFab Account</summary>
// ***********************************************************************
using System.Runtime.InteropServices;
using PlayFab.ClientModels;
using UnityEngine;


namespace PlayFab
{
    /// <summary>
    /// This class will register the user in play fab domain.
    /// </summary>
    public static class PlayfabRegistration 
    {
        #region Functions

        /// <summary>
        /// Registers the specified with display name.
        /// </summary>
        /// <param name="withDisplayName">Display name of the user.</param>
        /// <param name="withEmailId">The  email of the user.</param>
        /// <param name="withPassword">The password for the user.</param>
        /// <param name="isRequireBothUsernameAndEmail">The is require both username and email.</param>
        /// <param name="withTitleId">The title identifier.</param>
        /// <param name="withUserName">Name of the  user.</param>
        /// <param name="withPlayerSecret">The player secret.</param>
        /// <param name="withEncryptedRequest">Encrypted request.</param>
        /// <param name="wthRequestParameters">The request parameters.</param>
        /// <param name="withLoginTitlePlayerAccountEntity">if set to <c>true</c> [with login title player account entity].</param>
        public static void Register(string withDisplayName, string withEmailId, string withPassword,
            bool isRequireBothUsernameAndEmail, [Optional] string withTitleId, [Optional] string withUserName,
            [Optional] string withPlayerSecret, [Optional] string withEncryptedRequest,
            [Optional] GetPlayerCombinedInfoRequestParams wthRequestParameters,
            [Optional] bool withLoginTitlePlayerAccountEntity)
        {
            //create an request for register
            var playfabregistrationRequest = new RegisterPlayFabUserRequest
            {
                Email = withEmailId,
                DisplayName = withDisplayName,
                Password = withPassword,
                Username = withUserName,
                RequireBothUsernameAndEmail = isRequireBothUsernameAndEmail,
            };
            //fire the api
            PlayFabClientAPI.RegisterPlayFabUser(playfabregistrationRequest, Onsuccess,
                OnFailure, null, null);
        }


        /// <summary>
        /// On successes of APi call the specified result is received.
        /// </summary>
        /// <param name="result">The result.</param>
        private static void Onsuccess(RegisterPlayFabUserResult result)
        {
            //TODO: stop the log for main build.
            Debug.Log("Result received");
            Debug.Log("session ticket-->" + result.SessionTicket);
            Debug.Log("User name--->" + result.Username);
            Debug.Log("Setting for user" + result.SettingsForUser);

            //Registered success fully.
            UserFormController.Reference.ShowMessage("Registered successfully",true);
            //success
            //set playfab data
            PlayfabData.PlayfabSessionTicket = result.SessionTicket;
            PlayfabData.PlayfbUserName = result.Username;
        }


        /// <summary>
        /// Called when API failed to register on playfab server.
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

            //Registration failed
            UserFormController.Reference.ShowMessage(error.ErrorMessage,false);

        }

        #endregion
    }
}