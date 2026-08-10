using System;
using UnityEngine;

public class PublisherManager : Singleton<PublisherManager>
{
    [Header("Current Demand")]
    public DarkPatternType currentDemand;
    public DarkPatternData currentDemandData;

    public bool HasAnsweredCurrentDemand { get; private set; }

    private DarkPatternData[] demands;

    private string lastDecisionText = "";

    public string LastDecisionText
    {
        get { return lastDecisionText; }
    }
    
    private PublisherUI publisherUI;

    protected override void Awake()
    {
        base.Awake();
        
        DayManager.OnDayEnded += OnDayEnded;
        DayManager.OnDayStarted += StartPublisherDay;
        
        CreateDemands();
    }

    private void OnDestroy()
    {
        DayManager.OnDayEnded -= OnDayEnded;
        DayManager.OnDayStarted -= StartPublisherDay;
    }

    private void Start()
    {
        
    }

    //increase the days to 10, and day 1 will act as a tutorial
    private void CreateDemands()
    {
        demands = new DarkPatternData[]
        {
            // DAY 2
            new DarkPatternData(
                "Advertisements",
                "Add advertisements to the game to create an additional revenue stream.",

                -1, // Gameplay
                 0, // Story
                -1, // Style
                -1, // Audience
                 1, // Profit
                -5  // Ethics
            ),

            // DAY 3
            new DarkPatternData(
                "Microtransactions",
                "Allow players to purchase optional items and content with real money.",

                -1,
                 0,
                 0,
                 0,
                 2,
                -10
            ),

            // DAY 4
            new DarkPatternData(
                "Premium Currency",
                "Introduce a premium currency that players purchase before buying items.",

                -1,
                 0,
                -1,
                 0,
                 3,
                -15
            ),

            // DAY 5
            new DarkPatternData(
                "Loot Boxes",
                "Add purchasable random reward boxes containing items of varying rarity.",

                -1,
                 0,
                -1,
                 1,
                 4,
                -20
            ),

            // DAY 6
            new DarkPatternData(
                "Daily Rewards",
                "Reward players for returning every day, with larger rewards for maintaining a streak.",

                -1,
                 0,
                 0,
                 2,
                 4,
                -25
            ),

            // DAY 7
            new DarkPatternData(
                "Limited-Time Events",
                "Introduce rewards that are only available for a short period to encourage players to return.",

                -2,
                 0,
                 0,
                 2,
                 5,
                -30
            ),

            // DAY 8
            new DarkPatternData(
                "Progress Slowdown",
                "Increase the amount of time required to progress through the game.",

                -3,
                -1,
                 0,
                -1,
                 5,
                -35
            ),

            // DAY 9
            new DarkPatternData(
                "Pay to Skip",
                "Allow players to pay to bypass the slower progression and continue immediately.",

                -3,
                -1,
                -1,
                -2,
                 7,
                -40
            ),

            // DAY 10
            new DarkPatternData(
                "Gacha System",
                "Introduce a premium randomised reward system featuring rare characters and items.",

                -4,
                -2,
                -1,
                 3,
                 9,
                -50
            )
        };    
    }

    public void StudioFounded()
    {
        Debug.Log("Studio Founded");
        currentDemandData = null;
        HasAnsweredCurrentDemand = true;
        lastDecisionText = "";
        SendIntroEmail();
    }
    
    public void StartPublisherDay(int day)
    {
        HasAnsweredCurrentDemand = false;
        lastDecisionText = "";

        int demandIndex = day - 2;

        if (demandIndex < 0 ||
            demandIndex >= demands.Length)
        {
            currentDemandData = null;
            HasAnsweredCurrentDemand = true;

            //Debug.Log("No publisher demand for Day " + day);

            return;
        }

        currentDemandData = demands[demandIndex];

        Debug.Log(
            "Publisher demand created: " +
            currentDemandData.name);

        SendPublisherEmail(day);
    }

    private void SendPublisherEmail(int day)
    {
        if (currentDemandData == null)
            return;

        string emailId = "publisher_day_" + day;

        string subject = GetPublisherEmailSubject(day);

        EmailData email =
            new EmailData(
                emailId,
                "AAA Publishing",
                subject,
                GetPublisherEmailBody(day),
                true,
                currentDemandData
            );

        EmailManager.Instance.AddEmail(email);
    }
    
