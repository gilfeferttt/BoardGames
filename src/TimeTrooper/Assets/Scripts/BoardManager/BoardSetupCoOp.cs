using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardSetupCoOp
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
        TileTen = 10,
        TileEleven = 11,
        TileTwelve = 12,
        TileThirteen = 13,
        TileFourteen = 14,
        TileFifteen = 15,
        TileSixteen = 16,
        TileSeventeen = 17,
        TileEightteen = 18,
        TileNineteen = 19,
        TileTwenty = 20,
        TileTwentyone = 21,
        TileTwentytwo = 22,
        TileTwentythree = 23,
        TileTwentyfour = 24,
        TileTwentyfive = 25
    }

    private Dictionary<tilesLocation, Tile> tilesDictionary;
    private List<Tile> tiles;
    private List<Tile> currenttiles;
    public BoardSetupCoOp()
    {
        tilesDictionary = new Dictionary<tilesLocation, Tile>();

        tilesDictionary.Add(tilesLocation.TileOne, new Tile(new Vector2(-700f, 460f), 1));
        tilesDictionary.Add(tilesLocation.TileTwo, new Tile(new Vector2(-350f, 460f), 2));
        tilesDictionary.Add(tilesLocation.TileThree, new Tile(new Vector2(0f, 460f), 3));
        tilesDictionary.Add(tilesLocation.TileFour, new Tile(new Vector2(350f, 460f), 4));
        tilesDictionary.Add(tilesLocation.TileFive, new Tile(new Vector2(700f, 460f), 5));

        tilesDictionary.Add(tilesLocation.TileSix, new Tile(new Vector2(-700f, 150f), 6));
        tilesDictionary.Add(tilesLocation.TileSeven, new Tile(new Vector2(-350f, 150f), 7));
        tilesDictionary.Add(tilesLocation.TileEight, new Tile(new Vector2(0f, 150f), 8));
        tilesDictionary.Add(tilesLocation.TileNine, new Tile(new Vector2(350f, 150f), 9));
        tilesDictionary.Add(tilesLocation.TileTen, new Tile(new Vector2(700f, 150f), 10));

        tilesDictionary.Add(tilesLocation.TileEleven, new Tile(new Vector2(-700f, -150f), 11));
        tilesDictionary.Add(tilesLocation.TileTwelve, new Tile(new Vector2(-350f, -150f), 12));
        tilesDictionary.Add(tilesLocation.TileThirteen, new Tile(new Vector2(0f, -150f), 13));
        tilesDictionary.Add(tilesLocation.TileFourteen, new Tile(new Vector2(350f, -150f), 14));
        tilesDictionary.Add(tilesLocation.TileFifteen, new Tile(new Vector2(700f, -150f), 15));

        tilesDictionary.Add(tilesLocation.TileSixteen, new Tile(new Vector2(-700f, -500f), 16));
        tilesDictionary.Add(tilesLocation.TileSeventeen, new Tile(new Vector2(-350f, -500f), 17));
        tilesDictionary.Add(tilesLocation.TileEightteen, new Tile(new Vector2(0f, -500f), 18));
        tilesDictionary.Add(tilesLocation.TileNineteen, new Tile(new Vector2(350f, -500f), 19));
        tilesDictionary.Add(tilesLocation.TileTwenty, new Tile(new Vector2(700f, -500f), 20));

        tilesDictionary.Add(tilesLocation.TileTwentyone, new Tile(new Vector2(-700f, -900f), 21));
        tilesDictionary.Add(tilesLocation.TileTwentytwo, new Tile(new Vector2(-350f, -900f), 22));
        tilesDictionary.Add(tilesLocation.TileTwentythree, new Tile(new Vector2(0f, -900f), 23));
        tilesDictionary.Add(tilesLocation.TileTwentyfour, new Tile(new Vector2(350f, -900f), 24));
        tilesDictionary.Add(tilesLocation.TileTwentyfive, new Tile(new Vector2(700f, -900f), 25));

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
                int randomFrom = UnityEngine.Random.Range(0,25);
                Debug.Log("randomFrom-" + randomFrom);
                Tile fromtile = tiles[randomFrom];
                int randomTo = UnityEngine.Random.Range(0, 25);
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
