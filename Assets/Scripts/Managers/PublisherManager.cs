using UnityEngine;

public class PublisherManager : MonoBehaviour
{
    public static PublisherManager Instance;

    [Header("Current Demand")]
    public DarkPatternType currentDemand;
    public DarkPatternData currentDemandData;

    public bool HasAnsweredCurrentDemand { get; private set; }

    private DarkPatternData[] demands;

    // changed to assign automatically by PublisherUI when the Office scene loads.
    //
    private PublisherUI publisherUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        CreateDemands();
    }

    #region UI Registration

    public void RegisterPublisherUI(PublisherUI ui)
    {
        publisherUI = ui;

        Debug.Log("Publisher UI Registered.");

        // Restore an unanswered publisher demand when returning
        // to the Office scene.
        if (currentDemandData != null &&
            !HasAnsweredCurrentDemand &&
            StudioManager.Instance.currentDay > 1)
        {
            publisherUI.ShowDemand();
        }
    }

    public void UnregisterPublisherUI(PublisherUI ui)
    {
        if (publisherUI == ui)
        {
            publisherUI = null;

            Debug.Log("Publisher UI Unregistered.");
        }
    }

    #endregion

    private void CreateDemands()
    {
        demands = new DarkPatternData[]
        {
            new DarkPatternData(
                "Advertisements",
                "The publisher wants ads added to increase revenue.",
            
                -1, // Gameplay
                0, // Story
                -1, // Style
                -1, // Audience
                1, // Profit
                -10 // Ethics
            ),

            new DarkPatternData(
                "Microtransactions",
                "The publisher wants players to purchase extra content.",

                -1,
                0,
                0,
                -1,
                2,
                -20
            ),

            new DarkPatternData(
                "Loot Boxes",
                "The publisher wants random reward boxes added.",

                -1,
                0,
                -1,
                1,
                3,
                -35
            ),

            new DarkPatternData(
                "FOMO Events",
                "The publisher wants limited time events to keep players returning.",

                -1,
                0,
                0,
                1,
                2,
                -25
            ),

            new DarkPatternData(
                "Gacha System",
                "The publisher wants a gambling-style reward system.",

                -2,
                -1,
                1,
                2,
                4,
                -45
            )
        };
    }

    public void StartDayOne()
    {
        Debug.Log("Day 1: Studio Founded");

        StudioManager.Instance.currentDay = 1;
    }

    public void StartPublisherDay()
    {
        Debug.Log("StartPublisherDay called");

        int day = StudioManager.Instance.currentDay;

        HasAnsweredCurrentDemand = false;

        switch (day)
        {
            case 2:
                currentDemandData = demands[0]; // Advertisements
                break;

            case 3:
                currentDemandData = demands[1]; // Microtransactions
                break;

            case 4:
                currentDemandData = demands[2]; // Loot Boxes
                break;

            case 5:
                currentDemandData = demands[4]; // Gacha
                break;

            default:
                Debug.Log("No publisher demand for Day " + day);
                return;
        }

        Debug.Log(
            "Day " + day +
            " Publisher Demand: " +
            currentDemandData.name);

        if (publisherUI != null)
        {
            publisherUI.ShowDemand();
        }
        else
        {
            Debug.LogWarning(
                "PublisherUI has not registered yet.");
        }
    }

    public void AcceptDemand()
    {
        if (HasAnsweredCurrentDemand)
            return;

        ApplyDemand();

        HasAnsweredCurrentDemand = true;

        StudioManager.Instance.publisherSatisfaction =
            Mathf.Clamp(
                StudioManager.Instance.publisherSatisfaction + 5,
                0,
                100);

        StudioManager.Instance.NotifyStudioDataChanged();

        Debug.Log(
            "Accepted: " +
            currentDemandData.name +
            " | Publisher Trust: " +
            StudioManager.Instance.publisherSatisfaction);
    }

    public void RejectDemand()
    {
        if (HasAnsweredCurrentDemand)
            return;

        HasAnsweredCurrentDemand = true;

        StudioManager.Instance.publisherSatisfaction =
            Mathf.Clamp(
                StudioManager.Instance.publisherSatisfaction - 20,
                0,
                100);

        StudioManager.Instance.NotifyStudioDataChanged();

        Debug.Log(
            "Rejected: " +
            currentDemandData.name +
            " | Publisher Trust: " +
            StudioManager.Instance.publisherSatisfaction);
    }

    public void RejectDemandFromTimeout()
    {
        if (HasAnsweredCurrentDemand)
            return;

        HasAnsweredCurrentDemand = true;

        StudioManager.Instance.publisherSatisfaction =
            Mathf.Clamp(
                StudioManager.Instance.publisherSatisfaction - 20,
                0,
                100);

        StudioManager.Instance.NotifyStudioDataChanged();

        Debug.Log(
            "Demand timed out | Publisher Trust: " +
            StudioManager.Instance.publisherSatisfaction);

        if (publisherUI != null)
        {
            publisherUI.HideDemand();
        }
    }

    private void ApplyDemand()
    {
        GameProject project =
            StudioManager.Instance.currentProject;

        if (project == null || project.stats == null)
            return;

        project.stats.gameplay +=
            currentDemandData.gameplayModifier;

        project.stats.story +=
            currentDemandData.storyModifier;

        project.stats.style +=
            currentDemandData.styleModifier;

        project.stats.audience +=
            currentDemandData.audienceModifier;

        project.stats.profit +=
            currentDemandData.profitModifier;

        StudioManager.Instance.ethics +=
            currentDemandData.ethicsModifier;

        // stop stats from dropping below zero
        project.stats.gameplay =
            Mathf.Max(0, project.stats.gameplay);

        project.stats.story =
            Mathf.Max(0, project.stats.story);

        project.stats.style =
            Mathf.Max(0, project.stats.style);

        project.stats.audience =
            Mathf.Max(0, project.stats.audience);

        project.stats.profit =
            Mathf.Max(0, project.stats.profit);

        StudioManager.Instance.ethics =
            Mathf.Clamp(
                StudioManager.Instance.ethics,
                0,
                100);

        StudioManager.Instance.NotifyStudioDataChanged();
    }
}