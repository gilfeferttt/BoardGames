using System;
using UnityEngine;

[Serializable]
public class RFIDDetectionMode
{
    [SerializeField] public bool sendTagsStatusEveryEndOfRound;
    [SerializeField] public bool sendTagsStatusEveryEndOfRoundWhenChanged;
    [SerializeField] public bool turnOffAntennaAfterDetection;
    
    public RFIDDetectionMode()
    {
        sendTagsStatusEveryEndOfRound = false;
        sendTagsStatusEveryEndOfRoundWhenChanged = false;
        turnOffAntennaAfterDetection = false;
    }
}
