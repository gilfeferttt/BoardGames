using System;
public class RFIDTag
{
    public string SerialNumber { get; set; }
    public RFIDTag(string serialnumber)
    {
        SerialNumber = serialnumber;
    }
}
