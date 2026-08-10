using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayHUD : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text clockText;

    [Header("Controls")]
    [SerializeField] private Button endDayButton;

    [Header("Visuals")]
    [SerializeField] private GameObject hudVisuals;

    private void Awake()
    {
        DayManager.OnTimerTimeElapsed += UpdateClock;
        DayManager.OnDayChanged += UpdateDay;
    }

    private void Start()
    {
        if (endDayButton != null)
        {
            endDayButton.onClick.AddListener(EndDayButtonPress);
        }
        
        UpdateDay();
        UpdateClock();
    }
    
    private void Update()
    {
        // Read directly from the global managers.
        // This avoids problems caused by scene/manager initialization order.
        
        // UpdateDay(DayManager.Instance.CurrentDay);
        // UpdateClock(DayManager.Instance.CurrentDay);
        
        // ^^^ Avoid this! Having a bunch of random scripts access the managers every frame in update is bad! Especially for UI which usually doesn't have any business touching Update.
        // Instead of this hook into an event that is fired by the object when the state of the value the UI is meant to display changes.
        // For example the event you already added to DayManager that fires when the time changes. That way there is no operations happening unless the value has actually changed.
        // - Frank
        
        //endDayButton.interactable = DayManager.Instance.DayStarted;
    }
    
    private void UpdateDay()
    {
        if (dayText == null) return;

        if (DayManager.Instance == null)
        {
            dayText.text = "DAY --";
            return;
        }

        dayText.text = "DAY " + DayManager.Instance.CurrentDay;
    }
    
    private void UpdateClock()
    {
        if (clockText == null) return;

        if (DayManager.Instance == null)
        {
            clockText.text = "--:--";
            return;
        }

        int totalMinutes = DayManager.Instance.CurrentGameMinutes;

        int hour24 = totalMinutes / 60;
        int minute = totalMinutes % 60;

        string period = hour24 >= 12 ? "PM" : "AM";

        int hour12 = hour24 % 12;

        if (hour12 == 0)
        {
            hour12 = 12;
        }

        clockText.text = hour12 + ":" + minute.ToString("00") + " " + period;
    }
    
    public void EndDayButtonPress()
    {
        DayManager.Instance.EndDay();
    }
    
    public void ShowHUD()
    {
        if (hudVisuals != null)
        {
            hudVisuals.SetActive(true);
        }
    }
    
    public void HideHUD()
    {
        if (hudVisuals != null)
        {
            hudVisuals.SetActive(false);
        }
    }
}