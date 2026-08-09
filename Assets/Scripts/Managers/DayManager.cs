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


        // end the day at 5:00 PM.
        if (TimeRemaining <= 0f)
        {
            StartCoroutine(EndDayRoutine());
        }
    }


    // START DAY
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


        // Day 1 is the tutorial/setup day.
        // Publisher demands begin on Day 2.
        if (day > 1 &&
            PublisherManager.Instance != null)
        {
            PublisherManager.Instance.StartPublisherDay();
        }
    }


    // CLOCK OUT
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


    // END DAY

    private IEnumerator EndDayRoutine()
    {
        if (IsEndingDay)
            yield break;


        IsEndingDay = true;
        IsDayRunning = false;


        // Fade to black 
        if (ScreenFade.Instance != null)
        {
            yield return
                ScreenFade.Instance.FadeOut();
        }


        int completedDay =
            StudioManager.Instance.currentDay;


        Debug.Log(
            "Ending Day " +
            completedDay);


        // if player ignores the publisher email,automatically reject
        if (completedDay > 1 &&
            PublisherManager.Instance != null &&
            !PublisherManager.Instance.HasAnsweredCurrentDemand)
        {
            PublisherManager.Instance
                .RejectDemandFromTimeout();
        }


        // Calculate today's finances.
        if (StudioManager.Instance != null)
        {
            StudioManager.Instance
                .CalculateDailyBudget();
        }


        DayEnded?.Invoke(
            completedDay);


        
        // FINAL DAY

        if (completedDay >= finalDay)
        {
            Debug.Log(
                "Final day completed.");


            if (StudioManager.Instance != null)
            {
                StudioManager.Instance
                    .ReleaseGame();
            }


           
            if (ScreenFade.Instance != null)
            {
                yield return
                    ScreenFade.Instance.FadeIn();
            }


            yield break;
        }


        // NEXT DAy
        StudioManager.Instance.AdvanceDay();

        Debug.Log(
            "Advanced to Day " +
            StudioManager.Instance.currentDay);
        
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


    private void OnNextDayDesktopLoaded(string sceneName)
    {
        if (sceneName != "DesktopInterface")
            return;

        if (GameSceneLoader.Instance != null)
        {
            GameSceneLoader.Instance.OnGameSceneLoaded -=
                OnNextDayDesktopLoaded;
        }

        Debug.Log(
            "Desktop restarted for Day " +
            StudioManager.Instance.currentDay);

        StartCoroutine(
            StartDayAfterDesktopBoot());
    }
    private IEnumerator StartDayAfterDesktopBoot()
    {
        yield return null;

        StartCurrentDay();

        if (ScreenFade.Instance != null)
        {
            yield return ScreenFade.Instance.FadeIn();
        }
    }
    
    // DAY PROGRESS
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

    // GAME CLOCK

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


            // Before the timed day begins,
            // display 9:00 AM.
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

// STOP DAY
    

    public void StopCurrentDay()
    {
        IsDayRunning = false;
        IsEndingDay = true;


        Debug.Log(
            "Day stopped.");
    }
}