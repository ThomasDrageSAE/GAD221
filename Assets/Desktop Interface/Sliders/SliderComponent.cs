using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class SliderComponent : UIComponent
{
    #region --- Inspector References ---
    
    // -- In Scene --
    [SerializeField] private DragHandle dragHandle;
    
    // -- Resources & Prefabs --
    
    
    #endregion
    
    #region --- Properties & Variables ---
    
    // -- Public --
    
    
    // -- Private --
    [SerializeField] private SliderValue[] sliderValues;
    [SerializeField] private int defaultSliderIndex;
    private int sliderIndex;
    private float trackValue;
    
    
    private RectTransform handleRectTransform;
    private float dragStartX;
    
    [Serializable] public struct SliderValue
    {
        public int horizontalPos;
        public int value;

        public SliderValue(int horizontalPos, int value)
        {
            this.horizontalPos = horizontalPos;
            this.value = value;
        }
    }
    
    #endregion
    
    #region --- Events ---
    
    public UnityEvent<int> onSliderValueChanged;
    
    protected override void EventSubscription()
    {
        base.EventSubscription();
        
        dragHandle.onDragStart.AddListener(DragStart);
        dragHandle.onDrag.AddListener(Drag);
    }
    
    protected override void EventUnsubscription()
    {
        base.EventUnsubscription();
        
        dragHandle.onDragStart.RemoveListener(DragStart);
        dragHandle.onDrag.RemoveListener(Drag);
    }
    
    #endregion
    
    #region --- Initialization & Termination ---
    
    protected override void Start()
    {
        base.Start();
        
        handleRectTransform = dragHandle.transform as RectTransform;
        SetSliderIndex(defaultSliderIndex);
    }
    
    #endregion

    #region --- Getters & Setters ---
    
    public void SetSliderIndex(int index)
    {
        sliderIndex = Mathf.Clamp(index, 0, sliderValues.Length - 1); // Clamp index to array bounds
        
        Vector2 newHandlePos = handleRectTransform.anchoredPosition; // Get a copy of the handles position.
        newHandlePos.x = sliderValues[sliderIndex].horizontalPos; // Set the handle horizontal pos to the sliderValues horizontalPos.
        SetHandlePos(newHandlePos); // Set to new handle position.
        
        trackValue = (float)sliderIndex / (sliderValues.Length - 1); // Get track value (normalised float 0-1 range)
        onSliderValueChanged?.Invoke(GetSliderValue());
        
        Debug.Log("Index: " + GetSliderIndex() + ", Value: " + GetSliderValue());
    }
    
    public void SetHandlePos(Vector2 newHandlePos)
    {
        handleRectTransform.anchoredPosition = newHandlePos;
    }
    
    public float GetTrackValue()
    {
        return trackValue;
    }

    public int GetSliderIndex()
    {
        return sliderIndex;
    }
    
    public int GetSliderValue()
    {
        return sliderValues[sliderIndex].value;
    }
    
    private int GetNearestIndex(float horizontalPos)
    {
        int nearestIndex = 0;
        float nearestDistance = Mathf.Abs(sliderValues[0].horizontalPos - horizontalPos);
        
        for (int i = 1; i < sliderValues.Length; i++)
        {
            float distance = Mathf.Abs(sliderValues[i].horizontalPos - horizontalPos);
            
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestIndex = i;
            }
        }
        
        return nearestIndex;
    }
    
    #endregion
    
    private void DragStart(DragHandle.StartDragData data)
    {
        dragStartX = handleRectTransform.anchoredPosition.x;
    }
    
    private void Drag(DragHandle.DragData data)
    {
        float candidateX = dragStartX + data.posDifference.x;
        SetSliderIndex(GetNearestIndex(candidateX));
    }
}