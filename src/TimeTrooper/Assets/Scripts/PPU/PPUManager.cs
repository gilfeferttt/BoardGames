using System;
using UnityEngine;

public class PPUManager
{
    private bool emulator { get; set; }
    private string boardAddress { get; set; }
    public int boardConnectStatus { get; set; }
    private string gameobjectname { get; set; }

    public PPUManager(bool emulator)
    {
        this.emulator = emulator;
    }
    //2026/01/13 15:35:19.632 10927 10945 Info Unity Enter allDetectedTags() message-{"bleaddress":"84:0D:8E:23:8F:A6","tagid":"190232082239:3;014053076239:4"}

    public void ConnectToPPU(string gameobjectname)
    {
        Debug.Log("Enter ConnectToPPU()");
        try
        {
            if (emulator == false)
            {
                this.gameobjectname = gameobjectname;

                using (AndroidJavaClass jc = new AndroidJavaClass("com.twintera.ptov.controllers.Startup"))
                {
                    string message = jc.CallStatic<string>("GetMessage");
                    // Print the message from Java
                    Debug.Log("Message from GetMessage: " + message);
                    AndroidJavaObject currentActivity;
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    }

                    //Debug.Log("gameObject.name-" + gameObject.name);
                    //string[] regiaterParameters = new string[] { gameObject.name };
                    //message = jc.CallStatic<string, string>("RegisterUnityCallback", regiaterParameters);
                    //Debug.Log("Message from Java: " + message);
                    Debug.Log("Call start with gameObject.name-" + gameobjectname);
                    message = jc.CallStatic<string>("start", currentActivity, gameobjectname);
                    // Print the message from Java
                    Debug.Log("Message from start: " + message);
                    RCBL rcbl = JsonUtility.FromJson<RCBL>(message);
                    boardAddress = rcbl.address;
                    Debug.Log("rcbl.rc: " + rcbl.rc);
                    boardConnectStatus = rcbl.rc;
                    Debug.Log("boardConnectStatus: " + boardConnectStatus);

                    if(boardConnectStatus != 0 )
                    {
                        Debug.Log("Board is not connected, switching to emulator mode");
                        emulator = true;
                        boardConnectStatus = 1;
                    }
                }
            } else
            {
                boardConnectStatus = 1;
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
    public void SetRFIDConfiguration(int antennaNumber)
    {
        Debug.Log("Enter SetRFIDConfiguration()");
        try
        {
            if (emulator == false)
            {
                bool sendAllTagsEveryRound = true;
                bool detectTagOnce = false;

                bool turnOffAntennaAfterDetection = false;
                bool sendTagEveryDetection = false;
                bool sendTagUndetected = false;
                string antennatoturnon = null;
                if (antennaNumber == -1)
                {
                    antennatoturnon = "3:4:5:6:7:8:9:10:11";
                }
                else
                {
                    antennatoturnon = antennaNumber.ToString();
                }

                using (AndroidJavaClass jc = new AndroidJavaClass("com.twintera.ptov.controllers.Startup"))
                {
                    string message = jc.CallStatic<string>("GetMessage");
                    // Print the message from Java
                    Debug.Log("Message from GetMessage: " + message);
                    AndroidJavaObject currentActivity;
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    }

                    Debug.Log("Call setrfid with boardAddress-" + boardAddress + " sendAllTagsEveryRound-" + sendAllTagsEveryRound + " detectTagOnce -" + detectTagOnce + " turnOffAntennaAfterDetection-" + turnOffAntennaAfterDetection + " sendTagEveryDetection-" + sendTagEveryDetection + " sendTagUndetected-" + sendTagUndetected + " antennatoturnon-" + antennatoturnon);
                    message = jc.CallStatic<string>("setrfid", currentActivity, boardAddress, antennatoturnon, sendAllTagsEveryRound, detectTagOnce, turnOffAntennaAfterDetection, sendTagEveryDetection, sendTagUndetected);

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
            Debug.Log("Exit SetRFIDConfiguration()");
        }
    }
    public PPUData getPPUData(string message)
    {
        //{"bleaddress":"84:0D:8E:23:8F:A6","pputags":[{"id":"014053076239","antenna":"4"}]}
        Debug.Log("Enter getPPUData()");
        PPUData ppudata = JsonUtility.FromJson<PPUData>(message);
        Debug.Log("Exit getPPUData()" + ppudata.bleaddress + " " + ppudata.pputags.Length);
        return ppudata;
    }
    public void setAntennaLocation(int[] antennaLocations)
    {
        Debug.Log("Enter setAntennaLocation()");
        try
        {
            string antloc = null;
            foreach(int antennaLocation in antennaLocations)
            {
                if (antloc == null)
                {
                    antloc = antennaLocation.ToString();
                } else
                {
                    antloc += ":" + antennaLocation.ToString();
                }
            }
            AntennaLocation antlocconfig = new AntennaLocation();
            antlocconfig.antennatoturnon = antloc;
            if (emulator == false)
            {
                using (AndroidJavaClass jc = new AndroidJavaClass("com.twintera.ptov.controllers.Startup"))
                {
                    string message = jc.CallStatic<string>("GetMessage");
                    // Print the message from Java
                    Debug.Log("Message from GetMessage: " + message);
                    AndroidJavaObject currentActivity;
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    }

                    string antlocconfigjson = JsonUtility.ToJson(antlocconfig);
                    Debug.Log("Call setantennalocation with boardAddress-" + boardAddress + " antlocconfigjson-" + antlocconfigjson);
                    message = jc.CallStatic<string>("setantennalocation", currentActivity, boardAddress, antlocconfigjson);

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
    public void getPPUInfo()
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
                string message = jc.CallStatic<string>("GetInfo", boardAddress);
                // Print the message from Java
                Debug.Log("Message from GetInfo: " + message);
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
}
