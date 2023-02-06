using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITownPanel : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private UILevelButtonView _template;
    [SerializeField] private Transform _container;
    [SerializeField] private PlayerStats _playerStats;
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

    private void Start()
    {
        _levelButtons = new List<UILevelButtonView>();

        for (int i = 0; i < 24; i++)
        {
            var levelButton = Instantiate(_template, _container) as UILevelButtonView;
            levelButton.SetLevelNumber(i);
            levelButton.ButtonClicked += OnLevelButtonClicked;
            _levelButtons.Add(levelButton);
        }
    }

    public void InitPanel(int countryIndex, int townIndex)
    {
        _countryIndex = countryIndex;
        _townIndex = townIndex;
    }

    private void OnLevelButtonClicked(int levelNumber)
    {
        _game.GenerateLevel(_countryIndex, _townIndex, levelNumber);
        _gamePanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
