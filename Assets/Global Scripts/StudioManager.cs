using UnityEngine;

public class StudioManager : Singleton<StudioManager>
{
    [Header("Game State")]
    public bool setupComplete = false;
    [Header("Studio")]
    public string studioName = "";
    public int money = 5000;
    public int ethics = 100;
    public int publisherSatisfaction = 100;
    public int currentDay = 1;
    [Header("Current Game")]
    public GameProject currentProject;
    
    [Header("Daily Finance")]
    [SerializeField] private int dailyOperatingCost = 2000;

    public int lastDailyIncome;
    public int lastDailyCosts;
    public int lastBudgetChange;

    protected override void Awake()
    {
        base.Awake();
        
        
    }
    
    public void CreateNewProject()
    {
        currentProject = new GameProject();
    }

    public void AdvanceDay()
    {
        currentDay++;

        Debug.Log("Advanced to Day " + currentDay);
    }

    public void ReleaseGame()
    {
        GameCalculator.Calculate(currentProject);

        money += currentProject.moneyEarned;

        Debug.Log("Game Released!");
        Debug.Log("Review: " + currentProject.reviewScore);
        Debug.Log("Sales: " + currentProject.sales);
        Debug.Log("Money Earned: £" + currentProject.moneyEarned);
    }
    
    public event System.Action StudioDataChanged;

    public void NotifyStudioDataChanged()
    {
        StudioDataChanged?.Invoke();
    }
    
    public void CalculateDailyBudget()
    {
        if (currentProject == null || currentProject.stats == null)
        {
            Debug.LogWarning("Cannot calculate budget: no current project.");
            return;
        }

        GameStats stats = currentProject.stats;

        int gameplayIncome =
            stats.gameplay * 100;

        int storyIncome =
            stats.story * 75;

        int styleIncome =
            stats.style * 75;

        int audienceIncome =
            stats.audience * 150;

        int profitIncome =
            stats.profit * 400;

        lastDailyIncome =
            gameplayIncome +
            storyIncome +
            styleIncome +
            audienceIncome +
            profitIncome;

        lastDailyCosts = dailyOperatingCost;

        lastBudgetChange =
            lastDailyIncome - lastDailyCosts;

        money += lastBudgetChange;

        money = Mathf.Max(0, money);

        Debug.Log(
            "DAILY FINANCES\n" +
            "Gameplay: $" + gameplayIncome + "\n" +
            "Story: $" + storyIncome + "\n" +
            "Style: $" + styleIncome + "\n" +
            "Audience: $" + audienceIncome + "\n" +
            "Profit: $" + profitIncome + "\n" +
            "Operating Costs: -$" + dailyOperatingCost + "\n" +
            "Budget Change: $" + lastBudgetChange + "\n" +
            "New Budget: $" + money
        );

        NotifyStudioDataChanged();
    }
    
}
