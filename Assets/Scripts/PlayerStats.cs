using UnityEngine;
using Agava.YandexGames;
using System.Collections;

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
        ResetProgressInfo();
        LoadPlayerPrefs();
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif
        PlayerAccount.GetPlayerData((data) => SetProgressInfo(data));
    }

    private void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("0") == false)
        {
            UpdatePlayerPrefs();
        }
        else
        {
            for (int c = 0; c < _progressInfo.Levels.GetLength(0); c++)
            {
                for (int t = 0; t < _progressInfo.Levels.GetLength(1); t++)
                {
                    for (int l = 0; l < _progressInfo.Levels.GetLength(2); l++)
                    {
                        _progressInfo.Levels[c, t, l] = (ProgressStatus)PlayerPrefs.GetInt(c.ToString() + t.ToString() + l.ToString());
                    }
                }
            }

            for (int c = 0; c < _progressInfo.Towns.GetLength(0); c++)
            {
                for (int t = 0; t < _progressInfo.Towns.GetLength(1); t++)
                {
                    _progressInfo.Towns[c, t] = (ProgressStatus)PlayerPrefs.GetInt(c.ToString() + t.ToString());
                }
            }

            for (int c = 0; c < _progressInfo.Countries.Length; c++)
            {
                _progressInfo.Countries[c] = (ProgressStatus)PlayerPrefs.GetInt(c.ToString());
            }
        }
    }

    private void UpdatePlayerPrefs()
    {
        for (int c = 0; c < _progressInfo.Levels.GetLength(0); c++)
        {
            for (int t = 0; t < _progressInfo.Levels.GetLength(1); t++)
            {
                for (int l = 0; l < _progressInfo.Levels.GetLength(2); l++)
                {
                    PlayerPrefs.SetInt(c.ToString() + t.ToString() + l.ToString(), (int)_progressInfo.Levels[c, t, l]);
                }
            }
        }

        for (int c = 0; c < _progressInfo.Towns.GetLength(0); c++)
        {
            for (int t = 0; t < _progressInfo.Towns.GetLength(1); t++)
            {
                PlayerPrefs.SetInt(c.ToString() + t.ToString(), (int)_progressInfo.Towns[c, t]);
            }
        }

        for (int c = 0; c < _progressInfo.Countries.Length; c++)
        {
            PlayerPrefs.SetInt(c.ToString(), (int)_progressInfo.Countries[c]);
        }
    }

    public void Save()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif
        _data.Levels = LevelsToArray();
        _data.Towns = TownsToArray();
        _data.Countries = _progressInfo.Countries;
        string jsonString = JsonUtility.ToJson(_data);
        PlayerAccount.SetPlayerData(jsonString);
    }

    public void ResetAllProgressAndSave()
    {
        ResetProgressInfo();
        UpdatePlayerPrefs();
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
        _data = new YandexSave();
        _data = JsonUtility.FromJson<YandexSave>(value);

        if (IsSaveCurrupted(_data) == true)
        {
            _data = new YandexSave();
            Save();
        }
        else
        {
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

            UpdatePlayerPrefs();
        }
    }

    private bool IsSaveCurrupted(YandexSave data)
    {
        if (data.Countries[0] == ProgressStatus.Inactive
            || data.Towns[0] == ProgressStatus.Inactive
            || data.Levels[0] == ProgressStatus.Inactive)
            return true;

        int activeLevels = 0;

        for (int i = 0; i < data.Levels.Length; i++)
        {
            if (data.Levels[i] == ProgressStatus.Active)
                activeLevels++;
        }

        return activeLevels > 1;
    }

    private bool IsSaveCurrupted()
    {
        if (_progressInfo.Towns[0, 0] == ProgressStatus.Inactive
            || _progressInfo.Countries[0] == ProgressStatus.Inactive
            || _progressInfo.Levels[0, 0, 0] == ProgressStatus.Inactive)
            return true;

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

    public void ResetProgressInfo()
    {
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
        PlayerPrefs.SetInt(currentCountry.ToString() + currentTown.ToString() + currentLevel.ToString(), (int)_progressInfo.Levels[currentCountry, currentTown, currentLevel]);

        if (currentLevel + 1 < 24)
        {
            if (_progressInfo.Levels[currentCountry, currentTown, currentLevel + 1] != ProgressStatus.Done)
            {
                _progressInfo.Levels[currentCountry, currentTown, currentLevel + 1] = ProgressStatus.Active;
                PlayerPrefs.SetInt(currentCountry.ToString() + currentTown.ToString() + (currentLevel + 1).ToString(), (int)_progressInfo.Levels[currentCountry, currentTown, currentLevel + 1]);
            }
        }
        else if (currentTown + 1 < 4)
        {
            if (_progressInfo.Towns[currentCountry, currentTown + 1] == ProgressStatus.Inactive)
            {
                _progressInfo.Towns[currentCountry, currentTown + 1] = ProgressStatus.Active;
                _progressInfo.Levels[currentCountry, currentTown + 1, 0] = ProgressStatus.Active;
                PlayerPrefs.SetInt(currentCountry.ToString() + (currentTown + 1).ToString(), (int)_progressInfo.Towns[currentCountry, currentTown + 1]);
                PlayerPrefs.SetInt(currentCountry.ToString() + (currentTown + 1).ToString() + "0", (int)_progressInfo.Levels[currentCountry, currentTown + 1, 0]);
            }
        }
        else if (currentCountry + 1 < 6)
        {
            if (_progressInfo.Countries[currentCountry + 1] == ProgressStatus.Inactive)
            {
                _progressInfo.Countries[currentCountry + 1] = ProgressStatus.Active;
                _progressInfo.Towns[currentCountry + 1, 0] = ProgressStatus.Active;
                _progressInfo.Levels[currentCountry + 1, 0, 0] = ProgressStatus.Active;
                PlayerPrefs.SetInt((currentCountry + 1).ToString(), (int)_progressInfo.Countries[currentCountry + 1]);
                PlayerPrefs.SetInt((currentCountry + 1).ToString() + "0", (int)_progressInfo.Towns[currentCountry + 1, 0]);
                PlayerPrefs.SetInt((currentCountry + 1).ToString() + "00", (int)_progressInfo.Levels[currentCountry + 1, 0, 0]);
            }
        }

        Save();
    }
}
