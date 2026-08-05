using UnityEngine;

public class PublisherManager : MonoBehaviour
{
    public static PublisherManager Instance;

    [Header("Current Demand")]
    public DarkPatternType currentDemand;
    public DarkPatternData currentDemandData;

    public bool HasAnsweredCurrentDemand { get; private set; }

    private int currentDay = 0;
    private DarkPatternData[] demands;

    // changed to assign automatically by PublisherUI when the Office scene loads.
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
                -1,
                1,
                -10),

            new DarkPatternData(
                "Microtransactions",
                "The publisher wants players to purchase extra content.",
                -1,
                2,
                -20),

            new DarkPatternData(
                "Loot Boxes",
                "The publisher wants random reward boxes added.",
                1,
                3,
                -35),

            new DarkPatternData(
                "FOMO Events",
                "The publisher wants limited time events to keep players returning.",
                1,
                2,
                -25),

            new DarkPatternData(
                "Gacha System",
                "The publisher wants a gambling-style reward system.",
                2,
                4,
                -45)
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

        if (currentDay >= demands.Length)
        {
            Debug.Log("No more publisher demands.");
            return;
        }

        HasAnsweredCurrentDemand = false;

        currentDemandData = demands[currentDay];

        Debug.Log("Current demand: " + currentDemandData.name);

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

        Debug.Log(
            "Accepted publisher demand: " +
            currentDemandData.name);

        currentDay++;
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

        Debug.Log(
            "Rejected publisher demand: " +
            currentDemandData.name);

        currentDay++;
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

        Debug.Log(
            "Publisher demand automatically rejected.");

        currentDay++;

        if (publisherUI != null)
        {
            publisherUI.HideDemand();
        }
    }

    private void ApplyDemand()
    {
        GameProject project =
            StudioManager.Instance.currentProject;

        if (project == null)
            return;

        project.stats.audience +=
            currentDemandData.audienceModifier;

        project.stats.profit +=
            currentDemandData.profitModifier;

        StudioManager.Instance.ethics +=
            currentDemandData.ethicsModifier;
    }
}