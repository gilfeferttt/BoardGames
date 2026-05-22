using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;
using System.Collections.Generic;

public class BoardCoOp : BoardBase
{
    public enum GameDificulty
    {
        None,
        Basic,
        Master,
        Legendary
    }



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

    public enum TagDetectedStatus
    {
        NotSameNumberOfTags = 0,
        DifferentTag = 1,
        SameTag = 2,
        SameTagDifferentOrder = 3,
        SameTagSameOrder = 4
    }
    
    //GameDificulty gameDificulty;

    //Troopers troopers;
    BoardSetupCoOp boardSetup;
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

    //public static Board instance;
    //public VideoPlayer videoPlayer;

    [SerializeField]
    private TMP_InputField[] playersNames;

    //[SerializeField] private TMP_Text[] bannerPlayersNames;
    //[SerializeField] private RawImage[] Turns;
    //[SerializeField] private TMP_Text[] bannerPlayersPoints;
    //[SerializeField] private RawImage[] Harts;
    //s[SerializeField] private RawImage[] Stars;

    [SerializeField] private TMP_Text demoscreenPlayer1Name;
    [SerializeField] private TMP_Text demoscreenPlayer2Name;

    [SerializeField] private TMP_Text txtBattery;

    //private PlayerData playerdata;
    //private string playerfilePath;
    private PPUManager ppumanager;

