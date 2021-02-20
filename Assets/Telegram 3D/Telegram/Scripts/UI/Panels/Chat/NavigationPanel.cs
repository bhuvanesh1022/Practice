using System;
using UnityEngine;
using UnityEngine.UI;

namespace Telegram.Chat
{
    public enum ENavButtonType
    {
        Contacts = 0,
        Chats = 1,
        Settings = 2
    }

    public class NavigationPanel : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image _navBackground;
        [SerializeField] private Button _btnContacts;
        [SerializeField] private Button _btnChats;
        [SerializeField] private Button _btnSettings;

        public event Action<ENavButtonType> OnClick;

        private void Start()
        {
            _btnContacts.onClick.AddListener(() =>
            {
                OnClick?.Invoke(ENavButtonType.Contacts);
            });

            _btnChats.onClick.AddListener(() =>
            {
                OnClick?.Invoke(ENavButtonType.Chats);
            });

            _btnSettings.onClick.AddListener(() =>
            {
                OnClick?.Invoke(ENavButtonType.Settings);
            });
        }

        public void Init()
        {
            SetTheme();
        }

        public void SelectButton(ENavButtonType navButtonType)
        {
            switch (navButtonType)
            {
                case ENavButtonType.Contacts:
                    _btnContacts.image.color = ThemeManager.Instance.NavButtonBackground;
                    _btnContacts.GetComponentInChildren<Text>().color =
                        ThemeManager.Instance.NavButtonBackground;
                    //------------------------------------------------
                    _btnChats.image.color = ThemeManager.Instance.NavButtonDisable;
                    _btnChats.GetComponentInChildren<Text>().color = ThemeManager.Instance.NavButtonDisable;
                    _btnSettings.image.color = ThemeManager.Instance.NavButtonDisable;
                    _btnSettings.GetComponentInChildren<Text>().color =
                        ThemeManager.Instance.NavButtonDisable;
                    break;
                case ENavButtonType.Chats:
                    _btnChats.image.color = ThemeManager.Instance.NavButtonBackground;
                    _btnChats.GetComponentInChildren<Text>().color =
                        ThemeManager.Instance.NavButtonBackground;
                    //------------------------------------------------
                    _btnContacts.image.color = ThemeManager.Instance.NavButtonDisable;
                    _btnContacts.GetComponentInChildren<Text>().color =
                        ThemeManager.Instance.NavButtonDisable;
                    _btnSettings.image.color = ThemeManager.Instance.NavButtonDisable;
                    _btnSettings.GetComponentInChildren<Text>().color =
                        ThemeManager.Instance.NavButtonDisable;
                    break;
                case ENavButtonType.Settings:
                    _btnSettings.image.color = ThemeManager.Instance.NavButtonBackground;
                    _btnSettings.GetComponentInChildren<Text>().color =
                        ThemeManager.Instance.NavButtonBackground;
                    //------------------------------------------------
                    _btnContacts.image.color = ThemeManager.Instance.NavButtonDisable;
                    _btnContacts.GetComponentInChildren<Text>().color =
                        ThemeManager.Instance.NavButtonDisable;
                    _btnChats.image.color = ThemeManager.Instance.NavButtonDisable;
                    _btnChats.GetComponentInChildren<Text>().color = ThemeManager.Instance.NavButtonDisable;
                    break;
            }
        }

        private void SetTheme()
        {
            _navBackground.color = ThemeManager.Instance.NavBackground;
        }
    }
}