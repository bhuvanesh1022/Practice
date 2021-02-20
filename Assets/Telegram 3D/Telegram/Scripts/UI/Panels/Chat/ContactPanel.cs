using System;
using System.Collections.Generic;
using Telegram.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Telegram.Chat
{
    public class ContactPanel : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image _navBackground;
        [SerializeField] private Text _textNavHeader;
        [SerializeField] private InputField _inputFieldContact;
        [SerializeField] private UI_InfiniteScroll _scroll;

        private List<ContactModel> ContactDataBuffer { get; set; }
        private List<ContactModel> ContactData { get; set; }

        public event Action<string, string, string> OnClick;

        private int _count;

        private void Awake()
        {
            ContactDataBuffer = new List<ContactModel>();
            ContactData = new List<ContactModel>();
        }

        private void Start()
        {
            _inputFieldContact.onValueChanged.AddListener((text) => { Init(SearchContactNames(text)); });
        }

        public void InitData(List<ContactModel> data)
        {
            ContactDataBuffer = data;
            _inputFieldContact.text = "";
        }

        public void Init()
        {
            SetTheme();
        }

        public void Init(List<ContactModel> data)
        {
            if (data == null) return;

            ContactData = data;
            _count = data.Count;

            Initialize();
        }

        private void Initialize()
        {
            _scroll.FillItem += (index, item) =>
            {
                var contact = item.GetComponent<ContactElmnt>();
                contact.Initialize(ContactData[index].contact_name, ContactData[index].contact_phone,
                    ContactData[index].contact_status);
                contact.OnClick += ContactOnOnClick;
            };

            _scroll.PullLoad += (obj) =>
            {
                _count += 20;
                _scroll.ApplyDataTo(_count, 20, obj);
            };

            _scroll.InitData(_count);
        }

        private void ContactOnOnClick(string contactName, string contactPhone, string countryStatus)
        {
            OnClick?.Invoke(contactName, contactPhone, countryStatus);
        }

        private List<ContactModel> SearchContactNames(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var found = new List<ContactModel>();

                for (int j = 0; j < ContactDataBuffer.Count; j++)
                {
                    var str = ContactDataBuffer[j].contact_name.ToLower();

                    if (str.Contains(text.ToLower()))
                    {
                        var data = new ContactModel
                        {
                            contact_name = ContactDataBuffer[j].contact_name,
                            contact_phone = ContactDataBuffer[j].contact_phone,
                            contact_status = ContactDataBuffer[j].contact_status
                        };

                        found.Add(data);
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
            _navBackground.color = ThemeManager.Instance.NavBackground;
            _textNavHeader.color = ThemeManager.Instance.NavMainText;
        }
    }
}