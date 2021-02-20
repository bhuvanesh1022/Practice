namespace Telegram.Data
{
    public class MessageModel
    {
        public string Uid;
        public string Text;
        public string AuthorId;
        public long Timestamp;

        public MessageModel()
        {

        }

        public MessageModel(string uid, string text, string authorId, long timestamp)
        {
            this.Uid = uid;
            this.Text = text;
            this.AuthorId = authorId;
            this.Timestamp = timestamp;
        }
    }
}