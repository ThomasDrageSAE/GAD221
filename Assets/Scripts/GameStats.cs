using System;

[Serializable]
public class GameStats
{
    public int gameplay;
    public int story;
    public int style;
    public int audience;
    public int profit;


    public void Add(GameStats stats)
    {
        gameplay += stats.gameplay;
        story += stats.story;
        style += stats.style;
        audience += stats.audience;
        profit += stats.profit;
    }
}