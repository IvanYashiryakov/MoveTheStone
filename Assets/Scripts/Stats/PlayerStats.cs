using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private bool[] _countires;
    private bool[,] _towns;
    private bool[,,] _levels;

    public void ResetAll()
    {
        _countires = new bool[6];
        _towns = new bool[6, 4];
        _levels = new bool[6, 4, 24];

        _countires[0] = true;
        _towns[0, 0] = true;
        _levels[0, 0, 0] = true;
    }

    public bool IsLevelAvailable(int country, int town, int level)
    {
        return _countires[country] && _towns[country, town] && _levels[country, town, level];
    }

    public void SetLevelAvailable(int country, int town, int level)
    {
        _levels[country, town, level] = true;
    }

    public void SetTownAvailable(int country, int town)
    {
        _towns[country, town] = true;
    }

    public void SetCountryAvailable(int country)
    {
        _countires[country] = true;
    }
}
