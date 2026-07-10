using System.Collections;
using UnityEngine;

public enum GameState
{
    MainMenu,
    WiFi,
    Info,
    Status,
    Antenna,
    Restart,
    WiFiStatusUI,
    CheckP2V,
    BoardNotFound,
    BoardFound,
    BoardReconnect,
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
    public GameObject statusUI;
    public GameObject checkP2VUI;
    public GameObject antennaUI;
    public GameObject restartUI;
    public GameObject wifiStatusUI;
    public GameObject boardNotFoundUI;
    public GameObject reconnectingUI;

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
    public void ChangeToStatus()
    {
        Debug.Log("Enter ChangeToStatus()");
        try
        {
            BoardGenerator.instance.getBoardStatus();
            ChangeState(GameState.Status);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToStatus()");
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
    public void ChangeToP2V()
    {
        Debug.Log("Enter ChangeToP2V()");
        try
        {
            ChangeState(GameState.CheckP2V);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToP2V()");
        }
    }
    public void ChangeToCheckP2V()
    {
        Debug.Log("Enter ChangeToCheckP2V()");
        try
        {
            BoardGenerator.instance.checkP2V();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToCheckP2V()");
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
            boardNotFoundUI.SetActive(false);
            reconnectingUI.SetActive(true);
            StartCoroutine(TrasitionToReconnect());
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
    private IEnumerator TrasitionToReconnect()
    {
        Debug.Log("Enter TrasitionToReconnect()");
        try
        {
            yield return new WaitForSeconds(1);

            BoardGenerator.instance.ReconnectToPPU();
        }
        finally
        {
            Debug.Log("Exit TrasitionToReconnect()");
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
    public void ChangeToSetRFIDDetectionMode()
    {
        Debug.Log("Enter ChangeToSetRFIDDetectionMode()");
        try
        {
            BoardGenerator.instance.setRFIDDetectionMode();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToSetRFIDDetectionMode()");
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
    public void ChangeToSetRFPower()
    {
        Debug.Log("Enter ChangeToSetAntLoc()");
        try
        {
            BoardGenerator.instance.setRFPower();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToSetRFPower()");
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
                case GameState.Status:
                    if (statusUI)
                        statusUI.SetActive(true);
                    break;
                case GameState.CheckP2V:
                    if (checkP2VUI)
                        checkP2VUI.SetActive(true);
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
                case GameState.BoardNotFound:
                    if (boardNotFoundUI && reconnectingUI)
                    {
                        reconnectingUI.SetActive(false);
                        boardNotFoundUI.SetActive(true);
                    }
                    break;
                case GameState.BoardFound:
                    if (reconnectingUI && mainMenuUI)
                    {
                        reconnectingUI.SetActive(false);
                        mainMenuUI.SetActive(true);
                    }
                    break;
                case GameState.WiFiStatusUI:
                    if (wifiStatusUI)
                        wifiStatusUI.SetActive(true);
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
