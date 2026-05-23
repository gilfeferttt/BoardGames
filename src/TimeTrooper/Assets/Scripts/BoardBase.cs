using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBase : MonoBehaviour
{
    /*public enum GameMode
    {
        MultiMode = 1,
        CoOpMode = 2
    }*/

    protected bool emulator = false;
    //protected GameMode currentGameMode;

    protected List<Tile> nextTiles;
    protected List<Trooper> currenttroopers;
    protected bool orderTrooperMetter = false;
    protected Coroutine maketagdetectCoroutine = null;
    //protected Player currentplayer = null;

    public BoardBase()
    {
        Debug.Log("Enter BoardBase()");
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
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit BoardBase()");
        }

    }
    protected void Start()
    {
        Debug.Log("Enter Start() BoardBas");
        try
        {
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit Start() BoardBas");
        }
    }
    /*public void SetGameMode(GameMode gamemode)
    {
        Debug.Log("Enter SetGameMode()");
        try
        {
            currentGameMode = gamemode;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            Debug.Log("Exit SetGameMode()");
        }
    }
    public GameMode GetGameMode()
    {
        Debug.Log("Enter GetGameMode()");
        try
        {
            return currentGameMode;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
            throw;
        }
        finally
        {
            Debug.Log("Exit GetGameMode()");
        }
    }*/
    
        public void MakeTagDetected()
        {
            Debug.Log("Enter MakeTagDetected()");
            try
            {
                AudioManager.instance.stopCountdown();
                if (maketagdetectCoroutine != null)
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

                Message msgToSend = new Message();
                msgToSend.bleaddress = "84:0D:8E:23:8F:A6";

                numberOfTagsToSend++;
                if (orderTrooperMetter == false)
                {
                    msgToSend.pputags = new MessageTag[numberOfTagsToSend];
                    for (int x = 0; x < numberOfTagsToSend; x++)
                    {
                        msgToSend.pputags[x] = msg.pputags[x];
                    }
                }
                else
                {
                    msgToSend.pputags = new MessageTag[numberOfTagsToSend];
                    msgToSend.pputags[0] = msg.pputags[trooperX];
                }

                trooperX++;

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
        public virtual void allDetectedTags(string message)
        {
            Debug.Log("Enter BoardBase allDetectedTags() message-" + message);
        }
}
