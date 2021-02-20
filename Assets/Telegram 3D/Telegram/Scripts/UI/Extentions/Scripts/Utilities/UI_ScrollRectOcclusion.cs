using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Extensions/UI Scrollrect Occlusion")]
public class UI_ScrollRectOcclusion : MonoBehaviour
{
    public ScrollRect _scrollRect;
    private ContentSizeFitter _contentSizeFitter;
    private VerticalLayoutGroup _verticalLayoutGroup;
    private HorizontalLayoutGroup _horizontalLayoutGroup;
    private GridLayoutGroup _gridLayoutGroup;
    private bool _isVertical = false;
    private bool _isHorizontal = false;
    private float _disableMarginX = 0;
    private float _disableMarginY = 0;
    private bool _hasDisabledGridComponents = false;
    private List<RectTransform> items = new List<RectTransform>();

    private void Start()
    {
        _scrollRect.onValueChanged.RemoveAllListeners();
        _scrollRect.onValueChanged.AddListener(OnScroll);

        _isHorizontal = _scrollRect.horizontal;
        _isVertical = _scrollRect.vertical;

        for (int i = 0; i < _scrollRect.content.childCount; i++)
        {
            if (_scrollRect.content.GetChild(i).gameObject.activeSelf)
                items.Add(_scrollRect.content.GetChild(i).GetComponent<RectTransform>());
        }
        if (_scrollRect.content.GetComponent<VerticalLayoutGroup>() != null)
        {
            _verticalLayoutGroup = _scrollRect.content.GetComponent<VerticalLayoutGroup>();
        }
        if (_scrollRect.content.GetComponent<HorizontalLayoutGroup>() != null)
        {
            _horizontalLayoutGroup = _scrollRect.content.GetComponent<HorizontalLayoutGroup>();
        }
        if (_scrollRect.content.GetComponent<GridLayoutGroup>() != null)
        {
            _gridLayoutGroup = _scrollRect.content.GetComponent<GridLayoutGroup>();
        }
        if (_scrollRect.content.GetComponent<ContentSizeFitter>() != null)
        {
            _contentSizeFitter = _scrollRect.content.GetComponent<ContentSizeFitter>();
        }
    }

    public void DisableGridComponents()
    {
        if (_isVertical)
        {
            _disableMarginY = _scrollRect.GetComponent<RectTransform>().rect.height / 2 + items[0].sizeDelta.y;
        }

        if (_isHorizontal)
        {
            _disableMarginX = _scrollRect.GetComponent<RectTransform>().rect.width / 2 + items[0].sizeDelta.x;
        }

        if (_verticalLayoutGroup)
        {
            _verticalLayoutGroup.enabled = false;
        }
        if (_horizontalLayoutGroup)
        {
            _horizontalLayoutGroup.enabled = false;
        }
        if (_contentSizeFitter)
        {
            _contentSizeFitter.enabled = false;
        }
        if (_gridLayoutGroup)
        {
            _gridLayoutGroup.enabled = false;
        }

        _hasDisabledGridComponents = true;
    }

    public void OnScroll(Vector2 pos)
    {
        if (!_hasDisabledGridComponents)
        {
            DisableGridComponents();
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (_isVertical && _isHorizontal)
            {
                if (_scrollRect.transform.InverseTransformPoint(items[i].position).y < -_disableMarginY || _scrollRect.transform.InverseTransformPoint(items[i].position).y > _disableMarginY
                || _scrollRect.transform.InverseTransformPoint(items[i].position).x < -_disableMarginX || _scrollRect.transform.InverseTransformPoint(items[i].position).x > _disableMarginX)
                {
                    items[i].gameObject.SetActive(false);
                }
                else
                {
                    items[i].gameObject.SetActive(true);
                }
            }
            else
            {
                if (_isVertical)
                {
                    if (_scrollRect.transform.InverseTransformPoint(items[i].position).y < -_disableMarginY || _scrollRect.transform.InverseTransformPoint(items[i].position).y > _disableMarginY)
                    {
                        items[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        items[i].gameObject.SetActive(true);
                    }
                }

                if (_isHorizontal)
                {
                    if (_scrollRect.transform.InverseTransformPoint(items[i].position).x < -_disableMarginX || _scrollRect.transform.InverseTransformPoint(items[i].position).x > _disableMarginX)
                    {
                        items[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        items[i].gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}