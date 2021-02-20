using System;
using UnityEngine;
using UnityEngine.UI;

namespace Telegram.Intro
{
    public class IntroController : MonoBehaviour
    {
        [Header("Basic")] 
        [SerializeField] private GameObject _root;
        
        [Header("Components")] 
        [SerializeField] private Button _btnStartMessaging;
        [SerializeField] private Text _txtHeader0;
        [SerializeField] private Text _txtSubHeader0;
        [SerializeField] private Text _txtHeader1;
        [SerializeField] private Text _txtSubHeader1;
        [SerializeField] private Text _txtHeader2;
        [SerializeField] private Text _txtSubHeader2;
        [SerializeField] private Text _txtHeader3;
        [SerializeField] private Text _txtSubHeader3;
        [SerializeField] private Text _txtHeader4;
        [SerializeField] private Text _txtSubHeader4;
        [SerializeField] private Text _txtHeader5;
        [SerializeField] private Text _txtSubHeader5;
        [SerializeField] private Text _txtHeader6;
        [SerializeField] private Text _txtSubHeader6;

        public event Action OnClickStartMessaging;

        private void Start()
        {
            _btnStartMessaging.onClick.AddListener(() =>
            {
                OnClickStartMessaging?.Invoke();
            });
        }

        public void Init()
        {
            SetTheme();
        }

        public void EnableModule(bool value)
        {
            _root.SetActive(value);
        }

        private void SetTheme()
        {
            _txtHeader0.color = ThemeManager.Instance.MainText;
            _txtSubHeader0.color = ThemeManager.Instance.SubText;

            _txtHeader1.color = ThemeManager.Instance.MainText;
            _txtSubHeader1.color = ThemeManager.Instance.SubText;

            _txtHeader2.color = ThemeManager.Instance.MainText;
            _txtSubHeader2.color = ThemeManager.Instance.SubText;

            _txtHeader3.color = ThemeManager.Instance.MainText;
            _txtSubHeader3.color = ThemeManager.Instance.SubText;

            _txtHeader4.color = ThemeManager.Instance.MainText;
            _txtSubHeader4.color = ThemeManager.Instance.SubText;

            _txtHeader5.color = ThemeManager.Instance.MainText;
            _txtSubHeader5.color = ThemeManager.Instance.SubText;

            _txtHeader6.color = ThemeManager.Instance.MainText;
            _txtSubHeader6.color = ThemeManager.Instance.SubText;

            _btnStartMessaging.image.color = ThemeManager.Instance.ButtonBackground;
            _btnStartMessaging.GetComponentInChildren<Text>().color = ThemeManager.Instance.ButtonText;
        }
    }
}