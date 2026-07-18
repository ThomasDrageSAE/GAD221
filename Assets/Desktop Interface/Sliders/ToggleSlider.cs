using UnityEngine;

public class ToggleSlider : UIComponent
{
    [SerializeField] Animator animator;
    
    private bool toggleValue = true;

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