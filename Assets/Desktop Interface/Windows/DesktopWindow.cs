using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DesktopWindow : MonoBehaviour
{
    [SerializeField] WindowBar windowBar;

    private Vector2 dragOffset;
    
    public event Action onWindowOpen;
    public event Action onWindowMinimize;
    public event Action onWindowMaximize;
    public event Action onWindowClose;
    
    
    void Start()
    {
        windowBar.onDragStart += DragStart;
        windowBar.onDrag += Drag;
        windowBar.onDragEnd += DragEnd;
    }

    void OnDestroy()
    {
        windowBar.onDragStart -= DragStart;
        windowBar.onDrag -= Drag;
        windowBar.onDragEnd -= DragEnd;
    }

    public void Open()
    {
        Debug.Log("Window - Open");
        onWindowOpen?.Invoke();
        //Anim
    }
    
    public void Minimize()
    {
        Debug.Log("Window - Minimize");
        onWindowMinimize?.Invoke();
    }

    public void Maximize()
    {
        Debug.Log("Window - Maximize");
        onWindowMaximize?.Invoke();
    }


    public void Close()
    {
        Debug.Log("Window - Close");
        onWindowClose?.Invoke();
        Destroy(gameObject);
    }

    public void DragStart(PointerEventData eventData)
    {
        Debug.Log("Window - DragStart" + eventData.pressPosition);
        dragOffset = eventData.pressPosition - (Vector2)gameObject.transform.position;
    }

    public void Drag(PointerEventData eventData)
    {
        RectTransform rectTransform = (RectTransform)gameObject.transform;
        
        int taskBarHeight = 25;
        
        Vector2 windowSize = rectTransform.rect.size;
        Vector2 pivotPoint = rectTransform.pivot;
        
        
        float minX = windowSize.x * pivotPoint.x; float maxX = Screen.width - windowSize.x * (1f - pivotPoint.x);
        float minY = windowSize.y * pivotPoint.y + taskBarHeight; float maxY = Screen.height - windowSize.y * (1f - pivotPoint.y);
        
        Vector2 newPos = eventData.position - dragOffset;
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        
        gameObject.transform.position = newPos;
    }

    public void DragEnd(PointerEventData eventData)
    {
        Debug.Log("Window - DragEnd" + eventData.position);
    }
}
