using System;
using UnityEngine;

[Serializable]
public class RFIDConf
{
    [SerializeField] public bool sendAllTagsEveryRound;
    [SerializeField] public bool tagDetectOnce;
    [SerializeField] public bool turnOffAntennaAfterDetection;
    [SerializeField] public bool sendTagEveryDetection;
    [SerializeField] public bool sendTagUndetected;
    [SerializeField] public string antennatoturnon;

    public RFIDConf()
    {
        sendAllTagsEveryRound = false;
        tagDetectOnce = false;
        turnOffAntennaAfterDetection = false;
        sendTagEveryDetection = false;
        sendTagUndetected = false;
        antennatoturnon = "";
    }
}
