// ***********************************************************************
 // Assembly         : Assembly-CSharp
 // Author           : Reezoo
 // Created          : 04-22-2018
 //
 // Last Modified By : Reezoo
 // Last Modified On : 04-24-2018
 // ***********************************************************************
 // <copyright file="UserFormController.cs" author="reezoo">
 //     Copyright (c) . All rights reserved.
 // </copyright>
 // <summary>Control user Registration , Login , Forget password.</summary>
 // ***********************************************************************
 
 using System;
 using System.Collections;
 using System.Collections.Generic;
 using System.Linq;
 using UnityEngine;
 using UnityEngine.UI;
 using PlayFab;
 using ValintaMusicStreaming.SimpleJSON;
 
 
 /// <summary>
 /// Class User Form Controller.
 /// Control user Registration , Login , Forget password.
 /// </summary>
 /// <seealso cref="UnityEngine.MonoBehaviour" />
 public class UserFormController : MonoBehaviour
 {
     #region public variables 
     /// <summary>
     /// Amount of JoegamesDollar will be served to a new register user .
     /// </summary>
     [Header("Register user Reward JGD")]
     public string RegisterUser_JoegamesDollar="3000";
 
     /// <summary>
     /// The login text .
     /// </summary>
     [Header("Login Button")]
     public GameObject LoginButton;
     /// <summary>
     /// The main menu .
     /// </summary>
     [Header("Main Menu")]
     public GameObject MainMenu;
     /// <summary>
     /// The login page .
     /// </summary>
     [Header("Login Panel")] public GameObject LoginPage;
 
     /// <summary>
     /// The register page.
     /// </summary>
     [Header("Register For new User panel")]
     public GameObject RegisterPage;
 
     /// <summary>
     /// The forget password page.
     /// </summary>
     [Header("Forget Password panel")] public GameObject ForgetPasswordPage;
 
     /// <summary>
     /// The login email address .
     /// </summary>
     [Space(30f)] [Header("Email address for login")]
     public InputField Login_EmailAddress;
 
     /// <summary>
     /// The login password .
     /// </summary>
     [Header("Email address for login")] public InputField Login_Password;
 
     /// <summary>
     /// The login loading panel .
     /// </summary>
     [Header("Loading Panel for login")] public GameObject Login_LoadingPanel;
 
     /// <summary>
     /// The login message panel .
     /// </summary>
     [Header("Message Panel for login")] public GameObject Login_MessagePanel;
 
     /// <summary>
     /// The login user input panel .
     /// </summary>
     [Header("User Input Panel for login")] public GameObject Login_UserInputPanel;
 
 
     /// <summary>
     /// The login close button .
     /// </summary>
     [Header("Close button Of login")] public GameObject Login_CloseButton;
 
     /// <summary>
     /// The login message.
     /// </summary>
     [Header("Message text for login")] public Text Login_message;
 
     /// <summary>
     /// The register user name.
     /// </summary>
     [Space(30f)] [Header("User name for Register an user")]
     public InputField Register_UserName;
 
     /// <summary>
     /// The register email identifier.
     /// </summary>
     [Header("Email id for Register an user")]
     public InputField Register_EmailId;
 
     /// <summary>
     /// The register password .
     /// </summary>
     [Header("Password for Register an user")]
     public InputField Register_Password;
 
     /// <summary>
     /// The register loading panel.
     /// </summary>
     [Header("Loading Panel for Register")] public GameObject Register_LoadingPanel;
 
     /// <summary>
     /// The register message panel.
     /// </summary>
     [Header("Message Panel for Register")] public GameObject Register_MessagePanel;
 
     /// <summary>
     /// The register user input panel.
     /// </summary>
     [Header("User Input Panel for register")]
     public GameObject Register_UserInputPanel;
 
     /// <summary>
     /// The register message
     /// </summary>
     [Header("Message text for Register")] public Text Register_message;
 
     /// <summary>
     /// The forget password email identifier.
     /// </summary>
     [Space(30f)] [Header("Email id for ForgetPassword")]
     public InputField ForgetPassword_EmailId;
 
     /// <summary>
     /// The forget password loading panel.
     /// </summary>
     [Header("Loading Panel for ForgetPassword")]
     public GameObject ForgetPassword_LoadingPanel;
 
     /// <summary>
     /// The forget password message panel.
     /// </summary>
     [Header("Message Panel for ForgetPassword")]
     public GameObject ForgetPassword_MessagePanel;
 
     /// <summary>
     /// The forget password user input panel.
     /// </summary>
     [Header("User Input Panel for ForgetPassword")]
     public GameObject ForgetPassword_UserInputPanel;
 
     /// <summary>
     /// The forget password message
     /// </summary>
     [Header("Message text for ForgetPassword")]
     public Text ForgetPassword_message;
 
     /// <summary>
     /// The reference
     /// </summary>
     public static UserFormController Reference;
 
     /// <summary>
     /// Pages that will be visited .
     /// </summary>
     private enum Pages
     {
         /// <summary>
         /// The login page
         /// </summary>
         LoginPage = 0,
 
         /// <summary>
         /// The register page
         /// </summary>
         RegisterPage = 1,
 
         /// <summary>
         /// The forget password page
         /// </summary>
         ForgetPasswordPage = 2
     }
 
     /// <summary>
     /// Current Page as a Game object.
     /// </summary>
     /// <value>The current page.</value>
     private GameObject CurrentPage
     {
         get
         {
             switch (_currentPage)
             {
                 case Pages.LoginPage:
                     return LoginPage;
                 case Pages.RegisterPage:
                     return RegisterPage;
                 case Pages.ForgetPasswordPage:
                     return ForgetPasswordPage;
                 default:
                     return null;
             }
         }
     }
 
     #endregion
     
     
 
     #region private variables
 
     /// <summary>
     /// Current page we are now .
     /// </summary>
     private Pages _currentPage;
 
     /// <summary>
     /// Time for which message will be displayed.
     /// </summary>
     private float _messagedisplayTime = 1.3f;
 
     #endregion
 
     #region Unity Function
 
     /// <summary>
     /// CAlls only when the page game object is enabled.
     /// </summary>
     private void OnEnable()
     {
         InitiateDefaultLayout();
     }
 
     /// <summary>
     ///     Awakes this instance.
     /// </summary>
     private void Awake()
     {
         //Single tone
         if (Reference == null) Reference = this;
     }
 
     #endregion
 
     #region Normal Functions
 
 
 
 
     /// <summary>
     /// Switch between pages.
     /// </summary>
     /// <param name="pages">The pages.</param>
     private void SwitchPage(Pages pages)
     {
         //deactivate the current page
         CurrentPage.SetActive(false);
 
         //Activate the corresponding Page.
         switch (pages)
         {
             case Pages.LoginPage:
                 LoginPage.SetActive(true);
                 _currentPage = Pages.LoginPage;
                 DefaultLayoutForLogin();
                 break;
             case Pages.RegisterPage:
                 RegisterPage.SetActive(true);
                 _currentPage = Pages.RegisterPage;
                 DefaultLayoutForRegister();
                 break;
             case Pages.ForgetPasswordPage:
                 ForgetPasswordPage.SetActive(true);
                 _currentPage = Pages.ForgetPasswordPage;
                 DefaultLayoutForForgetPassword();
                 break;
             default:
                 break;
         }
     }
 
     /// <summary>
     /// Log in requested.
     /// </summary>
     public void Login()
     {
 
         //First Check the mandatory fields are filled.
         if (!string.IsNullOrEmpty(Login_EmailAddress.text) && !string.IsNullOrEmpty(Login_Password.text))
         {
             Debug.Log("Login requested");
             //create login.
             LoginUser(Login_Password.text, Login_EmailAddress.text);

             //PlayfabLogin.Login(Login_Password.text, Login_EmailAddress.text);
             //Login_LoadingPanel.SetActive(true);
         }
         else
         {
             ShowMessage("All fields are mandatory.", false);
         }
     }
 
     /// <summary>
     /// Default Layout will be initiated.
     /// </summary>
     private void InitiateDefaultLayout()
     {
         //If logged in make close button set active true .
         if (PlayfabData.IsPlayerRegisteredToPlayFab) Login_CloseButton.SetActive(true);
         //Check if registered then activate main menu .
         if (!PlayfabData.IsPlayerRegisteredToPlayFab) MainMenu.SetActive(false);
         //Deactivate all the windows except login 
         if (LoginPage != null && !LoginPage.activeInHierarchy) LoginPage.SetActive(true);
         //Register Page.
         if (RegisterPage != null && RegisterPage.activeInHierarchy) RegisterPage.SetActive(false);
         //ForgetPassword Page
         if (ForgetPasswordPage.activeInHierarchy && ForgetPasswordPage != null) ForgetPasswordPage.SetActive(false);
     }
 
     /// <summary>
     /// Shows message .
     /// </summary>
     /// <param name="message">The message.</param>
     /// <param name="success">True when shoing message for success</param>
     public void ShowMessage(string message, bool success)
     {
         //If current page is Log in page
         if (_currentPage == Pages.LoginPage)
         {
             //First check if loading is enable or not ?
             if (Login_LoadingPanel != null && Login_LoadingPanel.activeInHierarchy) Login_LoadingPanel.SetActive(false);
 
             //Now enable message window.
             if (Login_MessagePanel != null && !Login_MessagePanel.activeInHierarchy) Login_MessagePanel.SetActive(true);
 
             //Show message
             Login_message.text = message;
         }
 
         //If current page is Register in page
         if (_currentPage == Pages.RegisterPage)
         {
             //First check if loading is enable or not ?
             if (Register_LoadingPanel != null && Register_LoadingPanel.activeInHierarchy)
                 Register_LoadingPanel.SetActive(false);
 
             //Now enable message window.
             if (Register_MessagePanel != null && !Register_MessagePanel.activeInHierarchy)
                 Register_MessagePanel.SetActive(true);
 
             //Show message
             Register_message.text = message;
         }
 
         //If current page is Register in page
         if (_currentPage == Pages.ForgetPasswordPage)
         {
             //First check if loading is enable or not ?
             if (ForgetPassword_LoadingPanel != null && ForgetPassword_LoadingPanel.activeInHierarchy)
                 ForgetPassword_LoadingPanel.SetActive(false);
 
             //Now enable message window.
             if (ForgetPassword_MessagePanel != null && !ForgetPassword_MessagePanel.activeInHierarchy)
                 ForgetPassword_MessagePanel.SetActive(true);
 
             //Show message
             ForgetPassword_message.text = message;
         }
 
         DeactivatePanel(success);
     }
 
     /// <summary>
     /// Deactivates the panel.
     /// </summary>
     /// <param name="status">The status indicate message is a error message or success message</param>
     private void DeactivatePanel(bool status)
     {
         //hook the enumerator.
         var deactivate = DeactivateMessagePanel(status);
         //enumerator star.
         StartCoroutine(deactivate);
     }
 
     /// <summary>
     /// De activate the panel.
     /// </summary>
     /// <param name="status">The status indicate message is a error message or success message</param>
     /// <returns>IEnumerator.</returns>
     private IEnumerator DeactivateMessagePanel(bool status)
     {
         //Wait for certain time.
         yield return new WaitForSecondsRealtime(_messagedisplayTime);
 
         //If current page is the log in page
         if (_currentPage == Pages.LoginPage)
             if (Login_MessagePanel != null && Login_MessagePanel.activeInHierarchy)
             {
                 //deactivate it.
                 Login_MessagePanel.SetActive(false);
                 //clean the input fields.
                 if (status)
                 {
                     Login_EmailAddress.text = "";
                     Login_Password.text = "";
                 }
             }
 
         //if current page is Register page
         if (_currentPage == Pages.RegisterPage)
             if (Register_MessagePanel != null && Register_MessagePanel.activeInHierarchy)
             {
                 //deactivate it.
                 Register_MessagePanel.SetActive(false);
                 if (status)
                 {
                     Register_EmailId.text = "";
                     Register_Password.text = "";
                     Register_UserName.text = "";
                 }
             }
 
         //if current page  is ForgetPassword
         if (_currentPage == Pages.ForgetPasswordPage)
             if (ForgetPassword_MessagePanel != null && ForgetPassword_MessagePanel.activeInHierarchy)
             {
                 //deactivate it.
                 ForgetPassword_MessagePanel.SetActive(false);
                 if (status) ForgetPassword_EmailId.text = "";
             }
 
         //if success is true
         //Switch to login page if in other page.
         //else go to home page.
         if (status)
         {
             if (_currentPage == Pages.LoginPage)
             {
                 //If main menu is not active.
                 if (!MainMenu.activeInHierarchy) MainMenu.SetActive(true);
                 //Get the Image and activate it.
                 LoginButton.transform.GetChild(1).gameObject.SetActive(true);
                 //Go to home page .
                 UMP_Manager.Reference.ChangeWindow(0);
             }
             else
             {
                 SwitchPage(Pages.LoginPage);
             }
         }
             
     }
 
     /// <summary>
     /// sign in.
     /// </summary>
     public void SignIn()
     {
         SwitchPage(Pages.RegisterPage);
     }
 
     /// <summary>
     /// Forget password.
     /// </summary>
     public void ForGetPassword()
     {
         SwitchPage(Pages.ForgetPasswordPage);
     }
 
     /// <summary>
     /// Default Lay out for Log in page.
     /// </summary>
     private void DefaultLayoutForLogin()
     {
        
         //If logged in make close button set active true .
         if (PlayfabData.IsPlayerRegisteredToPlayFab) Login_CloseButton.SetActive(true);
         //All the panels will be De active only user in put panel will be active.
         if (Login_UserInputPanel != null && !Login_UserInputPanel.activeInHierarchy)
             Login_UserInputPanel.SetActive(true);
         if (Login_MessagePanel != null && Login_MessagePanel.activeInHierarchy) Login_MessagePanel.SetActive(false);
         if (Login_LoadingPanel != null && !Login_LoadingPanel.activeInHierarchy) Login_LoadingPanel.SetActive(false);
     }
 
 
 
     /// <summary>
     /// Chcek the email address is in valid format or not .
     /// </summary>
     /// <param name="email"></param>
     /// <returns></returns>
     private bool IsValidEmail(string email)
     {
         if (string.IsNullOrEmpty(email)) return false;
 
         // MUST CONTAIN ONE AND ONLY ONE @
         var atCount = email.Count(c => c == '@');
         if (atCount != 1) return false;
 
         // MUST CONTAIN PERIOD
         if (!email.Contains(".")) return false;
 
         // @ MUST OCCUR BEFORE LAST PERIOD
         var indexOfAt = email.IndexOf("@", StringComparison.Ordinal);
         var lastIndexOfPeriod = email.LastIndexOf(".", StringComparison.Ordinal);
         var atBeforeLastPeriod = lastIndexOfPeriod > indexOfAt;
         if (!atBeforeLastPeriod) return false; 
         try
         {
             var addr = new System.Net.Mail.MailAddress(email);
             return addr.Address == email;
         }
         catch
         {
             return false;
         }
     }
     
     /// <summary>
     /// Check valid password.
     /// </summary>
     /// <param name="password"></param>
     /// <returns></returns>
     /// <exception cref="ArgumentNullException"></exception>
      private bool IsValidPassword( string password )
         {
             const int minLength =  8 ;
             const int maxLength = 15 ;
 
             if ( password == null ) throw new ArgumentNullException() ;
 
             var meetsLengthRequirements = password.Length >= minLength && password.Length <= maxLength ;
             var hasUpperCaseLetter      = false ;
             var hasLowerCaseLetter      = false ;
             var hasDecimalDigit         = false ;
 
             if ( meetsLengthRequirements )
                 foreach (var c in password )
                     if      ( char.IsUpper(c) ) hasUpperCaseLetter = true ;
                     else if ( char.IsLower(c) ) hasLowerCaseLetter = true ;
                     else if ( char.IsDigit(c) ) hasDecimalDigit    = true ;
 
             var isValid = meetsLengthRequirements
                            && hasUpperCaseLetter
                            && hasLowerCaseLetter
                            && hasDecimalDigit
                 ;
             return isValid ;
 
         }
 
     /// <summary>
     /// Register an new user.
     /// </summary>
     public void NewUserSignUp()
     {
         //First Check the mandatory fields are filled.
         if (!string.IsNullOrEmpty(Register_EmailId.text) && !string.IsNullOrEmpty(Register_UserName.text) &&
             !string.IsNullOrEmpty(Register_Password.text))
         {
             if (IsValidEmail(Register_EmailId.text))
             {
                 if (IsValidPassword(Register_Password.text))
                 {
                     //TODO:Log
                     Debug.Log("Register requested");
                     //PlayfabRegistration.Register(Register_UserName.text, Register_EmailId.text,     Register_Password.text, true,
                     // withUserName: Register_UserName.text);
                     // Register_LoadingPanel.SetActive(true);
                     RegisterUser(Register_UserName.text, Register_Password.text, Register_EmailId.text,
                         RegisterUser_JoegamesDollar);
                 }
                 else
                 {
                     //Show message.
                     ShowMessage("Password is not in valid format", false);
                 }
             }
             else
             {
                 //Show message.
                 ShowMessage("Email is not valid .", false);
             }
         }
         else
         {
             //Show message.
             ShowMessage("All fields are mandatory.", false);
         }
     }
 
     /// <summary>
     /// Close button on register page .
     /// </summary>
     public void CloseRegisterPage()
     {
         SwitchPage(Pages.LoginPage);
     }
 
     /// <summary>
     /// Default layout for register page.
     /// </summary>
     private void DefaultLayoutForRegister()
     {
         //All the panels will be De active only user in put panel will be active.
         if (Register_UserInputPanel != null && !Register_UserInputPanel.activeInHierarchy)
             Register_UserInputPanel.SetActive(true);
         if (Register_MessagePanel != null && Register_MessagePanel.activeInHierarchy)
             Register_MessagePanel.SetActive(false);
         if (Register_LoadingPanel != null && !Register_LoadingPanel.activeInHierarchy)
             Register_LoadingPanel.SetActive(false);
     }
 
     /// <summary>
     /// Default layout for register page.
     /// </summary>
     private void DefaultLayoutForForgetPassword()
     {
         //All the panels will be De active only user in put panel will be active.
         if (ForgetPassword_UserInputPanel != null && !ForgetPassword_UserInputPanel.activeInHierarchy)
             ForgetPassword_UserInputPanel.SetActive(true);
         if (ForgetPassword_MessagePanel != null && ForgetPassword_MessagePanel.activeInHierarchy)
             ForgetPassword_MessagePanel.SetActive(false);
         if (ForgetPassword_LoadingPanel != null && !ForgetPassword_LoadingPanel.activeInHierarchy)
             ForgetPassword_LoadingPanel.SetActive(false);
     }
 
     /// <summary>
     /// Close button on Forget Password page .
     /// </summary>
     public void CloseForgetPasswordPage()
     {
         SwitchPage(Pages.LoginPage);
     }
 
     /// <summary>
     /// Retrieves the password by mail.
     /// </summary>
     public void RetrievePasswordByMail()
     {
         //check mandatory fields are filled up.
         if (!string.IsNullOrEmpty(ForgetPassword_EmailId.text))
         {
             Debug.Log("Request for reset password");
             PlayfabForgetPassword.ResetPassword(ForgetPassword_EmailId.text);
             ForgetPassword_LoadingPanel.SetActive(true);
         }
         else
         {
             ShowMessage("All fields are mandatory.", false);
         }
     }
 
 
     #endregion
 
     #region For creating register request .
 

 
     /// <summary>
     /// Register an user .
     /// </summary>
     /// <param name="username"> user name for user</param>
     /// <param name="password">password for user </param>
     /// <param name="emailid">email id for user .</param>
     /// <param name="joeGamesDollar">jgd for user </param>
     private void RegisterUser(string username, string password, string emailid, 
         string joeGamesDollar)
     {
         var registerRoutine = RegisterUserRequest(username, password, emailid, 
             joeGamesDollar, SystemInfo.deviceUniqueIdentifier);
         StartCoroutine(registerRoutine);
     }
 
 
     /// <summary>
     /// Register user .
     /// </summary>
     private IEnumerator RegisterUserRequest(string username , string password , string emailid , string joeGamesDollar, string deviceid="")
     {
         //activate loding panel .
         Register_LoadingPanel.SetActive(true);
         //Url need to heat .
         const string url = "https://long-run.herokuapp.com/Register";
         //var create json.
         var userregistration = new UserRegistration(
             username,
             password,
             emailid,
             joeGamesDollar,
             deviceid);
         //convert to json.
         var json = JsonUtility.ToJson(userregistration);
         //debug.
         Debug.Log("Print json"+json);
         //update json.
         json = json.Replace("'", "\"");
         //Encode the JSON string into a bytes
         var postData = System.Text.Encoding.UTF8.GetBytes(json);
         //create headers.
         //Add keys an values .
         var headers = new Dictionary<string, string> {{"Content-Type", "application/json"}};
         //Now call a new WWW request
         var www = new WWW(url,postData,headers);
         //Yield return www .
         yield return www;
 
         //deacctivate loding panel .
         Register_LoadingPanel.SetActive(false);
 
         //check for error.
         //Registered success fully.
         if (string.IsNullOrEmpty(www.error.ToString()))
         {
             var receivedData = JSON.Parse(www.text);
             ShowMessage(receivedData["message"], true);
             Debug.Log("Data" + JSON.Parse(www.text)["message"] + JSON.Parse(www.text)["user_details"].ToString());
         }
         else
         {
             var receivedData = www.error.ToString();
             ShowMessage(receivedData, false);
         }
 
         Debug.Log("Error" + www.error.ToString());
         
         //Debug.Log(www.responseHeaders+"Done"+www.isDone);
         //dispose.
         www.Dispose();
     }
 
     
 
     [Serializable]
     private class UserRegistration
     {
         //CAUTION:
         //NOTE:
         //DO NOT ALTER THE NAME OF PARAMETERS .
         //THE JSON WILL FAIL .
         /// <summary>
         /// User name .
         /// </summary>
         public  string username;
         /// <summary>
         /// Password .
         /// </summary>
         public string password;
         /// <summary>
         /// Emil id.
         /// </summary>
         public string email_id;
         /// <summary>
         /// JoeGames Doller .
         /// </summary>
         public string us_dollar;
         /// <summary>
         /// Device id .
         /// </summary>
         public  string device_id;
 
         /// <summary>
         /// Constructor .
         /// </summary>
         /// <param name="username"></param>
         /// <param name="password"></param>
         /// <param name="email_id"></param>
         /// <param name="us_dollar"></param>
         /// <param name="device_id"></param>
         public UserRegistration(string username, string password, string email_id, string us_dollar, string device_id)
         {
             this.username = username;
             this.password = password;
             this.email_id = email_id;
             this.us_dollar = us_dollar;
             this.device_id = device_id;
         }
 
     }
     #endregion


     #region ForLogin 
     
     
     /// <summary>
     /// Log in user .
     /// </summary>
     /// <param name="password"></param>
     /// <param name="emailid"></param>
     private void LoginUser(string password, string emailid)
     {
         var loginRoutine = LoginUserRoutine(password,emailid);
         StartCoroutine(loginRoutine);
     }
     
     
     private IEnumerator LoginUserRoutine(string password, string emailid)
     {
         //activate loding panel .
         Login_LoadingPanel.SetActive(true);
         //Url need to heat .
         const string url = "https://long-run.herokuapp.com/Login";
         //var create json.
         var login = new UserLogin(password, emailid);
         //convert to json.
         var json = JsonUtility.ToJson(login);
         //debug.
         Debug.Log("Print json"+json);
         //update json.
         json = json.Replace("'", "\"");
         //Encode the JSON string into a bytes
         var postData = System.Text.Encoding.UTF8.GetBytes(json);
         //create headers.
         //Add keys an values .
         var headers = new Dictionary<string, string> {{"Content-Type", "application/json"}};
         //Now call a new WWW request
         var www = new WWW(url,postData,headers);
         //Yield return www .
         yield return www;
 
         //deacctivate loding panel .
         Login_LoadingPanel.SetActive(false);
 
         //check for error.
         var receivedData = JSON.Parse(www.text);
         //message.
         var message = receivedData["Login"];
         
         //Login success fully.
         if (string.IsNullOrEmpty(www.error.ToString()))
         {
             ShowMessage(message, true);
             Debug.Log("Giving you user details"+ JSON.Parse(www.text)["user"].ToString());
         }
         else
         {
            
             ShowMessage(message, false);
         }
 
         Debug.Log("Error" + www.error.ToString());
         
         //Debug.Log(www.responseHeaders+"Done"+www.isDone);
         //dispose.
         www.Dispose();
     }

     [Serializable]
     private class UserLogin
     {
         /// <summary>
         /// Password .
         /// </summary>
         public string password;
         /// <summary>
         /// Emil id.
         /// </summary>
         public string email_id;
         
         /// <summary>
         /// Constructor.
         /// </summary>
         /// <param name="password"></param>
         /// <param name="email_id"></param>
         public UserLogin(string password, string email_id)
         {
             
             this.password = password;
             this.email_id = email_id;
             
         }
     }
     
     

     #endregion
 }