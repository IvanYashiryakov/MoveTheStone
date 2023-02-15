using UnityEngine;
using UnityEngine.UI;

public class UIGamePanel : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private UITownPanel _townPanel;
    [SerializeField] private GameObject _doneButton;
    [SerializeField] private GameObject _restartButton;

    private int _doneClickCount = 0;

    private void OnEnable()
    {
        _game.LevelDone += OnLevelDone;
        _game.LevelFailed += OnLevelFailed;
        _game.PreviousMoveLoaded += OnUndoClick;
    }

    private void OnDisable()
    {
        _game.LevelDone -= OnLevelDone;
        _game.LevelFailed -= OnLevelFailed;
        _game.PreviousMoveLoaded -= OnUndoClick;
    }

    public void OnLevelFailed()
    {
        _restartButton.SetActive(true);
    }

    public void OnLevelDone()
    {
        _doneButton.SetActive(true);
    }

    public void ExitLevel()
    {
        _game.ExitLevel();
        _doneButton.SetActive(false);
        _restartButton.SetActive(false);
        _townPanel.gameObject.SetActive(true);
        _townPanel.UpdateLevelButtons();
        gameObject.SetActive(false);
    }

    public void ButtonRestartClick()
    {
        _restartButton.SetActive(false);
        _game.RestartLevel();
    }

    public void ButtonDoneClick()
    {
        _doneClickCount++;
        _doneButton.SetActive(false);
        if (_game.TryLoadNextLevelInTown() == false)
        {
            Yandex.Instance.ShowInterstitial();
            _doneClickCount = 0;
            ExitLevel();

            return;
        }

        if(_doneClickCount >= 3)
        {
            Yandex.Instance.ShowInterstitial();
            _doneClickCount = 0;
        }
    }

    private void OnUndoClick(bool isLevelFailed)
    {
        _restartButton.SetActive(isLevelFailed);
    }
}
