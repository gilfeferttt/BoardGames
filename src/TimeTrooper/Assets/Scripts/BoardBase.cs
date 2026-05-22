using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardBase : MonoBehaviour
{
    public enum GameMode
    {
        MultiMode = 1,
        CoOpMode = 2
    }

    protected bool emulator = false;
    protected GameMode currentGameMode;

    public BoardBase()
    {
        Debug.Log("Enter BoardBase()");
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Debug.Log("Running on Android");
            }
            else
            {
                Debug.Log("Not running on Android");
                emulator = true;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit BoardBase()");
        }

    }
    protected void Start()
    {
        Debug.Log("Enter Start() BoardBas");
        try
        {
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit Start() BoardBas");
        }
    }
    public void SetGameMode(GameMode gamemode)
    {
        Debug.Log("Enter SetGameMode()");
        try
        {
            currentGameMode = gamemode;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit SetGameMode()");
        }
    }
    public GameMode GetGameMode()
    {
        Debug.Log("Enter GetGameMode()");
        try
        {
            return currentGameMode;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
            throw;
        }
        finally
        {
            Debug.Log("Exit GetGameMode()");
        }
    }
}
