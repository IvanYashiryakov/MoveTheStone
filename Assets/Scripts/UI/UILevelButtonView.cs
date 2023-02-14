using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UILevelButtonView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Sprite _spriteDone;
    [SerializeField] private Sprite _spriteActive;
    [SerializeField] private Sprite _spriteInactive;

    private int _levelNumber;

    public event UnityAction<int> ButtonClicked;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    public void SetLevelNumber(int levelNumber)
    {
        _levelNumber = levelNumber;
        _text.text = (levelNumber + 1).ToString();
    }

    public void SetState(ProgressStatus status)
    {
        Sprite sprite = null;
        _button.enabled = true;

        switch (status)
        {
            case ProgressStatus.Active:
                sprite = _spriteActive;
                break;
            case ProgressStatus.Inactive:
                sprite = _spriteInactive;
                _button.enabled = false;
                break;
            case ProgressStatus.Done:
                sprite = _spriteDone;
                break;
        }

        _button.GetComponent<Image>().sprite = sprite;
    }

    private void OnButtonClick()
    {
        ButtonClicked?.Invoke(_levelNumber);
    }
}
