using System;
public class Player
{
    public int life { get; set; }
    public int score { get; set; }

    public Player()
    {
        life = 3;
        score = 0;
    }
    public Player(int life)
    {
        this.life = life;
        score = 0;
    }
    public bool isLife()
    {
        return (life > 0) ? true : false;
    }
}
