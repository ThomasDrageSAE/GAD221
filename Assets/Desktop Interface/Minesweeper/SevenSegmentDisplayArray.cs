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
        
        Debug.Log("Maximum Value: " + maximumValue);
        Debug.Log("Minimum Value: " + minimumValue);
    }

    public void SetValue(int value)
    {
        //Debug.Log("SevenSegmentDisplayArray - SetValue: " + value);

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
            Debug.Log("Value length is greater than the amount of displays");
            return;
        }
        
        int emptyDisplays = displayCount - valueLength;
        
        for (int i = 0; i < displayCount; i++)
        {
            SevenSegmentDisplay display = displays[i];
            char character = valueString[i];
            
            if (i < emptyDisplays)
            {
                display.SetValue(-1);
                continue;
            }

            if (character.Equals('-'))
            {
                display.SetValue(10);
                continue;
            }

            if (character >= '0' && character <= '9')
            {
                display.SetValue(Convert.ToInt32(character));
                continue;
            }
            
            Debug.Log("Invalid Display Character: " + character);
        }
    }
}