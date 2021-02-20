using System;
using System.Collections;
using Firebase;
using Telegram.Core;
using Telegram.Data;
using Firebase.Auth;
using Loading;
using UnityEngine;

namespace Telegram.Auth
{
    public class PhoneManager : Singleton<PhoneManager>
    {
        public FirebaseAuth Auth { get; set; }
        public FirebaseUser User { get; private set; }
        private DependencyStatus _dependencyStatus = DependencyStatus.UnavailableOther;

        private string UserId { get; set; }

        private static float _elapsedTime;

        private PhoneAuthProvider _provider;
        private string _verificationId;
        private string _phoneNumber;
        private const uint PhoneAuthTimeout = 120000;

        public event Action<UserModel> OnUserNode;
        public event Action OnCodeSent;

        public IEnumerator InitAuthAsync()
        {
            _elapsedTime = 0;

            var task = FirebaseApp.CheckAndFixDependenciesAsync();
            yield return new WaitWhile(() => IsTask(task.IsCompleted));

            if (!task.IsCompleted)
            {
                Debug.LogError("ERROR" + task.Exception);
                yield break;
            }

            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("ERROR" + task.Exception);
                yield break;
            }

            _dependencyStatus = task.Result;

            if (_dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase initializing...");
                InitAuth();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + _dependencyStatus);
            }
        }

        private void InitAuth()
        {
            Auth = FirebaseAuth.DefaultInstance;
            Auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
        }

        private void AuthStateChanged(object sender, EventArgs eventArgs)
        {
            Debug.Log("Auth State Changed");

            if (Auth.CurrentUser != User)
            {
                var signedIn = User != Auth.CurrentUser && Auth.CurrentUser != null;

                if (!signedIn && User != null)
                {
                    Debug.Log("Signed out " + User.UserId);
                }

                User = Auth.CurrentUser;
                if (User != null)
                {
                    UserId = User.UserId;

                    if (signedIn)
                    {
                        Debug.Log("Signed in " + User.UserId);
                    }
                }
            }
            else
            {
                Debug.Log("Sign-in not done");
            }
        }

        public void VerifyPhoneNumber(string phoneNumber)
        {
            _phoneNumber = phoneNumber;

            _provider = PhoneAuthProvider.GetInstance(Auth);
            _provider.VerifyPhoneNumber(_phoneNumber, PhoneAuthTimeout, null,
                verificationCompleted: (credential) =>
                {
                    print("verification completed");
                },
                verificationFailed: (error) =>
                {
                    print("verification failed");
                },
                codeSent: (id, token) =>
                {
                    print("code sent");

                    _verificationId = id;

                    OnCodeSent?.Invoke();
                },
                codeAutoRetrievalTimeOut: (id) =>
                {
                    print("code auto retrieval timed Out");
                });
        }

        public void VerifyCode(string code, Action<bool, string> cb)
        {
            StartCoroutine(VerifyCodeAsync(code, cb));
        }
        private IEnumerator VerifyCodeAsync(string code, Action<bool, string> cb)
        {
            LoadingPanel.Instance.LoadingStart(ELoading.Load);

            _elapsedTime = 0;

            var credential = _provider.GetCredential(_verificationId, code);

            var task = Auth.SignInWithCredentialAsync(credential);
            yield return new WaitWhile(() => IsTask(task.IsCompleted));

            if (task.IsFaulted || task.IsCanceled)
            {
                cb(true, "The phone auth credential was created\nwith an empty SMS verification Code.");
                LoadingPanel.Instance.LoadingStop();
                yield return null;
            }
            else
            {
                yield return StartCoroutine(FirebaseCore.GetPlayer((error, model) =>
                {
                    if (error)
                    {
                        var newUser = task.Result;
                        var userName = "  ";
                        var phoneNumber = newUser.PhoneNumber;
                        var photoUrl = "  ";

                        PlayerPrefs.SetString(PrefsKeys.Name, "  ");
                        PlayerPrefs.SetString(PrefsKeys.Phone, "  ");
                        PlayerPrefs.SetString(PrefsKeys.PhotoUrl, "  ");
                        PlayerPrefs.Save();

                        var user = new UserModel(userName, phoneNumber, photoUrl);
                        FirebaseCore.CreateNewUser(user, newUser.UserId);

                        cb(false, phoneNumber);
                        LoadingPanel.Instance.LoadingStop();
                    }
                    else
                    {
                        PlayerPrefs.SetString(PrefsKeys.Name, model.UserName);
                        PlayerPrefs.SetString(PrefsKeys.Phone, model.PhoneNumber);
                        PlayerPrefs.SetString(PrefsKeys.PhotoUrl, model.PhotoUrl);
                        PlayerPrefs.Save();

                        OnUserNode?.Invoke(model);

                        LoadingPanel.Instance.LoadingStop();
                    }
                }));
            }
        }

        private static bool IsTask(bool value)
        {
            _elapsedTime += Time.deltaTime;

            if (value)
            {
                return false;
            }
            else
            {
                if (_elapsedTime > 10)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public void Logout()
        {
            Auth.SignOut();

            PlayerPrefs.SetString(PrefsKeys.Name, "  ");
            PlayerPrefs.SetString(PrefsKeys.PhotoUrl, "  ");
            PlayerPrefs.Save();
        }

        private void OnDestroy()
        {
            Auth.StateChanged -= AuthStateChanged;
            Auth = null;
        }
    }
}