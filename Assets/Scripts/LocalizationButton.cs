using UnityEngine;
using Michsky.MUIP;

[RequireComponent(typeof(ButtonManager))]
public class LocalizationButton : MonoBehaviour
{
    [SerializeField] private string _ru;
    [SerializeField] private string _en;
    [SerializeField] private string _tr;

    private void Start()
    {
        ButtonManager text = GetComponent<ButtonManager>();

        switch (Yandex.Instance.CurrentLanguage)
        {
            case "ru":
                text.buttonText = _ru;
                break;
            case "tr":
                text.buttonText = _tr;
                break;
            default:
                text.buttonText = _en;
                break;
        }
    }
}
