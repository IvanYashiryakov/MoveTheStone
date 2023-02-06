using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseOrientation : MonoBehaviour
{
    [SerializeField] private RectTransform _portraitUI;
    [SerializeField] private RectTransform _landscapeUI;

    private readonly Vector3 _cameraPortraitPostion = new Vector3(3, 5, -10);
    private readonly float _cameraPortraitSize = 7;

    private readonly Vector3 _cameraLandscapePostion = new Vector3(3, 4, -10);
    private readonly float _cameraLandscapeSize = 5;

    private readonly float _delayTime = 0.5f;

    private void OnEnable()
    {
        StartCoroutine(CheckOrientationChange());
    }

    private IEnumerator CheckOrientationChange()
    {
        WaitForSeconds delay = new WaitForSeconds(_delayTime);

        while (this.enabled == true)
        {
            SetOrientation();

            yield return delay;
        }
    }

    private void SetOrientation()
    {
        if (Camera.main.pixelWidth > Camera.main.pixelHeight)
        {
            Camera.main.transform.position = _cameraLandscapePostion;
            Camera.main.orthographicSize = _cameraLandscapeSize;
            _portraitUI.gameObject.SetActive(false);
            _landscapeUI.gameObject.SetActive(true);
        }
        else
        {
            Camera.main.transform.position = _cameraPortraitPostion;
            Camera.main.orthographicSize = _cameraPortraitSize;
            _landscapeUI.gameObject.SetActive(false);
            _portraitUI.gameObject.SetActive(true);
        }
    }
}
