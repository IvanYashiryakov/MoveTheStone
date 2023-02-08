using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMoveCount : MonoBehaviour
{
    private const string TextFirstPart = "Ход: ";
    private const string TextMiddlePart = " из ";

    [SerializeField] private Board _board;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _board.PreviousMovesCountChanged += OnPreviousMovesCountChanged;
        _text.text = TextFirstPart + (_board.CurrentMove - 1).ToString() + TextMiddlePart + _board.Level.MoveCount;
    }

    private void OnDisable()
    {
        _board.PreviousMovesCountChanged -= OnPreviousMovesCountChanged;
    }

    private void OnPreviousMovesCountChanged(int count)
    {
        if (count - 1 > _board.Level.MoveCount)
        {
            _text.text = "Провал";
        }
        else
        {
            _text.text = TextFirstPart + (count - 1).ToString() + TextMiddlePart + _board.Level.MoveCount;
        }
    }
}
