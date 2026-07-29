using UnityEngine;

public class PublisherManager : MonoBehaviour
{
    public static PublisherManager Instance;


    [Header("Current Demand")]
    public DarkPatternType currentDemand;

    public DarkPatternData currentDemandData;


    private int currentDay = 0;


    private DarkPatternData[] demands;


    private void Awake()
    {
        Instance = this;

        CreateDemands();
    }


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



    public void StartPublisherDay()
    {
        Debug.Log("StartPublisherDay called");

        if (currentDay >= demands.Length)
        {
            Debug.Log("No more publisher demands.");
            return;
        }

        currentDemandData = demands[currentDay];

        Debug.Log("Current demand: " + currentDemandData.name);

        PublisherUI ui = FindFirstObjectByType<PublisherUI>();

        if (ui != null)
        {
            Debug.Log("Found PublisherUI");
            ui.ShowDemand();
        }
        else
        {
            Debug.LogError("PublisherUI not found in the scene!");
        }
    }


    public void StartDayOne()
    {
        Debug.Log(
            "Day 1: Studio Founded"
        );


        StudioManager.Instance.currentDay = 1;
    }

    public void AcceptDemand()
    {
        ApplyDemand();

        Debug.Log(
            "Accepted publisher demand: " +
            currentDemandData.name
        );


        currentDay++;

        StudioManager.Instance.EndDay();
    }



    public void RejectDemand()
    {
        Debug.Log(
            "Rejected publisher demand: " +
            currentDemandData.name
        );


        currentDay++;

        StudioManager.Instance.EndDay();
    }



    private void ApplyDemand()
    {
        GameProject project =
            StudioManager.Instance.currentProject;


        project.stats.audience +=
            currentDemandData.audienceModifier;


        project.stats.profit +=
            currentDemandData.profitModifier;


        StudioManager.Instance.ethics +=
            currentDemandData.ethicsModifier;
    }
}