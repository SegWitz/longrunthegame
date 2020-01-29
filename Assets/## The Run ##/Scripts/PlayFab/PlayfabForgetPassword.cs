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
// <summary>Used for reset  user password of PlayFab Account</summary>
// ***********************************************************************

using System.Runtime.InteropServices;
using PlayFab.ClientModels;
using UnityEngine;


namespace PlayFab
{
    /// <summary>
    /// Class for reset forget password.
    /// </summary>
    public static class PlayfabForgetPassword
    {
        #region Function

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="registerEmailAddress">The register email address.</param>
        public static void ResetPassword(string registerEmailAddress)
        {
            //create an request for user.
            var requestforAccountRecoveryEmail = new SendAccountRecoveryEmailRequest
            {
                Email = registerEmailAddress,
                TitleId = PlayFabSettings.TitleId,
            };

            //fire teh Api
           PlayFabClientAPI.SendAccountRecoveryEmail(request : requestforAccountRecoveryEmail , resultCallback : Onsuccess , errorCallback :OnFailure);
        }


        /// <summary>
        /// On successes of APi call the specified result is received.
        /// </summary>
        /// <param name="result">The result.</param>
        private static void Onsuccess(SendAccountRecoveryEmailResult result)
        {
            //TODO: stop the log for main build.
            Debug.Log("Result received");
          

            //Registered success fully.
            UserFormController.Reference.ShowMessage("Email send successfully", true);
        }

        /// <summary>
        /// Called when API failed to send password recovery email .
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
 