using UnityEngine;

public class ProgressInfo
{
    public ProgressStatus[] Countries;
    public ProgressStatus[,] Towns;
    public ProgressStatus[,,] Levels;
}

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    private ProgressInfo _progressInfo;

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
        _progressInfo = new ProgressInfo
        {
            Countries = new ProgressStatus[6],
            Towns = new ProgressStatus[6, 4],
            Levels = new ProgressStatus[6, 4, 24]
        };

        if (PlayerPrefs.HasKey("0") == false)
            ResetAll();
        else
            Load();
    }

    private void Load()
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

    public void ResetAllProgressAndSave()
    {
        ResetAll();
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
        for (int c = 0; c < _progressInfo.Levels.GetLength(0); c++)
        {
            for (int t = 0; t < _progressInfo.Levels.GetLength(1); t++)
            {
                for (int l = 0; l < _progressInfo.Levels.GetLength(2); l++)
                {
                    _progressInfo.Levels[c, t, l] = ProgressStatus.Inactive;
                    PlayerPrefs.SetInt(c.ToString() + t.ToString() + l.ToString(), (int)_progressInfo.Levels[c, t, l]);
                }
            }
        }

        for (int c = 0; c < _progressInfo.Towns.GetLength(0); c++)
        {
            for (int t = 0; t < _progressInfo.Towns.GetLength(1); t++)
            {
                _progressInfo.Towns[c, t] = ProgressStatus.Inactive;
                PlayerPrefs.SetInt(c.ToString() + t.ToString(), (int)_progressInfo.Towns[c, t]);
            }
        }

        for (int c = 0; c < _progressInfo.Countries.Length; c++)
        {
            _progressInfo.Countries[c] = ProgressStatus.Inactive;
            PlayerPrefs.SetInt(c.ToString(), (int)_progressInfo.Countries[c]);
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

        PlayerPrefs.SetInt("0", (int)_progressInfo.Countries[0]);
        PlayerPrefs.SetInt("00", (int)_progressInfo.Towns[0, 0]);
        PlayerPrefs.SetInt("000", (int)_progressInfo.Levels[0, 0, 0]);
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

    public void SetNextLevelAvailableAndSave(int currentCountry, int currentTown, int currentLevel)
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
    }
}
