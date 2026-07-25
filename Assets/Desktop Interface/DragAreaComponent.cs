using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragAreaComponent : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    #region --- Inspector References ---
    
    // -- In Scene --
    [SerializeField] private RectTransform target;
    
    // -- Resources & Prefabs --
    
    
    #endregion
    
    #region --- Properties & Variables ---
    
    // -- Public --
    
    
    // -- Private --
    private Vector2 dragOffset;
    
    #endregion
    
    #region --- Events ---
    
    public UnityEvent<PointerEventData> onDragStart;
    public UnityEvent<PointerEventData> onDrag;
    public UnityEvent<PointerEventData> onDragEnd;
    
    #endregion
    
    #region --- Pointer Events ---
    
    public void OnPointerDown(PointerEventData eventData)
    {
        dragOffset = eventData.pressPosition - (Vector2)target.position;
        onDragStart?.Invoke(eventData);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        int taskBarHeight = 25;
        
        Vector2 componentSize = target.rect.size;
        Vector2 pivotPoint = target.pivot;
        
        float minX = componentSize.x * pivotPoint.x; float maxX = Screen.width - componentSize.x * (1f - pivotPoint.x);
        float minY = componentSize.y * pivotPoint.y + taskBarHeight; float maxY = Screen.height - componentSize.y * (1f - pivotPoint.y);
        
        Vector2 newPos = eventData.position - dragOffset;
        
        newPos.x = Mathf.Round(Mathf.Clamp(newPos.x, minX, maxX));
        newPos.y = Mathf.Round(Mathf.Clamp(newPos.y, minY, maxY));
        
        target.position = newPos;
        onDrag?.Invoke(eventData);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        onDragEnd?.Invoke(eventData);
    }
    
    #endregion
}
