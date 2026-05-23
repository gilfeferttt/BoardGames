using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BoardManager : BoardBase
{
    public static BoardManager instance;

    public enum GameMode
    {
        MultiMode = 1,
        CoOpMode = 2
    }
    protected GameMode currentGameMode;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void MakeTagDetected()
    {
        if (currentGameMode == GameMode.MultiMode)
        {
            Board.instance.MakeTagDetected();
        }
        else
        {
            BoardCoOp.instance.MakeTagDetected();
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
