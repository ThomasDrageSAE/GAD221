using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSlider : UIComponent
{
    [SerializeField] Animator animator;
    [SerializeField] Image image;
    [SerializeField] Sprite offSprite;
    [SerializeField] Sprite onSprite;

    public bool defaultValue;
    
    private bool toggleValue;

    private void Start()
    {
        SetToggleValue(defaultValue);
    }

    public void SetToggleValue(bool value)
    {
        toggleValue = value;

        if (value)
        {
            image.sprite = onSprite;
        }

        else
        {
            image.sprite = offSprite;
        }
    }
    
    public void Toggle()
    {
        toggleValue = !toggleValue;
        
        Debug.Log("Toggle Slider - " + toggleValue);
        
        if (toggleValue)
        {
            animator.StopPlayback();
            animator.Play("ToggleOn");
        }

        else
        {
            animator.StopPlayback();
            animator.Play("ToggleOff");
        }
    }

    public bool GetToggleValue()
    {
        return toggleValue;
    }
    
    override protected void LeftClick()
    {
        base.LeftClick();
        Toggle();
    }
}