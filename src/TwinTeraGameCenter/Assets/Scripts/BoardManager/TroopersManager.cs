using System;
using System.Collections.Generic;
using UnityEngine;

public class TrooperSetup
{
    public Trooper trooper { get; set; }
    public Tile tile { get; set; }
    public int side { get; set; }

    public TrooperSetup(Trooper trooper, Tile tile, int side)
    {
        this.trooper = trooper;
        this.tile = tile;
        this.side = side;
    }
}
public class TroopersManager
{
    public Dictionary<int, TrooperSetup> tropperssetup { get; set; }
    public TroopersManager()
    {
        tropperssetup = new Dictionary<int, TrooperSetup>();

    }
    public void clearTrooperSetup()
    {
        tropperssetup.Clear();
    }
    public void addTrooperSetup(Trooper trooper, Tile tile, int side)
    {
        tropperssetup.Add(tile.antenna, new TrooperSetup(trooper, tile, side));
    }
    public void removeTrooperSetup(int anenna)
    {
        tropperssetup.Remove(anenna);
    }
    public Trooper getTrooperOnAntenna(PPUTag pputag)
    {
        Debug.Log("Enter getTrooperOnAntenna()");
        try
        {
            TrooperSetup trooperSetup;
            if (tropperssetup.TryGetValue(pputag.antenna, out trooperSetup) == false)
            {
                Debug.Log("Trooper not on correct antenna");
                return null;
            }
            else
            {
                if (trooperSetup.trooper.GameObjects[trooperSetup.side].Tag.SerialNumber.CompareTo(pputag.id) == 0)
                {
                    Debug.Log("Trooper on correct antenna");
                    return trooperSetup.trooper;
                }
                else
                {
                    Debug.Log("Trooper on correct antenna BUT on a different side");
                    return null;
                }
            }
        }
        finally
        {
            Debug.Log("Exit getTrooperOnAntenna()");
        }
    }
    /*ublic int getRealAntennaTrooperIsOn(int antenna, string sn)
    {
        int realAntennalTrooperInOn = 0;
        foreach(int ant in tropperssetup.Keys)
        {
            if (ant != antenna)
            {
                TrooperSetup troop = tropperssetup[ant];
                if(sn.CompareTo(troop.trooper.GameObjects[0].Tag) == 0 || sn.CompareTo(troop.trooper.GameObjects[1].Tag) == 0)
                {
                    realAntennalTrooperInOn = ant;
                    break;
                }
            }
        }

        return realAntennalTrooperInOn;
    }*/
    public bool checkSameLayout(TroopersManager troppermanagerDest)
    {
        Debug.Log("Enter checkSameLayout()");
        try
        {
            int numberofmatchsfound = 0;
            foreach (int antennaDest in troppermanagerDest.tropperssetup.Keys)
            {
                TrooperSetup troopersetupDest = troppermanagerDest.tropperssetup[antennaDest];
                Trooper troopertoplayDest = troopersetupDest.trooper;
                string tagDest = troopertoplayDest.GameObjects[troopersetupDest.side].Tag.SerialNumber;
                Debug.Log("On antennaDest " + antennaDest + " place tagDest " + tagDest + " side " + troopersetupDest.side);

                foreach (int antennaSource in this.tropperssetup.Keys)
                {
                    TrooperSetup troopersetupSource = this.tropperssetup[antennaSource];
                    Trooper troopertoplaySource = troopersetupSource.trooper;
                    string tagSource = troopertoplaySource.GameObjects[troopersetupSource.side].Tag.SerialNumber;
                    Debug.Log("On antennaSource " + antennaSource + " place tagDest " + tagSource + " side " + troopersetupSource.side);
                    if (antennaSource == antennaDest && tagSource.CompareTo(tagDest) == 0)
                    {
                        numberofmatchsfound++;
                        break;
                    }
                }
            }
            
            if (numberofmatchsfound == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        } finally
        {
            Debug.Log("Exit checkSameLayout()");
        }
    }
    public void copyFrom(TroopersManager troppermanagerDest)
    {
        this.clearTrooperSetup();
        foreach (int antennaDest in troppermanagerDest.tropperssetup.Keys)
        {
            TrooperSetup troopersetupDest = troppermanagerDest.tropperssetup[antennaDest];
            Trooper troopertoplayDest = troopersetupDest.trooper;
            string tagDest = troopertoplayDest.GameObjects[troopersetupDest.side].GameImage.gameObject.tag;
            Debug.Log("On antennaDest" + antennaDest + " place tagDest " + tagDest);

            this.addTrooperSetup(troopertoplayDest, troopersetupDest.tile, troopersetupDest.side);
        }
    }
}

