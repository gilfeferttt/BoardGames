using System;
using UnityEngine;

[Serializable]
public class RFPower
{
    public enum RFPowerValue
    {
        Power18 = 1,
        Power18_2 = 2,
        Power23 = 3,
        Power23_2 = 4,
        Power33 = 5,
        Power38 = 6,
        Power43 = 7,
        Power48 = 8 
    }
    [SerializeField] public int power;

    public RFPower()
    {
        
    }
}
