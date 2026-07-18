using System;
using System.Collections.Generic;
using UnityEngine;

public class SevenSegmentDisplayArray : MonoBehaviour
{
    [SerializeField] List<SevenSegmentDisplay> displays;
    private int arrayValue;
    private int displayCount;
    private int maximumValue;

    private void Start()
    {
        Initialize();
        // Testing Remove
        SetValue(256);
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

        try
        {
            maximumValue = Convert.ToInt32(maxValueString);
        }
        
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void SetValue(int value)
    {
        //Debug.Log("SevenSegmentDisplayArray - SetValue: " + value);
        
        arrayValue = value;
        SetChildValues();
    }

    private void SetChildValues()
    {
        //Debug.Log("SevenSegmentDisplayArray - SetChildValues: ");
        
        string valueString = arrayValue.ToString();
        
        for (int i = 0; i < displayCount; i++)
        {
            if (i < valueString.Length - 1)
            {
                displays[i].SetValue(Convert.ToInt32(valueString.Substring(i, 1)));
            }

            else
            {
                displays[i].SetValue(0);
            }
        }
    }

    public int GetValue()
    {
        return arrayValue;
    }
}
