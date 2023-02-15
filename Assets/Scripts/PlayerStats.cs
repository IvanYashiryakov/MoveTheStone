using System.Runtime.InteropServices;
using UnityEngine;

public class ProgressInfo
{
    public ProgressStatus[] Countires;
    public ProgressStatus[,] Towns;
    public ProgressStatus[,,] Levels;
}

public class YandexSave
{
    public ProgressStatus[] Data;
}

public class PlayerStats : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SaveExtern(string data);

    [DllImport("__Internal")]
    private static extern void LoadExtern();

    private ProgressInfo _progressInfo;
    private YandexSave _data;

    private void Start()
    {
        ResetAll();
#if UNITY_EDITOR == false
        //LoadExtern();
#endif
    }

    public void Save()
    {
#if UNITY_EDITOR == false
        _data.Data = LevelsToArray();
        string jsonString = JsonUtility.ToJson(_data);
        SaveExtern(jsonString);
#endif
    }

    public void Load()
    {
        LoadExtern();
    }

    public void ResetAllProgressAndSave()
    {
        ResetAll();
        Save();
    }

    private ProgressStatus[] LevelsToArray()
    {
        ProgressStatus[] result = new ProgressStatus[_progressInfo.Levels.GetLength(0) * _progressInfo.Levels.GetLength(1) * _progressInfo.Levels.GetLength(2)];
        int i = 0;

        for (int c = 0; c < _progressInfo.Levels.GetLength(0); c++)
        {
            for (int t = 0; t < _progressInfo.Levels.GetLength(1); t++)
            {
                for (int l = 0; l < _progressInfo.Levels.GetLength(2); l++)
                {
                    result[i] = _progressInfo.Levels[c, t, l];
                    i++;
                }
            }
        }

        return result;
    }

    public void SetProgressInfo(string value)
    {
        _data = JsonUtility.FromJson<YandexSave>(value);
        int i = 0;

        for (int c = 0; c < _progressInfo.Levels.GetLength(0); c++)
        {
            for (int t = 0; t < _progressInfo.Levels.GetLength(1); t++)
            {
                for (int l = 0; l < _progressInfo.Levels.GetLength(2); l++)
                {
                    _progressInfo.Levels[c, t, l] = _data.Data[i];
                    i++;
                }
            }
        }

        if (IsSaveCurrupted() == true)
            ResetAllProgressAndSave();
    }

    private bool IsSaveCurrupted()
    {
        for (int c = 0; c < _progressInfo.Levels.GetLength(0); c++)
        {
            for (int t = 0; t < _progressInfo.Levels.GetLength(1); t++)
            {
                if (_progressInfo.Levels[c, t, 0] == ProgressStatus.Inactive)
                    return true;
            }
        }

        return false;
    }

    public int GetAvailableLevelsCount(int country, int town)
    {
        int result = 0;

        for (int i = 0; i < _progressInfo.Levels.GetLength(2); i++)
        {
            if (_progressInfo.Levels[country, town, i] == ProgressStatus.Done)
                result++;
        }

        return result;
    }

    public void ResetAll()
    {
        _data = new YandexSave();
        _progressInfo = new ProgressInfo
        {
            Countires = new ProgressStatus[6],
            Towns = new ProgressStatus[6, 4],
            Levels = new ProgressStatus[6, 4, 24]
        };

        for (int c = 0; c < _progressInfo.Levels.GetLength(0); c++)
        {
            for (int t = 0; t < _progressInfo.Levels.GetLength(1); t++)
            {
                for (int l = 0; l < _progressInfo.Levels.GetLength(2); l++)
                {
                    _progressInfo.Levels[c, t, l] = ProgressStatus.Inactive;
                }
            }
        }

        SetFirstLevelsActiveIfNot();
    }

    private void SetFirstLevelsActiveIfNot()
    {
        for (int c = 0; c < 6; c++)
        {
            _progressInfo.Countires[c] = ProgressStatus.Active;

            for (int t = 0; t < 4; t++)
            {
                _progressInfo.Towns[c, t] = ProgressStatus.Active;

                if (_progressInfo.Levels[c, t, 0] == ProgressStatus.Inactive)
                    _progressInfo.Levels[c, t, 0] = ProgressStatus.Active;
            }
        }
    }

    public ProgressStatus GetLevelStatus(int country, int town, int level)
    {
        return _progressInfo.Levels[country, town, level];
    }

    public void SetNextLevelAvailable(int currentCountry, int currentTown, int currentLevel)
    {
        _progressInfo.Levels[currentCountry, currentTown, currentLevel] = ProgressStatus.Done;

        if (currentLevel + 1 < 24 && _progressInfo.Levels[currentCountry, currentTown, currentLevel + 1] != ProgressStatus.Done)
            _progressInfo.Levels[currentCountry, currentTown, currentLevel + 1] = ProgressStatus.Active;

        Save();
    }

    public void SetTownAvailable(int country, int town)
    {
        _progressInfo.Towns[country, town] = ProgressStatus.Active;
    }

    public void SetCountryAvailable(int country)
    {
        _progressInfo.Countires[country] = ProgressStatus.Active;
    }
}
