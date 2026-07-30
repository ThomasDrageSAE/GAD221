using System;

[Serializable]
public class GameProject
{
    public string gameName = "Untitled Game";


    // Choices
    public Genre genre;
    public Theme theme;
    public Engine engine;
    public Mechanic mechanic;
    public Platform platform;


    // Calculated stats
    public GameStats stats = new GameStats();


    // Results
    public int reviewScore;
    public int sales;
    public int moneyEarned;



    public void ResetStats()
    {
        stats = new GameStats();

        reviewScore = 0;
        sales = 0;
        moneyEarned = 0;
    }
}