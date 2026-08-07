using System;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    [Header("Day Settings")]
    [SerializeField] private float dayDurationSeconds = 300f;
    [SerializeField] private int finalDay = 5;

    public float TimeRemaining { get; private set; }
    public bool IsDayRunning { get; private set; }
    public bool IsEndingDay { get; private set; }

    public event Action<float> TimeChanged;
    public event Action<int> DayStarted;
    public event Action<int> DayEnded;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        TimeRemaining = dayDurationSeconds;
    }

    private void Update()
    {
        if (!IsDayRunning || IsEndingDay)
            return;
    
        TimeRemaining -= Time.unscaledDeltaTime;
        TimeRemaining = Mathf.Max(0f, TimeRemaining);
    
        TimeChanged?.Invoke(TimeRemaining);
    
        if (TimeRemaining <= 0f)
            EndCurrentDay();
    }

    public void StartCurrentDay()
    {
        Debug.Log("Starting timed day");

        TimeRemaining = dayDurationSeconds;
        IsDayRunning = true;
        IsEndingDay = false;

        int day = StudioManager.Instance.currentDay;

        TimeChanged?.Invoke(TimeRemaining);
        DayStarted?.Invoke(day);

        if (day > 1)
            PublisherManager.Instance.StartPublisherDay();
    }

    public void EndDayEarly()
    {
        if (!IsDayRunning)
            return;

        if (IsEndingDay)
            return;

        Debug.Log("Clocking out from Day " + StudioManager.Instance.currentDay);

        EndCurrentDay();
    }

    private void EndCurrentDay()
    {
        if (IsEndingDay)
            return;

        IsEndingDay = true;
        IsDayRunning = false;

        int completedDay = StudioManager.Instance.currentDay;

        Debug.Log("Ending Day " + completedDay);

        if (completedDay > 1 &&
            PublisherManager.Instance != null &&
            !PublisherManager.Instance.HasAnsweredCurrentDemand)
        {
            PublisherManager.Instance.RejectDemandFromTimeout();
        }

        StudioManager.Instance.CalculateDailyBudget();
        
        DayEnded?.Invoke(completedDay);

        if (completedDay >= finalDay)
        {
            StudioManager.Instance.ReleaseGame();
            return;
        }

        StudioManager.Instance.AdvanceDay();

        Debug.Log(
            "Now starting Day " +
            StudioManager.Instance.currentDay);

        StartCurrentDay();
    }
    
    public float DayProgress
    {
        get
        {
            if (dayDurationSeconds <= 0f)
                return 1f;

            return Mathf.Clamp01(
                1f - TimeRemaining / dayDurationSeconds);
        }
    }

    public int CurrentGameMinutes
    {
        get
        {
            const int startMinutes = 9 * 60;  // 9:00 AM
            const int endMinutes = 17 * 60;   // 5:00 PM
            const int workdayMinutes = endMinutes - startMinutes;

            // Before a day has started, display 9:00 AM.
            if (!IsDayRunning && !IsEndingDay)
                return startMinutes;

            float progress = Mathf.Clamp01(
                1f - (TimeRemaining / dayDurationSeconds));

            return startMinutes +
                   Mathf.RoundToInt(progress * workdayMinutes);
        }
    }
}