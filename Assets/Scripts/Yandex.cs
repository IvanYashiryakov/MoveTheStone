using UnityEngine;
using Agava.YandexGames;

public class Yandex : MonoBehaviour
{
    [SerializeField] private Game _game;

    public static Yandex Instance;
    public string CurrentLanguage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
#if UNITY_EDITOR == false
            CurrentLanguage = YandexGamesSdk.Environment.i18n.lang;
#endif
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowInterstitial()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif
        InterstitialAd.Show(OnOpenCallback, OnCloseCallback);
    }

    public void ShowRewardForHint()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif
        VideoAd.Show(OnOpenCallback, OnRewardedHint, OnCloseCallback);
    }

    public void OnRewardedHint()
    {
        _game.ButtonHint();
        OnCloseCallback();
    }

    private void OnOpenCallback()
    {
        AudioListener.volume = 0f;
    }

    private void OnCloseCallback(bool b)
    {
        OnCloseCallback();
    }

    private void OnCloseCallback()
    {
        AudioListener.volume = 1f;
    }
}
