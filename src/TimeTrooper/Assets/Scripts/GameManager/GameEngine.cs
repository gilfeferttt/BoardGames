using System;
using System.Collections;
using System.Collections.Generic;
using static Board;

public class GameEngine : IEnumerable<GameRound>
{
    List<GameRound> gameRounds;
    public int timeToPlay { get; set; }
    private int currentIndex = -1;
    private double bonusMultiplier;
    public static int basicWaitTime = 12;
    public static int advancedWaitTime = 13;
    public static int expertWaitTime = 14;
    public static int masterWaitTime = 15;
    public static int eliteWaitTime = 16;
    public static int legendaryWaitTime = 17;

    public static int basicNumOfRounds = 9;
    public static int advancedNumOfRounds = 9;
    public static int expertNumOfRounds = 9;
    public static int masterNumOfRounds = 9;
    public static int eliteNumOfRounds = 9;
    public static int legendaryNumOfRounds = 9;
    public static int basicNumOfRoundsMin = 1;
    public static int advancedNumOfRoundsMin = 1;
    public static int expertNumOfRoundsMin = 1;
    public static int masterNumOfRoundsMin = 1;
    public static int eliteNumOfRoundsMin = 1;
    public static int legendaryNumOfRoundsMin = 1;

    public static int numberOfPlayers = 2;

    public static int eliteTimeToDisappear = 3;
    public static int legendaryTimeToDisappear = 3;

    public int currentTimeToDisappear;
    public GameDificulty currentGameDificulty, nextGameDificulty;
    public string currentGameDificultyName, nextGameDificultyName;

    public static int minNumberOfPlayers = 2;
    public static int maxNumberOfPlayers = 4;

