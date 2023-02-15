using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    public static bool CanMove = true;

    [SerializeField] private Country[] _countries;
    [SerializeField] private Level[] _levels;
    [SerializeField] private Board _board;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Background _background;

    private int _currentCountry = 0;
    private int _currentTown = 0;
    private int _currentLevel = 0;

    public Country[] Countires => _countries;

    [HideInInspector] public UnityAction LevelDone;
    [HideInInspector] public UnityAction LevelFailed;
    [HideInInspector] public UnityAction<bool> PreviousMoveLoaded;

    private void OnEnable()
    {
        _board.AllItemsDropped += OnAllItemsDropped;
        _board.AllMatchedItemsDestroyed += OnAllMatchedItemsDestroyed;
    }

    private void OnDisable()
    {
        _board.AllItemsDropped -= OnAllItemsDropped;
        _board.AllMatchedItemsDestroyed -= OnAllMatchedItemsDestroyed;
    }

    public void SetBackground(int index)
    {
        _background.SetBackground(_countries[index].Background);
    }

    public bool TryLoadNextLevelInTown()
    {
        if (_currentLevel + 1 < 24)
        {
            GenerateLevel(_currentCountry, _currentTown, _currentLevel + 1);
            return true;
        }
        return false;
    }

    public void ButtonLoadPreviousMove()
    {
        _board.LoadPreviousMove();
        PreviousMoveLoaded?.Invoke(_board.IsLevelFailed());
    }

    public void RestartLevel()
    {
        _board.RestartLevel();
    }

    public void ButtonHint()
    {
        RestartLevel();
        _board.StartNextHint();
    }

    public void GenerateLevel(int country, int town, int level)
    {
        _currentCountry = country;
        _currentTown = town;
        _currentLevel = level;
        bool showHint = false;

        if (country == 0 && town == 0 && level < 4)
            showHint = true;

        _board.GenerateLevel(_countries[country].Towns[town].Levels[level], showHint);
    }

    public void ExitLevel()
    {
        _board.ExitLevel();
    }

    private void OnAllItemsDropped(bool isNextBoardActionNeeded)
    {
        if (isNextBoardActionNeeded)
            _board.FindMatches();
    }

    private void OnAllMatchedItemsDestroyed(bool isNextBoardActionNeeded)
    {
        if (isNextBoardActionNeeded)
        {
            _board.DropItems();
        }
        else
        {
            CanMove = true;
            _board.SaveMove();
            _board.TryStartNextHint();

            if (_board.IsLevelFailed() == true)
            {
                LevelFailed?.Invoke();
            }
            else if (_board.IsLevelDone() == true)
            {
                _playerStats.SetNextLevelAvailable(_currentCountry, _currentTown, _currentLevel);
                LevelDone?.Invoke();
            }
        }
    }
}