   // private GameData gamedata;
    //private string gamefilePath;



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
    //GameEngine gameengine;
    //PlayersEngine playersengine;
    List<Trooper> currenttroopers;
    GameRound currentround = null;
    Player currentplayer = null;
    List<string> txtNumbers = new List<string> { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten" };
    List<string> txtTrooperString = new List<string> { "Troopers", "Trooper", "Troopers", "Troopers", "Troopers", "Troopers", "Troopers", "Troopers", "Troopers", "Troopers", "Troopers" };

    [SerializeField] private TMP_Text txtSimulatorLeft;
    [SerializeField] private TMP_Text txtSimulatorRight;

    [SerializeField] private Button btnSimulatorTag;

    private bool noNeedToDetectAgain = true;


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
    //[SerializeField] private TMP_InputField advancedWaitTime;
    //[SerializeField] private TMP_InputField expertWaitTime;
    [SerializeField] private TMP_InputField masterWaitTime;
    //[SerializeField] private TMP_InputField eliteWaitTime;
    [SerializeField] private TMP_InputField legendaryWaitTime;

    [SerializeField] private TMP_InputField basicNumOfRounds;
    //[SerializeField] private TMP_InputField advancedNumOfRounds;
    //[SerializeField] private TMP_InputField expertNumOfRounds;
    [SerializeField] private TMP_InputField masterNumOfRounds;
    //[SerializeField] private TMP_InputField eliteNumOfRounds;
    [SerializeField] private TMP_InputField legendaryNumOfRounds;

    [SerializeField] private TMP_InputField basicNumOfRoundsMin;
    //[SerializeField] private TMP_InputField advancedNumOfRoundsMin;
    //[SerializeField] private TMP_InputField expertNumOfRoundsMin;
    [SerializeField] private TMP_InputField masterNumOfRoundsMin;
    //[SerializeField] private TMP_InputField eliteNumOfRoundsMin;
    [SerializeField] private TMP_InputField legendaryNumOfRoundsMin;

   // [SerializeField] private TMP_InputField eliteTimeToDisappear;
    [SerializeField] private TMP_InputField legendaryTimeToDisappear;

    [SerializeField] private TMP_Text isTagDetected;

    [SerializeField] private TMP_InputField numberOfPlayers;

    [SerializeField] private TMP_Text finishLevelMessage;

    [SerializeField] private TMP_Text txtVersion;
    [SerializeField] private TMP_Text txtBuild;

    [SerializeField] private RawImage riNextTrooperToUse;

    int currentRoundScore;
    int orderTrooperMetterNextAntenna;
    int[] currentAntennaLocations;
    string lastDetectedMessage;
    


























    public static BoardCoOp instance;

    [SerializeField] private TMP_Text bannerTeamName;
    //[SerializeField] private RawImage Turns;
    [SerializeField] private TMP_Text bannerTeamPoints;
    //[SerializeField] private RawImage Harts;
   // [SerializeField] private RawImage Stars;

    private int numberofteammember = 0;
    [SerializeField] private Slider sliderNumberOfTeamMember;
    [SerializeField] private TMP_Text txtNumberOfTeamMember;
    [SerializeField] private TMP_InputField teamName;

    //[SerializeField] private TMP_InputField basicWaitTime;
    //[//SerializeField] private TMP_InputField masterWaitTime;
    //[SerializeField] private TMP_InputField legendaryWaitTime;
    //[SerializeField] private TMP_InputField basicNumOfRounds;
    //[SerializeField] private TMP_InputField masterNumOfRounds;
    //[SerializeField] private TMP_InputField legendaryNumOfRounds;
    //[SerializeField] private TMP_InputField basicNumOfRoundsMin;
    //[SerializeField] private TMP_InputField masterNumOfRoundsMin;
    //[SerializeField] private TMP_InputField legendaryNumOfRoundsMin;
    //[SerializeField] private TMP_InputField legendaryTimeToDisappear;

    GameDificulty gameDificulty;
    //int startroundCountdownSteps = 7;
    //List<object> trooperObjectToDisapear;




    //float startroundTotalTime = 7;
    //
    
    GameEngineCoOp gameengine;
    PlayersEngine playersengine;
    //[SerializeField] protected GameObject troopersUI;
   // bool getTrooperRandomally = false;
   // bool orderTrooperMetter = false;
   // bool trooperDisappear = false;
  //  Dictionary<string, string> detectTagsOrdered;
  //  Dictionary<string, string> detectTagsOrderedScore;

  //  bool ingamenames = false;
  //  int currenttropperset = 0;

    

    private GameDataCoOp gamedata;
    private string gamefilePath;
    
    private PlayerDataCoOp playerdata;
    private string playerfilePath;

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
    // Number of player on a team
    private void OnTeamMemberChanged(float value)
    {
        Debug.Log("Enter OnTeamMemberChanged()");
        try
        {
            txtNumberOfTeamMember.text = value.ToString() + " players";
            numberofteammember = ((int)value);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit OnTeamMemberChanged()");
        }
    }
    // Use this for initialization
    void Start()
    {
        Debug.Log("Enter Start() BoardCoOp");
        try
        {
            lastDetectedMessage = null;

            base.Start();

            if (competitiveMode == true)
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
            if (ppumanager.boardConnectStatus == 0)
            {
                Debug.Log("Board is connected");
            }
            else if (ppumanager.boardConnectStatus == 1)
            {
                Debug.Log("Board runs in emulation mode");
                base.emulator = true;
            }
            else
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
                sliderNumberOfTeamMember.onValueChanged.AddListener(OnTeamMemberChanged);

            playerfilePath = Path.Combine(Application.persistentDataPath, "playerdatacoop.json");
            //gamefilePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
            gamefilePath = Path.Combine(Application.persistentDataPath, "gamedatacoop.json");

            LoadPlayerData();
            LoadGameData();

            Debug.Log("numberofteammemebers - " + playerdata.numberofteammemebers);
            teamName.text = playerdata.teamName;
            sliderNumberOfTeamMember.value = playerdata.numberofteammemebers;
            }
            else
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
    private void setPointsOnTheBoard()
    {
        List<Player> players = playersengine.players;
        for (int x = 0; x < players.Count; x++)
        {
            //bannerPlayersPoints[x].text = players[x].points.ToString();
        }
    }
    private void LoadPlayerData()
    {
        playerdata = new PlayerDataCoOp();  // Default fresh data

        if (File.Exists(playerfilePath))
        {
            try
            {
                string json = File.ReadAllText(playerfilePath);
                playerdata = JsonUtility.FromJson<PlayerDataCoOp>(json);

                // Validate loaded data (optional safety)
                if (string.IsNullOrEmpty(playerdata.teamName))
                    playerdata.teamName = "Guest Team";
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
    }
    private void LoadGameData()
    {
        Debug.Log("Enter LoadCoOpGameData()");
        try
        {
            gamedata = new GameDataCoOp();

            if (File.Exists(gamefilePath))
            {
                try
                {
                    string json = File.ReadAllText(gamefilePath);
                    gamedata = JsonUtility.FromJson<GameDataCoOp>(json);
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
            if (string.IsNullOrEmpty(gamedata.masterWaitTime))
                gamedata.masterWaitTime = GameEngine.masterWaitTime.ToString();
            if (string.IsNullOrEmpty(gamedata.legendaryWaitTime))
                gamedata.legendaryWaitTime = GameEngine.legendaryWaitTime.ToString();

            if (string.IsNullOrEmpty(gamedata.basicNumOfRounds))
                gamedata.basicNumOfRounds = GameEngine.basicNumOfRounds.ToString();
            if (string.IsNullOrEmpty(gamedata.masterNumOfRounds))
                gamedata.masterNumOfRounds = GameEngine.masterNumOfRounds.ToString();
            if (string.IsNullOrEmpty(gamedata.legendaryNumOfRounds))
                gamedata.legendaryNumOfRounds = GameEngine.legendaryNumOfRounds.ToString();

            if (string.IsNullOrEmpty(gamedata.basicNumOfRoundsMin))
                gamedata.basicNumOfRoundsMin = GameEngine.basicNumOfRoundsMin.ToString();
            if (string.IsNullOrEmpty(gamedata.masterNumOfRoundsMin))
                gamedata.masterNumOfRoundsMin = GameEngine.masterNumOfRoundsMin.ToString();
            if (string.IsNullOrEmpty(gamedata.legendaryNumOfRoundsMin))
                gamedata.legendaryNumOfRoundsMin = GameEngine.legendaryNumOfRoundsMin.ToString();

            if (string.IsNullOrEmpty(gamedata.legendaryTimeToDisappear))
                gamedata.legendaryTimeToDisappear = GameEngine.legendaryTimeToDisappear.ToString();

            basicWaitTime.text = gamedata.basicWaitTime.ToString();
            masterWaitTime.text = gamedata.masterWaitTime.ToString();
            legendaryWaitTime.text = gamedata.legendaryWaitTime.ToString();

            basicNumOfRounds.text = gamedata.basicNumOfRounds.ToString();
            masterNumOfRounds.text = gamedata.masterNumOfRounds.ToString();
            legendaryNumOfRounds.text = gamedata.legendaryNumOfRounds.ToString();

            basicNumOfRoundsMin.text = gamedata.basicNumOfRoundsMin.ToString();
            masterNumOfRoundsMin.text = gamedata.masterNumOfRoundsMin.ToString();
            legendaryNumOfRoundsMin.text = gamedata.legendaryNumOfRoundsMin.ToString();

            legendaryTimeToDisappear.text = gamedata.legendaryTimeToDisappear.ToString();
        }
        finally
        {
            Debug.Log("Exit LoadCoOpGameData()");
        }
    }
    public bool SaveSettings()
    {
        Debug.Log("Enter SaveCoOpSettings()");
        try
        {
            if (validateInputs() == true)
            {
                gamedata.basicWaitTime = basicWaitTime.text;
                gamedata.masterWaitTime = masterWaitTime.text;
                gamedata.legendaryWaitTime = legendaryWaitTime.text;

                gamedata.basicNumOfRounds = basicNumOfRounds.text;
                gamedata.masterNumOfRounds = masterNumOfRounds.text;
                gamedata.legendaryNumOfRounds = legendaryNumOfRounds.text;

                gamedata.basicNumOfRoundsMin = basicNumOfRoundsMin.text;
                gamedata.masterNumOfRoundsMin = masterNumOfRoundsMin.text;
                gamedata.legendaryNumOfRoundsMin = legendaryNumOfRoundsMin.text;

                gamedata.legendaryTimeToDisappear = legendaryTimeToDisappear.text;

                string json = JsonUtility.ToJson(gamedata, true);
                File.WriteAllText(gamefilePath, json);

                return true;
            }
            else
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
            Debug.Log("Exit SaveCoOpSettings()");
        }
    }
    private bool validateInputs()
    {
        bool validinputs = true;
        Debug.Log("Enter validateCoOpInputs()");
        try
        {
            if (Convert.ToInt32(legendaryTimeToDisappear.text) >= Convert.ToInt32(legendaryWaitTime.text))
            {
                validinputs = false;
            }
            if (Convert.ToInt32(basicNumOfRoundsMin.text) > Convert.ToInt32(basicNumOfRounds.text))
            {
                validinputs = false;
            }
            if (Convert.ToInt32(masterNumOfRoundsMin.text) > Convert.ToInt32(masterNumOfRounds.text))
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
            Debug.Log("Exit validateCoOpInputs()");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool SetTeamMembersNames()
    {
        Debug.Log("Enter SetTeamMembersNames() teamName.text-" + teamName.text);
        try
        {
            if (emulator == true)
            {
                currenttropperset = 1;
            }

            bannerTeamName.text = teamName.text;
            /*int numberOfPlayers = Convert.ToInt32(gamedata.numberOfPlayers);
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
            */
            playerdata.numberofteammemebers = numberofteammember;
            playerdata.teamName = teamName.text;
            playerdata.currenttropperset = currenttropperset;
            ingamenames = false;

            Debug.Log("playerdata.numberofteammemebers - " + numberofteammember);

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
            Debug.Log("Exit SetTeamMembersNames()");
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
    public void StartGameDificulty(GameDificulty gameDificulty)
    {
        Debug.Log("Enter StartGameDificulty()");
        try
        {
            Debug.Log("1");
            gameengine = new GameEngineCoOp(gameDificulty, gamedata);
            Debug.Log("11");
            if (playersengine == null)
            {
                Debug.Log("3");
                playersengine = new PlayersEngine();
                Debug.Log("4-" + playerdata.numberofteammemebers);
                int numberOfPlayers = Convert.ToInt32(playerdata.numberofteammemebers);
                Debug.Log("5");
                //for (int x = 0; x < numberOfPlayers; x++)
                //{
                    playersengine.addNewPlayer(teamName.text);
                //}
            }
            else
            {
                playersengine.Reset();
            }
            Debug.Log("2");
            startroundTotalTime = gameengine.timeToPlay;
            startroundCountdownSteps = gameengine.timeToPlay;
            Debug.Log("3");

            gameDificulty = gameDificulty;
            if (this.gameDificulty == GameDificulty.Basic)
            {
                getTrooperRandomally = false;
            }
            else
            {
                getTrooperRandomally = true;
            }
            if (this.gameDificulty == GameDificulty.Legendary)
            {
                trooperDisappear = true;
                trooperObjectToDisapear = new List<object>();
            }
            else
            {
                trooperDisappear = false;
            }
            if (this.gameDificulty == GameDificulty.Master || this.gameDificulty == GameDificulty.Legendary)
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
        GameStateManagerMulti.instance.ChangeToGatReady(true);

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

        boardSetup = new BoardSetupCoOp();

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

        if (this.gameDificulty == GameDificulty.Basic)
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
            txtPlayerName.text = "ttt"; // currentplayer.Name;
            yield return StartCoroutine(StartGetReadyCountdown());
            GameStateManagerCoOp.instance.ChangeToPlay();
        }
        else
        {
            string finishlevel = string.Format("Finish Level {0}, Get Ready For The Next Level - {1}", gameengine.currentGameDificultyName, gameengine.nextGameDificultyName);
            finishLevelMessage.text = finishlevel;
            GameStateManagerCoOp.instance.ChangeToNextDifficulty();
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

        if (orderTrooperMetter == true)
        {
            troppermanagerPPU.clearTrooperSetup();
        }

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
                currentplayer.points += currentRoundScore;
                txtRoundPartialPoints.text = "+" + currentRoundScore;

                setPointsOnTheBoard();

                GameStateManagerMulti.instance.ChangeToFailRound();

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

        foreach (object trooperObjectToDisapear in trooperObjectToDisapear)
        {
            if (trooperObjectToDisapear is GameObject)
            {
                ((GameObject)trooperObjectToDisapear).SetActive(false);
            }
            else
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
                if (continueloop == true)
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
            if (orderTrooperMetter == false)
            {
                troppermanagerPPU.clearTrooperSetup();
            }
            TagDetectedStatus tagsDetectedCorrectly = TagDetectedStatus.NotSameNumberOfTags;
            foreach (PPUTag pputag in ppudata.pputags)
            {
                Debug.Log("pputag.id-" + pputag.id + " on antenna-" + pputag.antenna);
                tagsDetectedCorrectly = tagDetectedCorrectly(pputag);

                Tile tile = boardSetup.getAntennaTile(pputag.antenna);
                Trooper trooper = Troopers.instance.getTrooperByTagID(pputag.id);

                Debug.Log("--------------------addTrooperSetup tagsDetectedCorrectly--------------------" + tagsDetectedCorrectly);
                if (tagsDetectedCorrectly == TagDetectedStatus.SameTag || tagsDetectedCorrectly == TagDetectedStatus.SameTagSameOrder)
                {
                    Debug.Log("--------------------addTrooperSetup--------------------");
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
            currentRoundScore = (troppermanagerPPU.tropperssetup.Count * currentround.Points);
            TagDetectedStatus tagsDetectedCorrectly = TagDetectedStatus.NotSameNumberOfTags;
            if (ppudata != null)
            {
                Debug.Log("troppermanagerUI.tropperssetup.Count-" + troppermanagerUI.tropperssetup.Count.ToString() + "  ppudata.pputags.Length-" + ppudata.pputags.Length.ToString());
                if (orderTrooperMetter == false && troppermanagerUI.tropperssetup.Count != ppudata.pputags.Length)
                {
                    tagsDetectedCorrectly = TagDetectedStatus.NotSameNumberOfTags;
                }
                else
                {
                    Dictionary<int, TrooperSetup> tropperssetupUI = troppermanagerUI.tropperssetup;
                    /*if (orderTrooperMetter == true)
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
                    {*/
                    Dictionary<int, TrooperSetup> tropperssetupPPU = troppermanagerPPU.tropperssetup;
                    Debug.Log("tropperssetupPPU.Count-" + tropperssetupPPU.Count.ToString() + "  tropperssetupUI.Count-" + tropperssetupUI.Count.ToString());
                    if (tropperssetupPPU.Count == tropperssetupUI.Count)
                    {
                        tagsDetectedCorrectly = TagDetectedStatus.SameTag;
                    }
                    else
                    {
                        tagsDetectedCorrectly = TagDetectedStatus.DifferentTag;
                    }
                    //}
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
                    //if (detectTagsOrdered.Count != trooperCurrentlyOnAntenna.orderIndex - 1)
                    //{
                    //    tagsDetectedCorrectly = TagDetectedStatus.SameTagDifferentOrder;
                    //} else
                    //{
                    Debug.Log("currentAntennaLocations-" + currentAntennaLocations.Length);
                    if (orderTrooperMetterNextAntenna < currentAntennaLocations.Length)
                    {
                        Debug.Log("Turn on antenna-" + currentAntennaLocations[orderTrooperMetterNextAntenna].ToString());
                        ppumanager.setAntennaLocation(new int[] { currentAntennaLocations[orderTrooperMetterNextAntenna] });
                        //}
                        orderTrooperMetterNextAntenna++;
                    }
                    else
                    {
                        // Turn off all antennas
                        Debug.Log("Turn off all antennas");
                        ppumanager.setAntennaLocation(new int[] { });
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
            currentAntennaLocations = new int[tropperssetupUI.Keys.Count];
            int x = 0;
            foreach (int antenna in tropperssetupUI.Keys)
            {
                currentAntennaLocations[x] = antenna;
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
                if (trooperDisappear == true)
                {
                    if (orderText != null)
                    {
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

            // Turn on the antennas. In order metters turn on only the nessecery antenna
            if (orderTrooperMetter == false)
            {
                ppumanager.setAntennaLocation(currentAntennaLocations);
            }
            else
            {
                orderTrooperMetterNextAntenna = 0;
                Debug.Log("Turn on antenna-" + currentAntennaLocations[0].ToString());
                ppumanager.setAntennaLocation(new int[] { currentAntennaLocations[orderTrooperMetterNextAntenna] });
                orderTrooperMetterNextAntenna++;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
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
                    if (troopertoplay.orderText != null)
                    {
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
