using System.Runtime.InteropServices;
using UnityEngine;

public class Yandex : MonoBehaviour
{
    [SerializeField] private Game _game;

    public static Yandex Instance;
    public string CurrentLanguage;

    [DllImport("__Internal")]
    private static extern void ShowRewAdv();
    [DllImport("__Internal")]
    private static extern void ShowAdv();
    [DllImport("__Internal")]
    private static extern string GetLang();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
#if UNITY_EDITOR == false
            CurrentLanguage = GetLang();
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
#if UNITY_EDITOR == false
        ShowAdv();
#endif
    }

    public void ShowRewardForHint()
    {
#if UNITY_EDITOR == false
        ShowRewAdv();
#endif
    }

    public void OnRewardedHint()
    {
        _game.ButtonHint();
    }
}
