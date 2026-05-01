using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardSetup
{
    public enum tilesLocation
    {
        TileOne = 1,
        TileTwo = 2,
        TileThree = 3,
        TileFour = 4,
        TileFive = 5,
        TileSix = 6,
        TileSeven = 7,
        TileEight = 8,
        TileNine = 9, 
    }

    private Dictionary<tilesLocation, Tile> tilesDictionary;
    private List<Tile> tiles;
    private List<Tile> currenttiles;
    public BoardSetup()
    {
        tilesDictionary = new Dictionary<tilesLocation, Tile>();

        tilesDictionary.Add(tilesLocation.TileOne, new Tile(new Vector2(-650f, 460f), 7));
        tilesDictionary.Add(tilesLocation.TileTwo, new Tile(new Vector2(0f, 460f), 8));
        tilesDictionary.Add(tilesLocation.TileThree, new Tile(new Vector2(650f, 460f), 9));

        tilesDictionary.Add(tilesLocation.TileFour, new Tile(new Vector2(-650f, -200f), 12));
        tilesDictionary.Add(tilesLocation.TileFive, new Tile(new Vector2(0f, -200f), 13));
        tilesDictionary.Add(tilesLocation.TileSix, new Tile(new Vector2(650f, -200f), 14));

        tilesDictionary.Add(tilesLocation.TileSeven, new Tile(new Vector2(-650f, -860f), 17));
        tilesDictionary.Add(tilesLocation.TileEight, new Tile(new Vector2(0f, -860f), 18));
        tilesDictionary.Add(tilesLocation.TileNine, new Tile(new Vector2(650f, -860f),19));

        /*
        tilesDictionary.Add(tilesLocation.TileOne, new Tile(new Vector2(-650f, 460f), 11));
        tilesDictionary.Add(tilesLocation.TileTwo, new Tile(new Vector2(0f, 460f), 10));
        tilesDictionary.Add(tilesLocation.TileThree, new Tile(new Vector2(650f, 460f), 9));

        tilesDictionary.Add(tilesLocation.TileFour, new Tile(new Vector2(-650f, -200f), 8));
        tilesDictionary.Add(tilesLocation.TileFive, new Tile(new Vector2(0f, -200f), 7));
        tilesDictionary.Add(tilesLocation.TileSix, new Tile(new Vector2(650f, -200f), 6));

        tilesDictionary.Add(tilesLocation.TileSeven, new Tile(new Vector2(-650f, -860f), 5));
        tilesDictionary.Add(tilesLocation.TileEight, new Tile(new Vector2(0f, -860f), 4));
        tilesDictionary.Add(tilesLocation.TileNine, new Tile(new Vector2(650f, -860f),3));
         */

        tiles = new List<Tile>(tilesDictionary.Values);
        currenttiles = null;
    }
    public List<Tile> getCurrentTiles()
    {
        return currenttiles;
    }
    public List<Tile> getNextTiles(int numberOfTiles)
    {
        Debug.Log("Enter getNextTiles() numberOfTiles-" + numberOfTiles);
        try
        {
            List<Tile> nextTiles = new List<Tile>();

            ShuffleTiles();
            int x = numberOfTiles;
            foreach (Tile tile in tiles)
            {
                nextTiles.Add(tile);
                x--;
                if(x == 0)
                {
                    break;
                }
            }
            currenttiles = nextTiles;

            return nextTiles;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
            throw e;
        }
        finally
        {
            Debug.Log("Exit getNextTiles()");
        }
    }
    private void ShuffleTiles()
    {
        Debug.Log("Enter ShuffleTiles() tiles.Count-" + tiles.Count);
        try
        {
            for (int x = 0; x < 10; x++)
            {
                int randomFrom = UnityEngine.Random.Range(0,9);
                Debug.Log("randomFrom-" + randomFrom);
                Tile fromtile = tiles[randomFrom];
                int randomTo = UnityEngine.Random.Range(0, 9);
                Tile totile = tiles[randomTo];

                tiles[randomTo] = fromtile;
                tiles[randomFrom] = totile;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
            throw e;
        }
        finally
        {
            Debug.Log("Exit ShuffleTiles()");
        }
    }
    public Tile getAntennaTile(int antenna)
    {
        Debug.Log("Enter getAntennaTile()");
        try
        {
            Debug.Log("currenttiles.Count- " + currenttiles.Count);
            Tile tile = null;
            foreach (tilesLocation tileslocation in tilesDictionary.Keys)
            {
                tile = tilesDictionary[tileslocation];
                if(tile.antenna == antenna)
                {
                    break;
                }
            }
            return tile;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
            throw e;
        }
        finally
        {
            Debug.Log("Exit getAntennaTile()");
        }
    }
}
