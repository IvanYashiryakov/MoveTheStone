using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class ItemMovement : MonoBehaviour
{
    private Item _item;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private float _maxDistance = 1f;
    private float _offsetX = 0f;
    private float _offsetY = 0f;
    private Vector2 _newPosition;
    private int _direction = -1;    // -1 - N/A, 0 - vertical, 1 - horizontal
    private float _distanceToDetectDirection = 0.04f;
    private float _distanceToChangePosition = 0.65f;

    private Transform _potentialSwapItem;
    private bool isSelected = false;

    private void Start()
    {
        _item = GetComponent<Item>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        if (Game.CanMove == true)
            InitMoving();
    }

    private void OnMouseDrag()
    {
        if (Game.CanMove == true)
        {
            Move();
            MovePotentialSwapItem();
        }
    }

    private void OnMouseUp()
    {
        if (Game.CanMove == true)
            EndMoving();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isSelected == true)
        {
            if (_potentialSwapItem != null)
            {
                _potentialSwapItem.position = _potentialSwapItem.parent.position;
                _potentialSwapItem = null;
            }

            _potentialSwapItem = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isSelected == true && collision.transform == _potentialSwapItem)
        {
            if(Vector2.Distance(_potentialSwapItem.position, _potentialSwapItem.parent.position) < 0.5f)
            {
                ResetPotencialSwapItem();
            }
        }
    }

    private void InitMoving()
    {
        isSelected = true;
        _newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _offsetX = _newPosition.x - transform.position.x;
        _offsetY = _newPosition.y - transform.position.y;
        _direction = -1;
        _spriteRenderer.sortingOrder = 2;
        _rigidbody.isKinematic = false;
    }

    private void Move()
    {
        _newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _newPosition = new Vector2(_newPosition.x - _offsetX, _newPosition.y - _offsetY);

        if (_direction == -1)
            _direction = GetDirection(_newPosition);

        _newPosition = GetPositionInDirection(_direction);
        _newPosition = GetRestrictedPosition(_newPosition, transform.parent.position);

        transform.position = _newPosition;
    }

    private void MovePotentialSwapItem()
    {
        if (_potentialSwapItem != null)
        {
            Vector2 newPosition = (Vector2)_potentialSwapItem.parent.position - (_newPosition - (Vector2)transform.parent.position);
            newPosition = GetRestrictedPosition(newPosition, _potentialSwapItem.parent.transform.position);
            _potentialSwapItem.position = newPosition;

            if (Vector2.Distance(_potentialSwapItem.position, transform.position) > _maxDistance)
            {
                ResetPotencialSwapItem();
            }
        }
    }

    private void ResetPotencialSwapItem()
    {
        if (_potentialSwapItem != null)
        {
            _potentialSwapItem.position = _potentialSwapItem.parent.position;
            _potentialSwapItem = null;
        }
    }

    private void EndMoving()
    {
        isSelected = false;
        _spriteRenderer.sortingOrder = 1;
        _rigidbody.isKinematic = true;

        if (Vector2.Distance(transform.position, transform.parent.position) < _distanceToChangePosition)
        {
            transform.position = transform.parent.position;
            ResetPotencialSwapItem();
            _item.Moved?.Invoke(_item, null, false);
        }
        else
        {
            _item.Moved?.Invoke(_item, _potentialSwapItem?.GetComponent<Item>(), true);
        }
    }

    private int GetDirection(Vector2 newPosition)
    {
        if (newPosition.y - _distanceToDetectDirection > transform.position.y
            || newPosition.y + _distanceToDetectDirection < transform.position.y)
            return 0;

        if (newPosition.x - _distanceToDetectDirection > transform.position.x
            || newPosition.x + _distanceToDetectDirection < transform.position.x)
            return 1;

        return -1;
    }

    private Vector2 GetPositionInDirection(int direction)
    {
        return direction switch
        {
            0 => new Vector2(transform.parent.position.x, _newPosition.y),
            _ => new Vector2(_newPosition.x, transform.parent.position.y),
        };
    }

    private Vector2 GetRestrictedPosition(Vector2 position, Vector2 relativePosition)
    {
        float minX = Mathf.Max(relativePosition.x - _maxDistance, 0f);
        float maxX = Mathf.Min(relativePosition.x + _maxDistance, Board.Width - 1);

        float minY = Mathf.Max(relativePosition.y - _maxDistance, 0f);
        float maxY = Mathf.Min(relativePosition.y + _maxDistance, Board.Height - 1);

        float x = Mathf.Clamp(position.x, minX, maxX);
        float y = Mathf.Clamp(position.y, minY, maxY);

        position = new Vector2(x, y);

        return position;
    }
}