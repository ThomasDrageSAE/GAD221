using UnityEngine;

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowBar : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private bool isDragging;

    public event Action<PointerEventData> onDragStart;
    public event Action<PointerEventData> onDrag;
    public event Action<PointerEventData> onDragEnd;

    private void Awake()
    {
        
    }

    void DragStart(PointerEventData eventData)
    {
        Debug.Log("Drag Start");
        
        onDragStart?.Invoke(eventData);
    }
    
    void Drag(PointerEventData eventData)
    {
        Debug.Log("Drag");
        
        onDrag?.Invoke(eventData);
    }

    void DragEnd(PointerEventData eventData)
    {
        Debug.Log("Drag End");
        
        onDragEnd?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DragStart(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Drag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DragEnd(eventData);
    }
}
