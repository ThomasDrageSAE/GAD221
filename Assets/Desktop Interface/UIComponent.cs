using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class UIComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    #region --- Inspector References ---
    
    // -- In Scene --
    [SerializeField] protected Image highlight;
    
    // -- Resources & Prefabs --
    
    
    #endregion
    
    #region --- Properties & Variables ---
    
    // -- Public --
    
    
    // -- Private --
    [SerializeField] protected bool interactable = true;
    
    [SerializeField] protected bool hoverable = true;
    [SerializeField] protected bool leftClickable  = true;
    [SerializeField] protected bool rightClickable = true;
    
    protected bool hovered;
    protected bool interactionPaused;
    
    #endregion
    
    #region --- Events ---
    
    public UnityEvent onHoverStart;
    public UnityEvent onHoverStop;
    public UnityEvent onLeftClicked;
    public UnityEvent onRightClicked;
    
    protected virtual void EventSubscription()
    {
        
    }
    
    protected virtual void EventUnsubscription()
    {
        
    }
    
    #endregion
    
    #region --- Initialization & Termination ---
    
    protected virtual void Start()
    {
        EventSubscription();
        
        if (highlight != null)
        {
            highlight.enabled = false;
        }
    }
    
    protected virtual void OnDestroy()
    {
        EventUnsubscription();
        
        
    }
    
    #endregion
    
    protected virtual void HoverStart()
    {
        if (hoverable && interactable && !interactionPaused)
        {
            onHoverStart?.Invoke();
            highlight.enabled = true;
        }
    }

    protected virtual void HoverStop()
    {
        if (hoverable)
        {
            onHoverStop?.Invoke();
            highlight.enabled = false;
        }
    }

    protected virtual void LeftClick()
    {
        if (leftClickable && interactable && !interactionPaused)
        {
            onLeftClicked?.Invoke();
        }
    }
    
    protected virtual void RightClick()
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
    
    #endregion
}