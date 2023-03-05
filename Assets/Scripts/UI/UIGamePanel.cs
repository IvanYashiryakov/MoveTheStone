using UnityEngine;

public class UIGamePanel : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private UITownPanel _townPanel;
    [SerializeField] private GameObject _doneButton;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private ParticleSystem _doneEffect;

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

    public void OnLevelDone(bool isDoneFirstTime)
    {
        if (isDoneFirstTime)
        {
            _doneEffect.Play();
            _doneEffect.GetComponent<AudioSource>().Play();
        }

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
        Yandex.Instance.ShowInterstitial();
        _doneButton.SetActive(false);

        if (_game.TryLoadNextLevelInTown() == false)
        {
            ExitLevel();
        }
    }

    private void OnUndoClick(bool isLevelFailed)
    {
        _restartButton.SetActive(isLevelFailed);
    }
}
