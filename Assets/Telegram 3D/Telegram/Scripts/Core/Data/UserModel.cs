using System;
using System.Collections.Generic;

namespace Telegram.Data
{
    public class UserModel
    {
        public string Uid;
        public readonly string UserName;
        public readonly string PhoneNumber;
        public readonly string PhotoUrl;

        public UserModel(string userName, string phoneNumber,  string photoUrl)
        {
            this.UserName = userName;
            this.PhoneNumber = phoneNumber;
            this.PhotoUrl = photoUrl;
        }

        public UserModel(IDictionary<string, object> dict)
        {
            this.UserName = dict["user_name"].ToString();
            this.PhoneNumber = dict["phone_number"].ToString();
            this.PhotoUrl = dict["photo_url"].ToString();
        }
    }
}