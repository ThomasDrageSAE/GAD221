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
    public GameStats stats;


    // Results
    public int reviewScore;
    public int sales;
    public int moneyEarned;

    public GameProject(string gameName, Genre genre, Theme theme, Engine engine, Mechanic mechanic, Platform platform)
    {
        this.gameName = gameName;
        this.genre = genre;
        this.theme = theme;
        this.engine = engine;
        this.mechanic = mechanic;
        this.platform = platform;
        
        stats = new GameStats();
        
        reviewScore = 0;
        sales = 0;
        moneyEarned = 0;
    }
    
    public void ResetStats()
    {
        stats = new GameStats();

        reviewScore = 0;
        sales = 0;
        moneyEarned = 0;
    }
}