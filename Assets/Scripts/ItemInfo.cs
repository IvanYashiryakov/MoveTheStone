using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemInfo
{
    public int Id;
    public int X;
    public int Y;

    public ItemInfo(int id, int x, int y)
    {
        Id = id;
        X = x;
        Y = y;
    }
}