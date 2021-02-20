using System;
using System.Linq;
using UnityEngine;

namespace Telegram.Chat
{
    public class ChatController : MonoBehaviour
    {
        [Header("Basic")] [SerializeField] private GameObject _root;

        [Header("Panels")] [SerializeField] private ContactPanel _contactPanel;
        [SerializeField] private ChatPanel _chatPanel;
        [SerializeField] private SettingPanel _settingPanel;
        [SerializeField] private NavigationPanel _navigationPanel;
        [SerializeField] private MessagePanel _messagePanel;

        public event Action OnClickLogOut;

        private void Start()
        {
            _settingPanel.OnClickNotifications += SettingPanelOnOnClickNotifications;
            _settingPanel.OnClickChangePhone += SettingPanelOnOnClickChangePhone;
            _settingPanel.OnClickStorage += SettingPanelOnOnClickStorage;
            _settingPanel.OnClickPrivacy += SettingPanelOnOnClickPrivacy;
            _settingPanel.OnClickLogOut += SettingPanelOnClickLogOut;

            _navigationPanel.OnClick += SelectPanel;

            _chatPanel.OnClickChat += ChatPanelOnClickChat;

            _messagePanel.OnClickBack += MessagePanelOnClickBack;
            _messagePanel.OnClickMessage += MessagePanelOnClickMessage;
        }

        public void Init()
        {
            _contactPanel.Init();
            _chatPanel.Init();
            _settingPanel.Init();
            _navigationPanel.Init();
        }

        private void SettingPanelOnOnClickPrivacy()
        {
        }

        private void SettingPanelOnOnClickStorage()
        {
        }

        private void SettingPanelOnOnClickChangePhone()
        {
        }

        private void SettingPanelOnOnClickNotifications()
        {
        }

        private void SettingPanelOnClickLogOut()
        {
            OnClickLogOut?.Invoke();
        }

        private void ChatPanelOnClickChat(string nameChat)
        {
            _chatPanel.gameObject.SetActive(false);
            _messagePanel.gameObject.SetActive(true);
            _messagePanel.Initialize(nameChat);
        }

        private void MessagePanelOnClickBack()
        {
            _messagePanel.gameObject.SetActive(false);
            _chatPanel.gameObject.SetActive(true);
        }

        private void MessagePanelOnClickMessage(string meessage)
        {
        }

        private void SelectPanel(ENavButtonType navButtonType)
        {
            _navigationPanel.SelectButton(navButtonType);

            switch (navButtonType)
            {
                case ENavButtonType.Contacts:
                    _contactPanel.gameObject.SetActive(true);
                    _chatPanel.gameObject.SetActive(false);
                    _settingPanel.gameObject.SetActive(false);

                    InitContacts();
                    break;
                case ENavButtonType.Chats:
                    _contactPanel.gameObject.SetActive(false);
                    _chatPanel.gameObject.SetActive(true);
                    _settingPanel.gameObject.SetActive(false);
                    break;
                case ENavButtonType.Settings:
                    _contactPanel.gameObject.SetActive(false);
                    _chatPanel.gameObject.SetActive(false);
                    _settingPanel.gameObject.SetActive(true);

                    _settingPanel.SetProfile();
                    break;
            }
        }

        public void EnableModule(bool value)
        {
            _root.SetActive(value);
            SelectPanel(ENavButtonType.Chats);
        }

        private void InitContacts()
        {
            var data = LocalCore.GetDataFromJsonFromPath<ContactModel>("Contacts", "Contacts").ToList();

            _contactPanel.gameObject.SetActive(true);
            _contactPanel.InitData(data);
            _contactPanel.Init(data);
        }
    }
}
