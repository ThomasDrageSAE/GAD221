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

    private void Start()
    {
        if (endDayButton != null)
        {
            endDayButton.onClick.AddListener(EndDayEarly);
        }

        RefreshDay();
        RefreshClock();
    }

    private void OnEnable()
    {
        if (DayManager.Instance == null)
            return;

        DayManager.Instance.TimeChanged += OnTimeChanged;
        DayManager.Instance.DayStarted += OnDayStarted;
        DayManager.Instance.DayEnded += OnDayEnded;

        RefreshDay();
        RefreshClock();
    }

    private void OnDisable()
    {
        if (DayManager.Instance == null)
            return;

        DayManager.Instance.TimeChanged -= OnTimeChanged;
        DayManager.Instance.DayStarted -= OnDayStarted;
        DayManager.Instance.DayEnded -= OnDayEnded;
    }

    private void OnTimeChanged(float unusedTimeRemaining)
    {
        RefreshClock();
    }

    private void OnDayStarted(int day)
    {
        dayText.text = "DAY " + day;
        RefreshClock();

        if (endDayButton != null)
        {
            endDayButton.interactable = true;
        }
    }

    private void OnDayEnded(int day)
    {
        RefreshClock();

        if (endDayButton != null)
        {
            endDayButton.interactable = false;
        }
    }

    private void RefreshDay()
    {
        if (StudioManager.Instance == null)
        {
            dayText.text = "DAY --";
            return;
        }

        dayText.text =
            "DAY " + StudioManager.Instance.currentDay;
    }

    private void RefreshClock()
    {
        if (DayManager.Instance == null)
        {
            clockText.text = "--:--";
            return;
        }

        int totalMinutes =
            DayManager.Instance.CurrentGameMinutes;

        int hour24 = totalMinutes / 60;
        int minute = totalMinutes % 60;

        string period =
            hour24 >= 12 ? "PM" : "AM";

        int hour12 = hour24 % 12;

        if (hour12 == 0)
        {
            hour12 = 12;
        }

        clockText.text =
            hour12.ToString() +
            ":" +
            minute.ToString("00") +
            " " +
            period;
    }

    private void EndDayEarly()
    {
        if (DayManager.Instance != null)
        {
            DayManager.Instance.EndDayEarly();
        }
    }
}