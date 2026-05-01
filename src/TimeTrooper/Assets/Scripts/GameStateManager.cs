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
    public GameObject SelectDifficultyUI;
    public GameObject BannerUI;
    public GameObject LivesUI;
    public GameObject TroopersUI;
    public GameObject TimeProgressUI;
    public GameObject MainBoardUI;
    public GameObject GetReadyUI;
    public GameObject WellDoneUI;
    public GameObject FailRoundUI;
    public GameObject BonusUI;
    public GameObject PauseUI;
    public GameObject DemoScreenUI;
    public GameObject InstructionsUI;
    public GameObject SettingsScreenUI;
    public GameObject FinishLevelUI;

    List<GameObject> allPanelUI;

    private RectTransform finger;
    public RectTransform fingerDemo;
    public RectTransform fingerInstruction;
    private Vector2 startPos;
    private Vector2 endPos;
    private float duration = 1f;
    private Coroutine coroutinePlaySwipeHint;

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
    public void ChangeToGetNames()
    {
        Debug.Log("Enter ChangeToGetNames()");
        try
        {
            BackgroundUI.SetActive(true);
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
    public void ChangeToGetDificulty()
    {
        Debug.Log("Enter ChangeToGetDificulty()");
        try
        {
            StopCoroutine(coroutinePlaySwipeHint);
            InstructionsUI.SetActive(false);

            BannerUI.SetActive(true);
            LivesUI.SetActive(true);
            SelectDifficultyUI.SetActive(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToGetDificulty()");
        }
    }
    public void ChangeToGetInstructions()
    {
        Debug.Log("Enter ChangeToGetInstructions()");
        try
        {
            StopCoroutine(coroutinePlaySwipeHint);
            DemoScreenUI.SetActive(false);

            InstructionsUI.SetActive(true);
            finger = fingerInstruction;
            startPos = new Vector2(0, 400);
            endPos = new Vector2(-300, 400);
            coroutinePlaySwipeHint = StartCoroutine(PlaySwipeHint());
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToGetInstructions()");
        }
    }
    IEnumerator PlaySwipeHint()
    {
        while (true)
        {
            finger.anchoredPosition = startPos;

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / duration;
                finger.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                yield return null;
            }
            
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void ChangeToDemoScreen()
    {
        Debug.Log("Enter ChangeToDemoScreen()");
        try
        {
            if (Board.instance.SetPlayerNames() == true)
            {
                GetNamesUI.SetActive(false);

                DemoScreenUI.SetActive(true);
                finger = fingerDemo;
                startPos = endPos = new Vector2(0, 1000);
                endPos = new Vector2(-300, 1000);
                coroutinePlaySwipeHint = StartCoroutine(PlaySwipeHint());
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToDemoScreen()");
        }
    }
    public void ChangeToSettings()
    {
        Debug.Log("Enter ChangeToSettings()");
        try
        {
            GetNamesUI.SetActive(false);
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
    public void ChangeCloseSettings()
    {
        Debug.Log("Enter ChangeCloseSettings()");
        try
        {
            GetNamesUI.SetActive(true);
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
    public void ChangeSaveAndCloseSettings()
    {
        Debug.Log("Enter ChangeSaveAndCloseSettings()");
        try
        {
            if (Board.instance.SaveSettings() == true)
            {
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
    public void ChangeToBasic()
    {
        Debug.Log("Enter ChangeToBasic()");
        try
        {
            Board.instance.StartGameDificulty(Board.GameDificulty.Basic);
            ChangeToGatReady(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToBasic()");
        }
    }
    public void ChangeToAdvanced()
    {
        Debug.Log("Enter ChangeToAdvanced()");
        try
        {
            Board.instance.StartGameDificulty(Board.GameDificulty.Advanced);
            ChangeToGatReady(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToAdvanced()");
        }
    }
    public void ChangeToExpert()
    {
        Debug.Log("Enter ChangeToExpert()");
        try
        {
            Board.instance.StartGameDificulty(Board.GameDificulty.Expert);
            ChangeToGatReady(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToExpert()");
        }
    }
    public void ChangeToMaster()
    {
        Debug.Log("Enter ChangeToMaster()");
        try
        {
            Board.instance.StartGameDificulty(Board.GameDificulty.Master);
            ChangeToGatReady(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToMaster()");
        }
    }
    public void ChangeToElite()
    {
        Debug.Log("Enter ChangeToElite()");
        try
        {
            Board.instance.StartGameDificulty(Board.GameDificulty.Elite);
            ChangeToGatReady(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToElite()");
        }
    }
    public void ChangeToLegendary()
    {
        Debug.Log("Enter ChangeToLegendary()");
        try
        {
            Board.instance.StartGameDificulty(Board.GameDificulty.Legendary);
            ChangeToGatReady(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToLegendary()");
        }
    }
    public void ChangeToGatReady(bool BeginingOfGame)
    {
        Debug.Log("Enter ChangeToGatReady()");
        try
        {
            SelectDifficultyUI.SetActive(false);
            MainBoardUI.SetActive(false);
            BonusUI.SetActive(false);
            WellDoneUI.SetActive(false);
            FailRoundUI.SetActive(false);
            FinishLevelUI.SetActive(false);

            TimeProgressUI.SetActive(true);
            GetReadyUI.SetActive(true);
            
            Board.instance.StartGetReadyNow(BeginingOfGame);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToGatReady()");
        }
    }
    public void ChangeToPlay()
    {
        Debug.Log("Enter ChangeToPlay()");
        try
        {
            TroopersUI.SetActive(true);
            TimeProgressUI.SetActive(true);
            MainBoardUI.SetActive(true);
            TroopersUI.SetActive(true);

            GetReadyUI.SetActive(false);

            Board.instance.StartRound();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToPlay()");
        }
    }
    public void ChangeToNextDifficulty()
    {
        Debug.Log("Enter ChangeToNextDifficulty()");
        try
        {
            MainBoardUI.SetActive(false);
            TroopersUI.SetActive(false);
            TimeProgressUI.SetActive(false);
            GetReadyUI.SetActive(false);

            FinishLevelUI.SetActive(true);

            Board.instance.StartNextLevel();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToNextDifficulty()");
        }
    }
    public void ChangeToFailRound()
    {
        Debug.Log("Enter ChangeToFailRound()");
        try
        {
            MainBoardUI.SetActive(false);
            TroopersUI.SetActive(false);
            TimeProgressUI.SetActive(false);


            FailRoundUI.SetActive(true);


            Board.instance.StartFailRound();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToFailRound()");
        }
    }
    public void ChangeToWellDone()
    {
        Debug.Log("Enter ChangeToWellDone()");
        try
        {
            MainBoardUI.SetActive(false);
            TroopersUI.SetActive(false);
            TimeProgressUI.SetActive(false);

            
            WellDoneUI.SetActive(true);
            

            Board.instance.StartWellDone();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToWellDone()");
        }
    }
    public void ChangeToBonus()
    {
        Debug.Log("Enter ChangeToBonus()");
        try
        {
            WellDoneUI.SetActive(false);
            FailRoundUI.SetActive(false);
            BonusUI.SetActive(true);

            Board.instance.StartBonus();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToBonus()");
        }
    }
    /*public void ChangeState(GameState newState)
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
    }*/
    /*private IEnumerator TrasitionToState(GameState newState)
    {
        Debug.Log("Enter TrasitionToState()");
        try
        {
            //if (newState != GameState.MainMenu)
            yield return new WaitForSeconds(0);

            CurrentState = newState;
            HandleStateChange();
        }
        finally
        {
            Debug.Log("Exit TrasitionToState()");
        }
    }*/

    /*private void HandleStateChange()
    {
        Debug.Log("Enter HandleStateChange()");
        try
        {
            HideAllMenu();
            if(BackgroundUI.activeInHierarchy == false)
            {
                BackgroundUI.SetActive(true);
            }
            allPanelUI[CurrentState].SetActive(true);
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
            foreach(GameState gamestate in allPanelUI.Keys)
            {
                allPanelUI[gamestate].SetActive(false);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit HideAllMenu()");
        }
    }*/
}
