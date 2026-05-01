using System;
public class PtovStatus 
{
    public int rc;
    public string rcmsg;
}

public class PtovStatusWiFi : PtovStatus
{
    public string address;
}