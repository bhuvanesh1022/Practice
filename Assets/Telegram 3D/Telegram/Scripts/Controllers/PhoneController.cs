using System;
using System.Linq;
using Telegram.Auth;
using Telegram.Data;
using Telegram.Error;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Telegram.Phone
{
    public class PhoneController : MonoBehaviour
    {
        [Header("Basic")]
        [SerializeField] private GameObject _root;

        [Header("Panels")]
        [SerializeField] private HorizontalScrollSnap _horizontalScrollSnap;
        [SerializeField] private PhoneNumberPanel _phoneNumberPanel;
        [SerializeField] private VerificationPanel _verificationPanel;
        [SerializeField] private ProfilePanel _profilePanel;
        [SerializeField] private CountryPanel _countryPanel;
        [SerializeField] private ErrorPanel _errorPanel;

        public event Action OnClickDone;

        public void Init()
        {
            _phoneNumberPanel.Init();
            _phoneNumberPanel.OnClickCountry += PhoneNumberPanelOnClickCountry;
            _phoneNumberPanel.OnClickNext += GoToScreen;

            _verificationPanel.Init();
            _verificationPanel.OnClickNext += VerificationPanelOnClick;
            _verificationPanel.OnClickPrev += GoToScreen;
            _verificationPanel.OnClickResend += VerificationPanelOnClickResend;

            _profilePanel.Init();
            _profilePanel.OnClickDone += ProfilePanelOnClickDone;

            _countryPanel.Init();
            _countryPanel.OnClick += CountryPanelOnClick;
            _countryPanel.OnClickPrev += CountryPanelOnClickPrev;

            PhoneManager.Instance.OnCodeSent += InstanceOnCodeSent;
            PhoneManager.Instance.OnUserNode += InstanceOnUserNode;
            
        }

        private void PhoneNumberPanelOnClickCountry()
        {
            var data = LocalCore.GetDataFromJsonFromPath<CountryModel>("CountryCode", "CountryCode").ToList();

            _countryPanel.gameObject.SetActive(true);
            _countryPanel.InitData(data);
            _countryPanel.Init(data);
        }

        public void GoToScreen(int indexPage)
        {
            _horizontalScrollSnap.GoToScreen(indexPage);
            var phoneNumber = _phoneNumberPanel.GetPhoneNumber();

            if (indexPage == 0)
            {

            }
            else if (indexPage == 1)
            {
                _verificationPanel.Initialize(phoneNumber);
                _verificationPanel.InitTimer();
                PhoneManager.Instance.VerifyPhoneNumber(phoneNumber);
            }
            else if (indexPage == 2)
            {
                _profilePanel.Initialize(phoneNumber);
            }
        }

        private void VerificationPanelOnClick(string code)
        {
            PhoneManager.Instance.VerifyCode(code, (error, text) =>
            {
                if (error)
                {
                    _errorPanel.gameObject.SetActive(true);
                    _errorPanel.Initialize(text);
                }
                else
                {
                    GoToScreen(2);
                }
            });
        }

        private void InstanceOnUserNode(UserModel data)
        {
            _horizontalScrollSnap.GoToScreen(2);
            _profilePanel.Initialize(data.PhoneNumber, data.UserName, data.PhotoUrl);
        }

        private void InstanceOnCodeSent()
        {
            _verificationPanel.EnableNextButton(true);
        }

        private void VerificationPanelOnClickResend()
        {
            _verificationPanel.InitTimer();
        }

        private void CountryPanelOnClick(string diallingCode, string countryName)
        {
            _countryPanel.gameObject.SetActive(false);
            _phoneNumberPanel.gameObject.SetActive(true);
            _phoneNumberPanel.Initialize(diallingCode, countryName);
        }

        private void CountryPanelOnClickPrev()
        {
            _countryPanel.gameObject.SetActive(false);
            _phoneNumberPanel.gameObject.SetActive(true);
        }

        private void ProfilePanelOnClickDone()
        {
            OnClickDone?.Invoke();
        }

        public void EnableModule(bool value)
        {
            _root.SetActive(value);
        }
    }
}