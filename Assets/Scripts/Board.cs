using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Board : MonoBehaviour
{
    public static int Width;
    public static int Height;

    [SerializeField] private GameObject _tilePrafab;
    [SerializeField] private GameObject[] _itemPrefabs;
    [SerializeField] private GameObject _hintPrefab;

    private Tile[,] _tiles;
    private Level _level;
    private Stack<ItemInfo[]> _previousMoves = new Stack<ItemInfo[]>();

    private int _itemsToDrop;
    private int _itemsToDestroy;

    private Hint _hint;
    private bool _isHintShowing;

    public Tile[,] Tiles => _tiles;
    public Level Level => _level;
    public int CurrentMove => _previousMoves.Count;

    [HideInInspector] public UnityAction<bool> AllItemsDropped;
    [HideInInspector] public UnityAction<bool> AllMatchedItemsDestroyed;
    [HideInInspector] public UnityAction<int> PreviousMovesCountChanged;

    public bool IsLevelDone()
    {
        if (_previousMoves.Count - 1 != _level.MoveCount)
            return false;

        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++)
            {
                if (_tiles[w, h].Item != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void GenerateLevel(Level level)
    {
        _level = level;
        Width = _level.BoardWidth;
        Height = _level.BoardHeight;
        CreateTiles();
        RestartLevel();
    }

    public void RestartLevel()
    {
        DestroyItems();
        CreateItems(_level.Items);
        _previousMoves.Clear();
        SaveMove();
        StopHint();
    }

    public void DropItems()
    {
        _itemsToDrop = 0;

        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++)
            {
                if (_tiles[w, h].Item != null)
                {
                    if (TryFindDropPlace(_tiles[w, h], out Tile targetTile))
                    {
                        _tiles[w, h].MoveItemToTile(targetTile);
                        _itemsToDrop++;
                    }
                }
            }
        }

        CheckAllItemsDropped(true);
    }

    public void FindMatches()
    {
        _itemsToDestroy = 0;
        List<Tile> tilesToDeleteItem = new List<Tile>();

        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++)
            {
                if (_tiles[w, h].Item != null)
                {
                    if(TryFindMatch(_tiles[w, h], out List<Tile> matchedTiles))
                    {
                        foreach (var tile in matchedTiles)
                        {
                            tile.SetItemMatched();

                            if (tilesToDeleteItem.Contains(tile) == false)
                                tilesToDeleteItem.Add(tile);
                        }
                        
                    }
                }
            }
        }

        foreach (var tile in tilesToDeleteItem)
        {
            tile.SetItem(null);
        }

        _itemsToDestroy = tilesToDeleteItem.Count;
        CheckAllMatchedItemsDestroyed(false);
    }

    public void SaveMove()
    {
        List<ItemInfo> itemsList = new List<ItemInfo>();

        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++)
            {
                if (_tiles[w, h].Item != null)
                {
                    Item item = _tiles[w, h].Item;
                    itemsList.Add(new ItemInfo(item.Id, item.X, item.Y));
                }
            }
        }

        _previousMoves.Push(itemsList.ToArray());
        PreviousMovesCountChanged?.Invoke(_previousMoves.Count);
    }

    public void LoadPreviousMove()
    {
        if (_previousMoves.Count > 1)
        {
            _previousMoves.Pop();
            DestroyItems();
            CreateItems(_previousMoves.Pop());
            SaveMove();
            PreviousMovesCountChanged?.Invoke(_previousMoves.Count);

            if (_isHintShowing == true)
                StartNextHint();
        }
    }

    public void TryStartNextHint()
    {
        if (_isHintShowing == true)
            StartNextHint();
    }

    public void StartNextHint()
    {
        _isHintShowing = true;
        int currentHint = _previousMoves.Count - 1;

        if (currentHint >= _level.Hints.Length)
        {
            //StopHint();
            return;
        }

        if (_hint == null)
            _hint = Instantiate(_hintPrefab).GetComponent<Hint>();

        Vector2 startPosition = new Vector2(_level.Hints[currentHint].X, _level.Hints[currentHint].Y);
        _hint.Init(startPosition, _level.Hints[currentHint].Direction);
        _hint.StartMoving();
    }

    public void StopHint()
    {
        _isHintShowing = false;

        if (_hint != null)
            _hint.StopMoving();
    }

    public void ExitLevel()
    {
        StopHint();
        DestroyItems();
        DestroyTiles();
    }

    private bool TryFindMatch(Tile tile, out List<Tile> matchedTiles)
    {
        matchedTiles = FindHorizontalMatch(tile);
        matchedTiles.AddRange(FindVerticalMatch(tile));

        if (matchedTiles.Count > 1)
        {
            matchedTiles.Add(tile);
            return true;
        }

        return false;
    }

    private List<Tile> FindHorizontalMatch(Tile tile)
    {
        List<Tile> matchedTiles = new List<Tile>();
        int nextX = tile.X + 1;

        while (nextX < Width)
        {
            if (_tiles[nextX, tile.Y].Item == null)
                break;

            if (_tiles[nextX, tile.Y].Item.Id == tile.Item.Id)
            {
                matchedTiles.Add(_tiles[nextX, tile.Y]);
                nextX++;
            }
            else
            {
                break;
            }
        }

        if (matchedTiles.Count < 2)
            matchedTiles.Clear();

        return matchedTiles;
    }

    private List<Tile> FindVerticalMatch(Tile tile)
    {
        List<Tile> matchedTiles = new List<Tile>();
        int nextY = tile.Y + 1;

        while (nextY < Height)
        {
            if (_tiles[tile.X, nextY].Item == null)
                break;

            if (_tiles[tile.X, nextY].Item.Id == tile.Item.Id)
            {
                matchedTiles.Add(_tiles[tile.X, nextY]);
                nextY++;
            }
            else
            {
                break;
            }
        }

        if (matchedTiles.Count < 2)
            matchedTiles.Clear();

        return matchedTiles;
    }

    private bool TryFindDropPlace(Tile tile, out Tile targetTile)
    {
        int targetY = tile.Y - 1;
        targetTile = null;

        while (targetY >= 0)
        {
            if (_tiles[tile.X, targetY].Item == null)
                targetTile = _tiles[tile.X, targetY];

            targetY--;
        }

        return targetTile != null;
    }

    private void CreateItems(ItemInfo[] items)
    {
        foreach (var item in items)
        {
            var box = Instantiate(_itemPrefabs[(int)item.Id], new Vector3(item.X, item.Y), Quaternion.identity, _tiles[item.X, item.Y].transform);
            box.name = "(" + item.X + "," + item.Y + ")";
            _tiles[item.X, item.Y].SetItem(box.GetComponent<Item>());
            _tiles[item.X, item.Y].Item.SetPositionProperties(item.X, item.Y);
            AddListeners(_tiles[item.X, item.Y].Item);
        }
    }

    private void CreateTiles()
    {
        if (_tiles != null)
            return;

        _tiles = new Tile[Width, Height];

        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++)
            {
                var tile = Instantiate(_tilePrafab, new Vector3(w, h), Quaternion.identity, transform);
                tile.name = "(" + w + "," + h + ")";
                _tiles[w, h] = tile.GetComponent<Tile>();
                _tiles[w, h].Init(null, w, h);
            }
        }
    }

    private void DestroyItems()
    {
        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++)
            {
                if (_tiles[w, h].Item != null)
                {
                    _tiles[w, h].DestroyItem();
                }
            }
        }
    }

    private void DestroyTiles()
    {
        foreach (var tile in _tiles)
        {
            Destroy(tile.gameObject);
        }

        _tiles = null;
    }

    private void OnItemDropped(Item item)
    {
        _itemsToDrop--;
        CheckAllItemsDropped(true);
    }

    private void OnItemDestroyed(Item item)
    {
        _itemsToDestroy--;
        RemoveListeners(item);
        CheckAllMatchedItemsDestroyed(true);
    }

    private void OnItemMoved(Item item, Item swapItem, bool isItemMoved)
    {
        Game.CanMove = false;

        if (_isHintShowing == true)
            _hint.StopMoving();

        if (isItemMoved == true)
        {
            if (swapItem == null)
            {
                float signDeltaX = Mathf.Sign(item.transform.position.x - item.transform.parent.position.x);
                float deltaX = Mathf.Ceil(Mathf.Abs(item.transform.position.x - item.transform.parent.position.x)) * signDeltaX;

                float signDeltaY = Mathf.Sign(item.transform.position.y - item.transform.parent.position.y);
                float deltaY = Mathf.Ceil(Mathf.Abs(item.transform.position.y - item.transform.parent.position.y)) * signDeltaY;

                int newTileX = item.X + (int)deltaX;
                int newTileY = item.Y + (int)deltaY;

                _tiles[item.X, item.Y].SwapItems(_tiles[newTileX, newTileY]);
            }
            else
            {
                _tiles[item.X, item.Y].SwapItems(_tiles[swapItem.X, swapItem.Y]);
                swapItem.transform.position = swapItem.transform.parent.position;
            }

            item.transform.position = item.transform.parent.position;
        }

        DropItems();
    }

    private void AddListeners(Item item)
    {
        item.Dropped += OnItemDropped;
        item.Destroyed += OnItemDestroyed;
        item.Moved += OnItemMoved;
    }

    private void RemoveListeners(Item item)
    {
        item.Dropped -= OnItemDropped;
        item.Destroyed -= OnItemDestroyed;
        item.Moved -= OnItemMoved;
    }

    private void CheckAllItemsDropped(bool isNextBoardActionNeeded)
    {
        if (_itemsToDrop == 0)
            AllItemsDropped?.Invoke(isNextBoardActionNeeded);
    }

    private void CheckAllMatchedItemsDestroyed(bool isNextBoardActionNeeded)
    {
        if (_itemsToDestroy == 0)
            AllMatchedItemsDestroyed?.Invoke(isNextBoardActionNeeded);
    }
}