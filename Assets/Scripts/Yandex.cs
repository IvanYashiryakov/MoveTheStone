using UnityEngine;
using Agava.YandexGames;

public class Yandex : MonoBehaviour
{
    [SerializeField] private Game _game;

    public static Yandex Instance;
    public string CurrentLanguage;
    /*
    [DllImport("__Internal")]
    private static extern void ShowRewAdv();
    [DllImport("__Internal")]
    private static extern void ShowAdv();
    [DllImport("__Internal")]
    private static extern string GetLang();
    */

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
        InterstitialAd.Show();
    }

    public void ShowRewardForHint()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif
        VideoAd.Show(null, OnRewardedHint);
    }

    public void OnRewardedHint()
    {
        _game.ButtonHint();
    }
}
