using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
//using Mono.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BoardGenerator : MonoBehaviour
{
    public float moveSpeed = 5f;

    public static BoardGenerator instance;

    private readonly object lockObject = new object();

    bool emulator = false;


    public TextMeshProUGUI txtWiFiName;
    public TextMeshProUGUI txtWiFiPassword;
    public Button btnExitToWiFiMenu;
    public Button btnExitToMainMenu;

    public TextMeshProUGUI p2vInput;
    public TextMeshProUGUI p2vOutput;

    public Button btnWiFi;
    public TextMeshProUGUI txtWiFi;


    public TextMeshProUGUI txtStatus;

    public TextMeshProUGUI txtBatInfo;
    public TextMeshProUGUI txtWiFiStatus;
    public TextMeshProUGUI txtRFIDStatus;
    public TextMeshProUGUI txtBLEStatus;
    public TextMeshProUGUI txtCpuType;
    public TextMeshProUGUI txtNumOfAntenna;
    public TextMeshProUGUI txtCompass;
    public TextMeshProUGUI txtFirmwareVersion;
    public TextMeshProUGUI txtHardwareVersion;

    private string boardAddress;
    private int connectPPUStatus;

    public VideoPlayer videoPlayer;

    public Toggle tgSendTagsStatusEveryEndOfRound;
    public Toggle tgTurnOffAntennaAfterDetection;
    public Toggle tgSendTagsStatusEveryEndOfRoundWhenChanged;

    public Toggle Power18;
    public Toggle Power18_2;
    public Toggle Power23;
    public Toggle Power23_2;
    public Toggle Power33;
    public Toggle Power38;
    public Toggle Power43;
    public Toggle Power48;

    [SerializeField] public TextMeshProUGUI txtAntennaDetection;
    [SerializeField] public ScrollRect scrollRect;
    [SerializeField] public TMP_InputField txtAntennaToTurnOn;
    public TextMeshProUGUI txtNumberOfTagsDetected;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Enter Start()");
        try
        {
            txtAntennaToTurnOn.textComponent.textWrappingMode = TextWrappingModes.Normal;
            txtAntennaDetection.textWrappingMode = TextWrappingModes.Normal;

            tgSendTagsStatusEveryEndOfRound.onValueChanged.AddListener(OnToggleChanged_SendTagsStatusEveryEndOfRound);
            tgSendTagsStatusEveryEndOfRoundWhenChanged.onValueChanged.AddListener(OnToggleChanged_SendTagsStatusEveryEndOfRoundWhenChanged);

            Power18.onValueChanged.AddListener(OnToggleChanged_Power18);
            Power18_2.onValueChanged.AddListener(OnToggleChanged_Power18_2);
            Power23.onValueChanged.AddListener(OnToggleChanged_Power23);
            Power23_2.onValueChanged.AddListener(OnToggleChanged_Power23_2);

            Power33.onValueChanged.AddListener(OnToggleChanged_Power33);
            Power38.onValueChanged.AddListener(OnToggleChanged_Power38);
            Power43.onValueChanged.AddListener(OnToggleChanged_Power43);
            Power48.onValueChanged.AddListener(OnToggleChanged_Power48);

            connectPPUStatus = 1;
            playIntroVideo();
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
    public void AppendAntennaText(string newText)
    {
        // Append to existing text
        txtAntennaDetection.text = newText + "\n";  // Add newline for multi-line

        // Force layout rebuild
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
        // Scroll to bottom
        scrollRect.verticalNormalizedPosition = 0f;  // 0 = bottom, 1 = top
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
            StartNow();
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void StartNow()
    {
        Debug.Log("Enter StartNow()");
        try
        {
            //ConnectToBackend();
            ReconnectToPPU();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit StartNow()");
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ReconnectToPPU()
    {
        Debug.Log("Enter ReconnectToPPU()");
        try
        {
            if (emulator == false)
            {
                ConnectToPPU();
                if (connectPPUStatus != 0)
                {
                    GameStateManager.instance.ChangeState(GameState.BoardNotFound);
                }
                else
                {
                    GameStateManager.instance.ChangeState(GameState.BoardFound);
                }
            }
            else
            {
                GameStateManager.instance.ChangeState(GameState.BoardFound);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ReconnectToPPU()");
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    /*private async void ConnectToBackend()
    {
        // Check Firebase dependency
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            // Initialize Firebase
            firebaseApp = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.DefaultInstance;
            Debug.Log("Firebase initialized successfully!");

            // Example: Add a document
            //await AddExampleDocument();

            // Example: Read a document
            //await ReadExampleDocument();
        }
        else
        {
            Debug.LogError("Firebase dependencies failed: " + dependencyStatus);
            // Handle error (e.g., show UI message)
        }
    }*/

    public void setWiFiConfiguration()
    {
        Debug.Log("Enter setWiFiConfiguration1()");
        try
        {
            string wifiname = txtWiFiName.text;
            string wifipassword = txtWiFiPassword.text;
            wifiname = CleanInputText(wifiname);
            wifipassword = CleanInputText(wifipassword);

            Debug.Log("setWiFiConfiguration2 wifiname-" + wifiname);

            txtStatus.text = $"Setup WiFi {wifiname} with the password you just entered, please wait";
            GameStateManager.instance.ChangeState(GameState.WiFiStatusUI);

            if (emulator == false)
            {
                using (AndroidJavaClass jc = new AndroidJavaClass("com.twintera.ptov.controllers.Startup"))
                {
                    AndroidJavaObject currentActivity;
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    }

                    Debug.Log("Call setwifi with boardAddress-" + boardAddress + " wifiname -" + wifiname + " wifipassword-" + wifipassword);
                    string message = jc.CallStatic<string>("setwifi", currentActivity, boardAddress, wifiname, wifipassword);

                    PtovStatusWiFi status = JsonUtility.FromJson<PtovStatusWiFi>(message);
                    if (status.rc < 0)
                    {
                        txtStatus.text = status.rcmsg;
                        btnExitToWiFiMenu.gameObject.SetActive(true);
                    }
                    else
                    {
                        btnExitToMainMenu.gameObject.SetActive(true);
                    }

                    Debug.Log("Message from setwifi: " + message);
                }
            }

            //GameStateManager.instance.ChangeState(GameState.MainMenu);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit setWiFiConfiguration()");
        }
    }
    public void setRFIDDetectionMode()
    {
        Debug.Log("Enter setRFIDDetectionMode()");
        try
        {
            RFIDDetectionMode rfidDetectionMode = new RFIDDetectionMode();
            rfidDetectionMode.sendTagsStatusEveryEndOfRound = tgSendTagsStatusEveryEndOfRound.isOn;
            rfidDetectionMode.sendTagsStatusEveryEndOfRoundWhenChanged = tgSendTagsStatusEveryEndOfRoundWhenChanged.isOn;
            rfidDetectionMode.turnOffAntennaAfterDetection = tgTurnOffAntennaAfterDetection.isOn;
            if (emulator == false)
            {
                using (AndroidJavaClass jc = new AndroidJavaClass("com.twintera.ptov.controllers.Startup"))
                {
                    AndroidJavaObject currentActivity;
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    }

                    string rfidconfigjson = JsonUtility.ToJson(rfidDetectionMode);
                    Debug.Log("Call setrfiddetectionmode with boardAddress-" + boardAddress + " rfidconfigjson-" + rfidconfigjson);
                    string message = jc.CallStatic<string>("setrfiddetectionmode", currentActivity, boardAddress, rfidconfigjson);

                    PtovStatus status = JsonUtility.FromJson<PtovStatus>(message);
                    if (status.rc < 0)
                    {
                    }
                    else
                    {
                    }

                    Debug.Log("Message from setrfid: " + message);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit setRFIDDetectionMode()");
        }
    }
    public void setAntennaLocation()
    {
        Debug.Log("Enter setAntennaLocation()");
        try
        {
            AntennaLocation antlocconfig = new AntennaLocation();
            antlocconfig.antennatoturnon = CleanInputText(txtAntennaToTurnOn.text);
            if (emulator == false)
            {
                using (AndroidJavaClass jc = new AndroidJavaClass("com.twintera.ptov.controllers.Startup"))
                {
                    AndroidJavaObject currentActivity;
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    }

                    string antlocconfigjson = JsonUtility.ToJson(antlocconfig);
                    Debug.Log("Call setantennalocation with boardAddress-" + boardAddress + " antlocconfigjson-" + antlocconfigjson);
                    string message = jc.CallStatic<string>("setantennalocation", currentActivity, boardAddress, antlocconfigjson);

                    PtovStatus status = JsonUtility.FromJson<PtovStatus>(message);
                    if (status.rc < 0)
                    {
                    }
                    else
                    {
                    }

                    Debug.Log("Message from setrfid: " + message);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit setAntennaLocation()");
        }
    }
    public void setRFPower()
    {
        Debug.Log("Enter setRFPower()");
        try
        {
            RFPower rfPower = new RFPower();
            if (Power18.isOn)
            {
                rfPower.power = (int)RFPower.RFPowerValue.Power18;    
            }
            if (Power18_2.isOn)
            {
                rfPower.power = (int)RFPower.RFPowerValue.Power18_2;    
            }
            if (Power23.isOn)
            {
                rfPower.power = (int)RFPower.RFPowerValue.Power23;    
            }
            if (Power23_2.isOn)
            {
                rfPower.power = (int)RFPower.RFPowerValue.Power23_2;    
            }
            if (Power33.isOn)
            {
                rfPower.power = (int)RFPower.RFPowerValue.Power33;    
            }
            if (Power38.isOn)
            {
                rfPower.power = (int)RFPower.RFPowerValue.Power38;    
            }
            if (Power43.isOn)
            {
                rfPower.power = (int)RFPower.RFPowerValue.Power43;    
            }
            if (Power48.isOn)
            {
                rfPower.power = (int)RFPower.RFPowerValue.Power48;    
            }
            
            if (emulator == false)
            {
                using (AndroidJavaClass jc = new AndroidJavaClass("com.twintera.ptov.controllers.Startup"))
                {
                    AndroidJavaObject currentActivity;
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    }

                    string rfPowerjson = JsonUtility.ToJson(rfPower);
                    Debug.Log("Call setrfpower with boardAddress-" + boardAddress + " rfPowerjson-" + rfPowerjson);
                    string message = jc.CallStatic<string>("setrfpower", currentActivity, boardAddress, rfPowerjson);

                    PtovStatus status = JsonUtility.FromJson<PtovStatus>(message);
                    if (status.rc < 0)
                    {
                    }
                    else
                    {
                    }

                    Debug.Log("Message from setrfid: " + message);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit setRFPower()");
        }
    }
    private string CleanInputText(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";

        // Remove common invisible characters that sneak in from copy-paste
        string cleaned = input
            .Replace("\u200B", "")  // Zero Width Space
            .Replace("\u200C", "")  // Zero Width Non-Joiner
            .Replace("\u200D", "")  // Zero Width Joiner
            .Replace("\uFEFF", "")  // Zero Width No-Break Space (BOM)
            .Replace("\u00A0", "")  // Non-breaking space
            .Trim();                // Remove leading/trailing whitespace

        return cleaned;
    }
    void moveCamera()
    {
        Debug.Log("Enter moveCamera()");
        try
        {
            Camera cam = Camera.main; // Get the MainCamera
            if (cam == null)
            {
                Debug.LogError("No MainCamera found!");
                return;
            }

            Vector3 startPosition = cam.transform.position;
            Vector3 startEulerAngles = cam.transform.eulerAngles;

            cam.transform.position = new Vector3(startPosition.x, 5.4f, -4.4f);
            cam.transform.eulerAngles = new Vector3(90, 0, 0);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit moveCamera()");
        }
    }
    void positionCamera()
    {
        Debug.Log("Enter positionCamera()");
        try
        {
            Camera cam = Camera.main; // Get the MainCamera
            if (cam == null)
            {
                Debug.LogError("No MainCamera found!");
                return;
            }

            Vector3 startPosition = cam.transform.position;
            Vector3 startEulerAngles = cam.transform.eulerAngles;

            cam.transform.position = new Vector3(0, 4.5f, -7.3f);
            cam.transform.eulerAngles = new Vector3(60, 0, 0);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit positionCamera()");
        }
    }

    public void tagDetected(string message)
    {
        Debug.Log("Enter tagDetected() message-" + message);
        try
        {
            lock (lockObject)
            {
                PPUDetection ppuDetection = JsonUtility.FromJson<PPUDetection>(message);
                string[] tagdetected = ppuDetection.tagid.Split(":");
                string tagid = tagdetected[0];
                int antenna = System.Convert.ToInt32(tagdetected[1]);
                //int detecteddisknumber = tagsAndDiskNumber[tagid];
                Debug.Log("tagid-" + tagid + " antenna-" + antenna);

                /*if (GameStateManager.instance.getCurrentState() == GameState.WaitForTimeSelected)
                {
                    AudioManager.instance.StopSFX();
                    AudioManager.instance.UnPauseMusic();
                    hideCountDownTimer();

                    // By the tag id, get the time selected
                    int timeselected = getSelectedTime(tagid);

                    eachGameTime = timeselected;
                    Debug.LogError("gil12: " + eachGameTime.ToString());

                    //GameStateManager.instance.ChangeState(GameState.Playing);
                    StartTheGame();
                }*/
                //RemoveHourGlass(tagid);

                //AddHourGlass(tagid, antenna);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit tagDetected()");
        }
    }
    public void getBoardInfo()
    {
        Debug.Log("Enter getBoardInfo()");
        try
        {
            getPPUInfo();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit getBoardInfo()");
        }
    }
    private void getPPUInfo()
    {
        Debug.Log("Enter getPPUInfo()");
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Debug.Log("Running on Android");
            }
            else
            {
                Debug.Log("Not running on Android");
            }

            using (AndroidJavaClass jc = new AndroidJavaClass("com.twintera.ptov.controllers.Startup"))
            {
                string getBoardInfoResult = jc.CallStatic<string>("getBoardInfo", boardAddress);
                // Print the message from Java
                Debug.Log("Return from getBoardInfo: " + getBoardInfoResult);
                RCBL rcbl = JsonUtility.FromJson<RCBL>(getBoardInfoResult);

            }
        }

        catch (System.Exception e)
        {
            Debug.LogError("Java class NOT found: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit getPPUInfo()");
        }
    }
    public void getBoardStatus()
    {
        Debug.Log("Enter getBoardStatus()");
        try
        {
            getPPUStatus();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit getBoardStatus()");
        }
    }
    private void getPPUStatus()
    {
        Debug.Log("Enter getPPUStatus()");
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Debug.Log("Running on Android");
            }
            else
            {
                Debug.Log("Not running on Android");
            }

            using (AndroidJavaClass jc = new AndroidJavaClass("com.twintera.ptov.controllers.Startup"))
            {
                string getBoardStatusResult = jc.CallStatic<string>("getBoardStatus", boardAddress);
                // Print the message from Java
                Debug.Log("Return from getBoardStatus: " + getBoardStatusResult);
                RCBL rcbl = JsonUtility.FromJson<RCBL>(getBoardStatusResult);

            }
        }

        catch (System.Exception e)
        {
            Debug.LogError("Java class NOT found: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit getPPUInfo()");
        }
    }
    public void checkP2V()
    {
        Debug.Log("Enter checkP2V()");
        try
        {
            checkJarP2V();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit checkP2V()");
        }
    }
    private void checkJarP2V()
    {
        Debug.Log("Enter checkJarP2V()");
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Debug.Log("Running on Android");
            }
            else
            {
                Debug.Log("Not running on Android");
            }

            using (AndroidJavaClass jc = new AndroidJavaClass("com.twintera.ptov.controllers.Startup"))
            {
                string txtoutput = jc.CallStatic<string>("checkP2V", CleanInputText(p2vInput.text));
                // Print the message from Java
                Debug.Log("Message from checkP2V: " + txtoutput);
                p2vOutput.text = txtoutput;
            }
        }

        catch (System.Exception e)
        {
            Debug.LogError("Java class NOT found: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit checkJarP2V()");
        }
    }
    public void boardInfo(string message)
    {
        lock (lockObject)
        {
            Debug.Log("Enter boardInfo() message-" + message);
            try
            {
                BoardInfo boardInfo = JsonUtility.FromJson<BoardInfo>(message);
                txtCpuType.text = boardInfo.cputype;
                txtNumOfAntenna.text = boardInfo.numberofantenna;
                txtCompass.text = boardInfo.compass;

                txtFirmwareVersion.text = boardInfo.firmwareversion;
                txtHardwareVersion.text = boardInfo.hardwareversion;
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
    }
    public void boardStatus(string message)
    {
        lock (lockObject)
        {
            Debug.Log("Enter boardStatus() message-" + message);
            try
            {
                //BoardInfo boardInfo = JsonUtility.FromJson<BoardInfo>(message);
                BoardStatus boardStatus = JsonUtility.FromJson<BoardStatus>(message);
                txtBatInfo.text = boardStatus.batterystatus;
                int wifiinfo = Convert.ToInt32(boardStatus.wifistatus);
                int rfidstatus = Convert.ToInt32(boardStatus.rfidstatus);
                if (wifiinfo == 1)
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
                if (boardStatus.bleaddress == null)
                {
                    txtBLEStatus.text = "BLE is not ready yet";
                }
                else
                {
                    txtBLEStatus.text = boardStatus.bleaddress;
                }
                //txtFirmwareVersion.text = boardStatus.firmwareversion;
                //txtHardwareVersion.text = boardStatus.hardwareversion;

            }
            catch (System.Exception e)
            {
                Debug.LogError("Error: " + e.Message);
            }
            finally
            {
                Debug.Log("Exit boardStatus()");
            }
        }
    }
    public void tagUnDetected(string message)
    {
        lock (lockObject)
        {
            Debug.Log("Enter tagUnDetected() message-" + message);
            try
            {
                PPUDetection ppuDetection = JsonUtility.FromJson<PPUDetection>(message);
                string[] tagdetected = ppuDetection.tagid.Split(":");
                string tagid = tagdetected[0];
                int antenna = System.Convert.ToInt32(tagdetected[1]);
                //int detecteddisknumber = tagsAndDiskNumber[tagid];
                Debug.Log("tagid-" + tagid + " antenna-" + antenna);



                //numberOfTagsDetected--;
                //numberOfTags.text = numberOfTagsDetected.ToString();
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error: " + e.Message);
            }
            finally
            {
                Debug.Log("Exit tagUnDetected()");
            }
        }
    }
    public void allDetectedTags(string message)
    {
        Debug.Log("Enter before lockObject allDetectedTags() message-" + message);
        lock (lockObject)
        {
            Debug.Log("Enter after lockObject allDetectedTags() message-" + message);

            try
            {
                //{"bleaddress":"C8:F0:9E:B8:09:12","pputags":[{"id":"253202250178","antenna":13},{"id":"077119251178","antenna":14}]}
                PPUData ppudata = JsonUtility.FromJson<PPUData>(message);
                string texttosend = "BLE Address: " + ppudata.bleaddress + "\n";
                foreach (PPUTag pputag in ppudata.pputags)
                {
                    texttosend += "ID: " + pputag.id + " ANTENNA: " + pputag.antenna + "\n";
                }

                AppendAntennaText(texttosend);
                txtNumberOfTagsDetected.text = ppudata.pputags.Length.ToString();
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
    }
    private void ConnectToPPU()
    {
        Debug.Log("Enter ConnectToPPU()");
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Debug.Log("Running on Android");
            }
            else
            {
                Debug.Log("Not running on Android");
            }

            using (AndroidJavaClass jc = new AndroidJavaClass("com.twintera.ptov.controllers.Startup"))
            {
                AndroidJavaObject currentActivity;
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                }
                Debug.Log("Call connect with gameObject.name-" + gameObject.name);
                string connectResult = jc.CallStatic<string>("connect", currentActivity, gameObject.name);
                Debug.Log("result back from connect: " + connectResult);
                RCBL rcbl = JsonUtility.FromJson<RCBL>(connectResult);
                boardAddress = rcbl.address;
                connectPPUStatus = rcbl.rc;
            }
        }

        catch (System.Exception e)
        {
            Debug.LogError("Java class NOT found: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit ConnectToPPU()");
        }
    }
    void OnToggleChanged_SendTagsStatusEveryEndOfRound(bool isOn)
    {
        if (isOn)
        {
            tgSendTagsStatusEveryEndOfRoundWhenChanged.isOn = false;
        }
    }
    void OnToggleChanged_SendTagsStatusEveryEndOfRoundWhenChanged(bool isOn)
    {
        if (isOn)
        {
            tgSendTagsStatusEveryEndOfRound.isOn = false;
        }

    }
    void OnToggleChanged_Power18(bool isOn)
    {
        if (isOn)
        {
            turnOffPreviousePowers();
            Power18.isOn = true;
        }
    }
    void OnToggleChanged_Power18_2(bool isOn)
    {
        if (isOn)
        {
            turnOffPreviousePowers();
            Power18_2.isOn = true;
        }
    }
    void OnToggleChanged_Power23(bool isOn)
    {
        if (isOn)
        {
            turnOffPreviousePowers();
            Power23.isOn = true;
        }
    }
    void OnToggleChanged_Power23_2(bool isOn)
    {
        if (isOn)
        {
            turnOffPreviousePowers();
            Power23_2.isOn = true;
        }
    }
    void OnToggleChanged_Power33(bool isOn)
    {
        if (isOn)
        {
            turnOffPreviousePowers();
            Power33.isOn = true;
        }
    }
    void OnToggleChanged_Power38(bool isOn)
    {
        if (isOn)
        {
            turnOffPreviousePowers();
            Power38.isOn = true;
        }
    }
    void OnToggleChanged_Power43(bool isOn)
    {
        if (isOn)
        {
            turnOffPreviousePowers();
            Power43.isOn = true;
        }
    }
    void OnToggleChanged_Power48(bool isOn)
    {
        if (isOn)
        {
            turnOffPreviousePowers();
            Power48.isOn = true;
        }
    }
    void turnOffPreviousePowers() {
        if (Power18.isOn)
        {
            Power18.isOn = false;
        }
        if (Power18.isOn)
        {
            Power18_2.isOn = false;
        }
        if (Power18.isOn)
        {
            Power23.isOn = false;
        }
        if (Power18.isOn)
        {
            Power23_2.isOn = false;
        }

        if (Power18.isOn)
        {
            Power33.isOn = false;
        }
        if (Power18.isOn)
        {
            Power38.isOn = false;
        }
        if (Power18.isOn)
        {
            Power43.isOn = false;
        }
        if (Power18.isOn)
        {
            Power48.isOn = false;
        }
    }
}
public class PPUDetection
{
    public string bleaddress;
    public string tagid;
}
public class BoardInfo
{
    public string bleaddress;
    public string firmwareversion;
    public string hardwareversion;
    public string cputype;
    public string numberofantenna;
    public string compass;

}
public class BoardStatus
{
    public string bleaddress;
    public string wifistatus;
    public string rfidstatus;
    public string batterystatus;
}
public class RCBL
{
    public int rc;
    public string address;
}
public class FullBaordDetectedTags
{
    public enum DetectedStatus
    {
        failToDetectTags = -1,
        allTagsSetDetectedCorrectly = 1,
        allTagsSetNotDetectedCorrectly = 2,
        notAllTagsSetOnBoard = 3
    }
    public FullBaordDetectedTags(DetectedStatus detectedStatus, int numberOfTagsSetOnBoard, int numberOfTagsDetected)
    {
        this.detectedStatus = detectedStatus;
        this.numberOfTagsDetected = numberOfTagsDetected;
        this.numberOfTagsSetOnBoard = numberOfTagsSetOnBoard;
    }
    public DetectedStatus detectedStatus;
    public int numberOfTagsDetected;
    public int numberOfTagsSetOnBoard;
}