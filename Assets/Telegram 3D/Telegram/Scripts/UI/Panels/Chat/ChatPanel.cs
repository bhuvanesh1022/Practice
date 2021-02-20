using System;
using UnityEngine;
using UnityEngine.UI;

namespace Telegram.Chat
{
    public class ChatPanel : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private Image _navBackground;

        [SerializeField] private Text _textNavHeader;
        [SerializeField] private Button _btnChat;

        public event Action<string> OnClickChat;

        private void Start()
        {
            _btnChat.onClick.AddListener(() =>
            {
                if (OnClickChat == null) return;
                var nameChat = _btnChat.GetComponentInChildren<Text>().text;
                OnClickChat(nameChat);
            });
        }

        public void Init()
        {
            SetTheme();
        }

        private void SetTheme()
        {
            _navBackground.color = ThemeManager.Instance.NavBackground;
            _textNavHeader.color = ThemeManager.Instance.NavMainText;
        }
    }
}
