using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class DayManager : Singleton<DayManager>
{
    // Day
    [Header("Day")]
    public int FinalDay = 10;

    public int CurrentDay { get; private set; } = 1;
    public bool IsFinalDay => CurrentDay == FinalDay;
    
    public bool DayStarted { get; private set; }
    
    // Timer
    [SerializeField] private float timerDurationSeconds = 300f;
    private float timerSecondsRemaining;
    public float TimerProgress // Normalized day progress value between 0-1
    {
        get
        {
            if (timerDurationSeconds <= 0f)
            {
                return 1f;
            }

            return Mathf.Clamp01(1f - timerSecondsRemaining / timerDurationSeconds);
        }
    }
    public bool TimerActive { get; private set; }
    
    // Game Time
    public const int GameStartMinutes = 540;
    public const int GameEndMinutes = 1020;
    public const int TotalGameMinutes = GameEndMinutes - GameStartMinutes;
    public int CurrentGameMinutes => GameStartMinutes + Mathf.RoundToInt(TimerProgress * TotalGameMinutes);
    
    // Events
    public static event Action OnTimerTimeElapsed;
    
    public static event Action<int> OnDayStarted;
    
    public static event Action<int> OnDayEnded;
    
    public static event Action OnDayChanged;

    public static event Action OnFinalDayEnded;
    
    #region Lifecycle Methods

    protected override void Awake()
    {
        base.Awake();
        
        Initialize();
        
    }

    private void Initialize()
    {
        ResetDay();
    }
    
    private void Update()
    {
        if (!DayStarted)
        {
            return;
        }
        
        Timer();
    }
    #endregion
    
    #region Day Methods
    public void StartDay()
    {
        if (DayStarted) return;
        
        Debug.Log("Day: " + CurrentDay + " Started");
        DayStarted = true;
        StartTimer();
        OnDayStarted?.Invoke(CurrentDay);
    }

    public void EndDay()
    {
        if (!DayStarted) return;
        
        Debug.Log("Day: " + CurrentDay + " Ended");
        OnDayEnded?.Invoke(CurrentDay);

        if (IsFinalDay)
        {
            Debug.Log("Final Day Ended.");
            OnFinalDayEnded?.Invoke();
        }

        else
        {
            NextDay();
        }
    }
    
    public void ResetDay()
    {
        DayStarted = false;
        ResetTimer();
    }
    
    public void NextDay()
    {
        ResetDay();
        CurrentDay++;
        OnDayChanged?.Invoke();
        Debug.Log("Current Day: " + CurrentDay);
        
        StartDay();
    }
    #endregion
    
    #region Timer Methods
    public void StartTimer()
    {
        TimerActive = true;
    }
    
    public void StopTimer()
    {
        TimerActive = false;
    }

    private void ResetTimer()
    {
        timerSecondsRemaining = timerDurationSeconds;
        OnTimerTimeElapsed?.Invoke();
        TimerActive = false;
    }

    private void TimerFinished()
    {
        StopTimer();
        EndDay();
    }
    
    private void Timer()
    {
        //Debug.Log("Timer: " + timerSecondsRemaining);
        
        if (!TimerActive) return;
        
        timerSecondsRemaining -= Time.unscaledDeltaTime;
        timerSecondsRemaining = Mathf.Max(0f, timerSecondsRemaining);
        OnTimerTimeElapsed?.Invoke();
        
        if (timerSecondsRemaining <= 0f) TimerFinished();
    }
    #endregion
}