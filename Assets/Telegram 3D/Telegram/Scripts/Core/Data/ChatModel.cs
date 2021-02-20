using System.Collections.Generic;

namespace Telegram.Data
{
    public class ChatModel
    {
        public string Uid;
        public string Name;

        public List<MessageModel> Messages;
        public Dictionary<string, UserModel> Participants;

        public ChatModel()
        {
            // uid?
            Name = "Untitled";
            Participants = new Dictionary<string, UserModel>();
            Messages = new List<MessageModel>();
        }

        public ChatModel(string uid, string name)
        {
            this.Uid = uid;
            this.Name = name;
            Participants = new Dictionary<string, UserModel>();
            Messages = new List<MessageModel>();
        }
    }
}
