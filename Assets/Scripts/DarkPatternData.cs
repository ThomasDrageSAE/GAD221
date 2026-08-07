using System;

[Serializable]
public class DarkPatternData
{
    public string name;
    public string description;

    public int gameplayModifier;
    public int storyModifier;
    public int styleModifier;

    public int audienceModifier;
    public int profitModifier;
    public int ethicsModifier;

    public DarkPatternData(
        string name,
        string description,
        int gameplay,
        int story,
        int style,
        int audience,
        int profit,
        int ethics)
    {
        this.name = name;
        this.description = description;

        gameplayModifier = gameplay;
        storyModifier = story;
        styleModifier = style;

        audienceModifier = audience;
        profitModifier = profit;
        ethicsModifier = ethics;
    }
}