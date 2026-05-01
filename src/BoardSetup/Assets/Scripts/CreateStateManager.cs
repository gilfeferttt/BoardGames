using System.Collections;
using UnityEngine;

public enum GameState
{
    MainMenu,
    WiFi,
    Info,
    Antenna,
    Restart,
    StatusUI,
    Unknown
}
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public GameState CurrentState { get; private set; }
    
    public int delay = 1;

    public GameObject mainMenuUI;
    public GameObject WiFiUI;
    public GameObject infoUI;
    public GameObject antennaUI;
    public GameObject restartUI;
    public GameObject statusUI;

    public GameObject inGameStatusBar;

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
    public void ChangeToWiFi()
    {
        Debug.Log("Enter ChangeToWiFi()");
        try
        {
            ChangeState(GameState.WiFi);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToWiFi()");
        }
    }
    public void ChangeToRFID()
    {
        Debug.Log("Enter ChangeToRFID()");
        try
        {
            ChangeState(GameState.Antenna);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToRFID()");
        }
    }
    public void ChangeToInfo()
    {
        Debug.Log("Enter ChangeToInfo()");
        try
        {
            BoardGenerator.instance.getBoardInfo();
            ChangeState(GameState.Info);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToInfo()");
        }
    }
    public void ChangeToMainMenu()
    {
        Debug.Log("Enter ChangeToMainMenu()");
        try
        {
            ChangeState(GameState.MainMenu);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToMainMenu()");
        }
    }
    public void ChangeToExit()
    {
        Debug.Log("Enter ChangeToExit() CurrentState-" + CurrentState.ToString());
        try
        {
            if (CurrentState == GameState.MainMenu)
            {
                Application.Quit();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToExit()");
        }
    }
    public void ChangeToReconnect()
    {
        Debug.Log("Enter ChangeToReconnect()");
        try
        {
            BoardGenerator.instance.ReconnectToPPU();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToReconnect()");
        }
    }
    public void ChangeToSetWiFi()
    {
        Debug.Log("Enter ChangeToSetWiFi()");
        try
        {
            BoardGenerator.instance.setWiFiConfiguration();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToSetWiFi()");
        }
    }
    public void ChangeToSetRFID()
    {
        Debug.Log("Enter ChangeToSetRFID()");
        try
        {
            BoardGenerator.instance.setRFIDConfiguration();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToSetRFID()");
        }
    }
    public void ChangeToSetAntLoc()
    {
        Debug.Log("Enter ChangeToSetAntLoc()");
        try
        {
            BoardGenerator.instance.setAntennaLocation();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToSetAntLoc()");
        }
    }
    public void AddInGameStatusBar()
    {
        Debug.Log("Enter AddInGameStatusBar()");
        try
        {
            inGameStatusBar.SetActive(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit AddInGameStatusBar()");
        }
    }
    public void ChangeState(GameState newState)
    {
        Debug.Log("Enter ChangeState()");
        try
        {
            //if (CurrentState == newState) return;

            StartCoroutine(TrasitionToState(newState));
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeState()");
        }
    }
    public GameState getCurrentState()
    {
        Debug.Log("Enter getCurrentState()");
        try
        {
            return CurrentState;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
            return GameState.Unknown;
        }
        finally
        {
            Debug.Log("Exit getCurrentState()");
        }
    }
    private IEnumerator TrasitionToState(GameState newState)
    {
        Debug.Log("Enter TrasitionToState()");
        try
        {
            if (newState != GameState.MainMenu)
                yield return new WaitForSeconds(delay);

            CurrentState = newState;
            HandleStateChange();
        }
        finally
        {
            Debug.Log("Exit TrasitionToState()");
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Enter Start()");
        try
        {
            //inGameStatusBar.SetActive(true);
            //ChangeState(GameState.MainMenu);
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

    // Update is called once per frame
    void Update()
    {

    }

    private void HandleStateChange()
    {
        Debug.Log("Enter HandleStateChange()");
        try
        {
            HideAllMenu();

            switch (CurrentState)
            {
                case GameState.WiFi:
                    if (WiFiUI)
                        WiFiUI.SetActive(true);
                    break;
                case GameState.Info:
                    if (infoUI)
                        infoUI.SetActive(true);
                    break;
                case GameState.Antenna:
                    if (antennaUI)
                        antennaUI.SetActive(true);
                    break;
                case GameState.MainMenu:
                    if (mainMenuUI)
                        mainMenuUI.SetActive(true);
                    break;
                case GameState.Restart:
                    if (restartUI)
                        restartUI.SetActive(true);
                    break;
                case GameState.StatusUI:
                    if (statusUI)
                        statusUI.SetActive(true);
                    break;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit HandleStateChange()");
        }
    }
    private void HideAllMenu()
    {
        Debug.Log("Enter HideAllMenu()");
        try
        {
            if (mainMenuUI)
                mainMenuUI.SetActive(false);
            if (WiFiUI)
                WiFiUI.SetActive(false);
            if (infoUI)
                infoUI.SetActive(false);
            if(antennaUI)
                antennaUI.SetActive(false);
            if(restartUI)
                restartUI.SetActive(false);
            if (statusUI)
                statusUI.SetActive(false);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit HideAllMenu()");
        }
    }
}
