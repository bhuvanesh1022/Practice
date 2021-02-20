using System;
using UnityEngine;
using UnityEngine.UI;

namespace Telegram.UI
{
    public class CountryElmnt : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Button _btnCountry;
        [SerializeField] private Text _txtDiallingCode;
        [SerializeField] private Text _txtCountryName;

        public event Action<string, string> OnClick;

        private string _diallingCode;
        private string _countryName;

        private void Start()
        {
            _btnCountry.onClick.AddListener(() =>
            {
                OnClick?.Invoke(_diallingCode, _countryName);
            });
        }

        public void Initialize(string diallingCode, string countryName)
        {
            _txtDiallingCode.text = _diallingCode = diallingCode;
            _txtCountryName.text = _countryName = countryName;

            SetTheme();
        }

        private void SetTheme()
        {
            _btnCountry.image.color = ThemeManager.Instance.MainBackground;
            _txtDiallingCode.color = ThemeManager.Instance.MainText;
            _txtCountryName.color = ThemeManager.Instance.MainText;
        }
    }
}