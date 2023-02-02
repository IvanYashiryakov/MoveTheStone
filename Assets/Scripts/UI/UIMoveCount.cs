using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoveCount : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Board _board;

    private void OnEnable()
    {
        _board.PreviousMovesCountChanged += OnPreviousMovesCountChanged;
    }

    private void OnDisable()
    {
        _board.PreviousMovesCountChanged -= OnPreviousMovesCountChanged;
    }

    private void OnPreviousMovesCountChanged(int count)
    {
        _text.text = (count - 1).ToString();
    }
}
