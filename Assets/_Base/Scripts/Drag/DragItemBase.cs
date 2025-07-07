using System;
using UnityEngine;

namespace WellnessApp
{
    public enum DragActionType
    {
        Start,
        Dragging,
        Stop
    }

    public class DragItemBase : MonoBehaviour
    {
        public event Action<DragActionType> DraggingEvent;

        protected virtual void StartDrag()
        {
            IsDragging = true;
            DraggingEvent?.Invoke(DragActionType.Start);
        }

        protected virtual void Dragging()
        {
            DraggingEvent?.Invoke(DragActionType.Dragging);
        }

        protected virtual void StopDrag()
        {
            IsDragging = false;
            DraggingEvent?.Invoke(DragActionType.Stop);
        }

        public bool IsDragging { get; private set; }
    }
}
