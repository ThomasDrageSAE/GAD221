using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class UIComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] protected Image highlight;
    [SerializeField] protected UIComponent dragComponent;
    
    [SerializeField] protected bool interactable = true;
    
    [SerializeField] protected bool hoverable = true;
    [SerializeField] protected bool leftClickable  = true;
    [SerializeField] protected bool rightClickable = true;
    [SerializeField] protected bool draggable = false;
    
    public UnityEvent onHoverStart;
    public UnityEvent onHoverStop;
    public UnityEvent onLeftClicked;
    public UnityEvent onRightClicked;
    public UnityEvent<PointerEventData> onDragStart;
    public UnityEvent<PointerEventData> onDrag;
    public UnityEvent<PointerEventData> onDragEnd;

    protected bool hovered;
    protected bool interactionPaused;
    protected bool isDragging;
    protected Vector2 dragOffset;

    protected virtual void Start()
    {
        if (dragComponent != null)
        {
            dragComponent.onDragStart.AddListener(DragStart);
            dragComponent.onDrag.AddListener(Drag);
            dragComponent.onDragEnd.AddListener(DragEnd);
        }
    }
    
    protected virtual void OnDestroy()
    {
        if (dragComponent != null)
        {
            dragComponent.onDragStart.RemoveListener(DragStart);
            dragComponent.onDrag.RemoveListener(Drag);
            dragComponent.onDragEnd.RemoveListener(DragEnd);
        }
    }
    
    void Awake()
    {
        if (highlight != null)
        {
            highlight.enabled = false;
        }
    }
    
    virtual protected void HoverStart()
    {
        if (hoverable && interactable && !interactionPaused)
        {
            onHoverStart?.Invoke();
            highlight.enabled = true;
        }
    }

    virtual protected void HoverStop()
    {
        if (hoverable)
        {
            onHoverStop?.Invoke();
            highlight.enabled = false;
        }
    }

    virtual protected void LeftClick()
    {
        if (leftClickable && interactable && !interactionPaused)
        {
            onLeftClicked?.Invoke();
        }
    }
    
    virtual protected void RightClick()
    {
        if (leftClickable && interactable && !interactionPaused)
        {
            onRightClicked?.Invoke();
        }
    }

    public void PauseInteraction()
    {
        interactionPaused = true;
    }

    public void ResumeInteraction()
    {
        interactionPaused = false;

        if (hovered)
        {
            HoverStart();
        }
    }
    
    virtual protected void DragStart(PointerEventData eventData)
    {
        if (!draggable || !interactable || interactionPaused)
        {
            return;
        }
        
        Debug.Log("UIComponent - DragStart" + eventData.pressPosition);
        dragOffset = eventData.pressPosition - (Vector2)transform.position;
        onDragStart?.Invoke(eventData);
    }
    
    virtual protected void Drag(PointerEventData eventData)
    {
        if (!draggable || !interactable || interactionPaused)
        {
            return;
        }
        
        RectTransform rectTransform = (RectTransform)transform;
        
        int taskBarHeight = 25;
        
        Vector2 componentSize = rectTransform.rect.size;
        Vector2 pivotPoint = rectTransform.pivot;
        
        float minX = componentSize.x * pivotPoint.x; float maxX = Screen.width - componentSize.x * (1f - pivotPoint.x);
        float minY = componentSize.y * pivotPoint.y + taskBarHeight; float maxY = Screen.height - componentSize.y * (1f - pivotPoint.y);
        
        Vector2 newPos = eventData.position - dragOffset;
        
        newPos.x = Mathf.Round(Mathf.Clamp(newPos.x, minX, maxX));
        newPos.y = Mathf.Round(Mathf.Clamp(newPos.y, minY, maxY));
        
        gameObject.transform.position = newPos;
        onDrag?.Invoke(eventData);
    }

    virtual protected void DragEnd(PointerEventData eventData)
    {
        Debug.Log("UIComponent - DragEnd" + eventData.position);
        onDragEnd?.Invoke(eventData);
    }
    
    # region --- Pointer Events ---
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
        HoverStart();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        HoverStop();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            LeftClick();
        }
        
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightClick();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (draggable)
        {
            onDragStart?.Invoke(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggable)
        {
            onDrag?.Invoke(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (draggable)
        {
            onDragEnd?.Invoke(eventData);
        }
    }
    
    #endregion
}