    //changed the email subjects to better reflect the demands and dark patterns
    private string GetPublisherEmailSubject(int day)
    {
        switch (day)
        {
            case 2:
                return "Revenue Strategy";

            case 3:
                return "Additional Monetisation";

            case 4:
                return "Store Changes";

            case 5:
                return "Engagement Strategy";

            case 6:
                return "Player Retention";

            case 7:
                return "Limited-Time Content";

            case 8:
                return "Progression Changes";

            case 9:
                return "Monetisation Update";

            case 10:
                return "FINAL DIRECTIVE";

            default:
                return "Publisher Message";
        }
    }

    public void AcceptDemand()
    {
        if (HasAnsweredCurrentDemand)
            return;

        if (currentDemandData == null)
        {
            Debug.LogWarning(
                "Cannot accept demand: there is no active publisher demand.");

            return;
        }

        ApplyDemand();

        HasAnsweredCurrentDemand = true;

        StudioManager.Instance.publisherSatisfaction =
            Mathf.Clamp(
                StudioManager.Instance.publisherSatisfaction + 10,
                0,
                100);

        lastDecisionText =
            "Accepted: " +
            currentDemandData.name;

        StudioManager.Instance.NotifyStudioDataChanged();

        Debug.Log(
            lastDecisionText +
            " | Publisher Trust: " +
            StudioManager.Instance.publisherSatisfaction);
    }


    public void RejectDemand()
    {
        if (HasAnsweredCurrentDemand)
            return;

        if (currentDemandData == null)
        {
            Debug.LogWarning(
                "Cannot reject demand: there is no active publisher demand.");

            return;
        }

        HasAnsweredCurrentDemand = true;

        StudioManager.Instance.publisherSatisfaction =
            Mathf.Clamp(
                StudioManager.Instance.publisherSatisfaction - 30,
                0,
                100);

        lastDecisionText =
            "Rejected: " +
            currentDemandData.name;

        StudioManager.Instance.NotifyStudioDataChanged();

        Debug.Log(
            lastDecisionText +
            " | Publisher Trust: " +
            StudioManager.Instance.publisherSatisfaction);

        CheckPublisherFailure();
    }


    public void RejectDemandFromTimeout()
    {
        if (HasAnsweredCurrentDemand)
            return;

        if (currentDemandData == null)
            return;

        int day = DayManager.Instance.CurrentDay;

        HasAnsweredCurrentDemand = true;

        StudioManager.Instance.publisherSatisfaction =
            Mathf.Clamp(
                StudioManager.Instance.publisherSatisfaction - 35,
                0,
                100);

        lastDecisionText =
            "Ignored: " +
            currentDemandData.name;

        // Update the matching email.
        if (EmailManager.Instance != null)
        {
            EmailData email =
                EmailManager.Instance.GetEmail(
                    "publisher_day_" + day);

            if (email != null)
            {
                email.hasBeenAnswered = true;
                email.wasAccepted = false;
            }
        }

        StudioManager.Instance.NotifyStudioDataChanged();

        Debug.Log(
            lastDecisionText +
            " | Publisher Trust: " +
            StudioManager.Instance.publisherSatisfaction);

        CheckPublisherFailure();
    }


    private void CheckPublisherFailure()
    {
        if (StudioManager.Instance.publisherSatisfaction <= 0)
        {
            Debug.Log(
                "GAME OVER - Publisher has pulled funding.");

            // Fire a event.
        }
    }


    private void ApplyDemand()
    {
        if (StudioManager.Instance == null)
            return;

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


        // Prevent stats from dropping below zero.

        project.stats.gameplay =
            Mathf.Max(
                0,
                project.stats.gameplay);

        project.stats.story =
            Mathf.Max(
                0,
                project.stats.story);

        project.stats.style =
            Mathf.Max(
                0,
                project.stats.style);

        project.stats.audience =
            Mathf.Max(
                0,
                project.stats.audience);

        project.stats.profit =
            Mathf.Max(
                0,
                project.stats.profit);

        StudioManager.Instance.ethics =
            Mathf.Clamp(
                StudioManager.Instance.ethics,
                0,
                100);

        StudioManager.Instance.NotifyStudioDataChanged();
    }

