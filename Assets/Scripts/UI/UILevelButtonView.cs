using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UILevelButtonView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _text;

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

    private void OnButtonClick()
    {
        ButtonClicked?.Invoke(_levelNumber);
    }
}
