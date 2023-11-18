using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class RealTimeDB_FB : MonoBehaviour
{
    #region public fields
    public string dataBaseUrl = "https://unity-firebase-test-f29fa-default-rtdb.firebaseio.com/";

    public TMP_InputField emailSignUp, passwordSignUp, userNameSignUP;
    public TMP_InputField emailLogin, passwordLogin;
    #endregion

    #region private fields
    private const string Exp = "Experiences";
    private float expStartTime;
    private FirebaseDatabase database;

    //auth 
    private string custom_token = "qwertyui12345op";
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
	#endregion

	// Start is called before the first frame update
	void Start()
    {
        
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                InitializeFirebase();
                FirebaseApp app = FirebaseApp.DefaultInstance;
                //auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                database = FirebaseDatabase.GetInstance(dataBaseUrl);
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        //{
        //    FirebaseApp app = FirebaseApp.DefaultInstance;
        //    auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        //    //authenticateUser();
        //});
    }

    private void authenticateUser()
	{
        if (auth != null)
		{
            auth.SignInWithCustomTokenAsync(custom_token).ContinueWithOnMainThread(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCustomTokenAsync was canceled.++++");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCustomTokenAsync encountered an error:+++++ " + task.Exception);
                    return;
                }

                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully ++++++ : {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
            });
        }
    }

    public void signUpUser()
	{
        if (string.IsNullOrEmpty(emailSignUp.text) && string.IsNullOrEmpty(passwordSignUp.text))
		{
            Debug.Log(" please fill all fields before signup");
            return;
		}
        createNewUser(emailSignUp.text, passwordSignUp.text, userNameSignUP.text);
	}

    public void signinUser()
	{
        if (string.IsNullOrEmpty(emailLogin.text) && string.IsNullOrEmpty(passwordLogin.text))
        {
            Debug.Log(" please fill all fields before signIn");
            return;
        }
        signinExistingUser(emailLogin.text, passwordLogin.text);
    }

    private void signinExistingUser(string email, string password)
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
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        Debug.Log(GetErrorMessage(errorCode) + " is the error message");
                    }
                }
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully ++++++: {0} ({1})({2})",
                result.User.DisplayName, result.User.UserId, result.User.Email);

            //update these displayname and userid and email in UI if req.
        });
    }

    private void createNewUser(string email, string password, string userName)
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
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        Debug.Log(GetErrorMessage(errorCode) + " is the error message");
                    }
                }
                
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            updateUserProfile(userName);
        });
    }

    public void startExperience()
    {
        expStartTime = Time.time;
    }

    public void checkAndSetExperienceUseTime(String ExpName)
    {
        database.GetReference(Exp).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            ExpName = ExpName.ToLower();

            //calculate total exp use time
            float expUseTime = Time.time - expStartTime;
            TimeSpan totalTimeSpan = TimeSpan.FromSeconds(expUseTime);
            expStartTime = Time.time;

            if (task.IsFaulted)
            {
                Debug.Log("error while getting reference of data");
            }
            else if (task.IsCompleted)            
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.HasChild(ExpName))
                {
                    string timeSpanValue = (string)snapshot.Child(ExpName).Value;

                    string[] arr = timeSpanValue.Split(' ');

                    TimeSpan netTimeSpan = totalTimeSpan.Add(TimeSpan.Parse($"{arr[0]}:{arr[2]}:{arr[4]}"));

                    string timeFormat = $"{netTimeSpan.Hours} Hrs {netTimeSpan.Minutes} Mins {netTimeSpan.Seconds} Secs";
                    
                    database.GetReference(Exp).Child(ExpName).SetValueAsync(timeFormat);
                }
                else
                {
                    string timeFormat = $"{totalTimeSpan.Hours} Hrs {totalTimeSpan.Minutes} Mins {totalTimeSpan.Seconds} Secs";
                    database.GetReference(Exp).Child(ExpName).SetValueAsync(timeFormat);
                }
            }
        });
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    void updateUserProfile(string userName)
	{
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = userName,
                PhotoUrl = new System.Uri("https://s3-ap-south-1.amazonaws.com/co.techxr.system.backend.upload.dev/ohDvUhooMFdURyrR_2023-04-12T140611902088.ChickenLifeCycle.jpg"),
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
            });
        }
    }

    private static string GetErrorMessage(AuthError errorCode)
    {
        var message = "";
        switch (errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                message = "Account does not exist";
                break;
            case AuthError.MissingPassword:
                message = "missing password";
                break;
            case AuthError.WeakPassword:
                message = "Password so weak";
                break;
            case AuthError.WrongPassword:
                message = "Wrong password";
                break;
            case AuthError.EmailAlreadyInUse:
                message = "Your email already in exists";
                break;
            case AuthError.InvalidEmail:
                message = "Enter valid email";
                break;
            case AuthError.MissingEmail:
                message = "Your email is missing";
                break;
            default:
                message = "Invalid error";
                break;
        }
        return message;
    }
}
