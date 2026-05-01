using System;
public class GameRound
{
    public string Name { get; set; }
    public int NumberOfTroopers { get; set; }
    public int Points { get; set; }
    public int Bonuspoints { get; set; }
    public int RoundNumber { get; set; }

    public GameRound(int roundNumber, string name, int numberoftroopers, int points, int bonuspoints)
    {
        Name = name;
        NumberOfTroopers = numberoftroopers;
        Points = points;
        Bonuspoints = bonuspoints;
        RoundNumber = roundNumber;
    }
}
