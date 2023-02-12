using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Item _item;
    [SerializeField] private int _x;
    [SerializeField] private int _y;

    public Item Item => _item;
    public int X => _x;
    public int Y => _y;

    public void Init(Item item, int x, int y)
    {
        _item = item;
        _x = x;
        _y = y;
    }

    public void SetItem(Item item = null)
    {
        _item = item;
    }

    public void DestroyItem()
    {
        Destroy(_item.gameObject);
        SetItem(null);
    }

    public void MoveItemToTile(Tile tile)
    {
        _item.MoveToPosition(tile.transform.position);
        SwapItems(tile);
    }

    public void SwapItems(Tile tile)
    {
        Item swapTileItem = tile.Item;

        _item.transform.parent = tile.transform;
        _item.SetPositionProperties(tile.X, tile.Y);
        _item.gameObject.name = tile.gameObject.name;
        tile.SetItem(_item);
        SetItem(null);

        if (swapTileItem != null)
        {
            swapTileItem.transform.parent = transform;
            swapTileItem.SetPositionProperties(_x, _y);
            swapTileItem.gameObject.name = gameObject.name;
            SetItem(swapTileItem);
        }
    }

    public void SetItemMatched()
    {
        _item.StartDestoryAnimation();
    }
}