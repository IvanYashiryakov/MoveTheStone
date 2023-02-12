using UnityEngine;
using TMPro;

public class UIMoveCount : MonoBehaviour
{
    private string _textFirstPart = "���: ";
    private string _textMiddlePart = " �� ";
    private string _textFail = "������";

    [SerializeField] private Board _board;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        Localization();
        _board.PreviousMovesCountChanged += OnPreviousMovesCountChanged;
        _text.text = _textFirstPart + (_board.CurrentMove - 1).ToString() + _textMiddlePart + _board.Level.MoveCount;
    }

    private void OnDisable()
    {
        _board.PreviousMovesCountChanged -= OnPreviousMovesCountChanged;
    }

    private void OnPreviousMovesCountChanged(int count)
    {
        if (count - 1 > _board.Level.MoveCount)
        {
            _text.text = _textFail;
        }
        else
        {
            _text.text = _textFirstPart + (count - 1).ToString() + _textMiddlePart + _board.Level.MoveCount;
        }
    }

    private void Localization()
    {
        switch (Yandex.Instance.CurrentLanguage)
        {
            case "ru":
                _textFirstPart = "���: ";
                _textMiddlePart = " �� ";
                _textFail = "��������� ����";
                break;
            case "tr":
                _textFirstPart = "Hareket: ";
                _textMiddlePart = " dan ";
                _textFail = "Hamleler bitti";
                break;
            default:
                _textFirstPart = "Move: ";
                _textMiddlePart = " of ";
                _textFail = "Out of moves";
                break;
        }
    }
}
