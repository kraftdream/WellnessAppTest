using UnityEngine;
using UnityEngine.EventSystems;

namespace WellnessApp
{
    public class DraggableButton : DragItemBase, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Canvas _canvas;
        private RectTransform _rectTransform;
        private RectTransform _canvasRectTransform;
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            StartDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_canvas != null)
            {
                Vector2 newPos = _rectTransform.anchoredPosition + eventData.delta / _canvas.scaleFactor;
                Vector2 halfSize = _rectTransform.sizeDelta / 2;

                // Clamp position to stay within canvas bounds
                float minX = -_canvasRectTransform.sizeDelta.x / 2 + halfSize.x;
                float maxX = _canvasRectTransform.sizeDelta.x / 2 - halfSize.x;
                float minY = -_canvasRectTransform.sizeDelta.y / 2 + halfSize.y;
                float maxY = _canvasRectTransform.sizeDelta.y / 2 - halfSize.y;

                newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
                newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

                _rectTransform.anchoredPosition = newPos;

                Dragging();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            StopDrag();
        }
    }
}