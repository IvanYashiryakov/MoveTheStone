using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _differentBoxTypeCount;
    [SerializeField] private Board _gameBoard;

    private BoxType[,] _board;
    private List<ItemInfo> _itemInfos;
    private Level _level;

    public void GenerateBoard()
    {
        GenerateRandomBoard();

        //while (TryFindOneMoveSolution() == false) GenerateRandomBoard();

        CreateLevel();
        _gameBoard.GenerateLevel(_level);
    }

    private void GenerateRandomBoard()
    {
        ResetBoard();
        int totalBoxCount = 9 + Random.Range(0, _width * _height / 4);
        BoxType[] boxes = new BoxType[totalBoxCount];

        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i] = (BoxType)Random.Range(0, _differentBoxTypeCount);
        }

        ScatterOnTheBoard(boxes);
        DropItems();

        while (FindMathes())
        {
            DropItems();
        }
    }

    private bool TryFindOneMoveSolution()
    {
        for (int w = 0; w < _width; w++)
        {
            for (int h = 0; h < _height; h++)
            {
                if (_board[w, h] != (BoxType)(-1))
                {
                    if (TryFindOneMoveSolutionForItem(w, h))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool TryFindOneMoveSolutionForItem(int x, int y)
    {
        BoxType[,] originalBoard = CopyBoard(_board);

        int targetX = x;
        int targetY = y + 1;

        if (targetY < _height && _board[targetX, targetY] != (BoxType)(-1))
        {
            (_board[x, y], _board[targetX, targetY]) = (_board[targetX, targetY], _board[x, y]);

            while (FindMathes())
                DropItems();

            if (IsBoardEmpty() == true)
            {
                _board = CopyBoard(originalBoard);
                return true;
            }
        }

        targetX = x + 1;
        targetY = y;
        _board = CopyBoard(originalBoard);

        if (targetX < _width)
        {
            (_board[x, y], _board[targetX, targetY]) = (_board[targetX, targetY], _board[x, y]);

            while (FindMathes())
                DropItems();

            if (IsBoardEmpty() == true)
            {
                _board = CopyBoard(originalBoard);
                return true;
            }
        }

        targetX = x;
        targetY = y - 1;
        _board = CopyBoard(originalBoard);

        if (targetY >= 0)
        {
            (_board[x, y], _board[targetX, targetY]) = (_board[targetX, targetY], _board[x, y]);

            while (FindMathes())
                DropItems();

            if (IsBoardEmpty() == true)
            {
                _board = CopyBoard(originalBoard);
                return true;
            }
        }

        targetX = x - 1;
        targetY = y;
        _board = CopyBoard(originalBoard);

        if (targetX >= 0)
        {
            (_board[x, y], _board[targetX, targetY]) = (_board[targetX, targetY], _board[x, y]);

            while (FindMathes())
                DropItems();

            if (IsBoardEmpty() == true)
            {
                _board = CopyBoard(originalBoard);
                return true;
            }
        }

        _board = CopyBoard(originalBoard);
        return false;
    }

    private bool IsBoardEmpty()
    {
        for (int w = 0; w < _width; w++)
        {
            for (int h = 0; h < _height; h++)
            {
                if (_board[w, h] != (BoxType)(-1))
                    return false;
            }
        }

        return true;
    }

    private BoxType[,] CopyBoard(BoxType[,] board)
    {
        BoxType[,] newBoard = new BoxType[board.GetLength(0), board.GetLength(1)];

        for (int w = 0; w < board.GetLength(0); w++)
        {
            for (int h = 0; h < board.GetLength(1); h++)
            {
                newBoard[w, h] = board[w, h];
            }
        }

        return newBoard;
    }

    private bool FindMathes()
    {
        List<Position> positionsToDelete = new List<Position>();

        for (int w = 0; w < _width; w++)
        {
            for (int h = 0; h < _height; h++)
            {
                if (_board[w, h] != (BoxType)(-1))
                {
                    if (TryFindMatch(w, h, out List<Position> matchedItemsPositions))
                    {
                        foreach (var position in matchedItemsPositions)
                        {
                            if (positionsToDelete.Contains(position) == false)
                                positionsToDelete.Add(position);
                        }
                    }
                }
            }
        }

        foreach (var position in positionsToDelete)
        {
            _board[position.X, position.Y] = (BoxType)(-1);
        }

        return positionsToDelete.Count > 0;
    }

    private bool TryFindMatch(int w, int h, out List<Position> matchedItemsPositions)
    {
        matchedItemsPositions = FindHorizontalMatch(w, h);
        matchedItemsPositions.AddRange(FindVerticalMatch(w, h));

        if (matchedItemsPositions.Count > 1)
        {
            matchedItemsPositions.Add(new Position(w, h));
            return true;
        }

        return false;
    }

    private List<Position> FindHorizontalMatch(int x, int y)
    {
        List<Position> matchedItemsPositions = new List<Position>();
        int nextX = x + 1;

        while (nextX < _width)
        {
            if (_board[nextX, y] == (BoxType)(-1))
                break;

            if (_board[nextX, y] == _board[x, y])
            {
                matchedItemsPositions.Add(new Position(nextX, y));
                nextX++;
            }
            else
            {
                break;
            }
        }

        if (matchedItemsPositions.Count < 2)
            matchedItemsPositions.Clear();

        return matchedItemsPositions;
    }

    private List<Position> FindVerticalMatch(int x, int y)
    {
        List<Position> matchedItemsPositions = new List<Position>();
        int nextY = y + 1;

        while (nextY < _height)
        {
            if (_board[x, nextY] == (BoxType)(-1))
                break;

            if (_board[x, nextY] == _board[x, y])
            {
                matchedItemsPositions.Add(new Position(x, nextY));
                nextY++;
            }
            else
            {
                break;
            }
        }

        if (matchedItemsPositions.Count < 2)
            matchedItemsPositions.Clear();

        return matchedItemsPositions;
    }

    private void DropItems()
    {
        for (int w = 0; w < _width; w++)
        {
            for (int h = 0; h < _height; h++)
            {
                if (_board[w, h] != (BoxType)(-1))
                {
                    if (TryFindDropPlace(w, h, out int targetY))
                    {
                        (_board[w, h], _board[w, targetY]) = (_board[w, targetY], _board[w, h]);
                    }
                }
            }
        }
    }

    private bool TryFindDropPlace(int x, int y, out int targetY)
    {
        int nextY = y - 1;
        targetY = -1;

        while (nextY >= 0)
        {
            if (_board[x, nextY] == (BoxType)(-1))
                targetY = nextY;

            nextY--;
        }

        return targetY != -1;
    }

    private void CreateLevel()
    {
        _itemInfos = new List<ItemInfo>();

        for (int w = 0; w < _width; w++)
        {
            for (int h = 0; h < _height; h++)
            {
                if (_board[w, h] != (BoxType)(-1))
                {
                    _itemInfos.Add(new ItemInfo(_board[w, h], w, h));
                }
            }
        }

        ItemInfo[] itemInfos = _itemInfos.ToArray();
        _level = new Level();
        _level.BoardWidth = _width;
        _level.BoardHeight = _height;
        _level.MoveCount = 1;
        _level.Items = itemInfos;
    }

    private void ScatterOnTheBoard(BoxType[] boxes)
    {
        Queue<BoxType> queue = new Queue<BoxType>();

        foreach (var box in boxes)
            queue.Enqueue(box);

        while (queue.Count > 0)
        {
            for (int w = 2; w < _width - 2; w++)
            {
                for (int h = 2; h < _height - 2; h++)
                {
                    if (_board[w, h] == (BoxType)(-1))
                    {
                        if (Random.Range(0, 10) >= 7)
                        {
                            _board[w, h] = queue.Dequeue();

                            if (queue.Count <= 0)
                                return;
                        }
                    }
                }
            }
        }
    }

    private void ResetBoard()
    {
        _board = new BoxType[_width, _height];

        for (int w = 0; w < _width; w++)
        {
            for (int h = 0; h < _height; h++)
            {
                _board[w, h] = (BoxType)(-1);
            }
        }
    }
}
