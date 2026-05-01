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

    List<Tile> nextTiles;
    
    [SerializeField] private TMP_Text countdownText;


    public static Board instance;
    public VideoPlayer videoPlayer;

    [SerializeField]
    private TMP_InputField[] playersNames;
    [SerializeField]
    private TMP_Text[] profilesNames;

    [SerializeField] private RawImage[] profiles;
    [SerializeField] private Toggle[] profilesToggles;

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

    [SerializeField] private TMP_Text txtSimulator;

    //[SerializeField] private Button btnSimulatorTag;
        
    private bool noNeedToDetectAgain = true;
    private bool emulator = true;
    Coroutine maketagdetectCoroutine = null;

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

    [SerializeField] private TMP_Text isTagDetected;

    [SerializeField] private TMP_InputField numberOfPlayers;

    [SerializeField] private TMP_Text finishLevelMessage;

    [SerializeField] private TMP_Text txtVersion;
    [SerializeField] private TMP_Text txtBuild;

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

        numberOfPlayers.characterLimit = 1;
        numberOfPlayers.onValidateInput = ValidateInputNumberOfPlayers;
    }
    private char ValidateInputNumberOfPlayers(string text, int charIndex, char addedChar)
    {
        if (addedChar == '2' || addedChar == '3' || addedChar == '4')
        {
            return addedChar;
        }

        return '\0';
    }
    //private char ValidateInputNumberOfLevels(string text, int charIndex, char addedChar)
    //{
    //    if (addedChar == '0') 
    //    {
    //        return '\0';
    //    }
    ////    return addedChar;
    //}
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
        Debug.Log("Enter Board::Start()");
        try
        {
            VersionData versionData = Resources.Load<VersionData>("VersionData");
            txtVersion.text = versionData.displayVersion;
            txtBuild.text = versionData.buildNumber.ToString();

            // Prevents the screen from dimming / sleeping / turning off
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            //AudioManager.instance.PlaySFX(AudioManager.instance.PlayMusic();
            
            troppermanagerUI = new TroopersManager();
            troppermanagerPreviouseUI = new TroopersManager();
            troppermanagerPPU = new TroopersManager();
            if (emulator == true)
            {
                txtSimulator.gameObject.SetActive(true);
            }
            
            ppumanager = new PPUManager(emulator);
            ppumanager.ConnectToPPU(gameObject.name);
            if(ppumanager.boardConnectStatus == 0)
            {
                Debug.Log("Board is connected");
            }
            else if (ppumanager.boardConnectStatus == 1)
            {
                Debug.Log("Board runs in emulation mode");
            } else
            {
                Debug.Log("Board is not yet connected");
            }
            
            if (ppumanager.boardConnectStatus == 0 || ppumanager.boardConnectStatus == 1)
            {
                playerfilePath = Path.Combine(Application.persistentDataPath, "playerdata1.json");
                gamefilePath = Path.Combine(Application.persistentDataPath, "gamedata1.json");

                //ppumanager.SetRFIDConfiguration(-1);

                LoadGameData();
                LoadPlayerData();

                int x = 0;
                if (playerdata.playersNames != null)
                {
                    foreach (string playername in playerdata.playersNames)
                    {
                        playersNames[x].text = playername;
                        profilesNames[x].text = playername;
                        profilesToggles[x].isOn = playerdata.playersSelected[x];
                        x++;
                    }
                }
                currenttropperset = playerdata.currenttropperset;

                SetProfiles();

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
            Debug.Log("Exit Board::Start()");
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
                /*PlayTags(ppudata);
                TagDetectedStatus tagsDetectedStatus = calculateScore(ppudata);
                if (tagsDetectedStatus == TagDetectedStatus.SameTag || tagsDetectedStatus == TagDetectedStatus.SameTagSameOrder)
                {
                    Debug.Log("Tags are detected correctly");
                    noNeedToDetectAgain = true;

                    // Disable all antennas
                    ppumanager.setAntennaLocation(new int[] {  });

                    if (playCountDownRoutine != null)
                        StopCoroutine(playCountDownRoutine);

                    currentplayer.points += currentround.Points;

                    setPointsOnTheBoard();

                    txtRoundPoints.text = "+" + currentround.Points;
                    lock (syncTropperDetection)
                    {
                        if (tropperStillDetecting == true)
                        {
                            tropperStillDetecting = false;
                            AudioManager.instance.playSuccess();
                            //GameStateManager.instance.ChangeToWellDone();

                            unPrintDetectedTrooperOnBoard();
                        }
                    }
                }*/
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
    /*private void setPointsOnTheBoard()
    {
        List<Player> players = playersengine.players;
        for(int x = 0; x < players.Count; x++)
        {
            //bannerPlayersPoints[x].text = players[x].points.ToString();
        }
    }*/
    private void LoadPlayerData()
    {
        Debug.Log("Enter LoadPlayerData()");
        try
        {
            playerdata = new PlayerData();  // Default fresh data

            if (File.Exists(playerfilePath))
            {
                try
                {
                    string json = File.ReadAllText(playerfilePath);
                    playerdata = JsonUtility.FromJson<PlayerData>(json);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Load failed: {e.Message} - Using defaults");
                }
            } else
            {
                playerdata.playersNames = new string[4];
                Debug.Log("Can't load player data - File does not exist");
            }
        }
        finally
        {
            Debug.Log("Exit LoadPlayerData()");
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
                Debug.Log("Can't load game data - File does not exist");
            }

            if (string.IsNullOrEmpty(gamedata.numberOfPlayers))
                gamedata.numberOfPlayers = GameEngine.numberOfPlayers.ToString();

            numberOfPlayers.text = gamedata.numberOfPlayers.ToString();
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
                gamedata.numberOfPlayers = numberOfPlayers.text;

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
            GameStateManager.instance.ChangeToGameCenter();

            //ppumanager.setAntennaLocation(new int[] { 13 });

            // Start thread to get boardinfo
            //StartCoroutine(GetBoardInfoRoutine());
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
    public void StartTimeTrooper()
    {
        SetPlayerSelected();
        SavePlayerData();
        LaunchAndroidByPackage("com.twintera.timetrooper");
    }
    public void StartMimicStrikeBattle()
    {
        SetPlayerSelected();
        SavePlayerData();
        LaunchAndroidByPackage("com.twintera.mimicstrikebattle");
    }
    private void LaunchAndroidByPackage(string packageName)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", packageName);
        if (launchIntent != null)
            currentActivity.Call("startActivity", launchIntent);
        else
            Debug.LogError("Game not installed: " + packageName);
    }
    public void SetPlayerSelected()
    {
        Debug.Log("Enter SetPlayerSelected()");
        try
        {
            int x = 0;
            playerdata.playersSelected = new bool[4];
            foreach (Toggle toggle in profilesToggles)
            {
                playerdata.playersSelected[x] = toggle.isOn;
                x++;
            }
        }
        finally
        {
            Debug.Log("Exit SetPlayerSelected()");
        }
    }


















    public bool SetPlayerNames()
    {
        Debug.Log("Enter SetPlayerNames()");
        try
        {
            if (emulator == true)
            {
                currenttropperset = 1;
            }
            /*for (int x = 0; x < GameEngine.maxNumberOfPlayers; x++)
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
                Harts[x].gameObject.SetActive(true);
                Stars[x].gameObject.SetActive(true);
            }
            
            demoscreenPlayer1Name.text = playersNames[0].text;
            demoscreenPlayer2Name.text = playersNames[1].text;
            */
            int x = 0;
            foreach(TMP_InputField playername in playersNames)
            {
                playerdata.playersNames[x] = playername.text;
                profilesNames[x].text = playersNames[x].text;
                x++;
            }

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
    public void SetProfiles()
    {
        Debug.Log("Enter SetProfiles()");
        try
        {
            int x = 0;
            foreach (string playername in playerdata.playersNames)
            {
                if(string.IsNullOrEmpty(playername) == true)
                {
                    profiles[x].color = new Color(1, 1, 1, 0.5f);
                    profilesToggles[x].gameObject.SetActive(false);
                } else
                {
                    profiles[x].color = new Color(1, 1, 1, 1);
                    profilesToggles[x].gameObject.SetActive(true);
                }
                x++;
            }

        }
        finally
        {
            Debug.Log("Exit SetProfiles()");
        }
    }

    /*private void PlayCurrentMove(List<Trooper> troopers)
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
            TagDetectedStatus tagsDetectedCorrectly = TagDetectedStatus.NotSameNumberOfTags;
            if (troppermanagerUI.tropperssetup.Count != ppudata.pputags.Length)
            {
                tagsDetectedCorrectly = TagDetectedStatus.NotSameNumberOfTags;
            }
            else
            {
                Dictionary<int, TrooperSetup> tropperssetupUI = troppermanagerUI.tropperssetup;
                if (orderTrooperMetter == true)
                {
                    if(detectTagsOrdered.Count == detectTagsOrderedScore.Count)
                    {
                        tagsDetectedCorrectly = TagDetectedStatus.SameTagSameOrder;
                        int i = 0;
                        foreach(string key in detectTagsOrdered.Keys)
                        {
                            if(key.CompareTo(detectTagsOrderedScore.Keys.ToArray<string>()[i]) != 0)
                            {
                                tagsDetectedCorrectly = TagDetectedStatus.SameTagDifferentOrder;
                                break;
                            }
                            i++;   
                        }
                    }
                } else
                {
                    Dictionary<int, TrooperSetup> tropperssetupPPU = troppermanagerPPU.tropperssetup;
                    if (tropperssetupPPU.Count == tropperssetupUI.Count)
                    {
                        tagsDetectedCorrectly = TagDetectedStatus.SameTag;
                    } else
                    {
                        tagsDetectedCorrectly = TagDetectedStatus.DifferentTag;
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
    }*/
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
    /*
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
    }*/
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
