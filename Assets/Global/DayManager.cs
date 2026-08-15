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
    
    // Summary
    public bool HasPendingDaySummary { get; private set; }

    public int SummaryDay { get; private set; }

    public int SummaryBudgetChange { get; private set; }
    public int SummaryTrustChange { get; private set; }
    public int SummaryEthicsChange { get; private set; }

    public int SummaryGameplayChange { get; private set; }
    public int SummaryStoryChange { get; private set; }
    public int SummaryStyleChange { get; private set; }
    public int SummaryAudienceChange { get; private set; }
    public int SummaryProfitChange { get; private set; }

    private int startBudget;
    private int startTrust;
    private int startEthics;

    private int startGameplay;
    private int startStory;
    private int startStyle;
    private int startAudience;
    private int startProfit;

    
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
        
        
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        ResetDay();
        StartDay();
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
        
        //Debug.Log("Day: " + CurrentDay + " Started");
        DayStarted = true;
        StartTimer();
        OnDayStarted?.Invoke(CurrentDay);
        CaptureStartOfDayStats();
    }

    public void EndDay()
    {
        if (!DayStarted)
            return;

        Debug.Log("Day: " + CurrentDay + " Ended");

        StopTimer();

        if (StudioManager.Instance != null && StudioManager.Instance.money <= 0)
        {
            if (GameOverUI.Instance != null)
            {
                GameOverUI.Instance.ShowBankruptcy();
            }

            return;
        }

        OnDayEnded?.Invoke(CurrentDay);

        CaptureEndOfDaySummary(CurrentDay);

        if (IsFinalDay)
        {
            Debug.Log("Final Day Ended.");
            OnFinalDayEnded?.Invoke();
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
        ClearPendingDaySummary();
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
    
    #region Summary Methods
    private void CaptureStartOfDayStats()
    {
        
        GameProject project = StudioManager.Instance.currentProject;

        if (project == null || project.stats == null)
        {
            return;
        }

        startBudget = StudioManager.Instance.money;

        startTrust = StudioManager.Instance.publisherSatisfaction;

        startEthics = StudioManager.Instance.ethics;

        startGameplay = project.stats.gameplay;

        startStory = project.stats.story;

        startStyle = project.stats.style;

        startAudience = project.stats.audience;

        startProfit = project.stats.profit;

        Debug.Log(
            "Day " +
            CurrentDay +
            " starting stats captured.");
    }


    private void CaptureEndOfDaySummary(int completedDay)
    {
        if (StudioManager.Instance == null)
            return;

        GameProject project = StudioManager.Instance.currentProject;

        if (project == null || project.stats == null)
        {
            return;
        }

        SummaryDay = completedDay;

        SummaryBudgetChange = StudioManager.Instance.money - startBudget;

        SummaryTrustChange =
            StudioManager.Instance.publisherSatisfaction -
            startTrust;

        SummaryEthicsChange =
            StudioManager.Instance.ethics -
            startEthics;

        SummaryGameplayChange =
            project.stats.gameplay -
            startGameplay;

        SummaryStoryChange =
            project.stats.story -
            startStory;

        SummaryStyleChange =
            project.stats.style -
            startStyle;

        SummaryAudienceChange =
            project.stats.audience -
            startAudience;

        SummaryProfitChange =
            project.stats.profit -
            startProfit;

        HasPendingDaySummary = true;

        Debug.Log(
            "End of Day " +
            completedDay +
            " summary captured.");
    }


    public void ClearPendingDaySummary()
    {
        HasPendingDaySummary = false;

        Debug.Log("End-of-day summary cleared.");
    }
    #endregion
}