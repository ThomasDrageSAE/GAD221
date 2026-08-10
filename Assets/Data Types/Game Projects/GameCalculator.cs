using UnityEngine;


public static class GameCalculator
{

    public static void Calculate(GameProject project)
    {

        project.ResetStats();


        GameStats finalStats = new GameStats();


        // Add each choice
        finalStats.Add(GetGenre(project.genre));

        finalStats.Add(GetTheme(project.theme));

        finalStats.Add(GetEngine(project.engine));

        finalStats.Add(GetMechanic(project.mechanic));

        finalStats.Add(GetPlatform(project.platform));


        // Combo bonuses
        ApplyCombos(project, finalStats);



        project.stats = finalStats;



        // Review score
        project.reviewScore =
            Mathf.Clamp(
            (finalStats.gameplay * 2 +
             finalStats.story +
             finalStats.style)
             * 5,
             0,
             100);



        // Sales
        project.sales =
            Mathf.Max(0,
            finalStats.audience *
            project.reviewScore *
            100);



        // Money
        project.moneyEarned =
            project.sales *
            Mathf.Max(1, finalStats.profit);

    }



    static GameStats GetGenre(Genre genre)
    {
        switch (genre)
        {

            case Genre.RPG:
                return new GameStats
                {
                    gameplay = 2,
                    story = 3,
                    style = 2,
                    audience = 2
                };


            case Genre.FPS:
                return new GameStats
                {
                    gameplay = 3,
                    style = 1,
                    audience = 3
                };


            case Genre.Platformer:
                return new GameStats
                {
                    gameplay = 2,
                    style = 2,
                    audience = 2
                };


            case Genre.Strategy:
                return new GameStats
                {
                    gameplay = 2,
                    story = 2,
                    audience = 1
                };


            case Genre.Horror:
                return new GameStats
                {
                    gameplay = 2,
                    story = 2,
                    style = 3,
                    audience = 2
                };


            case Genre.Puzzle:
                return new GameStats
                {
                    gameplay = 2,
                    story = 1,
                    audience = 1
                };


            case Genre.Roguelike:
                return new GameStats
                {
                    gameplay = 3,
                    audience = 1
                };


            case Genre.CardGame:
                return new GameStats
                {
                    gameplay = 2,
                    audience = 1
                };


            case Genre.Racing:
                return new GameStats
                {
                    gameplay = 3,
                    style = 2,
                    audience = 2
                };


            case Genre.Survival:
                return new GameStats
                {
                    gameplay = 3,
                    story = 1,
                    style = 2,
                    audience = 2
                };

        }


        return new GameStats();
    }



    static GameStats GetTheme(Theme theme)
    {
        switch(theme)
        {

            case Theme.Fantasy:
                return new GameStats
                {
                    story = 2,
                    style = 3,
                    audience = 2
                };


            case Theme.SciFi:
                return new GameStats
                {
                    style = 3,
                    audience = 2
                };


            case Theme.Medieval:
                return new GameStats
                {
                    story = 2,
                    style = 2
                };


            case Theme.Cyberpunk:
                return new GameStats
                {
                    style = 3,
                    audience = 2
                };


            case Theme.Zombies:
                return new GameStats
                {
                    style = 2,
                    audience = 2
                };


            case Theme.Pirates:
                return new GameStats
                {
                    style = 2
                };


            case Theme.Space:
                return new GameStats
                {
                    style = 3,
                    audience = 2
                };


            case Theme.Detective:
                return new GameStats
                {
                    story = 3
                };


            case Theme.PostApocalyptic:
                return new GameStats
                {
                    style = 2,
                    audience = 2
                };

        }


        return new GameStats();
    }



    static GameStats GetEngine(Engine engine)
    {
        switch(engine)
        {

            case Engine.Twonity:
                return new GameStats
                {
                    style = 2,
                    gameplay = 2
                };


            case Engine.RealEngine:
                return new GameStats
                {
                    style = 3,
                    gameplay = 2
                };


            case Engine.Bodot:
                return new GameStats
                {
                    gameplay = 1,
                    style = 1
                };


            case Engine.CustomEngine:
                return new GameStats
                {
                    gameplay = 3,
                    style = 3
                };

        }


        return new GameStats();
    }



    static GameStats GetMechanic(Mechanic mechanic)
    {

        switch(mechanic)
        {

            case Mechanic.OpenWorld:
                return new GameStats
                {
                    gameplay = 3
                };


            case Mechanic.Crafting:
                return new GameStats
                {
                    gameplay = 2
                };


            case Mechanic.TurnBased:
                return new GameStats
                {
                    gameplay = 2
                };


            case Mechanic.DeckBuilding:
                return new GameStats
                {
                    gameplay = 2
                };


            case Mechanic.ProceduralGeneration:
                return new GameStats
                {
                    gameplay = 2,
                    profit = 1
                };


            case Mechanic.Gacha:
                return new GameStats
                {
                    profit = 3
                };


            case Mechanic.LootBoxes:
                return new GameStats
                {
                    profit = 2
                };


            case Mechanic.FOMOEvents:
                return new GameStats
                {
                    profit = 2
                };


            case Mechanic.Microtransactions:
                return new GameStats
                {
                    profit = 1
                };


            case Mechanic.Ads:
                return new GameStats
                {
                    profit = 1
                };

        }


        return new GameStats();
    }



    static GameStats GetPlatform(Platform platform)
    {

        switch(platform)
        {

            case Platform.PC:
                return new GameStats
                {
                    audience = 2,
                    profit = 2
                };


            case Platform.Mobile:
                return new GameStats
                {
                    audience = 3,
                    profit = 3
                };


            case Platform.Console:
                return new GameStats
                {
                    audience = 2,
                    profit = 2
                };

        }


        return new GameStats();
    }




    static void ApplyCombos(GameProject project, GameStats stats)
    {

        if(project.genre == Genre.RPG &&
           project.theme == Theme.Fantasy)
        {
            stats.audience += 3;
        }


        if(project.genre == Genre.Horror &&
           project.theme == Theme.Zombies)
        {
            stats.audience += 3;
        }


        if(project.genre == Genre.CardGame &&
           project.mechanic == Mechanic.DeckBuilding)
        {
            stats.gameplay += 4;
        }


        if(project.genre == Genre.Roguelike &&
           project.mechanic == Mechanic.ProceduralGeneration)
        {
            stats.gameplay += 3;
        }


        if(project.genre == Genre.Survival &&
           project.mechanic == Mechanic.Crafting)
        {
            stats.gameplay += 2;
        }

    }

}