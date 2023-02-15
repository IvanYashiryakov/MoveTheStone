using System.Runtime.InteropServices;
using UnityEngine;

public class ProgressInfo
{
    public ProgressStatus[] Countries;
    public ProgressStatus[,] Towns;
    public ProgressStatus[,,] Levels;
}

public class YandexSave
{
    public ProgressStatus[] Countries;
    public ProgressStatus[] Towns;
    public ProgressStatus[] Levels;
}

public class PlayerStats : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SaveExtern(string data);

    [DllImport("__Internal")]
    private static extern void LoadExtern();

    public static PlayerStats Instance;

    private ProgressInfo _progressInfo;
    private YandexSave _data;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetAll();
#if UNITY_EDITOR == false
        LoadExtern();
#endif
    }

    public void Save()
    {
#if UNITY_EDITOR == false
        _data.Levels = LevelsToArray();
        _data.Towns = TownsToArray();
        _data.Countries = _progressInfo.Countries;
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

    private ProgressStatus[] TownsToArray()
    {
        ProgressStatus[] result = new ProgressStatus[_progressInfo.Towns.GetLength(0) * _progressInfo.Towns.GetLength(1)];
        int i = 0;

        for (int c = 0; c < _progressInfo.Towns.GetLength(0); c++)
        {
            for (int t = 0; t < _progressInfo.Towns.GetLength(1); t++)
            {
                result[i] = _progressInfo.Towns[c, t];
                i++;
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
                    _progressInfo.Levels[c, t, l] = _data.Levels[i];
                    i++;
                }
            }
        }

        i = 0;
        for (int c = 0; c < _progressInfo.Towns.GetLength(0); c++)
        {
            for (int t = 0; t < _progressInfo.Towns.GetLength(1); t++)
            {
                _progressInfo.Towns[c, t] = _data.Towns[i];
                i++;
            }
        }

        for (int c = 0; c < _progressInfo.Countries.Length; c++)
        {
            _progressInfo.Countries[c] = _data.Countries[c];
        }

        if (IsSaveCurrupted() == true)
            ResetAllProgressAndSave();
    }

    private bool IsSaveCurrupted()
    {
        int activeLevels = 0;

        for (int c = 0; c < _progressInfo.Levels.GetLength(0); c++)
        {
            for (int t = 0; t < _progressInfo.Levels.GetLength(1); t++)
            {
                for (int l = 0; l < _progressInfo.Levels.GetLength(2); l++)
                {
                    if (_progressInfo.Levels[c, t, l] == ProgressStatus.Active)
                        activeLevels++;
                }
            }
        }

        return activeLevels > 1;
    }

    public int GetDoneLevelsCount(int country, int town)
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
            Countries = new ProgressStatus[6],
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

        for (int c = 0; c < _progressInfo.Towns.GetLength(0); c++)
        {
            for (int t = 0; t < _progressInfo.Towns.GetLength(1); t++)
            {
                _progressInfo.Towns[c, t] = ProgressStatus.Inactive;
            }
        }

        for (int c = 0; c < _progressInfo.Countries.Length; c++)
        {
            _progressInfo.Countries[c] = ProgressStatus.Inactive;
        }

        SetFirstLevelActive();
        SetAllLevelActive();
    }

    private void SetAllLevelActive()
    {
        for (int c = 0; c < _progressInfo.Levels.GetLength(0); c++)
        {
            for (int t = 0; t < _progressInfo.Levels.GetLength(1); t++)
            {
                for (int l = 0; l < _progressInfo.Levels.GetLength(2); l++)
                {
                    _progressInfo.Levels[c, t, l] = ProgressStatus.Active;
                }
            }
        }

        for (int c = 0; c < _progressInfo.Towns.GetLength(0); c++)
        {
            for (int t = 0; t < _progressInfo.Towns.GetLength(1); t++)
            {
                _progressInfo.Towns[c, t] = ProgressStatus.Active;
            }
        }

        for (int c = 0; c < _progressInfo.Countries.Length; c++)
        {
            _progressInfo.Countries[c] = ProgressStatus.Active;
        }
    }

    private void SetFirstLevelActive()
    {
        _progressInfo.Countries[0] = ProgressStatus.Active;
        _progressInfo.Towns[0, 0] = ProgressStatus.Active;
        _progressInfo.Levels[0, 0, 0] = ProgressStatus.Active;
    }

    public ProgressStatus GetLevelStatus(int country, int town, int level)
    {
        return _progressInfo.Levels[country, town, level];
    }

    public ProgressStatus GetTownStatus(int country, int town)
    {
        return _progressInfo.Towns[country, town];
    }

    public ProgressStatus GetCountryStatus(int country)
    {
        return _progressInfo.Countries[country];
    }

    public void SetNextLevelAvailable(int currentCountry, int currentTown, int currentLevel)
    {
        _progressInfo.Levels[currentCountry, currentTown, currentLevel] = ProgressStatus.Done;

        if (currentLevel + 1 < 24)
        {
            if (_progressInfo.Levels[currentCountry, currentTown, currentLevel + 1] != ProgressStatus.Done)
                _progressInfo.Levels[currentCountry, currentTown, currentLevel + 1] = ProgressStatus.Active;
        }
        else if (currentTown + 1 < 4)
        {
            if (_progressInfo.Towns[currentCountry, currentTown + 1] == ProgressStatus.Inactive)
            {
                _progressInfo.Towns[currentCountry, currentTown + 1] = ProgressStatus.Active;
                _progressInfo.Levels[currentCountry, currentTown + 1, 0] = ProgressStatus.Active;
            }
        }
        else if (currentCountry + 1 < 6)
        {
            if (_progressInfo.Countries[currentCountry + 1] == ProgressStatus.Inactive)
            {
                _progressInfo.Countries[currentCountry + 1] = ProgressStatus.Active;
                _progressInfo.Towns[currentCountry + 1, 0] = ProgressStatus.Active;
                _progressInfo.Levels[currentCountry + 1, 0, 0] = ProgressStatus.Active;
            }
        }

        Save();
    }

    public void SetTownAvailable(int country, int town)
    {
        _progressInfo.Towns[country, town] = ProgressStatus.Active;
    }

    public void SetCountryAvailable(int country)
    {
        _progressInfo.Countries[country] = ProgressStatus.Active;
    }
}
