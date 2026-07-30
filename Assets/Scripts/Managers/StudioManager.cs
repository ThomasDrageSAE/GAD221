using UnityEngine;

public class StudioManager : MonoBehaviour
{
    public static StudioManager Instance;

    [Header("Studio")]
    public string studioName = "";
    public int money = 5000;
    public int ethics = 100;
    public int currentDay = 1;

    [Header("Current Game")]
    public GameProject currentProject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreateNewProject()
    {
        currentProject = new GameProject();
    }

    public void EndDay()
    {
        currentDay++;

        if (currentDay > 5)
        {
            ReleaseGame();
        }
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
}