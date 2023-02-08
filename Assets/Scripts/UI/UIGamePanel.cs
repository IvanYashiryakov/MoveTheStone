using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePanel : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private UITownPanel _townPanel;
    [SerializeField] private GameObject _doneButton;

    private void OnEnable()
    {
        _game.LevelDone += OnLevelDone;
    }

    private void OnDisable()
    {
        _game.LevelDone -= OnLevelDone;
    }

    public void OnLevelDone()
    {
        _doneButton.SetActive(true);
    }

    public void ExitLevel()
    {
        _game.ExitLevel();
        _doneButton.SetActive(false);
        _townPanel.gameObject.SetActive(true);
        _townPanel.UpdateLevelButtons();
        gameObject.SetActive(false);
    }

    public void ButtonDoneClick()
    {
        _doneButton.SetActive(false);

        if (_game.TryLoadNextLevelInTown() == false)
        {
            ExitLevel();
        }
    }
}
