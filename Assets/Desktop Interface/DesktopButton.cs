using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DesktopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Image buttonHighlight;
    
    public UnityEvent onClicked;
    
    void Awake()
    {
        Debug.Log("DesktopButton - Awake");
        buttonHighlight.enabled = false;
    }
    
    void HoverStart()
    {
        Debug.Log("DesktopButton - HoverStart");
        buttonHighlight.enabled = true;
    }

    void HoverStop()
    {
        Debug.Log("DesktopButton - HoverStop");
        buttonHighlight.enabled = false;
    }

    void Click()
    {
        Debug.Log("DesktopButton - Click");
        onClicked?.Invoke();
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

