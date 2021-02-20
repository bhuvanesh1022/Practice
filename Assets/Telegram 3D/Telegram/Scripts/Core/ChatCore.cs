using Firebase;
using Firebase.Database;
//using Firebase.Unity.Editor;
using UnityEngine;
using System.Collections.Generic;
using Telegram.Data;

namespace Telegram.Core
{
    public class ChatCore
    {
        private const string DatabaseUrl = "https://fir-example-chat.firebaseio.com";
        private UserModel _user;
        private ChatModel _chat;

        private DatabaseReference _databaseReference;

        public void Initialize()
        {
            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DatabaseUrl);
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void CreateNewRoom(string uid, string name)
        {
            _chat = new ChatModel(uid, name);
        }

        private void LoadParticipants(ChatModel chat)
        {
            var databaseReference = _databaseReference.Child("participants").Child(chat.Uid);
            databaseReference.ChildAdded += HandleUserAdded;
            databaseReference.GetValueAsync()
                .ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                        }
                        else if (task.IsCompleted)
                        {
                            var snapshot = task.Result;
                            var index = (Dictionary<string, bool>) snapshot.GetValue(false);
                            LoadUsers(index);
                        }
                    }
                );
        }

        private void LoadUsers(Dictionary<string, bool> index)
        {
            foreach (var key in index.Keys)
            {
                var databaseReference = _databaseReference.Child("users").Child(key);
                databaseReference.ValueChanged += HandleUserValueChanged;
                databaseReference.GetValueAsync()
                    .ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                            }
                            else if (task.IsCompleted)
                            {
                                var snapshot = task.Result;
                                var user = JsonUtility.FromJson<UserModel>(snapshot.GetRawJsonValue());
                                _chat.Participants.Add(key, user);
                            }
                        }
                    );
            }
        }

        private void HandleUserAdded(object sender, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }

            var userId = args.Snapshot.Key;

            var databaseReference = _databaseReference.Child("users").Child(userId);
            databaseReference.ValueChanged += HandleUserValueChanged;
            databaseReference.GetValueAsync()
                .ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                        }
                        else if (task.IsCompleted)
                        {
                            var snapshot = task.Result;
                            var user = JsonUtility.FromJson<UserModel>(snapshot.GetRawJsonValue());
                            _chat.Participants.Add(userId, user);
                        }
                    }
                );
        }

        private void HandleUserValueChanged(object sender, ValueChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }

            var user = JsonUtility.FromJson<UserModel>(args.Snapshot.GetRawJsonValue());
            _chat.Participants[user.Uid] = user;
            foreach (var model in _chat.Messages)
            {
                if (model.AuthorId.Equals(user.Uid))
                {
//                    Text text = textDisplays[model.uid];
//                    string ts = new DateTime(model.timestamp).ToString("HH:mm:ss");
//                    text.text = String.Format(MESSAGE_TEMPLATE, user.name, ts, model.text);
//                    text.color = user.GetColor();
                }
            }
        }
    }
}
