using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Item : MonoBehaviour
{
    [SerializeField] private BoxType _id;
    [SerializeField] private int _x;
    [SerializeField] private int _y;

    private Coroutine _corutine;
    private Animator _animator;
    private float _fallSpeed = 13f;

    [HideInInspector] public UnityAction<Item> Dropped;
    [HideInInspector] public UnityAction<Item> Destroyed;
    [HideInInspector] public UnityAction<Item, Item, bool> Moved;

    public BoxType Id => _id;
    public int X => _x;
    public int Y => _y;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetPositionProperties(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public void MoveToPosition(Vector2 targetPosition)
    {
        if (_corutine != null)
            StopCoroutine(_corutine);

        _corutine = StartCoroutine(Move(targetPosition));
    }

    public void StartDestoryAnimation()
    {
        _animator.SetTrigger("Destroy");
    }

    public void DestroySelf()
    {
        Destroyed?.Invoke(this);
        Destroy(gameObject);
    }

    private IEnumerator Move(Vector2 targetPosition)
    {
        Vector2 currentPosition = transform.position;

        while(currentPosition != targetPosition)
        {
            Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, _fallSpeed * Time.deltaTime);
            transform.position = newPosition;

            currentPosition = transform.position;
            yield return null;
        }

        Dropped?.Invoke(this);
    }
}