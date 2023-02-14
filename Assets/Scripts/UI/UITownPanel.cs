using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITownPanel : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private UILevelButtonView _template;
    [SerializeField] private Transform _container;
    [SerializeField] private GameObject _gamePanel;

    private List<UILevelButtonView> _levelButtons;
    private int _countryIndex;
    private int _townIndex;

    private void OnDestroy()
    {
        foreach (var levelButton in _levelButtons)
        {
            levelButton.ButtonClicked -= OnLevelButtonClicked;
        }
    }

    public void InitPanel(int countryIndex, int townIndex)
    {
        _countryIndex = countryIndex;
        _townIndex = townIndex;
        CreateLevelButtons();
        UpdateLevelButtons();
    }

    public void UpdateLevelButtons()
    {
        for (int i = 0; i < _levelButtons.Count; i++)
        {
            _levelButtons[i].SetState(_playerStats.GetLevelStatus(_countryIndex, _townIndex, i));
        }
    }

    private void CreateLevelButtons()
    {
        if (_levelButtons != null)
            return;

        _levelButtons = new List<UILevelButtonView>();

        for (int i = 0; i < 24; i++)
        {
            var levelButton = Instantiate(_template, _container) as UILevelButtonView;
            levelButton.SetLevelNumber(i);
            levelButton.ButtonClicked += OnLevelButtonClicked;
            _levelButtons.Add(levelButton);
        }
    }

    private void OnLevelButtonClicked(int levelNumber)
    {
        _game.GenerateLevel(_countryIndex, _townIndex, levelNumber);
        _gamePanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
