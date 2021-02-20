using System;
using UnityEngine;
using UnityEngine.UI;

namespace Telegram.Error
{
    public class ErrorPanel : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Button _btnClose;
        [SerializeField] private Text _txtDescription;

        private void Start()
        {
            _btnClose.onClick.AddListener(() => { gameObject.SetActive(false); });
        }

        public void Initialize(string error)
        {
            _txtDescription.text = error;
        }
    }
}