    public GameEngine(GameDificulty gameDificulty, GameData gamedata)
    {
        currentGameDificulty = gameDificulty;
        currentTimeToDisappear = -1;

        gameRounds = new List<GameRound>();
        List<GameRound> tempGameRounds = new List<GameRound>();
        int numberOfRoundConfigured = 0;
        int numberOfRoundConfiguredMin = 0;
        bonusMultiplier = 1;
        if (gameDificulty == GameDificulty.Basic || gameDificulty == GameDificulty.Expert)
        {
            nextGameDificulty = GameDificulty.Advanced;
            currentGameDificultyName = "Basic";
            nextGameDificultyName = "Advanced";
            bonusMultiplier = 1;
            numberOfRoundConfigured = Convert.ToInt32(gamedata.basicNumOfRounds);
            numberOfRoundConfiguredMin = Convert.ToInt32(gamedata.basicNumOfRoundsMin);
            timeToPlay = Convert.ToInt32(gamedata.basicWaitTime);
            if (gameDificulty == GameDificulty.Expert)
            {
                timeToPlay = Convert.ToInt32(gamedata.expertWaitTime);
                bonusMultiplier = 2;
                numberOfRoundConfigured = Convert.ToInt32(gamedata.expertNumOfRounds);
                numberOfRoundConfiguredMin = Convert.ToInt32(gamedata.expertNumOfRoundsMin);

                nextGameDificulty = GameDificulty.Master;
                currentGameDificultyName = "expert";
                nextGameDificultyName = "Master";
            }
            tempGameRounds.Add(new GameRound(1, "Round 1", 1, 10, 5));
            tempGameRounds.Add(new GameRound(2, "Round 2", 2, 10, 10));
            tempGameRounds.Add(new GameRound(3, "Round 3", 3, 10, 15));
            tempGameRounds.Add(new GameRound(4, "Round 4", 4, 10, 20));
            tempGameRounds.Add(new GameRound(5, "Round 5", 5, 10, 25));
            tempGameRounds.Add(new GameRound(6, "Round 6", 6, 10, 30));
            tempGameRounds.Add(new GameRound(7, "Round 7", 7, 10, 35));
            tempGameRounds.Add(new GameRound(8, "Round 8", 8, 10, 40));
            tempGameRounds.Add(new GameRound(9, "Round 9", 9, 10, 45));
        }
        else if (gameDificulty == GameDificulty.Advanced || gameDificulty == GameDificulty.Master)
        {
            nextGameDificulty = GameDificulty.Expert;
            currentGameDificultyName = "Advanced";
            nextGameDificultyName = "Expert";
            numberOfRoundConfigured = Convert.ToInt32(gamedata.advancedNumOfRounds);
            numberOfRoundConfiguredMin = Convert.ToInt32(gamedata.advancedNumOfRoundsMin);
            bonusMultiplier = 1.5;
            timeToPlay = Convert.ToInt32(gamedata.advancedWaitTime);
            if (gameDificulty == GameDificulty.Master)
            {
                nextGameDificulty = GameDificulty.Elite;
                currentGameDificultyName = "Master";
                nextGameDificultyName = "Elite";
                timeToPlay = Convert.ToInt32(gamedata.masterWaitTime);
                bonusMultiplier = 2.5;
                numberOfRoundConfigured = Convert.ToInt32(gamedata.masterNumOfRounds);
                numberOfRoundConfiguredMin = Convert.ToInt32(gamedata.masterNumOfRoundsMin);
            }
            tempGameRounds.Add(new GameRound(1, "Round 1", 1, 10, 5));
            tempGameRounds.Add(new GameRound(2, "Round 2", 2, 20, 10));
            tempGameRounds.Add(new GameRound(3, "Round 3", 3, 30, 15));
            tempGameRounds.Add(new GameRound(4, "Round 4", 4, 40, 20));
            tempGameRounds.Add(new GameRound(5, "Round 5", 5, 50, 25));
            tempGameRounds.Add(new GameRound(6, "Round 6", 6, 60, 30));
            tempGameRounds.Add(new GameRound(7, "Round 7", 7, 70, 35));
            tempGameRounds.Add(new GameRound(8, "Round 8", 8, 80, 40));
            tempGameRounds.Add(new GameRound(9, "Round 9", 9, 90, 45));
        }
        else if (gameDificulty == GameDificulty.Elite)
        {
            currentTimeToDisappear = Convert.ToInt32(gamedata.eliteTimeToDisappear);
            nextGameDificulty = GameDificulty.Legendary;
            currentGameDificultyName = "Elite";
            nextGameDificultyName = "Legendary";
            numberOfRoundConfigured = Convert.ToInt32(gamedata.eliteNumOfRounds);
            numberOfRoundConfiguredMin = Convert.ToInt32(gamedata.eliteNumOfRoundsMin);
            bonusMultiplier = 3;
            timeToPlay = Convert.ToInt32(gamedata.eliteWaitTime);
            tempGameRounds.Add(new GameRound(1, "Round 1", 1, 10, 5));
            tempGameRounds.Add(new GameRound(2, "Round 2", 2, 10, 10));
            tempGameRounds.Add(new GameRound(3, "Round 3", 3, 10, 15));
            tempGameRounds.Add(new GameRound(4, "Round 4", 4, 10, 20));
            tempGameRounds.Add(new GameRound(5, "Round 5", 5, 10, 25));
            tempGameRounds.Add(new GameRound(6, "Round 6", 6, 10, 30));
            tempGameRounds.Add(new GameRound(7, "Round 7", 7, 10, 35));
            tempGameRounds.Add(new GameRound(8, "Round 8", 8, 10, 40));
            tempGameRounds.Add(new GameRound(9, "Round 9", 9, 10, 45));
        }
        else if (gameDificulty == GameDificulty.Legendary)
        {
            currentTimeToDisappear = Convert.ToInt32(gamedata.legendaryTimeToDisappear);
            nextGameDificulty = GameDificulty.None;
            currentGameDificultyName = "Legendary";
            nextGameDificultyName = "";
            numberOfRoundConfigured = Convert.ToInt32(gamedata.legendaryNumOfRounds);
            numberOfRoundConfiguredMin = Convert.ToInt32(gamedata.legendaryNumOfRoundsMin);
            bonusMultiplier = 4;
            timeToPlay = Convert.ToInt32(gamedata.legendaryWaitTime);
            tempGameRounds.Add(new GameRound(1, "Round 1", 1, 10, 5));
            tempGameRounds.Add(new GameRound(2, "Round 2", 2, 10, 10));
            tempGameRounds.Add(new GameRound(3, "Round 3", 3, 10, 15));
            tempGameRounds.Add(new GameRound(4, "Round 4", 4, 10, 20));
            tempGameRounds.Add(new GameRound(5, "Round 5", 5, 10, 25));
            tempGameRounds.Add(new GameRound(6, "Round 6", 6, 10, 30));
            tempGameRounds.Add(new GameRound(7, "Round 7", 7, 10, 35));
            tempGameRounds.Add(new GameRound(8, "Round 8", 8, 10, 40));
            tempGameRounds.Add(new GameRound(9, "Round 9", 9, 10, 45));
        }
        if(numberOfRoundConfigured > tempGameRounds.Count)
        {
            numberOfRoundConfigured = tempGameRounds.Count;
        }
        for(int x = numberOfRoundConfiguredMin - 1; x< numberOfRoundConfigured; x++)
        {
            gameRounds.Add(tempGameRounds[x]);
        }
    }

    public IEnumerator<GameRound> GetEnumerator()
    {
        foreach (GameRound round in gameRounds)
        {
            yield return round;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public GameRound? GetNextRound()
    {
        if (currentIndex >= gameRounds.Count - 1)
            return null;

        currentIndex++;
        return gameRounds[currentIndex];
    }
    public void Reset()
    {
        currentIndex = -1;
    }
}


