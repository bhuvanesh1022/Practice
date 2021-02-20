using System;
using Telegram.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Telegram.Chat
{
    public class MessagePanel : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private Image _navBackground;

        [SerializeField] private Text _txtNavHeader;
        [SerializeField] private Button _btnBack;
        [SerializeField] private VerticalLayoutGroup _txtLayoutGroup;
        [SerializeField] private Text _txtPrefab;
        [SerializeField] private InputField _inputFieldText;

        public event Action OnClickBack;
        public event Action<string> OnClickMessage;

        private void Start()
        {
            _btnBack.onClick.AddListener(() => { OnClickBack?.Invoke(); });

            _inputFieldText.onEndEdit.AddListener(text => { OnClickMessage?.Invoke(text); });
        }

        public void Initialize(string nameChat)
        {
            _txtNavHeader.text = nameChat;
        }

        public void AddMessage(UserModel userModel, MessageModel messageModel)
        {
            var text = Instantiate(_txtPrefab, _txtLayoutGroup.transform, false);
            var timeTamp = new DateTime(messageModel.Timestamp).ToString("HH:mm:ss");
            text.name = messageModel.Uid;
            text.text = $"{userModel.UserName} [{timeTamp}]: {messageModel.Text}";

            var rect = _txtLayoutGroup.GetComponent<RectTransform>();
            var offset = 25.0f * _txtLayoutGroup.transform.childCount;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, offset);
        }
    }
}
