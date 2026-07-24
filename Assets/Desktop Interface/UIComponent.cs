using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class UIComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected Image highlight;
    
    [SerializeField] protected bool interactable = true;
    
    [SerializeField] protected bool hoverable = true;
    [SerializeField] protected bool leftClickable  = true;
    [SerializeField] protected bool rightClickable = true;
    
    public UnityEvent onHoverStart;
    public UnityEvent onHoverStop;
    public UnityEvent onLeftClicked;
    public UnityEvent onRightClicked;

    protected bool hovered;
    protected bool interactionPaused;
    
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
        
        Debug.Log($"{name} - Resume, hovered: {hovered}");
    }
    
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
}
