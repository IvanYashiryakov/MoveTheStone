using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Hint : MonoBehaviour
{
    [SerializeField] private Vector2 _startPosition;
    [SerializeField] private DirectionType _direction;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Vector2 _endPosition;
    private Coroutine _corutine;

    public void Init(Vector2 startPosition, DirectionType direction)
    {
        _startPosition = startPosition;
        _direction = direction;
    }

    public void StopMoving()
    {
        if (_corutine != null)
            StopCoroutine(_corutine);

        _spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    public void StartMoving()
    {
        transform.position = _startPosition;
        _spriteRenderer.color = new Color(1, 1, 1, 0);
        DetermineEndPosition();

        if (_corutine != null)
            StopCoroutine(_corutine);

        _corutine = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (_spriteRenderer.color.a < 1)
        {
            float alpha = Mathf.Lerp(_spriteRenderer.color.a, 2f, Time.deltaTime);
            _spriteRenderer.color = new Color(1, 1, 1, alpha);

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        while((Vector2)transform.position != _endPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, _endPosition, 0.05f);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        while (_spriteRenderer.color.a > 0)
        {
            float alpha = Mathf.Lerp(_spriteRenderer.color.a, -1f, Time.deltaTime);
            _spriteRenderer.color = new Color(1, 1, 1, alpha);

            yield return null;
        }

        StartMoving();
    }

    private void DetermineEndPosition()
    {
        float x = 0;
        float y = 0;

        switch (_direction)
        {
            case DirectionType.Up:
                y = 1;
                break;
            case DirectionType.Right:
                x = 1;
                break;
            case DirectionType.Down:
                y = -1;
                break;
            case DirectionType.Left:
                x = -1;
                break;
        }

        _endPosition = new Vector2(_startPosition.x + x, _startPosition.y + y);
    }
}
