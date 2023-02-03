using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HintInfo
{
    public int X;
    public int Y;
    public DirectionType Direction;

    public HintInfo(int x, int y, DirectionType direction)
    {
        X = x;
        Y = y;
        Direction = direction;
    }
}
