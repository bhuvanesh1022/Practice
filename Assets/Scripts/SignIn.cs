using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class SignIn : MonoBehaviour
{
    private string phoneNumber;
    private string secureCode;
    protected string receivedCode = "";
    private string verificationId;
    private string verificationCode;
    FirebaseAuth m_auth;
    public Text PhoneNumberInputFieldText;
    public Text SecureCodeInputFieldText;
    public Button SendSecureCodeButton;
    public Button SubmitSecureCodeButton;
    
  

    // The verification id needed along with the sent code for phone authentication.
    private string phoneAuthVerificationId;
    
    // Whether to sign in / link or reauthentication *and* fetch user profile data.
    protected bool signInAndFetchProfile = false;
    // Flag set when a token is being fetched.  This is used to avoid printing the token
    // in IdTokenChanged() when the user presses the get token button.
    private bool fetchingToken = false;
    
    //Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    public  void Start() {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                InitializeFirebase();
            } else {
                Debug.LogError(
                    "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
        SendSecureCodeButton.onClick.AddListener(VerifyPhoneNumber);
    }

    // Handle initialization of the necessary firebase modules:
    protected void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        m_auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    // void SignInMobile()
    // {
    //     //FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    //     phoneNumber = "+919344393169" ;
    //     Debug.Log(phoneNumber);
    //     uint phoneAuthTimeoutMs = 10000;
    //     PhoneAuthProvider provider = PhoneAuthProvider.GetInstance(m_auth);
    //     provider.VerifyPhoneNumber(phoneNumber, phoneAuthTimeoutMs,null,
    //         verificationCompleted: (credential) => {
    //             // Auto-sms-retrieval or instant validation has succeeded (Android only).
    //             // There is no need to input the verification code.
    //             // `credential` can be used instead of calling GetCredential().
    //             
    //         },
    //         verificationFailed: (error) => {
    //             // The verification code was not sent.
    //             // `error` contains a human readable explanation of the problem.
    //             Debug.Log("Vertification failed due to "+ error);
    //         },
    //         codeSent: (id, token) =>
    //         {
    //             // Verification code was successfully sent via SMS.
    //             // `id` contains the verification id that will need to passed in with
    //             // the code from the user when calling GetCredential().
    //             // `token` can be used if the user requests the code be sent again, to
    //             // tie the two requests together.
    //             Debug.Log("SMS Has been sent " + id);
    //         },
    //         codeAutoRetrievalTimeOut : (id) => {
    //             // Called when the auto-sms-retrieval has timed out, based on the given
    //             // timeout parameter.
    //             // `id` contains the verification id of the request that timed out.
    //         });
    // }
    
    // Begin authentication with the phone number.
    
    private void VerifyPhoneNumber() {
        var phoneAuthProvider = Firebase.Auth.PhoneAuthProvider.GetInstance(m_auth);
        phoneNumber = "919344393169" ;
        Debug.Log(phoneNumber);
        uint phoneAuthTimeoutMs = 60 * 1000;
        phoneAuthProvider.VerifyPhoneNumber(phoneNumber, phoneAuthTimeoutMs, null,
            verificationCompleted: (cred) => {
                Debug.Log("Phone Auth, auto-verification completed");
                // if (signInAndFetchProfile) {
                //     m_auth.SignInAndRetrieveDataWithCredentialAsync(cred).ContinueWithOnMainThread(
                //         HandleSignInWithSignInResult);
                // } else {
                //     m_auth.SignInWithCredentialAsync(cred).ContinueWithOnMainThread(HandleSignInWithUser);
                // }
            },
            verificationFailed: (error) => {
                Debug.Log("Phone Auth, verification failed: " + error);
            },
            codeSent: (id, token) => {
                phoneAuthVerificationId = id;
                Debug.Log("Phone Auth, code sent");
                Debug.Log(id);
            },
            codeAutoRetrievalTimeOut: (id) => {
                Debug.Log("Phone Auth, auto-verification timed out");
            });
    }

    // Sign in using phone number authentication using code input by the user.
    protected void VerifyReceivedPhoneCode() {
        var phoneAuthProvider = Firebase.Auth.PhoneAuthProvider.GetInstance(m_auth);
        // receivedCode should have been input by the user.
        var cred = phoneAuthProvider.GetCredential(phoneAuthVerificationId, receivedCode);
        if (signInAndFetchProfile) {
            m_auth.SignInAndRetrieveDataWithCredentialAsync(cred).ContinueWithOnMainThread(
                HandleSignInWithSignInResult);
        } else {
            m_auth.SignInWithCredentialAsync(cred).ContinueWithOnMainThread(HandleSignInWithUser);
        }
    }

    void HandleSignInWithSignInResult(Task<Firebase.Auth.SignInResult> task) {
        
        if (LogTaskCompletion(task, "Sign-in")) {
            DisplaySignInResult(task.Result, 1);
        }
    }
    
    // Log the result of the specified task, returning true if the task
    // completed successfully, false otherwise.
    protected bool LogTaskCompletion(Task task, string operation) {
        bool complete = false;
        if (task.IsCanceled) {
            Debug.Log(operation + " canceled.");
        } else if (task.IsFaulted) {
            Debug.Log(operation + " encounted an error.");
            foreach (Exception exception in task.Exception.Flatten().InnerExceptions) {
                string authErrorCode = "";
                Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                if (firebaseEx != null) {
                    authErrorCode = String.Format("AuthError.{0}: ",
                        ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                }
                Debug.Log(authErrorCode + exception.ToString());
            }
        } else if (task.IsCompleted) {
            Debug.Log(operation + " completed");
            complete = true;
        }
        return complete;
    }
    
    // Display user information reported
    protected void DisplaySignInResult(Firebase.Auth.SignInResult result, int indentLevel) {
        string indent = new String(' ', indentLevel * 2);
        //DisplayDetailedUserInfo(result.User, indentLevel);
        var metadata = result.Meta;
        if (metadata != null) {
            Debug.Log(String.Format("{0}Created: {1}", indent, metadata.CreationTimestamp));
            Debug.Log(String.Format("{0}Last Sign-in: {1}", indent, metadata.LastSignInTimestamp));
        }
        var info = result.Info;
        if (info != null) {
            Debug.Log(String.Format("{0}Additional User Info:", indent));
            Debug.Log(String.Format("{0}  User Name: {1}", indent, info.UserName));
            Debug.Log(String.Format("{0}  Provider ID: {1}", indent, info.ProviderId));
        }
    }
    
    // Called when a sign-in without fetching profile data completes.
    void HandleSignInWithUser(Task<Firebase.Auth.FirebaseUser> task)
    {
        if (LogTaskCompletion(task, "Sign-in"))
        {
            Debug.Log(String.Format("{0} signed in", task.Result.DisplayName));
        }
    }
}
