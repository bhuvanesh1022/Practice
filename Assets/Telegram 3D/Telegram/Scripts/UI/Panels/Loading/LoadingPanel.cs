using UnityEngine;
using UnityEngine.UI;

public enum ELoading
{
    Load,
    Unload
}

namespace Loading
{
    public class LoadingPanel : Singleton<LoadingPanel>
    {
        [Header("Basic")]
        [SerializeField] private GameObject _root;
        
        [Header("Components")]
        [SerializeField] private Text _txtLoading;
        [SerializeField] private Slider _slider;

        private float _speed = 0.1f;
        public bool Load { get; private set; }

        private void Update()
        {
            if (Load)
            {
                if (_slider.value < 1.0f)
                {
                    _slider.value = _slider.value + Time.deltaTime * _speed;

                }
                else
                {
                    _slider.value = 0.0f;
                }
            }
        }

        public void LoadingStart(ELoading state)
        {
            _root.SetActive(true);
            Load = true;

            switch (state)
            {
                case ELoading.Load:
                    _txtLoading.text = "DOWNLOAD DATA ...";
                    break;
                case ELoading.Unload:
                    _txtLoading.text = "LOADING RESOURCES ...";
                    break;
                default:
                    break;
            }
        }

        public void LoadingStop()
        {
            _slider.value = 0.0f;
            Load = false;
            _root.SetActive(false);
        }
    }
}