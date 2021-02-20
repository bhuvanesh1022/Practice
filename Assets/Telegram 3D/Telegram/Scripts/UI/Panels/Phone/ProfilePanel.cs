using System;
using System.Collections;
using Telegram.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Telegram.Phone
{
    public class ProfilePanel : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image _navBackground;
        [SerializeField] private Button _btnDone;
        [SerializeField] private Image _imgAvatar;
        [SerializeField] private InputField _inputFieldName;
        [SerializeField] private Text _txtPhoneNumber;

        public event Action OnClickDone;

        private void Start()
        {
            _btnDone.onClick.AddListener(UpdateProfile);
        }

        public void Init()
        {
            SetTheme();
        }

        public void Initialize(string phoneNumber, string name = null, string photoUrl = null)
        {
            if (name != null)
            {
                _inputFieldName.text = name;
            }

            _txtPhoneNumber.text = phoneNumber;
        }

        private void UpdateProfile()
        {
            StartCoroutine(UpdateProfileAsync());
        }

        private IEnumerator UpdateProfileAsync()
        {
            yield return StartCoroutine(FirebaseCore.UpdateUserNameAsync(_inputFieldName.text));

            OnClickDone?.Invoke();
        }

        private void SetTheme()
        {
            _navBackground.color = ThemeManager.Instance.NavBackground;
            _btnDone.image.color = ThemeManager.Instance.NavButtonBackground;
            _btnDone.GetComponentInChildren<Text>().color = ThemeManager.Instance.NavButtonBackground;
        }
    }
}