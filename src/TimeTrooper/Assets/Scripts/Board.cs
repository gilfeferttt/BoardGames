using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Video;
using static BoardSetup;
using UnityEngine.UI;
using System.Linq;

public class Board : MonoBehaviour
{
    public Material ringMaterial;

    float failRoundTotalTime = 3f;
    int failRoundCountdownSteps = 3;
    int failRoundLastSecond = 0;
    float welldoneTotalTime = 3f;
    int welldoneCountdownSteps = 3;
    int welldoneLastSecond = 0;
    float bonusTotalTime = 5f;
    int bonusCountdownSteps = 5;
    int bonusLastSecond = 0;
    float startroundTotalTime = 7;
    int startroundCountdownSteps = 7;
    int startroundLastSecond = 4;
    float getreadyTotalTime = 3;
    int getreadyCountdownSteps = 3;
    int getreadyLastSecond = 0;

    bool isPaused = false;

    [SerializeField] private GameObject troopersUI;

    public enum GameDificulty
    {
        None,
        Basic,
        Advanced,
        Expert,
        Master,
        Elite,
        Legendary
    }
    public enum TagDetectedStatus
    {
        NotSameNumberOfTags = 0,
        DifferentTag = 1,
        SameTag = 2,
        SameTagDifferentOrder = 3,
        SameTagSameOrder = 4
    }
    GameDificulty gameDificulty;

    //Troopers troopers;
    BoardSetup boardSetup;
    List<Tile> nextTiles;
    List<Tile> currentTiles;

    Coroutine playCountDownRoutine;
    Coroutine getreadyCountDownRoutine;
    Coroutine getbonusCountDownRoutine;
    Coroutine welldoneCountDownRoutine;
    Coroutine failRoundCountDownRoutine;
    Coroutine disapearTropperCountDownRoutine;
    [SerializeField] private TMP_Text countdownText;
    int countdownCounter;

    public static Board instance;
    public VideoPlayer videoPlayer;

    [SerializeField]
    private TMP_InputField[] playersNames;

    [SerializeField] private TMP_Text[] bannerPlayersNames;
    [SerializeField] private RawImage[] Turns;
    [SerializeField] private TMP_Text[] bannerPlayersPoints;
    [SerializeField] private RawImage[] Harts;
    [SerializeField] private RawImage[] Stars;

    [SerializeField] private TMP_Text demoscreenPlayer1Name;
    [SerializeField] private TMP_Text demoscreenPlayer2Name;

    [SerializeField] private TMP_Text txtBattery;

    private PlayerData playerdata;
    private string playerfilePath;
    private PPUManager ppumanager;

    private GameData gamedata;
    private string gamefilePath;

    [SerializeField] private TMP_Text txtRound;
    [SerializeField] private TMP_Text txtTrooperNumber;
    [SerializeField] private TMP_Text txtTrooper;
    [SerializeField] private TMP_Text txtPlayerName;
    [SerializeField] private TMP_Text txtGetReady;

    [SerializeField] private TMP_Text txtRoundPoints;
    [SerializeField] private TMP_Text txtRoundPartialPoints;
    [SerializeField] private TMP_Text txtBonusPoints;
    [SerializeField] private TMP_Text txtWinsRoundNumber;

    TroopersManager troppermanagerUI;
    TroopersManager troppermanagerPreviouseUI;
    TroopersManager troppermanagerPPU;
    GameEngine gameengine;
    PlayersEngine playersengine;
    List<Trooper> currenttroopers;
    GameRound currentround = null;
    Player currentplayer = null;
    List<string> txtNumbers = new List<string> { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten" };
    List<string> txtTrooperString = new List<string> { "Troopers", "Trooper", "Troopers", "Troopers", "Troopers", "Troopers", "Troopers", "Troopers", "Troopers", "Troopers", "Troopers" };

    [SerializeField] private TMP_Text txtSimulatorLeft;
    [SerializeField] private TMP_Text txtSimulatorRight;

    [SerializeField] private Button btnSimulatorTag;
        
    private bool noNeedToDetectAgain = true;
    private bool emulator = false;

    Coroutine maketagdetectCoroutine = null;

    bool competitiveMode = true;

    bool getTrooperRandomally = false;
    bool orderTrooperMetter = false;
    bool trooperDisappear = false;
    Dictionary<string, string> detectTagsOrdered;
    Dictionary<string, string> detectTagsOrderedScore;
    List<object> trooperObjectToDisapear;

    bool ingamenames = false;
    int currenttropperset = 0;

    [Header("Flicker Settings")]
    [SerializeField] [Range(1f, 8f)] private float speed = 3.5f;        // higher = faster flicker
    [SerializeField] [Range(0.1f, 1f)] private float minAlpha = 0.35f;    // how dark it gets
    [SerializeField] [Range(0.6f, 1f)] private float maxAlpha = 1f;
    [SerializeField] private TMP_Text txtPauseGame;

    readonly object syncTropperDetection = new object();
    bool tropperStillDetecting = true;

    [SerializeField] private TMP_InputField basicWaitTime;
    [SerializeField] private TMP_InputField advancedWaitTime;
    [SerializeField] private TMP_InputField expertWaitTime;
    [SerializeField] private TMP_InputField masterWaitTime;
    [SerializeField] private TMP_InputField eliteWaitTime;
    [SerializeField] private TMP_InputField legendaryWaitTime;

    [SerializeField] private TMP_InputField basicNumOfRounds;
    [SerializeField] private TMP_InputField advancedNumOfRounds;
    [SerializeField] private TMP_InputField expertNumOfRounds;
    [SerializeField] private TMP_InputField masterNumOfRounds;
    [SerializeField] private TMP_InputField eliteNumOfRounds;
    [SerializeField] private TMP_InputField legendaryNumOfRounds;

    [SerializeField] private TMP_InputField basicNumOfRoundsMin;
    [SerializeField] private TMP_InputField advancedNumOfRoundsMin;
    [SerializeField] private TMP_InputField expertNumOfRoundsMin;
    [SerializeField] private TMP_InputField masterNumOfRoundsMin;
    [SerializeField] private TMP_InputField eliteNumOfRoundsMin;
    [SerializeField] private TMP_InputField legendaryNumOfRoundsMin;

    [SerializeField] private TMP_InputField eliteTimeToDisappear;
    [SerializeField] private TMP_InputField legendaryTimeToDisappear;

    [SerializeField] private TMP_Text isTagDetected;

    [SerializeField] private TMP_InputField numberOfPlayers;

    [SerializeField] private TMP_Text finishLevelMessage;

    [SerializeField] private TMP_Text txtVersion;
    [SerializeField] private TMP_Text txtBuild;

    [SerializeField] private RawImage riNextTrooperToUse;

    int currentscore;

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
        
        basicWaitTime.contentType = TMP_InputField.ContentType.IntegerNumber;
        advancedWaitTime.contentType = TMP_InputField.ContentType.IntegerNumber;
        expertWaitTime.contentType = TMP_InputField.ContentType.IntegerNumber;
        masterWaitTime.contentType = TMP_InputField.ContentType.IntegerNumber;
        eliteWaitTime.contentType = TMP_InputField.ContentType.IntegerNumber;
        legendaryWaitTime.contentType = TMP_InputField.ContentType.IntegerNumber;

        eliteTimeToDisappear.contentType = TMP_InputField.ContentType.IntegerNumber;
        legendaryTimeToDisappear.contentType = TMP_InputField.ContentType.IntegerNumber;

        numberOfPlayers.characterLimit = 1;
        numberOfPlayers.onValidateInput = ValidateInputNumberOfPlayers;

        basicNumOfRoundsMin.onValidateInput = ValidateInputNumberOfLevels;
        advancedNumOfRoundsMin.onValidateInput = ValidateInputNumberOfLevels;
        expertNumOfRoundsMin.onValidateInput = ValidateInputNumberOfLevels;
        masterNumOfRoundsMin.onValidateInput = ValidateInputNumberOfLevels;
        eliteNumOfRoundsMin.onValidateInput = ValidateInputNumberOfLevels;
        legendaryNumOfRoundsMin.onValidateInput = ValidateInputNumberOfLevels;

        basicNumOfRounds.onValidateInput = ValidateInputNumberOfLevels;
        advancedNumOfRounds.onValidateInput = ValidateInputNumberOfLevels;
        expertNumOfRounds.onValidateInput = ValidateInputNumberOfLevels;
        masterNumOfRounds.onValidateInput = ValidateInputNumberOfLevels;
        eliteNumOfRounds.onValidateInput = ValidateInputNumberOfLevels;
        legendaryNumOfRounds.onValidateInput = ValidateInputNumberOfLevels;
    }
    private char ValidateInputNumberOfPlayers(string text, int charIndex, char addedChar)
    {
        if (addedChar == '2' || addedChar == '3' || addedChar == '4')
        {
            return addedChar;
        }

        return '\0';
    }
    private char ValidateInputNumberOfLevels(string text, int charIndex, char addedChar)
    {
        if (addedChar == '0') 
        {
            return '\0';
        }
        return addedChar;
    }
    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            // Beautiful smooth sine flicker
            float alpha = Mathf.Lerp(minAlpha, maxAlpha,
                (Mathf.Sin(Time.time * speed) + 1f) * 0.5f);

