using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class UIComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected Image highlight;
    
    [SerializeField] bool hoverable = true;
    [SerializeField] bool clickable  = true;
    
    public UnityEvent onHoverStart;
    public UnityEvent onHoverStop;
    public UnityEvent onClicked;

    private bool hovered;
    
    void Awake()
    {
        if (highlight != null)
        {
            highlight.enabled = false;
        }
    }
    
    virtual protected void HoverStart()
    {
        if (hoverable)
        {
            onHoverStart?.Invoke();
            hovered = true;
            highlight.enabled = true;
        }
    }

    virtual protected void HoverStop()
    {
        if (hoverable)
        {
            onHoverStop?.Invoke();
            hovered = false;
            highlight.enabled = false;
        }
    }

    virtual protected void Click()
    {
        if (clickable)
        {
            onClicked?.Invoke();
        }
    }

    public bool IsHovered()
    {
        return hovered;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverStart();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverStop();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click();
    }
}
