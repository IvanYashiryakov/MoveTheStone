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
    [SerializeField] private Color _light;
    [SerializeField] private Color _dark;

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

    private void SetColor(int index)
    {
        Color targetColor;

        if (index == 0 || index == 2 || index == 5)
            targetColor = _dark;
        else
            targetColor = _light;

        _countryName.color = targetColor;

        foreach (var text in _completeLevelsCount)
            text.color = targetColor;
    }

    private void SetCountryAndTowns(int index)
    {
        SetColor(index);
        SetCountryName(index);
        _game.SetBackground(index);
        Town[] towns = _game.Countires[index].Towns;

        for (int i = 0; i < towns.Length; i++)
        {
            _townNames[i].text = Yandex.Instance.CurrentLanguage switch
            {
                "ru" => towns[i].RuName,
                "tr" => towns[i].TrName,
                _ => towns[i].EnName,
            };
        }

        SetTowns();
    }

    private void SetCountryName(int index)
    {
        _countryName.text = Yandex.Instance.CurrentLanguage switch
        {
            "ru" => _game.Countires[index].RuName,
            "tr" => _game.Countires[index].TrName,
            _ => _game.Countires[index].EnName,
        };
    }

    private void SetTowns()
    {
        for (int i = 0; i < _completeLevelsCount.Length; i++)
        {
            _completeLevelsCount[i].text = _playerStats.GetAvailableLevelsCount(_currentCountryIndex, i).ToString() + TotalLevels;
        }
    }
}
