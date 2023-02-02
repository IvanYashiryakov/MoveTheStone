using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemInfo
{
    public BoxType Id;
    public int X;
    public int Y;

    public ItemInfo(BoxType id, int x, int y)
    {
        Id = id;
        X = x;
        Y = y;
    }
}