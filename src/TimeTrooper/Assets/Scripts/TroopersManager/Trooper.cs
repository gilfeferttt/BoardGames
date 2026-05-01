using System;
using System.Collections.Generic;
using TMPro;

public class Trooper
{
    public TextMeshProUGUI orderText { get; set; }
    public int orderIndex { get; set; }
    public List<TwinteraGameObject> GameObjects { get; set; }
    public Trooper(List<TwinteraGameObject> gameObjects)
    {
        GameObjects = gameObjects;
        orderIndex = 0;
    }
}
