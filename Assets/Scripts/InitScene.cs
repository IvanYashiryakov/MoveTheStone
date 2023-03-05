using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using Agava.YandexGames;

public class InitScene : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _background;

    private bool _sdkInitialized = false;
    private Color _startColor = new Color(1, 1, 1, 0);
    private Color _endColor = new Color(1, 1, 1, 1);

    private void OnEnable()
    {
        _background.color = _startColor;
        StartCoroutine(FadeIn());
    }

    private IEnumerator Start()
    {
#if UNITY_EDITOR == false
        yield return YandexGamesSdk.Initialize();
#endif

        _sdkInitialized = true;
        yield return null;
    }

    private IEnumerator FadeIn()
    {
        while (_background.color.a < _endColor.a)
        {
            _background.color = new Color(1, 1, 1, _background.color.a + 0.01f);
            yield return null;
        }

        while (_sdkInitialized == false)
        {
            yield return null;
        }

        SceneManager.LoadScene(1);
    }
}
