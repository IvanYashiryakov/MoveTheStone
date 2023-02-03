using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level/New Level", order = 51)]
public class Level : ScriptableObject
{
    public int BoardWidth = 7;
    public int BoardHeight = 9;
    public int MoveCount = 1;
    public ItemInfo[] Items;
    public HintInfo[] Hints;
}