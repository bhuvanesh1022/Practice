using System;
using System.Collections.Generic;
using Telegram.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Telegram.Phone
{
    public class CountryPanel : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image _background;
        [SerializeField] private Image _navBackground;
        [SerializeField] private Text _textNavHeader;
        [SerializeField] private Image _iconBtnPrev;
        [SerializeField] private Button _btnPrev;
        [SerializeField] private InputField _inputFieldCountry;
        [SerializeField] private UI_InfiniteScroll _scroll;

        private List<CountryModel> CountryDataBufferNames { get; set; }
        private List<CountryModel> CountryDataNames { get; set; }

        public event Action<string, string> OnClick;
        public event Action OnClickPrev;

        private int _count;

        private void Awake()
        {
            CountryDataBufferNames = new List<CountryModel>();
            CountryDataNames = new List<CountryModel>();
        }

        private void Start()
        {
            _inputFieldCountry.onValueChanged.AddListener((text) => { Init(SearchCountryNames(text)); });

            _btnPrev.onClick.AddListener(() => { OnClickPrev?.Invoke(); });
        }

        public void InitData(List<CountryModel> data)
        {
            CountryDataBufferNames = data;
            _inputFieldCountry.text = "";
        }

        public void Init()
        {
            SetTheme();
        }

        public void Init(List<CountryModel> data)
        {
            if (data == null) return;

            CountryDataNames = data;
            _count = data.Count;

            Initialize();
        }

        private void Initialize()
        {
            _scroll.FillItem += (index, item) =>
            {
                var country = item.GetComponent<CountryElmnt>();
                country.Initialize(CountryDataNames[index].dialling_code, CountryDataNames[index].country_name);
                country.OnClick += CountryOnClick;
            };

            _scroll.PullLoad += (obj) =>
            {
                _count += 20;
                _scroll.ApplyDataTo(_count, 20, obj);
            };

            _scroll.InitData(_count);
        }

        private void CountryOnClick(string diallingCode, string countryName)
        {
            OnClick?.Invoke(diallingCode, countryName);
        }

        private List<CountryModel> SearchCountryNames(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var found = new List<CountryModel>();

                for (int j = 0; j < CountryDataBufferNames.Count; j++)
                {
                    var str = CountryDataBufferNames[j].country_name.ToLower();

                    if (str.Contains(text.ToLower()))
                    {
                        var countryData = new CountryModel
                        {
                            country_name = CountryDataBufferNames[j].country_name,
                            dialling_code = CountryDataBufferNames[j].dialling_code
                        };

                        found.Add(countryData);
                    }
                }

                return found.Count != 0 ? found : null;
            }
            else
            {
                return null;
            }
        }

        private void SetTheme()
        {
            _background.color = ThemeManager.Instance.MainBackground;
            _navBackground.color = ThemeManager.Instance.NavBackground;
            _textNavHeader.color = ThemeManager.Instance.NavMainText;
            _iconBtnPrev.color = ThemeManager.Instance.NavButtonBackground;
        }
    }
}