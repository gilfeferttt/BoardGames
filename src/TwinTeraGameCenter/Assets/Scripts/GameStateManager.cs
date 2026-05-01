using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public GameState CurrentState { get; private set; }

    public GameObject BackgroundUI;
    public GameObject BoardNotConnectedUI;
    public GameObject GetNamesUI;
    public GameObject SettingsScreenUI;

    List<GameObject> allPanelUI;

    void Start()
    {
        Debug.Log("Enter GameStateManager::Start()");
        try
        {
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit GameStateManager::Start()");
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
    public void ChangeToGameCenter()
    {
        Debug.Log("Enter ChangeToGameCenter()");
        try
        {
            BackgroundUI.SetActive(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToGameCenter()");
        }
    }
    public void ChangeToGetNames()
    {
        Debug.Log("Enter ChangeToGetNames()");
        try
        {
            BackgroundUI.SetActive(false);
            GetNamesUI.SetActive(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToGetNames()");
        }
    }
    public void ChangeToSettings()
    {
        Debug.Log("Enter ChangeToSettings()");
        try
        {
            BackgroundUI.SetActive(false);
            SettingsScreenUI.SetActive(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToSettings()");
        }
    }
    public void ChangeSaveAndCloseNames()
    {
        Debug.Log("Enter ChangeSaveAndCloseNames()");
        try
        {
            if (Board.instance.SetPlayerNames() == true)
            {
                Board.instance.SetProfiles();
                ChangeCloseNames();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeSaveAndCloseNames()");
        }
    }
    public void ChangeCloseNames()
    {
        Debug.Log("Enter ChangeCloseNames()");
        try
        {
            BackgroundUI.SetActive(true);
            GetNamesUI.SetActive(false);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeCloseNames()");
        }
    }
    public void ChangeSaveAndCloseSettings()
    {
        Debug.Log("Enter ChangeSaveAndCloseSettings()");
        try
        {
            if (Board.instance.SaveSettings() == true)
            {
                Board.instance.SetProfiles();
                ChangeCloseSettings();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeSaveAndCloseSettings()");
        }
    }
    public void ChangeCloseSettings()
    {
        Debug.Log("Enter ChangeCloseSettings()");
        try
        {
            BackgroundUI.SetActive(true);
            SettingsScreenUI.SetActive(false);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeCloseSettings()");
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
