using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersEngine : IEnumerable<Player>
{
    public List<Player> players { get; set; }
    private int currentIndex = -1;
    
    public PlayersEngine()
    {
        players = new List<Player>();
    }
    public IEnumerator<Player> GetEnumerator()
    {
        foreach (Player player in players)
        {
            yield return player;
        }
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public Player? GetNextPlayer()
    {
        if (currentIndex >= players.Count - 1)
            return null;

        currentIndex++;
        Player player = players[currentIndex];
        player.playerIndex = currentIndex;
        if (currentIndex >= players.Count - 1)
            player.lastPlayer = true;
        else
            player.lastPlayer = false;

        return player;
    }
    public List<Player> GetWinnerPlayers()
    {
        Debug.Log("Enter GetWinnerPlayers()");
        try
        {
            List<Player> sortedPlayers = new List<Player>(players);
            sortedPlayers.Sort((x, y) => y.timeLeftToPlay - x.timeLeftToPlay);
            Debug.Log("timeLeftToPlay for 0-" + sortedPlayers[0].timeLeftToPlay);
            Debug.Log("timeLeftToPlay for 1-" + sortedPlayers[1].timeLeftToPlay);
            List <Player> bonusPlayers = new List<Player>();
            int timelefttoplay = -1;
            foreach(Player p in sortedPlayers)
            {
                if(timelefttoplay == -1)
                {
                    timelefttoplay = p.timeLeftToPlay;
                    bonusPlayers.Add(p);
                } else if(timelefttoplay == p.timeLeftToPlay)
                {
                    bonusPlayers.Add(p);
                }
                else
                {
                    break;
                }
            }
            return bonusPlayers;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
            return null;
        }
        finally
        {
            Debug.Log("Exit GetWinnerPlayers()");
        }
    }
    public void Reset()
    {
        currentIndex = -1;
    }
    public void addNewPlayer(string name)
    {
        players.Add(new Player(name));
    }
}
