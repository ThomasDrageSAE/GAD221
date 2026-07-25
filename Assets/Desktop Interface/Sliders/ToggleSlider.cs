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

    protected override void Start()
    {
        base.Start();

        if (defaultValue)
        {
            toggleValue = true;
            image.sprite = onSprite;
            animator.Play("ToggleOn", 0, 1f);
        }

        else
        {
            toggleValue = false;
            image.sprite = offSprite;
            animator.Play("ToggleOff", 0, 1f);
        }
    }
    
    public void Toggle()
    {
        Debug.Log("Toggle Slider Was - " + toggleValue);
        
        if (toggleValue)
        {
            ToggleOff();
        }

        else
        {
            ToggleOn();
        }
        
        toggleValue = !toggleValue;
        
        Debug.Log("Toggle Slider Now - " + toggleValue);
    }

    public void ToggleOn()
    {
        Debug.Log("Toggle Slider Anim - ToggleOn");
        animator.Play("ToggleOn");
    }

    public void ToggleOff()
    {
        Debug.Log("Toggle Slider Anim - ToggleOff");
        animator.Play("ToggleOff");
    }
    
    override protected void LeftClick()
    {
        base.LeftClick();
        Toggle();
    }
}