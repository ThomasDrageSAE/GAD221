using System;
using System.Collections.Generic;
using UnityEngine;

public class SevenSegmentDisplayArray : MonoBehaviour
{
    [SerializeField] List<SevenSegmentDisplay> displays;
    private int arrayValue;
    private int displayCount;
    private int maximumValue;
    private int minimumValue;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        //Debug.Log("SevenSegmentDisplayArray - Initialize");
        
        displayCount = displays.Count;

        string maxValueString = "";
        for (int i = 0; i < displayCount; i++)
        {
            maxValueString += "9";
        }
        
        maximumValue = Convert.ToInt32(maxValueString);
        minimumValue = -(maximumValue / 10);
        
        //Debug.Log("Maximum Value: " + maximumValue);
        //Debug.Log("Minimum Value: " + minimumValue);
    }

    public void SetValue(int value)
    {
        //Debug.Log("SevenSegmentDisplayArray - SetTrackValue: " + value);

        if (value <= maximumValue && value >= minimumValue)
        {
            arrayValue = value;
            SetChildValues(arrayValue);
        }
        
        else
        {
            Debug.Log("Value is outside of range for amount of displays");
        }
    }

    private void SetChildValues(int value)
    {
        string valueString = value.ToString();
        int valueLength = valueString.Length;
        
        if (valueLength > displays.Count)
        {
            //Debug.Log("Value length is greater than the amount of displays");
            return;
        }
        
        int emptyDisplays = displayCount - valueLength;
        
        for (int i = 0; i < displayCount; i++)
        {
            SevenSegmentDisplay display = displays[i];
            
            if (i < emptyDisplays)
            {
                //Debug.Log("empty");
                display.SetValue(11);
                continue;
            }
            
            char character = valueString[i - emptyDisplays];

            if (character.Equals('-'))
            {
                //Debug.Log("dash");
                display.SetValue(10);
                continue;
            }

            if (character >= '0' && character <= '9')
            {
                //Debug.Log("num: " + character);
                display.SetValue((int)char.GetNumericValue(character));
                continue;
            }
            
            //Debug.Log("Invalid Display Character: " + character);
        }
    }

    public int GetMaxValue()
    {
        return maximumValue;
    }

    public int GetMinValue()
    {
        return minimumValue;
    }

    public int GetDisplayCount()
    {
        return displayCount;
    }

    public void ResetDisplay()
    {
        arrayValue = 0;
        
        foreach (SevenSegmentDisplay display in displays)
        {
            display.SetValue(11);    
        }
    }
}