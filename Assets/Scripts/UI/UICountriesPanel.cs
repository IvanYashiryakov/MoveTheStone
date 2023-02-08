using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICountriesPanel : MonoBehaviour
{
    private const string TotalLevels = " / 24";

    [SerializeField] private Game _game;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private TMP_Text _countryName;
    [SerializeField] private TMP_Text[] _townNames;
    [SerializeField] private TMP_Text[] _completeLevelsCount;
    [SerializeField] private UITownPanel _townPanel;

    private int _currentCountryIndex = 0;

    private void OnEnable()
    {
        SetTowns();
    }

    private void Start()
    {
        SetCountryAndTowns(_currentCountryIndex);
    }

    public void ButtonNextCountry()
    {
        _currentCountryIndex++;

        if (_currentCountryIndex >= _game.Countires.Length)
            _currentCountryIndex = 0;

        SetCountryAndTowns(_currentCountryIndex);
    }

    public void ButtonPrevCountry()
    {
        _currentCountryIndex--;

        if (_currentCountryIndex < 0)
            _currentCountryIndex = _game.Countires.Length - 1;

        SetCountryAndTowns(_currentCountryIndex);
    }

    public void ButtonTownClicked(int townIndex)
    {
        _townPanel.gameObject.SetActive(true);
        _townPanel.InitPanel(_currentCountryIndex, townIndex);
        gameObject.SetActive(false);
    }

    private void SetCountryAndTowns(int index)
    {
        _countryName.text = _game.Countires[index].Name;
        Town[] towns = _game.Countires[index].Towns;

        for (int i = 0; i < towns.Length; i++)
        {
            _townNames[i].text = towns[i].Name;
        }

        SetTowns();
    }

    private void SetTowns()
    {
        for (int i = 0; i < _completeLevelsCount.Length; i++)
        {
            _completeLevelsCount[i].text = _playerStats.GetAvailableLevelsCount(_currentCountryIndex, i).ToString() + TotalLevels;
        }
    }
}
