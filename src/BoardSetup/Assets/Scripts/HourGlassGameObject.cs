using System;
using UnityEngine;

public class HourGlassGameObject
{
    public GameObject hourglass;
    public GameObject hourglassInitiated;
    public Tile ontile;
    public HourGlasTag[] tags;
    public HourGlasTag selectedTag;
    public HourGlassGameObject(GameObject prefabesHourGlass)
    {
        this.hourglass = prefabesHourGlass;
        this.hourglassInitiated = null;
    }
}
