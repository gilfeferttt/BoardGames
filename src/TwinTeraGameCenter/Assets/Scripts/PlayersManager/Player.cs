using System;
public class Player
{
    public string Name { get; set; }
    public int playerIndex { get; set; }
    public int timeLeftToPlay { get; set; }
    public int points { get; set; }
    public bool lastPlayer { get; set; }

    public Player(string name)
    {
        Name = name;
        playerIndex = -1;
        timeLeftToPlay = -1;
        points = 0;
    }
}
