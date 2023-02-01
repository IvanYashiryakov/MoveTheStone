using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level/New Level", order = 51)]
public class Level : ScriptableObject
{
    [SerializeField] private int _boardWidth = 7;
    [SerializeField] private int _boardHeight = 9;
    [SerializeField] private ItemInfo[] _items;

    public ItemInfo[] Items => _items;
    public int BoardWidth => _boardWidth;
    public int BoardHeight => _boardHeight;
}