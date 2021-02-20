using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Telegram.Phone
{
    public class PhoneNumberPanel : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image _navBackground;
        [SerializeField] private Button _btnNext;
        [SerializeField] private Image _iconBtnNext;
        [SerializeField] private Text _txtHeader;
        [SerializeField] private Text _txtDescription;
        [SerializeField] private Button _btnCountry;
        [SerializeField] private InputField _inputFieldPhoneNumber;
        [SerializeField] private Text _txtDiallingCode;
        [SerializeField] private Text _txtCountryName;

        public event Action OnClickCountry;
        public event Action<int> OnClickNext;

        private void Start()
        {
            _btnCountry.onClick.AddListener(() => { OnClickCountry?.Invoke(); });

            _inputFieldPhoneNumber.onValueChanged.AddListener((text) =>
            {
                _btnNext.interactable = IsPhoneNumber(text);
            });

            _btnNext.onClick.AddListener(() => { OnClickNext?.Invoke(1); });
        }

        public void Init()
        {
            SetTheme();
        }

        public void Initialize(string diallingCode, string countryName)
        {
            _txtDiallingCode.text = diallingCode;
            _txtCountryName.text = countryName;
        }

        private bool IsPhoneNumber(string number)
        {
            var phone = _txtDiallingCode.text + number;
            return Regex.Match(phone, @"\+?[0-9]{10}").Success;
        }

        public string GetPhoneNumber()
        {
            return _txtDiallingCode.text + _inputFieldPhoneNumber.text;
        }

        private void SetTheme()
        {
            _navBackground.color = ThemeManager.Instance.NavBackground;
            _iconBtnNext.color = ThemeManager.Instance.NavButtonBackground;
            _txtHeader.color = ThemeManager.Instance.MainText;
            _txtDescription.color = ThemeManager.Instance.SubText;
            _btnCountry.image.color = ThemeManager.Instance.ButtonBackground;
            _txtCountryName.color = ThemeManager.Instance.ButtonText;
            _txtDiallingCode.color = ThemeManager.Instance.SubText;
        }
    }
}