    private void OnDayEnded(int day)
    {
        // If at day end player did not answer publisher request, automatically reject it.
        if (day > 1 && HasAnsweredCurrentDemand)
        {
            RejectDemandFromTimeout();
        }
    }
    
    // added email demands to seem like something in an actual email form a publisher
    private string GetPublisherEmailBody(int day)
    {
        switch (day)
        {
            case 2:
                return
                    "Hi,\n\n" +
                    "We've reviewed the current revenue projections and we'd like to make a small adjustment.\n\n" +
                    "Please add advertisements to the game. This should give us an additional source of revenue without requiring major changes to the project.\n\n" +
                    "Let us know if you'll move forward with this.\n\n" +
                    "- Publishing";
            case 3:
                return
                    "Hi,\n\n" +
                    "The advertising strategy is a start, but we'd like to explore additional revenue opportunities.\n\n" +
                    "Please introduce optional microtransactions. Players could purchase cosmetic items or additional content directly.\n\n" +
                    "We believe this will significantly improve the project's commercial potential.\n\n" +
                    "- Publishing";
            case 4:
                return
                    "Team,\n\n" +
                    "We'd like purchases to operate through a premium in-game currency rather than direct prices.\n\n" +
                    "Players will purchase currency bundles first and then use that currency in the store.\n\n" +
                    "This gives us more flexibility with pricing and promotions going forward.\n\n" +
                    "- Publishing";
            case 5:
                return
                    "Team,\n\n" +
                    "We're looking for stronger engagement with the store.\n\n" +
                    "Please introduce purchasable reward boxes with random contents and varying item rarity.\n\n" +
                    "Rare rewards should give players a reason to make repeat purchases.\n\n" +
                    "- Publishing";
            case 6:
                return
                    "Team,\n\n" +
                    "Our retention numbers need improvement.\n\n" +
                    "Introduce daily login rewards, with increasingly valuable rewards for consecutive days.\n\n" +
                    "Players who break their streak should have to begin again.\n\n" +
                    "This should encourage more consistent engagement.\n\n" +
                    "- Publishing";
            case 7:
                return
                    "Team,\n\n" +
                    "We need to increase how frequently players return to the game.\n\n" +
                    "Create limited-time events with exclusive rewards that disappear when the event ends.\n\n" +
                    "Players should understand that missing the event means missing the rewards.\n\n" +
                    "- Publishing";
            case 8:
                return
                    "Team,\n\n" +
                    "Current progression is allowing players to move through the game too quickly.\n\n" +
                    "Increase progression requirements and extend the time needed to unlock later content.\n\n" +
                    "Slower progression should improve long-term engagement and create additional monetisation opportunities.\n\n" +
                    "- Publishing";
            case 9:
                return
                    "Team,\n\n" +
                    "We've reviewed the new progression model.\n\n" +
                    "Players who don't want to wait should be given the option to pay to skip progression requirements.\n\n" +
                    "Make sure this option is clearly available whenever progress slows down.\n\n" +
                    "- Publishing";
            case 10:
                return
                    "Final directive.\n\n" +
                    "We want the monetisation systems consolidated into a premium gacha system.\n\n" +
                    "Players should spend premium currency for random chances at rare characters and items. Limited availability should encourage repeat purchases.\n\n" +
                    "This is an important part of our commercial strategy. We expect this feature to be implemented.\n\n" +
                    "- Publishing";
            default:
                return "";
        }
    }
    
    private void SendIntroEmail() //first email the player sees
    {
        if (EmailManager.Instance == null)
        {
            Debug.LogError("PublisherManager could not find EmailManager.Instance.");
            return;
        }

        EmailData email = new EmailData(
            "publisher_intro",
            "AAA Publishing",
            "Welcome to AAA Publishing",
            "Hi,\n\n" +
            "Welcome aboard.\n\n" +
            "We're excited to be working with your studio on its first project. " +
            "We'll be providing the funding and support needed to get the game to market.\n\n" +
            "From time to time, we'll send you requests regarding the direction of the project. " +
            "These decisions can affect the game's performance and our working relationship, " +
            "so make sure to keep an eye on your inbox.\n\n" +
            "We're looking forward to seeing what you create.\n\n" +
            "- AAA Publishing"
        );

        EmailManager.Instance.AddEmail(email);
    }
}