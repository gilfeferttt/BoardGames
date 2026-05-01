using System;
using UnityEngine;

public class HourGlasTag
{
    public string tag;
    public string name;
    public int timeoutselected;
    public HourGlasTag(string tag, string name, int timeoutselected)
    {
        this.tag = tag;
        this.name = name;
        this.timeoutselected = timeoutselected;
    }
}
