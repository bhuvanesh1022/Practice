using System;
using UnityEngine;
using UnityEngine.UI;

namespace Telegram.UI
{
    public class ContactElmnt : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Button _btnContact;
        [SerializeField] private Text _txtContact;
        [SerializeField] private Text _txtStatus;

        public event Action<string, string, string> OnClick;

        private string _contactName;
        private string _contactStatus;
        private string _contactPhone;

        private void Start()
        {
            _btnContact.onClick.AddListener(() =>
            {
                OnClick?.Invoke(_contactName, _contactPhone, _contactStatus);
            });
        }

        public void Initialize(string contactName, string contactPhone, string contactStatus)
        {
            _txtContact.text = _contactName = contactName;
            _txtStatus.text = _contactStatus = contactStatus;
            _contactPhone = contactPhone;

            SetTheme();
        }

        private void SetTheme()
        {
            _btnContact.image.color = ThemeManager.Instance.MainBackground;
            _txtContact.color = ThemeManager.Instance.MainText;
            _txtStatus.color = ThemeManager.Instance.SubText;
        }
    }
}