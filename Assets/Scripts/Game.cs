using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static bool CanMove = true;

    [SerializeField] private Level[] _levels;
    [SerializeField] private Board _board;

    private int _currentLevel = 0;

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

    private void Start()
    {
        GenerateLevel(_currentLevel);
    }

    public void ButtonGenerateClick()
    {
        GenerateLevel(_currentLevel);
    }

    public void ButtonDropClick()
    {
        _board.DropItems();
    }

    public void ButtonMatch()
    {
        _board.FindMatches();
    }

    private void GenerateLevel(int levelNumber)
    {
        _board.GenerateLevel(_levels[levelNumber]);
    }

    private void OnAllItemsDropped(bool isNextBoardActionNeeded)
    {
        if (isNextBoardActionNeeded)
            _board.FindMatches();
    }

    private void OnAllMatchedItemsDestroyed(bool isNextBoardActionNeeded)
    {
        if (isNextBoardActionNeeded)
            _board.DropItems();
        else
            CanMove = true;
    }
}