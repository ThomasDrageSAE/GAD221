using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigitDisplay : UIComponent
{
    #region --- Inspector References ---
    
    // -- In Scene --
    [SerializeField] private Image digitImage;
    
    // -- Resources & Prefabs --
    [SerializeField] private List<Sprite> digitSprites;
    
    #endregion
    
    #region --- Properties & Variables ---
    
    // -- Public --
    
    
    // -- Private --
    private int currentDigit = -1;
    
    #endregion
    
    public void SetDigit(int digit)
    {
        if (digit < 0 || digit > 9)
        {
            Debug.Log("DigitDisplay - Invalid Digit: " + digit);
            digit = 0;
        }
        
        if (digit == currentDigit)
        {
            return;
        }
        
        currentDigit = digit;
        digitImage.sprite = digitSprites[digit];
    }
    
    public int GetDigit()
    {
        return currentDigit;
    }
}