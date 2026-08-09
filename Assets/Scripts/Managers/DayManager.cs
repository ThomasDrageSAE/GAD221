using System;
using System.Collections;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    [Header("Day Settings")]
    [SerializeField] private float dayDurationSeconds = 300f;
    [SerializeField] private int finalDay = 10;

    public float TimeRemaining { get; private set; }

    public bool IsDayRunning { get; private set; }
    public bool IsEndingDay { get; private set; }

    public event Action<float> TimeChanged;
    public event Action<int> DayStarted;
    public event Action<int> DayEnded;

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

        TimeRemaining =
            Mathf.Max(0f, TimeRemaining);

        TimeChanged?.Invoke(TimeRemaining);

        if (TimeRemaining <= 0f)
        {
            StartCoroutine(EndDayRoutine());
        }
    }


    public void StartCurrentDay()
    {
        if (StudioManager.Instance == null)
        {
            Debug.LogError(
                "DayManager could not find StudioManager.Instance.");

            return;
        }

        int day =
            StudioManager.Instance.currentDay;

        Debug.Log(
            "Starting timed Day " +
            day);

        TimeRemaining =
            dayDurationSeconds;

        IsDayRunning = true;
        IsEndingDay = false;

        TimeChanged?.Invoke(TimeRemaining);
        DayStarted?.Invoke(day);

        CaptureStartOfDayStats();

        if (day > 1 &&
            PublisherManager.Instance != null)
        {
            PublisherManager.Instance.StartPublisherDay();
        }
    }


    public void EndDayEarly()
    {
        if (!IsDayRunning)
            return;

        if (IsEndingDay)
            return;

        Debug.Log(
            "Clocking out from Day " +
            StudioManager.Instance.currentDay);

        StartCoroutine(
            EndDayRoutine());
    }


    private IEnumerator EndDayRoutine()
    {
        if (IsEndingDay)
            yield break;

        IsEndingDay = true;
        IsDayRunning = false;

        if (ScreenFade.Instance != null)
        {
            yield return ScreenFade.Instance.FadeOut();

        }

        int completedDay =
            StudioManager.Instance.currentDay;

        Debug.Log(
            "Ending Day " +
            completedDay);

        if (completedDay > 1 &&
            PublisherManager.Instance != null &&
            !PublisherManager.Instance.HasAnsweredCurrentDemand)
        {
            PublisherManager.Instance
                .RejectDemandFromTimeout();
        }

        if (StudioManager.Instance != null)
        {
            StudioManager.Instance
                .CalculateDailyBudget();
        }

        CaptureEndOfDaySummary(
            completedDay);

        DayEnded?.Invoke(
            completedDay);

        if (EndDaySummaryUI.Instance != null)
        {
            EndDaySummaryUI.Instance
                .ShowSummary();
        }
        else
        {
            Debug.LogWarning(
                "EndDaySummaryUI.Instance could not be found.");
        }
    }


    public void ContinueToNextDay()
    {
        if (!HasPendingDaySummary)
            return;

        StartCoroutine(
            ContinueToNextDayRoutine());
    }


    private IEnumerator ContinueToNextDayRoutine()
    {
        int completedDay =
            SummaryDay;

        ClearPendingDaySummary();

        if (completedDay >= finalDay)
        {
            Debug.Log(
                "Final day completed.");

            if (StudioManager.Instance != null)
            {
                StudioManager.Instance
                    .ReleaseGame();
            }

            yield break;
        }

        if (StudioManager.Instance == null)
        {
            Debug.LogError(
                "StudioManager.Instance is missing.");

            yield break;
        }

        StudioManager.Instance
            .AdvanceDay();

        Debug.Log(
            "Advanced to Day " +
            StudioManager.Instance.currentDay);

        TimeRemaining =
            dayDurationSeconds;

        TimeChanged?.Invoke(
            TimeRemaining);

        if (GameSceneLoader.Instance != null)
        {
            GameSceneLoader.Instance.OnGameSceneLoaded +=
                OnNextDayDesktopLoaded;

            GameSceneLoader.Instance.LoadScene(
                "DesktopInterface");

            yield break;
        }

        Debug.LogError(
            "DayManager could not find GameSceneLoader.Instance.");
    }


    private void OnNextDayDesktopLoaded(
        string sceneName)
    {
        if (sceneName != "DesktopInterface")
            return;

        if (GameSceneLoader.Instance != null)
        {
            GameSceneLoader.Instance.OnGameSceneLoaded -=
                OnNextDayDesktopLoaded;
        }

        StartCoroutine(
            StartNextDayAfterLoad());
    }


    private IEnumerator StartNextDayAfterLoad()
    {
        yield return null;

        if (ScreenFade.Instance != null)
        {
            yield return ScreenFade.Instance.FadeIn();
        }

        StartCurrentDay();
    }


    public float DayProgress
    {
        get
        {
            if (dayDurationSeconds <= 0f)
                return 1f;

            return Mathf.Clamp01(
                1f -
                (TimeRemaining /
                 dayDurationSeconds));
        }
    }


    public int CurrentGameMinutes
    {
        get
        {
            const int startMinutes =
                9 * 60;

            const int endMinutes =
                17 * 60;

            const int workdayMinutes =
                endMinutes - startMinutes;

            if (!IsDayRunning &&
                !IsEndingDay)
            {
                return startMinutes;
            }

            float progress =
                Mathf.Clamp01(
                    1f -
                    (TimeRemaining /
                     dayDurationSeconds));

            return startMinutes +
                   Mathf.RoundToInt(
                       progress *
                       workdayMinutes);
        }
    }


    public void StopCurrentDay()
    {
        IsDayRunning = false;
        IsEndingDay = true;

        Debug.Log(
            "Day stopped.");
    }


    private void CaptureStartOfDayStats()
    {
        if (StudioManager.Instance == null)
            return;

        GameProject project =
            StudioManager.Instance.currentProject;

        if (project == null ||
            project.stats == null)
        {
            return;
        }

        startBudget =
            StudioManager.Instance.money;

        startTrust =
            StudioManager.Instance.publisherSatisfaction;

        startEthics =
            StudioManager.Instance.ethics;

        startGameplay =
            project.stats.gameplay;

        startStory =
            project.stats.story;

        startStyle =
            project.stats.style;

        startAudience =
            project.stats.audience;

        startProfit =
            project.stats.profit;

        Debug.Log(
            "Day " +
            StudioManager.Instance.currentDay +
            " starting stats captured.");
    }


    private void CaptureEndOfDaySummary(
        int completedDay)
    {
        if (StudioManager.Instance == null)
            return;

        GameProject project =
            StudioManager.Instance.currentProject;

        if (project == null ||
            project.stats == null)
        {
            return;
        }

        SummaryDay =
            completedDay;

        SummaryBudgetChange =
            StudioManager.Instance.money -
            startBudget;

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

        Debug.Log(
            "End-of-day summary cleared.");
    }


    private void OnDestroy()
    {
        if (GameSceneLoader.Instance != null)
        {
            GameSceneLoader.Instance.OnGameSceneLoaded -=
                OnNextDayDesktopLoaded;
        }
    }
}