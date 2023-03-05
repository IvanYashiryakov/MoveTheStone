using System.Collections;
using UnityEngine;

public class IntroSound : MonoBehaviour
{
    [SerializeField] private float _secondsToLive = 5f;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_secondsToLive);
        Destroy(gameObject);
    }
}
