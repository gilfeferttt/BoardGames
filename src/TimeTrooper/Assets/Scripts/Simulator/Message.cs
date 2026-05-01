using System;
[System.Serializable]
public class MessageTag
{
    public string id;
    public int antenna;
    public MessageTag()
    {
    }
}
[System.Serializable]
public class Message
{
    public string bleaddress;
    public MessageTag[] pputags;
    public Message()
    {
    }
}
