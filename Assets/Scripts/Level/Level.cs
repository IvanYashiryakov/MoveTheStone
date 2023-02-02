using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level/New Level", order = 51)]
public class Level : ScriptableObject
{
    public int BoardWidth = 7;
    public int BoardHeight = 9;
    public int TryCount = 1;
    
    public ItemInfo[] Items;

    //public int BoardWidth => _boardWidth;
    //public int BoardHeight => _boardHeight;
    //public int TryCount => _tryCount;
}