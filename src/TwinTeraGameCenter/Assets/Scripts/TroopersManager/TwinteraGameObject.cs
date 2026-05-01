using System;
using UnityEngine.UI;

public class TwinteraGameObject
{
    public RawImage GameImage { get; set; }
    public RawImage GameImageTransparent { get; set; }
    public RFIDTag Tag { get; set; }

    public TwinteraGameObject(RawImage gameImage, RawImage gameImageTransparent, RFIDTag tag)
    {
        GameImage = gameImage;
        GameImageTransparent = gameImageTransparent;
        Tag = tag;
    }
}