            Color c = txtPauseGame.color;
            c.a = alpha;
            txtPauseGame.color = c;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Enter Start()");
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

            if(competitiveMode == true)
            {

            }
            VersionData versionData = Resources.Load<VersionData>("VersionData");
            txtVersion.text = versionData.displayVersion;
            txtBuild.text = versionData.buildNumber.ToString();

            // Prevents the screen from dimming / sleeping / turning off
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            //AudioManager.instance.PlaySFX(AudioManager.instance.PlayMusic();

            troppermanagerUI = new TroopersManager();
            troppermanagerPreviouseUI = new TroopersManager();
            troppermanagerPPU = new TroopersManager();
            
            ppumanager = new PPUManager(emulator);
            ppumanager.ConnectToPPU(gameObject.name);
            if(ppumanager.boardConnectStatus == 0)
            {
                Debug.Log("Board is connected");
            }
            else if (ppumanager.boardConnectStatus == 1)
            {
                Debug.Log("Board runs in emulation mode");
                emulator = true;
            } else
            {
                Debug.Log("Board is not yet connected");
            }
            if (emulator == true)
            {
                txtSimulatorLeft.gameObject.SetActive(true);
                txtSimulatorRight.gameObject.SetActive(true);
                btnSimulatorTag.gameObject.SetActive(true);
            }

