using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIMoveCount : MonoBehaviour
{
    private string _textFirstPart = "Ход: ";
    private string _textMiddlePart = " из ";
    private string _textFail = "Провал";

    [SerializeField] private Board _board;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _undoButton;

    private void OnEnable()
    {
        Localization();
        _board.PreviousMovesCountChanged += OnPreviousMovesCountChanged;
        _text.text = _textFirstPart + (_board.CurrentMove).ToString() + _textMiddlePart + _board.Level.MoveCount;
    }

    private void OnDisable()
    {
        _board.PreviousMovesCountChanged -= OnPreviousMovesCountChanged;
    }

    private void OnPreviousMovesCountChanged(int count)
    {
        _undoButton.interactable = count > 1;

        if (count > _board.Level.MoveCount)
        {
            _text.text = _textFail;
        }
        else
        {
            _text.text = _textFirstPart + (count).ToString() + _textMiddlePart + _board.Level.MoveCount;
        }
    }

    private void Localization()
    {
        switch (Yandex.Instance.CurrentLanguage)
        {
            case "ru":
                _textFirstPart = "Ход: ";
                _textMiddlePart = " из ";
                _textFail = "Кончились ходы";
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
