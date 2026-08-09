using System;
using System.Collections;
using UnityEngine;

public class DayManager : Singleton<DayManager>
{
    [Header("Day Settings")]
    [SerializeField] private float dayDurationSeconds = 300f;
    [SerializeField] private int finalDay = 5;

    [Header("Scene Names")]
    [SerializeField] private string officeSceneName = "Office";
    [SerializeField] private string desktopSceneName = "DesktopInterface";

    public float TimeRemaining { get; private set; }

    public bool IsDayRunning { get; private set; }
    public bool IsEndingDay { get; private set; }

    public event Action<float> TimeChanged;
    public event Action<int> DayStarted;
    public event Action<int> DayEnded;

    private bool waitingForOfficeToLoad;


    protected override void Awake()
    {
        base.Awake();
        
        
    }

    private void Start()
    {
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

        Debug.Log(
            "Starting timed Day " +
            StudioManager.Instance.currentDay);

        TimeRemaining = dayDurationSeconds;

        IsDayRunning = true;
        IsEndingDay = false;

        int day =
            StudioManager.Instance.currentDay;

        TimeChanged?.Invoke(TimeRemaining);

        DayStarted?.Invoke(day);

        // Day 1 has no publisher demand.
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

        StartCoroutine(EndDayRoutine());
    }


    private IEnumerator EndDayRoutine()
    {
        if (IsEndingDay)
            yield break;

        IsEndingDay = true;
        IsDayRunning = false;

        // Fade to black.
        if (ScreenFade.Instance != null)
        {
            yield return ScreenFade.Instance.FadeOut();
        }

        int completedDay =
            StudioManager.Instance.currentDay;

        Debug.Log(
            "Ending Day " +
            completedDay);

        // If  player did not answer the publisher request,automatically reject it.
        if (completedDay > 1 &&
            PublisherManager.Instance != null &&
            !PublisherManager.Instance.HasAnsweredCurrentDemand)
        {
            PublisherManager.Instance
                .RejectDemandFromTimeout();
        }

        // Calculate finances at the end of the day.
        StudioManager.Instance
            .CalculateDailyBudget();

        DayEnded?.Invoke(completedDay);

        // Final day.
        if (completedDay >= finalDay)
        {
            Debug.Log("Final day completed.");

            StudioManager.Instance.ReleaseGame();

            if (ScreenFade.Instance != null)
            {
                yield return ScreenFade.Instance.FadeIn();
            }

            yield break;
        }

        // Move to next day.
        StudioManager.Instance.AdvanceDay();

        Debug.Log(
            "Advanced to Day " +
            StudioManager.Instance.currentDay);

        // If  using the desktop,return to the Office before starting the next day.
        if (GameSceneLoader.Instance != null &&
            GameSceneLoader.Instance.currentGameScene ==
            desktopSceneName)
        {
            Debug.Log(
                "Clocked out from Desktop - returning to Office.");

            waitingForOfficeToLoad = true;

            GameSceneLoader.Instance.OnGameSceneLoaded +=
                OnGameSceneLoaded;

            GameSceneLoader.Instance.LoadScene(
                officeSceneName);

            yield break;
        }

        StartCurrentDay();

        // Fade back in.
        if (ScreenFade.Instance != null)
        {
            yield return ScreenFade.Instance.FadeIn();
        }
    }


    private void OnGameSceneLoaded(
        string sceneName)
    {
        if (!waitingForOfficeToLoad)
            return;

        if (sceneName != officeSceneName)
            return;

        waitingForOfficeToLoad = false;

        if (GameSceneLoader.Instance != null)
        {
            GameSceneLoader.Instance.OnGameSceneLoaded -=
                OnGameSceneLoaded;
        }

        Debug.Log(
            "Office loaded - preparing Day " +
            StudioManager.Instance.currentDay);

        StartCoroutine(
            StartDayAfterSceneLoad());
    }


    private IEnumerator StartDayAfterSceneLoad()
    {
        
        yield return null;

        StartCurrentDay();

        if (ScreenFade.Instance != null)
        {
            yield return ScreenFade.Instance.FadeIn();
        }
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
            
            // show 9:00 AM.
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


    private void OnDestroy()
    {
        if (GameSceneLoader.Instance != null)
        {
            GameSceneLoader.Instance.OnGameSceneLoaded -=
                OnGameSceneLoaded;
        }
    }
    
    public void StopCurrentDay()
    {
        IsDayRunning = false;
        IsEndingDay = true;
    }
}