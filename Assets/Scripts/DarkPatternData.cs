using System;

[Serializable]
public class DarkPatternData
{
    public string name;
    public string description;

    public int audienceModifier;
    public int profitModifier;
    public int ethicsModifier;


    public DarkPatternData(
        string name,
        string description,
        int audience,
        int profit,
        int ethics)
    {
        this.name = name;
        this.description = description;

        audienceModifier = audience;
        profitModifier = profit;
        ethicsModifier = ethics;
    }
}