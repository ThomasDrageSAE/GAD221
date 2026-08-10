using System;

[Serializable]
public class DarkPattern
{
    public string patternName;

    public int audienceModifier;
    public int profitModifier;
    public int ethicsModifier;

    public DarkPattern(string name, int audience, int profit, int ethics)
    {
        patternName = name;

        audienceModifier = audience;
        profitModifier = profit;
        ethicsModifier = ethics;
    }
}