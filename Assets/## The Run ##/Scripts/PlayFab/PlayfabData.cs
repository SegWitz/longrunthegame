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
// <summary>Store Playfab related data</summary>
// **

using System;
using PlayFab.ClientModels;
using UnityEngine;

namespace PlayFab
{
    /// <summary>
    /// Class PlayfabData.
    /// stores data retrived from playfab
    /// </summary>
    public static class PlayfabData
    {

        /// <summary>
        /// Gets or sets a value indicating whether the  player is registered to PlayFab.
        /// </summary>
        /// <value><c>true</c> if player registered to PlayFab; otherwise, <c>false</c>.</value>
        public static bool IsPlayerRegisteredToPlayFab
        {
            get
            {
                if (PlayerPrefs.GetInt("PlayerRegistered", 0) == 0)
                    return false;
                return PlayerPrefs.GetInt("PlayerRegistered", 0) == 1;
            }
            set { PlayerPrefs.SetInt("PlayerRegistered", value == true ? 1 : 0); }
        }
        /// <summary>
        /// Gets or sets the playfab session ticket.
        /// The ticket can be received at the time of Login or Registration.
        /// </summary>
        /// <value>The playfab session ticket.</value>
        public static string PlayfabSessionTicket
        {
            get { return (PlayerPrefs.GetString("SessionTicket", string.Empty)); }
            set { PlayerPrefs.SetString("SessionTicket", value); }
        }

        /// <summary>
        /// Gets or sets the playfab Entity Token.
        /// The ticket can be received at the time of Login or Registration.
        /// </summary>
        /// <value>The playfab entity token</value>
        public static string PlayfbEntityToken
        {
            get { return (PlayerPrefs.GetString("EntityToken", string.Empty)); }
            set { PlayerPrefs.SetString("EntityToken", value); }
        }

        /// <summary>
        /// Gets or sets the user name corresponding to playfab account.
        /// The username can be received at the time of Login or Registration.
        /// </summary>
        /// <value>The playfab user name</value>
        public static string PlayfbUserName
        {
            get { return (PlayerPrefs.GetString("UserName", string.Empty)); }
            set { PlayerPrefs.SetString("UserName", value); }
        }

        /// <summary>
        /// Gets or sets the playfab account last login time.
        /// </summary>
        /// <value>The playfab account last login time.</value>
        public static string PlayfabAccountLastLoginTime
        {
            get { return (PlayerPrefs.GetString("LastLoginTime", string.Empty)); }
            set { PlayerPrefs.SetString("LastLoginTime", value); }
        }
    }
}