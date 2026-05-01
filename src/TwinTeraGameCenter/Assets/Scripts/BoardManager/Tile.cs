using System;
using UnityEngine;
public class Tile
{
    public Vector3 location;
    public int antenna;

    public Tile(Vector2 location, int antenna)
    {
        this.location = location;
        this.antenna = antenna;
    }
}