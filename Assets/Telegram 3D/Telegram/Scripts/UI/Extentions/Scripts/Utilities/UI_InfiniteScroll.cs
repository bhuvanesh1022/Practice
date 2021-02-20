using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
    public class UI_InfiniteScroll : MonoBehaviour, IDropHandler
    {
        public enum Direction
        {
            Top,
            Bottom,
        };

        public event Action<int, GameObject> FillItem = delegate { };
        public event Action<Direction> PullLoad = delegate { };

        [Header("Item settings")]
        public GameObject Prefab;
        public int Height = 110;

        [Header("Padding")]
        public int Top = 10;
        public int Bottom = 10;
        public int Spacing = 2;

        [Header("Labels")]
        public string TopPullLabel = "Pull to refresh";
        public string TopReleaseLabel = "Release to load";
        public string BottomPullLabel = "Pull to refresh";
        public string BottomReleaseLabel = "Release to load";

        [Header("Directions")]
        public bool IsPullTop = true;
        public bool IsPullBottom = true;

        [Header("Pull coefficient")]
        [Range(0.01f, 0.1f)]
        public float PullValue = 0.05f;

        [HideInInspector]
        public Text TopLabel;
        [HideInInspector]
        public Text BottomLabel;

        private ScrollRect _scroll;
        private RectTransform _content;
        private RectTransform[] _rects;
        private GameObject[] _views;
        private bool _isCanLoadUp;
        private bool _isCanLoadDown;
        private int _previousPosition;
        private int _count;

        private void Awake()
        {
            _scroll = GetComponent<ScrollRect>();
            _scroll.onValueChanged.AddListener(OnScrollChange);
            _content = _scroll.viewport.transform.GetChild(0).GetComponent<RectTransform>();
            CreateViews();
            CreateLabels();
        }

        private void Update()
        {
            if (_count == 0)
            {
                return;
            }

            var topPosition = _content.anchoredPosition.y - Spacing;

            if (topPosition <= 0f && _rects[0].anchoredPosition.y < -Top - 10f)
            {
                InitData(_count);
                return;
            }

            if (topPosition < 0f)
            {
                return;
            }

            var position = Mathf.FloorToInt(topPosition / (Height + Spacing));

            if (_previousPosition == position)
            {
                return;
            }

            if (position > _previousPosition)
            {
                if (position - _previousPosition > 1)
                {
                    position = _previousPosition + 1;
                }

                var newPosition = position % _views.Length;

                newPosition--;

                if (newPosition < 0)
                {
                    newPosition = _views.Length - 1;
                }

                var index = position + _views.Length - 1;

                if (index < _count)
                {
                    var pos = _rects[newPosition].anchoredPosition;
                    pos.y = -(Top + index * Spacing + index * Height);
                    _rects[newPosition].anchoredPosition = pos;
                    FillItem(index, _views[newPosition]);
                }
            }
            else
            {
                if (_previousPosition - position > 1)
                {
                    position = _previousPosition - 1;
                }

                var newIndex = position % _views.Length;
                var pos = _rects[newIndex].anchoredPosition;
                pos.y = -(Top + position * Spacing + position * Height);
                _rects[newIndex].anchoredPosition = pos;
                FillItem(position, _views[newIndex]);
            }
            _previousPosition = position;
        }

        private void OnScrollChange(Vector2 vector)
        {
            float coef = _count / _views.Length;
            float y = 0f;
            _isCanLoadUp = false;
            _isCanLoadDown = false;

            if (vector.y > 1f)
            {
                y = (vector.y - 1f) * coef;
            }
            else if (vector.y < 0f)
            {
                y = vector.y * coef;
            }

            if (y > PullValue && IsPullTop)
            {
                TopLabel.gameObject.SetActive(true);
                TopLabel.text = TopPullLabel;

                if (y > PullValue * 2)
                {
                    TopLabel.text = TopReleaseLabel;
                    _isCanLoadUp = true;
                }
            }
            else
            {
                TopLabel.gameObject.SetActive(false);
            }

            if (y < -PullValue && IsPullBottom)
            {
                BottomLabel.gameObject.SetActive(true);
                BottomLabel.text = BottomPullLabel;

                if (y < -PullValue * 2)
                {
                    BottomLabel.text = BottomReleaseLabel;
                    _isCanLoadDown = true;
                }
            }
            else
            {
                BottomLabel.gameObject.SetActive(false);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (_isCanLoadUp)
            {
                PullLoad(Direction.Top);
            }
            else if (_isCanLoadDown)
            {
                PullLoad(Direction.Bottom);
            }

            _isCanLoadUp = false;
            _isCanLoadDown = false;
        }

        public void InitData(int count)
        {
            _previousPosition = 0;
            _count = count;

            var h = Height * count * 1f + Top + Bottom + (count == 0 ? 0 : ((count - 1) * Spacing));

            _content.sizeDelta = new Vector2(_content.sizeDelta.x, h);

            var pos = _content.anchoredPosition;
            pos.y = 0f;

            _content.anchoredPosition = pos;

            var y = Top;
            for (var i = 0; i < _views.Length; i++)
            {
                var showed = i < count;
                _views[i].gameObject.SetActive(showed);
                pos = _rects[i].anchoredPosition;
                pos.y = -y;
                pos.x = 0f;
                _rects[i].anchoredPosition = pos;
                y += Spacing + Height;

                if (i + 1 > _count)
                {
                    continue;
                }

                FillItem(i, _views[i]);
            }
        }

        public void ApplyDataTo(int count, int newCount, Direction direction)
        {
            _count = count;

            var newHeight = Height * count * 1f + Top + Bottom + (count == 0 ? 0 : ((count - 1) * Spacing));
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, newHeight);
            var pos = _content.anchoredPosition;

            if (direction == Direction.Top)
            {
                pos.y = (Height + Spacing) * newCount;
                _previousPosition = newCount;
            }
            else
            {
                pos.y = newHeight - (Height * Spacing) * newCount - (float)Screen.currentResolution.height;
            }

            _content.anchoredPosition = pos;

            var topPosition = _content.anchoredPosition.y - Spacing;
            var index = Mathf.FloorToInt(topPosition / (Height + Spacing));
            var all = Top + index * Spacing + index * Height;

            for (var i = 0; i < _views.Length; i++)
            {
                var newIndex = index % _views.Length;
                FillItem(index, _views[newIndex]);
                pos = _rects[newIndex].anchoredPosition;
                pos.y = -all;
                _rects[newIndex].anchoredPosition = pos;
                all += Spacing + Height;
                index++;

                if (index == _count)
                {
                    break;
                }
            }
        }

        private void CreateViews()
        {
            var fillCount = Mathf.RoundToInt((float)Screen.currentResolution.height / Height) + 2;
            _views = new GameObject[fillCount];

            for (var i = 0; i < fillCount; i++)
            {
                var clone = (GameObject)Instantiate(Prefab, Vector3.zero, Quaternion.identity);
                clone.transform.SetParent(_content);
                clone.transform.localScale = Vector3.one;
                clone.transform.localPosition = Vector3.zero;
                var rect = clone.GetComponent<RectTransform>();
                rect.pivot = new Vector2(0.5f, 1f);
                rect.anchorMin = new Vector2(0f, 1f);
                rect.anchorMax = new Vector2(1f, 1f);
                rect.offsetMax = new Vector2(0f, 0f);
                rect.offsetMin = new Vector2(0f, -Height);
                _views[i] = clone;
            }

            _rects = new RectTransform[_views.Length];

            for (int i = 0; i < _views.Length; i++)
            {
                _rects[i] = _views[i].gameObject.GetComponent<RectTransform>();
            }
        }

        private void CreateLabels()
        {
            var topText = new GameObject("TopLabel");
            topText.transform.SetParent(_scroll.viewport.transform);
            TopLabel = topText.AddComponent<Text>();
            TopLabel.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            TopLabel.fontSize = 24;
            TopLabel.transform.localScale = Vector3.one;
            TopLabel.alignment = TextAnchor.MiddleCenter;
            TopLabel.text = TopPullLabel;
            var rect = TopLabel.GetComponent<RectTransform>();
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.offsetMax = new Vector2(0f, 0f);
            rect.offsetMin = new Vector2(0f, -55f);
            rect.anchoredPosition3D = Vector3.zero;
            topText.SetActive(false);

            var bottomText = new GameObject("BottomLabel");
            bottomText.transform.SetParent(_scroll.viewport.transform);
            BottomLabel = bottomText.AddComponent<Text>();
            BottomLabel.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            BottomLabel.fontSize = 24;
            BottomLabel.transform.localScale = Vector3.one;
            BottomLabel.alignment = TextAnchor.MiddleCenter;
            BottomLabel.text = BottomPullLabel;
            BottomLabel.transform.position = Vector3.zero;
            rect = BottomLabel.GetComponent<RectTransform>();
            rect.pivot = new Vector2(0.5f, 0f);
            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.offsetMax = new Vector2(0f, 55f);
            rect.offsetMin = new Vector2(0f, 0f);
            rect.anchoredPosition3D = Vector3.zero;
            bottomText.SetActive(false);
        }
    }
}