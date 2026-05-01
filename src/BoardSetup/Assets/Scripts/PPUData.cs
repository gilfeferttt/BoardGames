using System;
[System.Serializable]
public class PPUTag
{
    public string id;
    public int antenna;
    public PPUTag()
    {

    }
}
[System.Serializable]
public class PPUData
{
    public string bleaddress;
    public PPUTag[] pputags;

    public PPUData()
    {
    }
}
