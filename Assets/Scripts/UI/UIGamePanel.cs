using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGamePanel : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private GameObject _townPanel;

    public void ExitLevel()
    {
        _game.ExitLevel();
        _townPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
