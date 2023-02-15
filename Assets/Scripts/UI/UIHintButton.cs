using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHintButton : MonoBehaviour
{
    [SerializeField] private Board _board;
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        //_board.HintStarted += OnHintStarted;
    }

    private void OnDisable()
    {
        //_board.HintStarted -= OnHintStarted;
    }

    private void OnHintStarted()
    {
        Debug.Log("HintStarted button");
        if (_board.IsShowingHint == true)
            _button.interactable = false;
    }
}
