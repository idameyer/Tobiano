using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using TMPro;

public class LoginScript : MonoBehaviour
{

    //https://www.youtube.com/watch?v=V143hJru1-g

    public GameObject loginPanel, registerPanel, forgotPWPanel, errorPanel;
    public TMP_InputField loginEmail, loginPassword, registerUsername, registerEmail, registerPassword, registerRPassword, forgotPWEmail;
    public TMP_Text errorTitle, errorText;
    public Toggle rememberMe;
    public static string profileUserName;
    public static string userID;
    FirebaseAuth auth;
    FirebaseUser user;

    //Tab to change fields
    public int InputSelected;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            InputSelected--;
            if (InputSelected > 1) InputSelected = 0;
            SelectInputField();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InputSelected++;
            if (InputSelected > 1) InputSelected = 0;
            SelectInputField();
        }
        void SelectInputField()
        {
            switch (InputSelected)
            {
                case 0: loginEmail.Select();
                    break;
                case 1:
                    loginPassword.Select();
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoginUser();
        }
    }
    public void SelectEmailInput() => InputSelected = 0;
    public void SelectPasswordInput() => InputSelected = 1;



    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                InitializeFirebase();
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

    }

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        forgotPWPanel.SetActive(false);
    }

    public void OpenRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        forgotPWPanel.SetActive(false);
    }

    public void OpenForgotPWPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        forgotPWPanel.SetActive(true);
    }


    public void LoginUser()
    {
        if (string.IsNullOrEmpty(loginEmail.text) && string.IsNullOrEmpty(loginPassword.text))
        {
            ShowErrorMessage("Error", "Please fill in all fields");
        }
        //login user

        SignInUser(loginEmail.text, loginPassword.text);
    }

    public void RegisterUser()
    {
        if (string.IsNullOrEmpty(registerEmail.text) && string.IsNullOrEmpty(registerPassword.text) && string.IsNullOrEmpty(registerRPassword.text))
        {
            ShowErrorMessage("Error", "Please fill in all fields");
        }
        //register user
        CreateUser(registerEmail.text, registerPassword.text, registerUsername.text);
    }

    public void RetrievePassword()
    {
        if (string.IsNullOrEmpty(forgotPWEmail.text))
        {
            ShowErrorMessage("Error", "Email field empty");
        }
        //get pw

        ForgotPasswordSubmit(forgotPWEmail.text);

    }

    private void ShowErrorMessage(string title, string message)
    {
        errorTitle.text = "" + title;
        errorText.text = "" + message;

        errorPanel.SetActive(true);
    }

    public void CloseErrorPanel()
    {
        errorTitle.text = "";
        errorText.text = "";

        errorPanel.SetActive(false);
    }

    //public void LogOut()
    //{
    //    //close profile window
    //    //hide profile text
    //    //switch to login scene
    //}

    void CreateUser(string email, string password, string Username)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    FirebaseException firebaseEx = exception as FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        ShowErrorMessage("Error", GetErrorMessage(errorCode));
                    }

                }
                return;

            }

            // Firebase user has been created.
            FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            UpdateUserProfile(Username);
        });
    }

    public void SignInUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    FirebaseException firebaseEx = exception as FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        ShowErrorMessage("Error", GetErrorMessage(errorCode));
                    }
                }

                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            profileUserName = newUser.DisplayName;
            userID = newUser.UserId;
            SceneManager.LoadScene("Stables");

        });
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                //isSignIn = true;
            }
        }
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    void UpdateUserProfile(string UserName)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            UserProfile profile = new UserProfile
            {
                DisplayName = UserName,
                PhotoUrl = new Uri("https://via.placeholder.com/150c/0%20https://placeholder.com/"),
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");

                ShowErrorMessage("Success!", "Created a new user");
            });
        }
    }

    //bool isSigned = false;
    //private void Update()
    //{
    //    if (isSignIn)
    //    {
    //        isSigned = true;
    //    }
    //}

    private static string GetErrorMessage(AuthError errorCode)
    {
        var message = "";
        switch (errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                message = "This account does not exist";
                break;
            case AuthError.MissingPassword:
                message = "Missing password";
                break;
            case AuthError.WeakPassword:
                message = "Password is weak";
                break;
            case AuthError.WrongPassword:
                message = "Incorrect password";
                break;
            case AuthError.EmailAlreadyInUse:
                message = "This email is already in use";
                break;
            case AuthError.InvalidEmail:
                message = "Invalid email";
                break;
            case AuthError.MissingEmail:
                message = "Email must be filled out";
                break;
            default:
                message = "Invalid error";
                break;
        }
        return message;
    }

    void ForgotPasswordSubmit(string forgotPWEmail)
    {
        auth.SendPasswordResetEmailAsync(forgotPWEmail).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SendPasswordResetEmailAsync was canceled");
            }
            if (task.IsFaulted)
            {
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    FirebaseException firebaseEx = exception as FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        ShowErrorMessage("Error", GetErrorMessage(errorCode));
                    }
                }

            }

            ShowErrorMessage("Success", "New password has been sent");

        }
        );
    }
    
}
