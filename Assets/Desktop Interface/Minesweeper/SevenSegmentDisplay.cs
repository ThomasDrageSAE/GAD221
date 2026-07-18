using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SevenSegmentDisplay : UIComponent
{
    [SerializeField] Image image;
    [SerializeField] private List<Sprite> sprites;
    
    SevenSegmentValues value;

    public enum SevenSegmentValues
    {
        Empty, Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Dash
    }

    private void UpdateDisplay()
    {
        Sprite sprite = sprites[GetValueNumeric()];
        
        //Debug.Log("SevenSegmentDisplay - " + sprite.name);
        
        image.sprite = sprites[GetValueNumeric()];
    }

    public void SetValue(SevenSegmentValues ssValue)
    {
        //Debug.Log("SevenSegmentDisplay - SetValue: " + ssValue);
        
        value = ssValue;
        UpdateDisplay();
    }

    public void SetValue(int numValue)
    {
        //Debug.Log("SevenSegmentDisplay - SetValueNumeric: " + numValue);
        
        switch (numValue)
        {
            case 0:
                value = SevenSegmentValues.Zero;
                break;
            case 1:
                value = SevenSegmentValues.One;
                break;
            case 2:
                value = SevenSegmentValues.Two;
                break;
            case 3:
                value = SevenSegmentValues.Three;
                break;
            case 4:
                value = SevenSegmentValues.Four;
                break;
            case 5:
                value = SevenSegmentValues.Five;
                break;
            case 6:
                value = SevenSegmentValues.Six;
                break;
            case 7:
                value = SevenSegmentValues.Seven;
                break;
            case 8:
                value = SevenSegmentValues.Eight;
                break;
            case 9:
                value = SevenSegmentValues.Nine;
                break;
            case 10:
                value = SevenSegmentValues.Dash;
                break;
            case 11:
                value = SevenSegmentValues.Empty;
                break;
            default:
                value = SevenSegmentValues.Empty;
                break;
        }
        
        UpdateDisplay();
    }

    public SevenSegmentValues GetValue()
    {
        //Debug.Log("SevenSegmentDisplay - GetValue: " + value);
        
        return value;
    }

    public int GetValueNumeric()
    {
        //Debug.Log("SevenSegmentDisplay - GetValueNumeric: " + value);
        
        switch (value)
        {
            case SevenSegmentValues.Zero:
                return 0;
            case SevenSegmentValues.One:
                return 1;
            case SevenSegmentValues.Two:
                return 2;
            case SevenSegmentValues.Three:
                return 3;
            case SevenSegmentValues.Four:
                return 4;
            case SevenSegmentValues.Five:
                return 5;
            case SevenSegmentValues.Six:
                return 6;
            case SevenSegmentValues.Seven:
                return 7;
            case SevenSegmentValues.Eight:
                return 8;
            case SevenSegmentValues.Nine:
                return 9;
            case SevenSegmentValues.Dash:
                return 10;
            case SevenSegmentValues.Empty:
                return 11;
            default:
                return 11;
        }
    }
}