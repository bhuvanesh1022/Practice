using System;
using UnityEngine;
using UnityEngine.UI;

namespace Telegram.Chat
{
    public class SettingPanel : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image _navBackground;
        [SerializeField] private Text _txtNavHeader;
        [SerializeField] private Text _txtContact;
        [SerializeField] private Text _txtPhone;
        [SerializeField] private Image _profileBackground;
        [SerializeField] private Button _btnNotifications;
        [SerializeField] private Button _btnChangePhone;
        [SerializeField] private Button _btnStorage;
        [SerializeField] private Button _btnPrivacy;
        [SerializeField] private Button _btnLogOut;
        [SerializeField] private Toggle _tglTheme;
        [SerializeField] private Image _imgViewport;

        public event Action OnClickNotifications;
        public event Action OnClickChangePhone;
        public event Action OnClickStorage;
        public event Action OnClickPrivacy;
        public event Action OnClickLogOut;

        private void Start()
        {
            _btnNotifications.onClick.AddListener(() => { OnClickNotifications?.Invoke(); });
            _btnChangePhone.onClick.AddListener(() => { OnClickChangePhone?.Invoke(); });
            _btnStorage.onClick.AddListener(() => { OnClickStorage?.Invoke(); });
            _btnPrivacy.onClick.AddListener(() => { OnClickPrivacy?.Invoke(); });
            _btnLogOut.onClick.AddListener(() => { OnClickLogOut?.Invoke(); });

            _tglTheme.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    PlayerPrefs.SetString(PrefsKeys.Theme, "light");
                    PlayerPrefs.Save();
                }
                else
                {
                    PlayerPrefs.SetString(PrefsKeys.Theme, "dark");
                    PlayerPrefs.Save();
                }
            });
        }

        public void Init()
        {
            SetTheme();
        }

        public void SetProfile()
        {
            _txtContact.text = PlayerPrefs.GetString(PrefsKeys.Name);
            _txtPhone.text = PlayerPrefs.GetString(PrefsKeys.Phone);
        }

        private void SetTheme()
        {
            _tglTheme.isOn = PlayerPrefs.GetString(PrefsKeys.Theme) != "dark";

            _navBackground.color = ThemeManager.Instance.NavBackground;
            _txtNavHeader.color = ThemeManager.Instance.NavMainText;
            _imgViewport.color = ThemeManager.Instance.MainBackground;

            _btnNotifications.image.color = ThemeManager.Instance.NavBackground;
            _btnChangePhone.image.color = ThemeManager.Instance.NavBackground;
            _btnStorage.image.color = ThemeManager.Instance.NavBackground;
            _btnPrivacy.image.color = ThemeManager.Instance.NavBackground;
            _btnLogOut.image.color = ThemeManager.Instance.NavBackground;

            _btnNotifications.GetComponentInChildren<Text>().color = ThemeManager.Instance.MainText;
            _btnChangePhone.GetComponentInChildren<Text>().color = ThemeManager.Instance.MainText;
            _btnStorage.GetComponentInChildren<Text>().color = ThemeManager.Instance.MainText;
            _btnPrivacy.GetComponentInChildren<Text>().color = ThemeManager.Instance.MainText;
            _btnLogOut.GetComponentInChildren<Text>().color = ThemeManager.Instance.MainText;

            _profileBackground.color = ThemeManager.Instance.NavBackground;
            _txtContact.color = ThemeManager.Instance.MainText;
            _txtPhone.color = ThemeManager.Instance.SubText;
        }
    }
}