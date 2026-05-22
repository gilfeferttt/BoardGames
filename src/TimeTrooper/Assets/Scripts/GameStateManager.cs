using System;
using UnityEditor;
using UnityEngine;

public enum GameState
{
    GetNames,
    SelectDifficulty,
    Unknown
}
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public GameObject BoardNotConnectedUI;
    public GameObject BackgroundUI;
    public GameObject SelectModeUI;
    public GameObject BannerUI;
    public GameObject TroopersUI;
    public GameObject TimeProgressUI;
    public GameObject GetReadyUI;
    public GameObject WellDoneUI;
    public GameObject FailRoundUI;
    public GameObject FinishLevelUI;
    public GameObject BonusUI;
    public GameObject PauseUI;

    void Start()
    {
        Debug.Log("Enter Start()");
        try
        {
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit Start()");
        }
    }
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
    public void StopGame()
    {
        Debug.Log("Enter StopGame()");
        try
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();  // Quits the built application
#endif
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit StopGame()");
        }
    }
    public void PauseGame()
    {
        Debug.Log("Enter PauseGame()");
        try
        {
            Board.instance.pauseGame();
            PauseUI.SetActive(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit PauseGame()");
        }
    }
    public void RestartGame()
    {
        Debug.Log("Enter RestartGame()");
        try
        {
            PauseUI.SetActive(false);
            Board.instance.restartGame();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit RestartGame()");
        }
    }
    public void ChangeToSelectMode()
    {
        Debug.Log("Enter ChangeToSelectMode()");
        try
        {
            BackgroundUI.SetActive(true);
            SelectModeUI.SetActive(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToSelectMode()");
        }
    }
    public void ChangeToBoardNotConnected()
    {
        Debug.Log("Enter ChangeToBoardNotConnected()");
        try
        {
            BackgroundUI.SetActive(true);
            BoardNotConnectedUI.SetActive(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToBoardNotConnected()");
        }
    }
}
