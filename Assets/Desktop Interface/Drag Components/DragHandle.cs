using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Random Note To Self: Drag threshold can be adjusted in EventSystem
    
    #region --- Inspector References ---
    
    // -- In Scene --
    
    
    // -- Resources & Prefabs --
    
    
    #endregion
    
    #region --- Properties & Variables ---
    
    // -- Public --
    
    
    // -- Private --
    private bool dragging;
    
    #endregion
    
    #region --- Events ---
    
    public UnityEvent<StartDragData> onDragStart;
    public UnityEvent<DragData> onDrag;
    public UnityEvent<EndDragData> onDragEnd;
    
    #endregion
    
    #region --- Pointer Events ---
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
        onDragStart?.Invoke(new StartDragData(eventData.pressPosition));
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke(new DragData(eventData.pressPosition, eventData.position));
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        onDragEnd?.Invoke(new EndDragData(eventData.pressPosition, eventData.position));
    }
    
    #endregion

    public bool IsDragging()
    {
        return dragging;
    }
    
    # region --- Drag Data Structs ---
    
    public struct StartDragData
    {
        public Vector2 startPos;

        public StartDragData(Vector2 startPos)
        {
            this.startPos = startPos;
        }
    }

    public struct DragData
    {
        public Vector2 startPos;
        public Vector2 currentPos;
        public Vector2 posDifference;

        public DragData(Vector2 startPos, Vector2 currentPos)
        {
            this.startPos = startPos;
            this.currentPos = currentPos;
            posDifference = this.currentPos - this.startPos;
        }
    }

    public struct EndDragData
    {
        public Vector2 startPos;
        public Vector2 endPos;
        public Vector2 posDifference;

        public EndDragData(Vector2 startPos, Vector2 endPos)
        {
            this.startPos = startPos;
            this.endPos = endPos;
            posDifference = this.endPos - this.startPos;
        }
    }
    
    #endregion
}
