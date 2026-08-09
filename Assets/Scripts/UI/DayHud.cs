using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayHUD : MonoBehaviour
{
    public static DayHUD Instance { get; private set; }

    [Header("Text")]
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text clockText;

    [Header("Controls")]
    [SerializeField] private Button endDayButton;

    [Header("Visuals")]
    [SerializeField] private GameObject hudVisuals;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        if (endDayButton != null)
        {
            endDayButton.onClick.AddListener(EndDayEarly);
        }

        RefreshDay();
        RefreshClock();
    }


    private void Update()
    {
        // Read directly from the global managers.
        // This avoids problems caused by scene/manager initialization order.

        RefreshDay();
        RefreshClock();

        if (endDayButton != null && DayManager.Instance != null)
        {
            endDayButton.interactable =
                DayManager.Instance.IsDayRunning &&
                !DayManager.Instance.IsEndingDay;
        }
    }


    private void RefreshDay()
    {
        if (dayText == null)
            return;

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
        if (clockText == null)
            return;

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