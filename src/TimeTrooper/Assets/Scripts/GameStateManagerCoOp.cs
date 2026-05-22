using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;


public class GameStateManagerCoOp : MonoBehaviour
{
    public static GameStateManagerCoOp instance;

    public GameState CurrentState { get; private set; }

    

    public GameObject GetNameUI;
    public GameObject SelectDifficultyUI;
    
    public GameObject LivesUI;

    public GameObject MainBoardUI;
    
    public GameObject DemoScreenUI;
    public GameObject InstructionsUI;
    public GameObject SettingsScreenUI;

    List<GameObject> allPanelUI;

    private RectTransform finger;
    public RectTransform fingerDemo;
    public RectTransform fingerCoOpInstruction;
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
    
    
    
    
    public void ChangeToSelectMode()
    {
        Debug.Log("Enter ChangeToSelectMode()");
        try
        {
            Troopers.instance.setTroppersScale(BoardBase.GameMode.CoOpMode);
            GameStateManager.instance.SelectModeUI.SetActive(false);
            ChangeToGetNames();
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
    public void ChangeToGetNames()
    {
        Debug.Log("Enter ChangeToGetNames()");
        try
        {
            //BackgroundUI.SetActive(true);
            GetNameUI.SetActive(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToGetOpNames()");
        }
    }
    
    public void ChangeToStartGame()
    {
        Debug.Log("Enter ChangeToStartGame()");
        try
        {
            Board.instance.SetGameMode(Board.GameMode.CoOpMode);
            ChangeToDemoScreen();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToStartGame()");
        }
    }
    public void ChangeToGetDificulty()
    {
        Debug.Log("Enter ChangeToGetDificulty()");
        try
        {
            StopCoroutine(coroutinePlaySwipeHint);
            InstructionsUI.SetActive(false);

            GameStateManager.instance.BannerUI.SetActive(true);
            LivesUI.SetActive(true);
            SelectDifficultyUI.SetActive(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ChangeToGetCoOpDificulty()");
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
            finger = fingerCoOpInstruction;
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
            bool setNamesOK = false;
            
            setNamesOK = BoardCoOp.instance.SetTeamMembersNames();
            
            if (setNamesOK == true)
            {
                //GetNamesUI.SetActive(false);
                GetNameUI.SetActive(false);

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
            GetNameUI.SetActive(false);
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
            GetNameUI.SetActive(true);
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
            if (BoardCoOp.instance.SaveSettings() == true)
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
            BoardCoOp.instance.StartGameDificulty(BoardCoOp.GameDificulty.Basic);
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
    
    public void ChangeToMaster()
    {
        Debug.Log("Enter ChangeToMaster()");
        try
        {
            BoardCoOp.instance.StartGameDificulty(BoardCoOp.GameDificulty.Master);
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
    
    public void ChangeToLegendary()
    {
        Debug.Log("Enter ChangeToLegendary()");
        try
        {
            BoardCoOp.instance.StartGameDificulty(BoardCoOp.GameDificulty.Legendary);
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
            GameStateManager.instance.BonusUI.SetActive(false);
            GameStateManager.instance.WellDoneUI.SetActive(false);
            GameStateManager.instance.FailRoundUI.SetActive(false);
            //FinishLevelUI.SetActive(false);

            GameStateManager.instance.TimeProgressUI.SetActive(true);
            GameStateManager.instance.GetReadyUI.SetActive(true);

            BoardCoOp.instance.StartGetReadyNow(BeginingOfGame);
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
        Debug.Log("Enter ChangeToPlay() CoOp");
        try
        {
            GameStateManager.instance.TroopersUI.SetActive(true);
            GameStateManager.instance.TimeProgressUI.SetActive(true);
            MainBoardUI.SetActive(true);
            GameStateManager.instance.TroopersUI.SetActive(true);

            GameStateManager.instance.GetReadyUI.SetActive(false);

            BoardCoOp.instance.StartRound();
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
            GameStateManager.instance.TroopersUI.SetActive(false);
            GameStateManager.instance.TimeProgressUI.SetActive(false);
            GameStateManager.instance.GetReadyUI.SetActive(false);

            //FinishLevelUI.SetActive(true);

            BoardCoOp.instance.StartNextLevel();
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
            GameStateManager.instance.TroopersUI.SetActive(false);
            GameStateManager.instance.TimeProgressUI.SetActive(false);


            GameStateManager.instance.FailRoundUI.SetActive(true);


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
            GameStateManager.instance.TroopersUI.SetActive(false);
            GameStateManager.instance.TimeProgressUI.SetActive(false);


            GameStateManager.instance.WellDoneUI.SetActive(true);


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
            GameStateManager.instance.WellDoneUI.SetActive(false);
            GameStateManager.instance.FailRoundUI.SetActive(false);
            GameStateManager.instance.BonusUI.SetActive(true);

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


}