            if (ppumanager.boardConnectStatus == 0 || ppumanager.boardConnectStatus == 1)
            {
                playerfilePath = Path.Combine(Application.persistentDataPath, "playerdata.json");
                gamefilePath = Path.Combine(Application.persistentDataPath, "gamedata.json");

                //ppumanager.SetRFIDConfiguration(-1);

                LoadPlayerData();
                LoadGameData();

                playersNames[0].text = playerdata.player1Name;
                playersNames[1].text = playerdata.player2Name;
                playersNames[2].text = playerdata.player3Name;
                playersNames[3].text = playerdata.player4Name;
                //player1Name.text = playerdata.player1Name;
                //player2Name.text = playerdata.player2Name;
                currenttropperset = playerdata.currenttropperset;

                playIntroVideo();
            } else
            {
                GameStateManager.instance.ChangeToBoardNotConnected();
            }
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
    public void pauseGame()
    {
        Debug.Log("Enter pauseGame()");
        try
        {
            isPaused = true;
            AudioManager.instance.PauseSFX();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit pauseGame()");
        }
    }
    public void restartGame()
    {
        Debug.Log("Enter restartGame()");
        try
        {
            AudioManager.instance.UnpauseSFX();
            isPaused = false;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit restartGame()");
        }
    }
    public void MakeTagDetected()
    {
        Debug.Log("Enter MakeTagDetected()");
        try
        {
            AudioManager.instance.stopCountdown();
            if(maketagdetectCoroutine != null)
            {
                StopCoroutine(maketagdetectCoroutine);
                maketagdetectCoroutine = null;
            }
            maketagdetectCoroutine = StartCoroutine(MakeTagDetectedFlow());
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit MakeTagDetected()");
        }
    }
    IEnumerator MakeTagDetectedFlow()
    {
        Debug.Log("Enter MakeTagDetected(currenttroopers.Count,nextTiles.Count) " + currenttroopers.Count + " " + nextTiles.Count);

        int trooperX = 0;

        int numberoftagstosimulate = nextTiles.Count;
        Message msg = new Message();
        msg.bleaddress = "84:0D:8E:23:8F:A6";
        msg.pputags = new MessageTag[numberoftagstosimulate];
        int numberOfTagsToSend = 0;
        foreach (Tile tile in nextTiles)
        {
            numberoftagstosimulate--;
            Trooper troopertoplay = currenttroopers[trooperX];
            MessageTag msgtag = new MessageTag();
            msgtag.antenna = tile.antenna;
            msgtag.id = troopertoplay.GameObjects[0].Tag.SerialNumber;
            msg.pputags[trooperX] = msgtag;

            trooperX++;
            
            numberOfTagsToSend++;
            Message msgToSend = new Message();
            msgToSend.bleaddress = "84:0D:8E:23:8F:A6";
            msgToSend.pputags = new MessageTag[numberOfTagsToSend];
            for(int x = 0; x < numberOfTagsToSend; x++)
            {
                msgToSend.pputags[x] = msg.pputags[x];
            }
            
            string message = JsonUtility.ToJson(msgToSend, false);
            Debug.Log("Send tags - " + message);
            allDetectedTags(message);

            if (numberoftagstosimulate == 0)
            {
                break;
            }
            yield return new WaitForSeconds(1);
        }

        yield return null;
        
        Debug.Log("Exit MakeTagDetected()");
    }
    public void allDetectedTags(string message)
    {
        Debug.Log("Enter allDetectedTags() message-" + message);
            
        try
        {
            if (ingamenames == true)
            {
                Debug.Log("allDetectedTags - ingamenames");
                // just detect the tropper set to play with
                PPUData ppudata = ppumanager.getPPUData(message);
                if (ppudata.pputags.Length > 0)
                {
                    Debug.Log("allDetectedTags - ingamenames - tag detected");
                    isTagDetected.text = "Detected";
                    currenttropperset = Troopers.instance.getTrooperSetByTagID(ppudata.pputags[0].id);
                }
            }
            else
            {
                if (noNeedToDetectAgain == true)
                {
                    return;
                }

                PPUData ppudata = ppumanager.getPPUData(message);
                PlayTags(ppudata);
                TagDetectedStatus tagsDetectedStatus = calculateScore(ppudata);
                Debug.Log("tagsDetectedStatus-" + tagsDetectedStatus.ToString());
                if (tagsDetectedStatus == TagDetectedStatus.SameTag || tagsDetectedStatus == TagDetectedStatus.SameTagSameOrder)
                {
                    Debug.Log("Tags are detected correctly");
                    noNeedToDetectAgain = true;

                    // Disable all antennas
                    ppumanager.setAntennaLocation(new int[] {  });

                    if (playCountDownRoutine != null)
                        StopCoroutine(playCountDownRoutine);

                    currentplayer.points += currentscore; // currentround.Points;

                    setPointsOnTheBoard();

                    txtRoundPoints.text = "+" + currentscore; // currentround.Points;
                    lock (syncTropperDetection)
                    {
                        if (tropperStillDetecting == true)
                        {
                            tropperStillDetecting = false;
                            AudioManager.instance.playSuccess();
                            GameStateManager.instance.ChangeToWellDone();

                            unPrintDetectedTrooperOnBoard();
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit allDetectedTags()");
        }
    }
    private void setPointsOnTheBoard()
    {
        List<Player> players = playersengine.players;
        for(int x = 0; x < players.Count; x++)
        {
            bannerPlayersPoints[x].text = players[x].points.ToString();
        }
    }
    private void LoadPlayerData()
    {
        playerdata = new PlayerData();  // Default fresh data

        if (File.Exists(playerfilePath))
        {
            try
            {
                string json = File.ReadAllText(playerfilePath);
                playerdata = JsonUtility.FromJson<PlayerData>(json);

                // Validate loaded data (optional safety)
                if (string.IsNullOrEmpty(playerdata.player1Name))
                    playerdata.player1Name = "Guest 1";
                if (string.IsNullOrEmpty(playerdata.player1Name))
                    playerdata.player2Name = "Guest 2";
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Load failed: {e.Message} - Using defaults");
            }
        } else
        {
            Debug.Log("Can't load data - File does not exist");
        }
    }
    private void LoadGameData()
    {
        Debug.Log("Enter LoadGameData()");
        try
        {
            gamedata = new GameData();

            if (File.Exists(gamefilePath))
            {
                try
                {
                    string json = File.ReadAllText(gamefilePath);
                    gamedata = JsonUtility.FromJson<GameData>(json);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Load failed: {e.Message} - Using defaults");
                }
            }
            else
            {
                Debug.Log("Can't load data - File does not exist");
            }
            if (string.IsNullOrEmpty(gamedata.basicWaitTime))
                gamedata.basicWaitTime = GameEngine.basicWaitTime.ToString();
            if (string.IsNullOrEmpty(gamedata.advancedWaitTime))
                gamedata.advancedWaitTime = GameEngine.advancedWaitTime.ToString();
            if (string.IsNullOrEmpty(gamedata.expertWaitTime))
                gamedata.expertWaitTime = GameEngine.expertWaitTime.ToString();
            if (string.IsNullOrEmpty(gamedata.masterWaitTime))
                gamedata.masterWaitTime = GameEngine.masterWaitTime.ToString();
            if (string.IsNullOrEmpty(gamedata.eliteWaitTime))
                gamedata.eliteWaitTime = GameEngine.eliteWaitTime.ToString();
            if (string.IsNullOrEmpty(gamedata.legendaryWaitTime))
                gamedata.legendaryWaitTime = GameEngine.legendaryWaitTime.ToString();

            if (string.IsNullOrEmpty(gamedata.basicNumOfRounds))
                gamedata.basicNumOfRounds = GameEngine.basicNumOfRounds.ToString();
            if (string.IsNullOrEmpty(gamedata.advancedNumOfRounds))
                gamedata.advancedNumOfRounds = GameEngine.advancedNumOfRounds.ToString();
            if (string.IsNullOrEmpty(gamedata.expertNumOfRounds))
                gamedata.expertNumOfRounds = GameEngine.expertNumOfRounds.ToString();
            if (string.IsNullOrEmpty(gamedata.masterNumOfRounds))
                gamedata.masterNumOfRounds = GameEngine.masterNumOfRounds.ToString();
            if (string.IsNullOrEmpty(gamedata.eliteNumOfRounds))
                gamedata.eliteNumOfRounds = GameEngine.eliteNumOfRounds.ToString();
            if (string.IsNullOrEmpty(gamedata.legendaryNumOfRounds))
                gamedata.legendaryNumOfRounds = GameEngine.legendaryNumOfRounds.ToString();

            if (string.IsNullOrEmpty(gamedata.basicNumOfRoundsMin))
                gamedata.basicNumOfRoundsMin = GameEngine.basicNumOfRoundsMin.ToString();
            if (string.IsNullOrEmpty(gamedata.advancedNumOfRoundsMin))
                gamedata.advancedNumOfRoundsMin = GameEngine.advancedNumOfRoundsMin.ToString();
            if (string.IsNullOrEmpty(gamedata.expertNumOfRoundsMin))
                gamedata.expertNumOfRoundsMin = GameEngine.expertNumOfRoundsMin.ToString();
            if (string.IsNullOrEmpty(gamedata.masterNumOfRoundsMin))
                gamedata.masterNumOfRoundsMin = GameEngine.masterNumOfRoundsMin.ToString();
            if (string.IsNullOrEmpty(gamedata.eliteNumOfRoundsMin))
                gamedata.eliteNumOfRoundsMin = GameEngine.eliteNumOfRoundsMin.ToString();
            if (string.IsNullOrEmpty(gamedata.legendaryNumOfRoundsMin))
                gamedata.legendaryNumOfRoundsMin = GameEngine.legendaryNumOfRoundsMin.ToString();

            if (string.IsNullOrEmpty(gamedata.numberOfPlayers))
                gamedata.numberOfPlayers = GameEngine.numberOfPlayers.ToString();

            if (string.IsNullOrEmpty(gamedata.eliteTimeToDisappear))
                gamedata.eliteTimeToDisappear = GameEngine.eliteTimeToDisappear.ToString();
            if (string.IsNullOrEmpty(gamedata.legendaryTimeToDisappear))
                gamedata.legendaryTimeToDisappear = GameEngine.legendaryTimeToDisappear.ToString();

            Debug.Log("gamedata.basicWaitTime.ToString()-" + gamedata.basicWaitTime.ToString());
            Debug.Log("gamedata.basicNumOfRounds.ToString()-" + gamedata.basicNumOfRounds.ToString());
            basicWaitTime.text = gamedata.basicWaitTime.ToString();
            advancedWaitTime.text = gamedata.advancedWaitTime.ToString();
            expertWaitTime.text = gamedata.expertWaitTime.ToString();
            masterWaitTime.text = gamedata.masterWaitTime.ToString();
            eliteWaitTime.text = gamedata.eliteWaitTime.ToString();
            legendaryWaitTime.text = gamedata.legendaryWaitTime.ToString();

            basicNumOfRounds.text = gamedata.basicNumOfRounds.ToString();
            advancedNumOfRounds.text = gamedata.advancedNumOfRounds.ToString();
            expertNumOfRounds.text = gamedata.expertNumOfRounds.ToString();
            masterNumOfRounds.text = gamedata.masterNumOfRounds.ToString();
            eliteNumOfRounds.text = gamedata.eliteNumOfRounds.ToString();
            legendaryNumOfRounds.text = gamedata.legendaryNumOfRounds.ToString();

            basicNumOfRoundsMin.text = gamedata.basicNumOfRoundsMin.ToString();
            advancedNumOfRoundsMin.text = gamedata.advancedNumOfRoundsMin.ToString();
            expertNumOfRoundsMin.text = gamedata.expertNumOfRoundsMin.ToString();
            masterNumOfRoundsMin.text = gamedata.masterNumOfRoundsMin.ToString();
            eliteNumOfRoundsMin.text = gamedata.eliteNumOfRoundsMin.ToString();
            legendaryNumOfRoundsMin.text = gamedata.legendaryNumOfRoundsMin.ToString();

            numberOfPlayers.text = gamedata.numberOfPlayers.ToString();

            eliteTimeToDisappear.text = gamedata.eliteTimeToDisappear.ToString();
            legendaryTimeToDisappear.text = gamedata.legendaryTimeToDisappear.ToString();
        }
        finally
        {
            Debug.Log("Exit LoadGameData()");
        }
    }
    public void SavePlayerData()
    {
        Debug.Log("Enter SavePlayerData()");
        try
        {
            string json = JsonUtility.ToJson(playerdata, true);  // Pretty print for readability
            File.WriteAllText(playerfilePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit SavePlayerData()");
        }
    }
    public bool SaveSettings()
    {
        Debug.Log("Enter SaveSettings()");
        try
        {
            if(validateInputs() == true)
            {
                gamedata.basicWaitTime = basicWaitTime.text;
                gamedata.advancedWaitTime = advancedWaitTime.text;
                gamedata.expertWaitTime = expertWaitTime.text;
                gamedata.masterWaitTime = masterWaitTime.text;
                gamedata.eliteWaitTime = eliteWaitTime.text;
                gamedata.legendaryWaitTime = legendaryWaitTime.text;

                gamedata.basicNumOfRounds = basicNumOfRounds.text;
                gamedata.advancedNumOfRounds = advancedNumOfRounds.text;
                gamedata.expertNumOfRounds = expertNumOfRounds.text;
                gamedata.masterNumOfRounds = masterNumOfRounds.text;
                gamedata.eliteNumOfRounds = eliteNumOfRounds.text;
                gamedata.legendaryNumOfRounds = legendaryNumOfRounds.text;

                gamedata.basicNumOfRoundsMin = basicNumOfRoundsMin.text;
                gamedata.advancedNumOfRoundsMin = advancedNumOfRoundsMin.text;
                gamedata.expertNumOfRoundsMin = expertNumOfRoundsMin.text;
                gamedata.masterNumOfRoundsMin = masterNumOfRoundsMin.text;
                gamedata.eliteNumOfRoundsMin = eliteNumOfRoundsMin.text;
                gamedata.legendaryNumOfRoundsMin = legendaryNumOfRoundsMin.text;

                gamedata.numberOfPlayers = numberOfPlayers.text;

                gamedata.eliteTimeToDisappear = eliteTimeToDisappear.text;
                gamedata.legendaryTimeToDisappear = legendaryTimeToDisappear.text;

                string json = JsonUtility.ToJson(gamedata, true);  // Pretty print for readability
                File.WriteAllText(gamefilePath, json);

                return true;
            } else
            {
                return false;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
            return false;
        }
        finally
        {
            Debug.Log("Exit SaveSettings()");
        }
    }
    private bool validateInputs()
    {
        bool validinputs = true;
        Debug.Log("Enter validateInputs()");
        try
        {
            Debug.Log("eliteTimeToDisappear.text-" + eliteTimeToDisappear.text + " legendaryTimeToDisappear.text-" + legendaryTimeToDisappear.text);
            if (Convert.ToInt32(eliteTimeToDisappear.text) >= Convert.ToInt32(eliteWaitTime.text) || Convert.ToInt32(legendaryTimeToDisappear.text) >= Convert.ToInt32(legendaryWaitTime.text))
            {
                validinputs = false;
            }
            if (Convert.ToInt32(basicNumOfRoundsMin.text) > Convert.ToInt32(basicNumOfRounds.text)) {
                validinputs = false;
            }
            if (Convert.ToInt32(advancedNumOfRoundsMin.text) > Convert.ToInt32(advancedNumOfRounds.text))
            {
                validinputs = false;
            }
            if (Convert.ToInt32(expertNumOfRoundsMin.text) > Convert.ToInt32(expertNumOfRounds.text))
            {
                validinputs = false;
            }
            if (Convert.ToInt32(masterNumOfRoundsMin.text) > Convert.ToInt32(masterNumOfRounds.text))
            {
                validinputs = false;
            }
            if (Convert.ToInt32(eliteNumOfRoundsMin.text) > Convert.ToInt32(eliteNumOfRounds.text))
            {
                validinputs = false;
            }
            if (Convert.ToInt32(legendaryNumOfRoundsMin.text) > Convert.ToInt32(legendaryNumOfRounds.text))
            {
                validinputs = false;
            }
            return validinputs;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
            return false;
        }
        finally
        {
            Debug.Log("Exit validateInputs()");
        }
    }
    private void playIntroVideo()
    {
        Debug.Log("Enter playIntroVideo()");
        try
        {
            // Subscribe to the video end event
            videoPlayer.loopPointReached += OnVideoFinished;

            // Play the video (optional, if Play on Awake is unchecked)
            videoPlayer.Play();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit playIntroVideo()");
        }
    }
    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Enter OnVideoFinished()");
        try
        {
            videoPlayer.enabled = false;
            ingamenames = true;
            GameStateManager.instance.ChangeToGetNames();

            ppumanager.setAntennaLocation(new int[] { 13 });

            // Start thread to get boardinfo
            StartCoroutine(GetBoardInfoRoutine());
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit OnVideoFinished()");
        }
    }
    private IEnumerator GetBoardInfoRoutine()
    {
        while (true)
        {
            GetBoardInfo();
            yield return new WaitForSeconds(10f);
        }
    }
    private void GetBoardInfo()
    {
        ppumanager.getPPUInfo();
    }
    public void boardInfo(string message)
    {
        Debug.Log("Enter boardInfo() message-" + message);
        try
        {
            BoardInfo boardInfo = JsonUtility.FromJson<BoardInfo>(message);
            
            BoardStatus boardStatus = JsonUtility.FromJson<BoardStatus>(boardInfo.boardinfo);
            
            string batInfo = boardStatus.batteryinfo;
            
            Debug.Log("batInfo-" + batInfo);

            txtBattery.text = batInfo + "%";
            /*if (wifiinfo == 1)
            {
                txtWiFiStatus.text = "WiFi connected";
            }
            else
            {
                txtWiFiStatus.text = "WiFi is not avaliable";
            }
            if (rfidstatus == 0)
            {
                txtRFIDStatus.text = "RFID operational: functioning perfectly";
            }
            else
            {
                txtRFIDStatus.text = "RFID failure: not operational";
            }
            if (boardInfo.bleaddress == null)
            {
                txtBLEStatus.text = "BLE is not ready yet";
            }
            else
            {
                txtBLEStatus.text = boardInfo.bleaddress;
            }*/
            //txtFirmwareVersion.text = boardStatus.firmwareversion;
            //txtHardwareVersion.text = boardStatus.hardwareversion;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit boardInfo()");
        }
    }
    public void StartGameDificulty(GameDificulty gameDificulty)
    {
        Debug.Log("Enter StartGameDificulty()");
        try
        {
            gameengine = new GameEngine(gameDificulty, gamedata);
            if (playersengine == null)
            {
                playersengine = new PlayersEngine();
                int numberOfPlayers = Convert.ToInt32(gamedata.numberOfPlayers);
                for (int x = 0; x < numberOfPlayers; x++)
                {
                    playersengine.addNewPlayer(playersNames[x].text);
                }
            } else
            {
                playersengine.Reset();
            }
            startroundTotalTime = gameengine.timeToPlay;
            startroundCountdownSteps = gameengine.timeToPlay;

            this.gameDificulty = gameDificulty;
            if(this.gameDificulty == GameDificulty.Basic || this.gameDificulty == GameDificulty.Advanced)
            {
                getTrooperRandomally = false;
            } else
            {
                getTrooperRandomally = true;
            }
            if (this.gameDificulty == GameDificulty.Elite || this.gameDificulty == GameDificulty.Legendary)
            {
                trooperDisappear = true;
                trooperObjectToDisapear = new List<object>();
            }
            else
            {
                trooperDisappear = false;
            }
            if (this.gameDificulty == GameDificulty.Advanced || this.gameDificulty == GameDificulty.Master || this.gameDificulty == GameDificulty.Legendary)
            {
                orderTrooperMetter = true;
                detectTagsOrdered = new Dictionary<string, string>();
                detectTagsOrderedScore = new Dictionary<string, string>();
            }
            else
            {
                orderTrooperMetter = false;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit StartGameDificulty()");
        }
    }
    /****************************
     * 
     * 
     * Start Next Level
     * 
     * 
     ***************************/
    public void StartNextLevel()
    {
        Debug.Log("Enter StartNextLevel()");
        try
        {
            StartCoroutine(StartNextLevelCountdown(gameengine.nextGameDificulty));
            //yield return StartCoroutine(GetReadyCountdown());

        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit StartNextLevel()");
        }
    }
    IEnumerator StartNextLevelCountdown(GameDificulty gameDificulty)
    {
        Debug.Log("Enter GetReadyCountdown()");

        float fill = 1f;
        ringMaterial.SetFloat("_Fill", fill);

        float stepTime = getreadyTotalTime / getreadyCountdownSteps;
        float step = 1f / getreadyCountdownSteps;

        // convert seconds → fill threshold
        float warningFill = getreadyLastSecond / getreadyTotalTime;
        ringMaterial.SetFloat("_WarningFill", warningFill);
        countdownText.color = new Color(0f, 1f, 0f);

        for (int i = 0; i < getreadyCountdownSteps; i++)
        {
            while (isPaused)
            {
                yield return null;
            }
            yield return new WaitForSeconds(stepTime);

            countdownCounter--;
            countdownText.text = countdownCounter.ToString();
            if (countdownCounter == getreadyLastSecond)
            {
                countdownText.color = new Color(1f, 0f, 0f);
            }

            fill -= step;
            ringMaterial.SetFloat("_Fill", fill);
        }

        ringMaterial.SetFloat("_Fill", 0f);

        StartGameDificulty(gameengine.nextGameDificulty);
        GameStateManager.instance.ChangeToGatReady(true);

        Debug.Log("Exit GetReadyCountdown()");
    }
    /****************************
     * 
     * 
     * Start Next Level
     * 
     * 
     ***************************/

    /****************************
     * 
     * 
     * Start Get Ready
     * 
     * 
     ***************************/
    public void StartGetReadyNow(bool BeginingOfGame)
    {
        Debug.Log("Enter StartGetReadyNow()");

        tropperStillDetecting = true;
        if (orderTrooperMetter == true)
        {
            detectTagsOrdered.Clear();
            detectTagsOrderedScore.Clear();
        }
        if (trooperDisappear == true)
        {
            trooperObjectToDisapear.Clear();
        }

        boardSetup = new BoardSetup();

        StartCoroutine(StartGetReadyNowFlow(BeginingOfGame));

        Debug.Log("Enter StartGetReadyNow()");
    }
    IEnumerator StartGetReadyNowFlow(bool BeginingOfGame)
    {
        Debug.Log("Enter StartGetReadyNowFlow()");
        bool changeToNextDiffecult = false;
        currentplayer = playersengine.GetNextPlayer();
        if (BeginingOfGame == true)
        {
            Debug.Log("StartGetReadyNowFlow 1");
            currentround = gameengine.GetNextRound();
            AudioManager.instance.playRound(currentround.RoundNumber - 1);
        }
        else if (currentplayer == null)
        {
            Debug.Log("StartGetReadyNowFlow 2");
            currentround = gameengine.GetNextRound();
            if (currentround == null)
            {
                Debug.Log("StartGetReadyNowFlow no more rounds!");
                changeToNextDiffecult = true;
            }
            else
            {
                playersengine.Reset();
                currentplayer = playersengine.GetNextPlayer();
                AudioManager.instance.playRound(currentround.RoundNumber - 1);
            }
        }

        currenttroopers = Troopers.instance.getNextTroppers(currentround.NumberOfTroopers, getTrooperRandomally);

        if (this.gameDificulty == GameDificulty.Basic || this.gameDificulty == GameDificulty.Advanced)
        {
            // Get the next new trooper. Only in Basic or Advanced
            Trooper troopertoplay = currenttroopers[currenttroopers.Count - 1];
            riNextTrooperToUse.texture = troopertoplay.GameObjects[0].GameImage.texture;
        }
        if (changeToNextDiffecult == false)
        {
            Debug.Log("StartGetReadyNowFlow currentround-" + currentround.RoundNumber);
            txtRound.text = currentround.Name;
            txtTrooperNumber.text = txtNumbers[currentround.NumberOfTroopers];
            txtTrooper.text = txtTrooperString[currentround.NumberOfTroopers];
            txtPlayerName.text = currentplayer.Name;
            yield return StartCoroutine(StartGetReadyCountdown());
            GameStateManager.instance.ChangeToPlay();
        } else
        {
            string finishlevel = string.Format("Finish Level {0}, Get Ready For The Next Level - {1}", gameengine.currentGameDificultyName, gameengine.nextGameDificultyName);
            finishLevelMessage.text = finishlevel;
            GameStateManager.instance.ChangeToNextDifficulty();
        }
        Debug.Log("Exit StartGetReadyNowFlow()");
    }
    IEnumerator StartGetReadyCountdown()
    {
        if (getreadyCountDownRoutine != null)
            StopCoroutine(getreadyCountDownRoutine);

        countdownCounter = getreadyCountdownSteps;
        countdownText.text = countdownCounter.ToString();

        getreadyCountDownRoutine = StartCoroutine(GetReadyCountdown());

        yield return getreadyCountDownRoutine;
    }
    IEnumerator GetReadyCountdown()
    {
        Debug.Log("Enter GetReadyCountdown()");

        float fill = 1f;
        ringMaterial.SetFloat("_Fill", fill);

        float stepTime = getreadyTotalTime / getreadyCountdownSteps;
        float step = 1f / getreadyCountdownSteps;

        // convert seconds → fill threshold
        float warningFill = getreadyLastSecond / getreadyTotalTime;
        ringMaterial.SetFloat("_WarningFill", warningFill);
        countdownText.color = new Color(0f, 1f, 0f);

        for (int i = 0; i < getreadyCountdownSteps; i++)
        {
            while (isPaused)
            {
                yield return null;
            }
            yield return new WaitForSeconds(stepTime);
            
            countdownCounter--;
            countdownText.text = countdownCounter.ToString();
            if (countdownCounter == getreadyLastSecond)
            {
                countdownText.color = new Color(1f, 0f, 0f);
            }
            
            fill -= step;
            ringMaterial.SetFloat("_Fill", fill);
        }

        ringMaterial.SetFloat("_Fill", 0f);

        Debug.Log("Exit GetReadyCountdown()");
    }
    /****************************
     * 
     * 
     * Start Get Ready
     * 
     * 
     ***************************/

    /****************************
     * 
     * 
     * Start Round
     * 
     * 
     ***************************/
    public void StartRound()
    {
        Debug.Log("Enter StartRound()");

        noNeedToDetectAgain = false;

        StartCoroutine(StartRoundFlow());

        Debug.Log("Exit StartRound()");
    }
    IEnumerator StartRoundFlow()
    {
        Debug.Log("Enter StartRoundFlow()");

        PlayCurrentMove(currenttroopers);


        yield return StartCoroutine(StartPlayCountdown());

        Debug.Log("Exit StartRoundFlow()");  
    }
    IEnumerator StartPlayCountdown()
    {
        if (playCountDownRoutine != null)
            StopCoroutine(playCountDownRoutine);

        AudioManager.instance.playCountdown();

        countdownCounter = startroundCountdownSteps;
        countdownText.text = countdownCounter.ToString();

        playCountDownRoutine = StartCoroutine(PlayCountdown());

        yield return playCountDownRoutine;
    }
    IEnumerator PlayCountdown()
    {
        Debug.Log("Enter PlayCountdown()");

        float fill = 1f;
        ringMaterial.SetFloat("_Fill", fill);

        float stepTime = startroundTotalTime / startroundCountdownSteps;
        float step = 1f / startroundCountdownSteps;

        // convert seconds → fill threshold
        float warningFill = startroundLastSecond / startroundTotalTime;
        ringMaterial.SetFloat("_WarningFill", warningFill);
        countdownText.color = new Color(0f, 1f, 0f);

        for (int i = 0; i < startroundCountdownSteps; i++)
        {
            while (isPaused)
            {
                yield return null;
            }
            yield return new WaitForSeconds(stepTime);
            
            countdownCounter--;
            countdownText.text = countdownCounter.ToString();
            if (countdownCounter == startroundLastSecond)
            {
                AudioManager.instance.stopCountdown();
                AudioManager.instance.playLastSecondsCountdown();
                countdownText.color = new Color(1f, 0f, 0f);
            }

            // if he puts all troopers the last time recorded will be his time
            currentplayer.timeLeftToPlay = countdownCounter;

            fill -= step;
            ringMaterial.SetFloat("_Fill", fill);
        }

        ringMaterial.SetFloat("_Fill", 0f);
        lock (syncTropperDetection)
        {
            if (tropperStillDetecting == true)
            {
                tropperStillDetecting = false;

                noNeedToDetectAgain = true;
                // Disable all antennas
                ppumanager.setAntennaLocation(new int[] { });

                AudioManager.instance.stopCountdown();
                AudioManager.instance.playFail();

                TagDetectedStatus tagsDetectedStatus = calculateScore(null);
                currentplayer.points += currentscore; // currentround.Points;
                txtRoundPartialPoints.text = "+" + currentscore; // currentround.Points;
                
                setPointsOnTheBoard();

                GameStateManager.instance.ChangeToFailRound();

                unPrintDetectedTrooperOnBoard();
            }
        }
        
        Debug.Log("Exit PlayCountdown()");
    }
    /****************************
     * 
     * 
     * Start Round
     * 
     * 
     ***************************/

    /****************************
     * 
     * 
     * Well Done
     * 
     * 
     ***************************/
    public void StartWellDone()
    {
        Debug.Log("Enter StartWellDone()");
        
        StartCoroutine(StartWellDoneFlow());

        Debug.Log("Exit StartWellDone()");
    }
    IEnumerator StartWellDoneFlow()
    {
        Debug.Log("Enter StartWellDoneFlow()");

        
        yield return StartCoroutine(StartWellDoneCountdown());

        Debug.Log("Exit StartWellDoneFlow()");



    }
    IEnumerator StartWellDoneCountdown()
    {
        if (welldoneCountDownRoutine != null)
            StopCoroutine(welldoneCountDownRoutine);

        welldoneCountDownRoutine = StartCoroutine(WellDoneCountdown());

        yield return welldoneCountDownRoutine;
    }
    IEnumerator WellDoneCountdown()
    {
        Debug.Log("Enter WellDoneCountdown()");

        float fill = 1f;
        ringMaterial.SetFloat("_Fill", fill);

        float stepTime = welldoneTotalTime / welldoneCountdownSteps;
        float step = 1f / welldoneCountdownSteps;

        // convert seconds → fill threshold
        float warningFill = welldoneLastSecond / welldoneTotalTime;
        ringMaterial.SetFloat("_WarningFill", warningFill);
        countdownText.color = new Color(0f, 1f, 0f);

        for (int i = 0; i < welldoneCountdownSteps; i++)
        {
            while (isPaused)
            {
                yield return null;
            }
            yield return new WaitForSeconds(stepTime);
            
            fill -= step;
            ringMaterial.SetFloat("_Fill", fill);
        }

        ringMaterial.SetFloat("_Fill", 0f);
        if (currentplayer.lastPlayer == true)
        {
            List<Player> winnerplayers = playersengine.GetWinnerPlayers();
            string playersnames = null;

            txtBonusPoints.text = "+" + currentround.Bonuspoints;
            foreach (Player winnerplayer in winnerplayers)
            {
                if(playersnames != null)
                {
                    playersnames += ", " + winnerplayer.Name;
                }
                else
                {
                    playersnames = winnerplayer.Name;
                }
                winnerplayer.points += currentround.Bonuspoints;
            }
            if (winnerplayers.Count == 2)
            {
                txtWinsRoundNumber.text = "Round " + currentround.Name + " is a tie!";
            } else
            {
                txtWinsRoundNumber.text = playersnames + " wins " + currentround.Name;
            }
            // Set the points on the board
            setPointsOnTheBoard();
            GameStateManager.instance.ChangeToBonus();
        } else {
            Turns[currentplayer.playerIndex].gameObject.SetActive(false);
            Turns[currentplayer.playerIndex + 1].gameObject.SetActive(true);
            GameStateManager.instance.ChangeToGatReady(false);
        }

        Debug.Log("Exit WellDoneCountdown()");
    }
    /****************************
    * 
    * 
    * Well Done
    * 
    * 
    ***************************/

    /****************************
     * 
     * 
     * Fail Round
     * 
     * 
     ***************************/
    public void StartFailRound()
    {
        Debug.Log("Enter StartFailRound()");

        StartCoroutine(StartFailRoundFlow());

        Debug.Log("Exit StartFailRound()");
    }
    IEnumerator StartFailRoundFlow()
    {
        Debug.Log("Enter StartFailRoundFlow()");


        yield return StartCoroutine(StartFailRoundCountdown());

        Debug.Log("Exit StartFailRoundFlow()");



    }
    IEnumerator StartFailRoundCountdown()
    {
        if (failRoundCountDownRoutine != null)
            StopCoroutine(failRoundCountDownRoutine);

        countdownText.text = countdownCounter.ToString();

        failRoundCountDownRoutine = StartCoroutine(FailRoundCountdown());

        yield return failRoundCountDownRoutine;
    }
    IEnumerator FailRoundCountdown()
    {
        Debug.Log("Enter FailRoundCountdown()");

        float fill = 1f;
        ringMaterial.SetFloat("_Fill", fill);

        float stepTime = failRoundTotalTime / failRoundCountdownSteps;
        float step = 1f / failRoundCountdownSteps;

        // convert seconds → fill threshold
        float warningFill = failRoundLastSecond / failRoundTotalTime;
        ringMaterial.SetFloat("_WarningFill", warningFill);
        countdownText.color = new Color(0f, 1f, 0f);

        for (int i = 0; i < failRoundCountdownSteps; i++)
        {
            while (isPaused)
            {
                yield return null;
            }
            yield return new WaitForSeconds(stepTime);

            fill -= step;
            ringMaterial.SetFloat("_Fill", fill);
        }

        ringMaterial.SetFloat("_Fill", 0f);
        if (currentplayer.lastPlayer == true)
        {
            // Set the points on the board
            setPointsOnTheBoard();
            GameStateManager.instance.ChangeToBonus();
        }
        else
        {
            Turns[currentplayer.playerIndex].gameObject.SetActive(false);
            Turns[currentplayer.playerIndex + 1].gameObject.SetActive(true);
            GameStateManager.instance.ChangeToGatReady(false);
        }
        Debug.Log("Exit FailRoundCountdown()");
    }
    /****************************
    * 
    * 
    * Fail Round
    * 
    * 
    ***************************/

    /****************************
    * 
    * 
    * Bonus
    * 
    * 
    ***************************/
    public void StartBonus()
    {
        Debug.Log("Enter StartBonus()");
        StartCoroutine(StartBonusFlow());
        Debug.Log("Exit StartBonus()");
    }
    IEnumerator StartBonusFlow()
    {
        Debug.Log("Enter StartBonusFlow()");


        yield return StartCoroutine(StartBonusCountdown());

        Debug.Log("Exit StartBonusFlow()");



    }
    IEnumerator StartBonusCountdown()
    {
        if (getbonusCountDownRoutine != null)
            StopCoroutine(getbonusCountDownRoutine);

        getbonusCountDownRoutine = StartCoroutine(BonusCountdown());

        yield return getbonusCountDownRoutine;
    }
    IEnumerator BonusCountdown()
    {
        Debug.Log("Enter BonusCountdown()");

        float fill = 1f;
        ringMaterial.SetFloat("_Fill", fill);

        float stepTime = bonusTotalTime / bonusCountdownSteps;
        float step = 1f / bonusCountdownSteps;

        // convert seconds → fill threshold
        float warningFill = bonusLastSecond / bonusTotalTime;
        ringMaterial.SetFloat("_WarningFill", warningFill);
        countdownText.color = new Color(0f, 1f, 0f);

        for (int i = 0; i < bonusCountdownSteps; i++)
        {
            while (isPaused)
            {
                yield return null;
            }
            yield return new WaitForSeconds(stepTime);
            
            fill -= step;
            ringMaterial.SetFloat("_Fill", fill);
        }

        ringMaterial.SetFloat("_Fill", 0f);
        Turns[0].gameObject.SetActive(true);
        Turns[Convert.ToInt32(gamedata.numberOfPlayers) - 1].gameObject.SetActive(false);
        GameStateManager.instance.ChangeToGatReady(false);
        Debug.Log("Exit BonusCountdown()");
    }
    /****************************
    * 
    * 
    * Bonus
    * 
    * 
    ***************************/
    /****************************
    * 
    * 
    * Start the disapear of troppers
    * 
    * 
    ***************************/
    public void StartDisapearTropper()
    {
        Debug.Log("Enter StartDisapearTropper()");

        StartCoroutine(StartDisapearTropperFlow());

        Debug.Log("Exit StartDisapearTropper()");
    }
    IEnumerator StartDisapearTropperFlow()
    {
        Debug.Log("Enter StartDisapearTropperFlow()");

        yield return StartCoroutine(StartDisapearTropperCountdown());

        Debug.Log("Exit StartDisapearTropperFlow()");
    }
    IEnumerator StartDisapearTropperCountdown()
    {
        if (disapearTropperCountDownRoutine != null)
            StopCoroutine(disapearTropperCountDownRoutine);

        disapearTropperCountDownRoutine = StartCoroutine(DisapearTropperCountdown());

        yield return disapearTropperCountDownRoutine;
    }
    IEnumerator DisapearTropperCountdown()
    {
        Debug.Log("Enter DisapearTropperCountdown()");
        
        for (int i = 0; i < gameengine.currentTimeToDisappear; i++)
        {
            while (isPaused)
            {
                yield return null;
            }
            yield return new WaitForSeconds(1);
        }

        foreach(object trooperObjectToDisapear in trooperObjectToDisapear)
        {
            if(trooperObjectToDisapear is GameObject)
            {
                ((GameObject)trooperObjectToDisapear).SetActive(false);
            } else
            {
                ((TextMeshProUGUI)trooperObjectToDisapear).gameObject.SetActive(false);
            }
        }

        Debug.Log("Exit DisapearTropperCountdown()");
    }
    /****************************
    * 
    * 
    * Start the disapear of troppers
    * 
    * 
    ***************************/

    public bool SetPlayerNames()
    {
        Debug.Log("Enter SetPlayerNames()");
        try
        {
            if (emulator == true)
            {
                currenttropperset = 1;
            }
            for (int x = 0; x < GameEngine.maxNumberOfPlayers; x++)
            {
                bannerPlayersNames[x].gameObject.SetActive(false);
                bannerPlayersPoints[x].gameObject.SetActive(false);
                Harts[x].gameObject.SetActive(false);
                Stars[x].gameObject.SetActive(false);
            }

            int numberOfPlayers = Convert.ToInt32(gamedata.numberOfPlayers);
            for (int x = 0; x < numberOfPlayers; x++)
            {
                bannerPlayersNames[x].text = playersNames[x].text;
                bannerPlayersNames[x].gameObject.SetActive(true);
                bannerPlayersPoints[x].gameObject.SetActive(true);
                if (competitiveMode == false)
                {
                    Harts[x].gameObject.SetActive(true);
                }
                Stars[x].gameObject.SetActive(true);
            }
            
            demoscreenPlayer1Name.text = playersNames[0].text;
            demoscreenPlayer2Name.text = playersNames[1].text;

            playerdata.player1Name = playersNames[0].text;
            playerdata.player2Name = playersNames[1].text;
            playerdata.player3Name = playersNames[2].text;
            playerdata.player4Name = playersNames[3].text;

            playerdata.currenttropperset = currenttropperset;
            ingamenames = false;

            SavePlayerData();
            Debug.Log("currenttropperset - " + currenttropperset);
            if (currenttropperset == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        finally
        {
            Debug.Log("Exit SetPlayerNames()");
        }
    }
    private void PlayCurrentMove(List<Trooper> troopers)
    {
        Debug.Log("Enter PlayCurrentMove()");
        try
        {
            bool continueloop = false;
            do
            {
                nextTiles = boardSetup.getNextTiles(troopers.Count);
                Debug.Log("nextTiles-" + nextTiles.Count);
                int trooperX = 0;
                troppermanagerUI.clearTrooperSetup();
                foreach (Tile tile in nextTiles)
                {
                    Trooper troopertoplay = troopers[trooperX];
                    trooperX++;
                    troppermanagerUI.addTrooperSetup(troopertoplay, tile, 0);
                }

                // Check that troopers selected now on antenna doesnt match the previouse layout
                continueloop = troppermanagerPreviouseUI.checkSameLayout(troppermanagerUI);
                if(continueloop == true)
                {
                    Debug.Log("---------------------------- LOOPING TO FIND NEW ANTENNA ---------------------------------------");
                }
            } while (continueloop);
            troppermanagerPreviouseUI.copyFrom(troppermanagerUI);
            printTrooperOnBoard();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit PlayCurrentMove()");
        }
    }
    private void PlayTags(PPUData ppudata)
    {
        Debug.Log("Enter PlayTags()");
        try
        {
            troppermanagerPPU.clearTrooperSetup();
            TagDetectedStatus tagsDetectedCorrectly = TagDetectedStatus.NotSameNumberOfTags;
            foreach (PPUTag pputag in ppudata.pputags)
            {
                Debug.Log("pputag.id-" + pputag.id + " on antenna-" + pputag.antenna);
                tagsDetectedCorrectly = tagDetectedCorrectly(pputag);

                Tile tile = boardSetup.getAntennaTile(pputag.antenna);
                Trooper trooper = Troopers.instance.getTrooperByTagID(pputag.id);

                if (tagsDetectedCorrectly == TagDetectedStatus.SameTag || tagsDetectedCorrectly == TagDetectedStatus.SameTagSameOrder)
                {
                    troppermanagerPPU.addTrooperSetup(trooper, tile, 0);
                }

                if (orderTrooperMetter == true)
                {
                    if (detectTagsOrdered.ContainsKey(pputag.id) == false)
                    {
                        if (tagsDetectedCorrectly == TagDetectedStatus.SameTagSameOrder)
                        {   
                            detectTagsOrdered.Add(pputag.id, pputag.id);
                        }
                    }
                    if (detectTagsOrderedScore.ContainsKey(pputag.id) == false)
                    {
                        detectTagsOrderedScore.Add(pputag.id, pputag.id);
                    }
                }
            }
            printDetectedTrooperOnBoard();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit PlayTags()");
        }
    }
    private TagDetectedStatus calculateScore(PPUData ppudata)
    {
        Debug.Log("Enter calculateScore()");
        try
        {
            currentscore = (troppermanagerPPU.tropperssetup.Count * 10);
            TagDetectedStatus tagsDetectedCorrectly = TagDetectedStatus.NotSameNumberOfTags;
            if (ppudata != null)
            {
                Debug.Log("troppermanagerUI.tropperssetup.Count-" + troppermanagerUI.tropperssetup.Count.ToString() + "  ppudata.pputags.Length-" + ppudata.pputags.Length.ToString());
                if (troppermanagerUI.tropperssetup.Count != ppudata.pputags.Length)
                {
                    tagsDetectedCorrectly = TagDetectedStatus.NotSameNumberOfTags;
                }
                else
                {
                    Dictionary<int, TrooperSetup> tropperssetupUI = troppermanagerUI.tropperssetup;
                    if (orderTrooperMetter == true)
                    {
                        if (detectTagsOrdered.Count == detectTagsOrderedScore.Count)
                        {
                            tagsDetectedCorrectly = TagDetectedStatus.SameTagSameOrder;
                            int i = 0;
                            foreach (string key in detectTagsOrdered.Keys)
                            {
                                if (key.CompareTo(detectTagsOrderedScore.Keys.ToArray<string>()[i]) != 0)
                                {
                                    tagsDetectedCorrectly = TagDetectedStatus.SameTagDifferentOrder;
                                    break;
                                }
                                i++;
                            }
                        }
                    }
                    else
                    {
                        Dictionary<int, TrooperSetup> tropperssetupPPU = troppermanagerPPU.tropperssetup;
                        if (tropperssetupPPU.Count == tropperssetupUI.Count)
                        {
                            tagsDetectedCorrectly = TagDetectedStatus.SameTag;
                        }
                        else
                        {
                            tagsDetectedCorrectly = TagDetectedStatus.DifferentTag;
                        }
                    }
                }
            }
            return tagsDetectedCorrectly;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
            throw e;
        }
        finally
        {
            Debug.Log("Exit calculateScore()");
        }
    }
    private TagDetectedStatus tagDetectedCorrectly(PPUTag pputag)
    {
        Debug.Log("Enter tagDetectedCorrectly()");
        try
        {
            TagDetectedStatus tagsDetectedCorrectly = TagDetectedStatus.NotSameNumberOfTags;
            Debug.Log("pputag.id-" + pputag.id + " " + pputag.antenna);
            // Get the current active object on the tile
            Trooper trooperCurrentlyOnAntenna = troppermanagerUI.getTrooperOnAntenna(pputag);
            if (trooperCurrentlyOnAntenna == null)
            {
                tagsDetectedCorrectly = TagDetectedStatus.DifferentTag;
            }
            else
            {
                tagsDetectedCorrectly = TagDetectedStatus.SameTag;
                // Check if by correct order
                if (orderTrooperMetter == true)
                {
                    Debug.Log("gilgil");
                    tagsDetectedCorrectly = TagDetectedStatus.SameTagSameOrder;
                    //Debug.Log("Number of troppers-" + detectTagsOrdered.Values.ToList<string>().Count + " index-" + (trooperCurrentlyOnAntenna.orderIndex - 1));
                    //string tafidintheorder = detectTagsOrdered.Values.ToList<string>()[trooperCurrentlyOnAntenna.orderIndex - 1];
                    Debug.Log("---------------------------------------------------------");
                    Debug.Log("detectTagsOrdered.Count-" + detectTagsOrdered.Count.ToString() + " trooperCurrentlyOnAntenna.orderIndex-" + trooperCurrentlyOnAntenna.orderIndex.ToString());
                    Debug.Log("---------------------------------------------------------");
                    if (detectTagsOrdered.Count != trooperCurrentlyOnAntenna.orderIndex - 1)
                    //if (tafidintheorder.CompareTo(pputag.id) != 0)
                    {
                        tagsDetectedCorrectly = TagDetectedStatus.SameTagDifferentOrder;
                    }
                }
            }
            Debug.Log("tagsDetectedCorrectly - " + tagsDetectedCorrectly.ToString());
            
            return tagsDetectedCorrectly;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
            throw e;
        }
        finally
        {
            Debug.Log("Exit tagDetectedCorrectly()");
        }
    }
    private void printTrooperOnBoard()
    {
        Debug.Log("Enter printTrooperOnBoard()");
        try
        {
            Dictionary<int, TrooperSetup> tropperssetupUI = troppermanagerUI.tropperssetup;
            int[] antennaLocations = new int[tropperssetupUI.Keys.Count];
            int x = 0;
            foreach (int antenna in tropperssetupUI.Keys)
            {
                antennaLocations[x] = antenna;
                x++;
                Debug.Log("Using antenna - " + antenna.ToString());
                TrooperSetup troppersetup = tropperssetupUI[antenna];
                Trooper troopertoplay = troppersetup.trooper;
                troopertoplay.orderText = null;
                Tile tile = troppersetup.tile;

                RectTransform rectTransform = null;

                GameObject trooperObjectToPrint = troopertoplay.GameObjects[troppersetup.side].GameImage.gameObject;
                trooperObjectToPrint.SetActive(true);
                rectTransform = troopertoplay.GameObjects[troppersetup.side].GameImage.GetComponent<RectTransform>();

                rectTransform.anchoredPosition = tile.location;
                rectTransform.sizeDelta = new Vector2(200f, 200f);

                TextMeshProUGUI orderText = null;
                if (orderTrooperMetter == true)
                {
                    troopertoplay.orderIndex = x;
                    Vector2 anchoredPosition = rectTransform.anchoredPosition;
                    orderText = CreateUITextAt(anchoredPosition, x.ToString(), 168f, new Color(255, 255, 0));
                    troopertoplay.orderText = orderText;
                }
                if(trooperDisappear == true)
                {
                    if (orderText != null) {
                        trooperObjectToDisapear.Add(orderText);
                     }
                    trooperObjectToDisapear.Add(trooperObjectToPrint);
                }
            }
            // Start the dispear countdown
            if (trooperDisappear == true)
            {
                StartDisapearTropper();
            }
            ppumanager.setAntennaLocation(antennaLocations);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally {
            Debug.Log("Exit printTrooperOnBoard()");
        }
    }
    private void printDetectedTrooperOnBoard()
    {
        Debug.Log("Enter printDetectedTrooperOnBoard()");
        try
        {
            Dictionary<int, TrooperSetup> tropperssetupUI = troppermanagerUI.tropperssetup;
            Dictionary<int, TrooperSetup> tropperssetupPPU = troppermanagerPPU.tropperssetup;
            Debug.Log("tropperssetupPPU.Keys.Count-" + tropperssetupPPU.Keys.Count.ToString());
            
            foreach (int antenna in tropperssetupPPU.Keys)
            {
                Debug.Log("------Detected trooper on antenna - " + antenna);
                TrooperSetup troppersetup;
                if (tropperssetupUI.TryGetValue(antenna, out troppersetup) == true)
                {
                    Trooper troopertoplay = troppersetup.trooper;
                    Tile tile = troppersetup.tile;
                    RectTransform rectTransform = null;
                    troopertoplay.GameObjects[troppersetup.side].GameImage.gameObject.SetActive(false);
                    troopertoplay.GameObjects[troppersetup.side].GameImageTransparent.gameObject.SetActive(true);
                    rectTransform = troopertoplay.GameObjects[troppersetup.side].GameImageTransparent.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = tile.location;
                    rectTransform.sizeDelta = new Vector2(200f, 200f);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit printDetectedTrooperOnBoard()");
        }
    }
    private void unPrintDetectedTrooperOnBoard()
    {
        Debug.Log("Enter unPrintDetectedTrooperOnBoard()");
        try
        {
            Dictionary<int, TrooperSetup> tropperssetupUI = troppermanagerUI.tropperssetup;
            foreach (int antenna in tropperssetupUI.Keys)
            {
                TrooperSetup troppersetup;
                if (tropperssetupUI.TryGetValue(antenna, out troppersetup) == true)
                {
                    Trooper troopertoplay = troppersetup.trooper;
                    Tile tile = troppersetup.tile;
                    troopertoplay.GameObjects[troppersetup.side].GameImage.gameObject.SetActive(false);
                    troopertoplay.GameObjects[troppersetup.side].GameImageTransparent.gameObject.SetActive(false);
                    if(troopertoplay.orderText != null) {
                        troopertoplay.orderText.gameObject.SetActive(false);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit unPrintDetectedTrooperOnBoard()");
        }
    }
    public TextMeshProUGUI CreateUITextAt(Vector2 anchoredPosition, string initialText, float fontSize = 36f, Color? color = null)
    {
        // Create empty GameObject
        GameObject textObj = new GameObject("Dynamic TMP Text");

        // Parent it to the canvas (important for UI scaling & sorting)
        textObj.transform.SetParent(troopersUI.transform, false);

        // Add & get the UI TextMeshPro component
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();

        // Basic setup
        tmp.text = initialText;
        tmp.fontSize = fontSize;
        tmp.color = color ?? Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        //tmp.enableWordWrapping = false;           // or true - your choice

        // Optional: use custom font if you assigned one
        //if (fontAsset != null)
        //    tmp.font = fontAsset;

        // Position it (anchored position in canvas space)
        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = new Vector2(400, 100);   // width × height — adjust as needed

        return tmp;
    }
}
