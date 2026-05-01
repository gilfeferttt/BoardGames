using System;
public class FullBaordDetectedTags
{
    public DetectedStatus detectedStatus { get; set; }
    public int numberOfTagsDetected { get; set; }
    public int numberOfTagsSetOnBoard { get; set; }

    public enum DetectedStatus
    {
        failToDetectTags = -1,
        allTagsSetDetectedCorrectly = 1,
        allTagsSetNotDetectedCorrectly = 2,
        notAllTagsSetOnBoard = 3
    }
    public FullBaordDetectedTags(DetectedStatus detectedStatus, int numberOfTagsSetOnBoard, int numberOfTagsDetected)
    {
        this.detectedStatus = detectedStatus;
        this.numberOfTagsDetected = numberOfTagsDetected;
        this.numberOfTagsSetOnBoard = numberOfTagsSetOnBoard;
    }
}
