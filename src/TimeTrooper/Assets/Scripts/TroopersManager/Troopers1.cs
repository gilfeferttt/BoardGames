using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Troopers1
{
    private Trooper[] troopers;

    public Troopers1(Trooper[] troopers)
    {
        this.troopers = troopers;
    }
    public Trooper[] getTroopersToPlay(int numberOfTroopers)
    {
        return troopers;
    }
}
