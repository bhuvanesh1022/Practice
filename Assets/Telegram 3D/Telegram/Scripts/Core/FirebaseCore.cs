using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
//using Firebase.Unity.Editor;
using Loading;
using Telegram.Auth;
using Telegram.Data;

namespace Telegram.Core
{
    public class FirebaseCore
    {
        private static DatabaseReference BaseRef { get; set; }
        private const string DataUrl = "https://fir-example-chat.firebaseio.com/";
        private static float _elapsedTime;

        public static void Init()
        {
            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DataUrl);
            BaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public static void CreateNewUser(UserModel userName, string uid)
        {
            var playerJson = JsonUtility.ToJson(userName);
            BaseRef.Child("users").Child(uid).SetRawJsonValueAsync(playerJson);
        }

        public static IEnumerator GetPlayer(Action<bool, UserModel> cb)
        {
            _elapsedTime = 0;

            if (PhoneManager.Instance.User == null)
            {
                cb(true, null);
                yield break;
            }

            var task = BaseRef.Child("users").Child(PhoneManager.Instance.User.UserId).GetValueAsync();
            yield return new WaitWhile(() => IsTask(task.IsCompleted));

            if (!task.IsCompleted)
            {
                LoadingPanel.Instance.LoadingStop();

                cb(true, null);
                yield return null;
            }

            if (task.IsFaulted || task.IsCanceled)
            {
                LoadingPanel.Instance.LoadingStop();

                cb(true, null);
                yield break;
            }

            var player = task.Result;

            if (player == null)
            {
                LoadingPanel.Instance.LoadingStop();
                Debug.Log("player null");

                cb(true, null);
                yield break;
            }

            if (player.Value == null)
            {
                LoadingPanel.Instance.LoadingStop();
                Debug.Log("value null");

                cb(true, null);
                yield break;
            }

            var playerDict = (IDictionary<string, object>) player.Value;
            var userModel = new UserModel(playerDict);

            cb(false, userModel);
        }

        public static IEnumerator UpdateUserNameAsync(string userName)
        {
            LoadingPanel.Instance.LoadingStart(ELoading.Load);

            _elapsedTime = 0;

            if (PhoneManager.Instance.User == null)
            {
                LoadingPanel.Instance.LoadingStop();
                yield break;
            }

            var task = BaseRef.Child("users").Child(PhoneManager.Instance.User.UserId).Child("user_name").SetValueAsync(userName);
            yield return new WaitWhile(() => IsTask(task.IsCompleted));

            if (task.IsFaulted || task.IsCanceled)
            {
                LoadingPanel.Instance.LoadingStop();
                yield break;
            }

            LoadingPanel.Instance.LoadingStop();
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
    }
}
