using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizationText : MonoBehaviour
{
    [SerializeField] private string _ru;
    [SerializeField] private string _en;
    [SerializeField] private string _tr;

    private void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        switch (Yandex.Instance.CurrentLanguage)
        {
            case "ru":
                text.text = _ru;
                break;
            case "tr":
                text.text = _tr;
                break;
            default:
                text.text = _en;
                break;
        }
    }
}
