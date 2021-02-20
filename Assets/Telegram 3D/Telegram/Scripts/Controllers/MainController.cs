using System.Collections;
using Loading;
using Telegram.Auth;
using Telegram.Chat;
using Telegram.Core;
using Telegram.Intro;
using Telegram.Phone;
using UnityEngine;

namespace Telegram.Controller
{
    public class MainController : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] private IntroController _introController;
        [SerializeField] private PhoneController _phoneController;
        [SerializeField] private ChatController _chatController;

        [Header("Default theme")]
        [SerializeField] private ThemeManager.Theme _theme;

        private IEnumerator Start()
        {
            LoadingPanel.Instance.LoadingStart(ELoading.Load);

            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(InitAsync());

            LoadingPanel.Instance.LoadingStop();
        }

        private IEnumerator InitAsync()
        {
            //PlayerPrefs.DeleteAll();

            switch (_theme)
            {
                case ThemeManager.Theme.Light:
                    PlayerPrefs.SetString(PrefsKeys.Theme, "light");
                    PlayerPrefs.Save();
                    break;
                case ThemeManager.Theme.Dark:
                    PlayerPrefs.SetString(PrefsKeys.Theme, "dark");
                    PlayerPrefs.Save();
                    break;
            }

            ThemeManager.Instance.Init(PlayerPrefs.GetString(PrefsKeys.Theme) == "dark"
                ? ThemeManager.Theme.Dark
                : ThemeManager.Theme.Light);

            yield return StartCoroutine(PhoneManager.Instance.InitAuthAsync());

            _introController.Init();
            _phoneController.Init();
            _chatController.Init();

            _introController.OnClickStartMessaging += IntroControllerOnClickStartMessaging;
            _phoneController.OnClickDone += PhoneControllerOnClickDone;
            _chatController.OnClickLogOut += ChatControllerOnClickLogOut;

            Init();
        }

        private void Init()
        {
            FirebaseCore.Init();

//            if (PhoneManager.Instance.User != null)
//            {
//                _chatController.EnableModule(true);
//            }
//            else
//            {
//                _introController.EnableModule(true);
//            }
            
            _chatController.EnableModule(true);
        }

        private void IntroControllerOnClickStartMessaging()
        {
            _introController.EnableModule(false);
            _phoneController.EnableModule(true);
        }

        private void PhoneControllerOnClickDone()
        {
            _phoneController.EnableModule(false);
            _chatController.EnableModule(true);
        }

        private void ChatControllerOnClickLogOut()
        {
            PhoneManager.Instance.Logout();

            _chatController.EnableModule(false);
            _phoneController.EnableModule(true);
            _phoneController.GoToScreen(0);
        }
    }
}