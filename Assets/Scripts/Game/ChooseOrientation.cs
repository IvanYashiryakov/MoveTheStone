using UnityEngine;

public class ChooseOrientation : MonoBehaviour
{
    [SerializeField] private RectTransform _portraitUI;
    [SerializeField] private RectTransform _landscapeUI;

    private readonly Vector3 _cameraPortraitPostion = new Vector3(3f, 5.1f, -10);
    private readonly float _cameraPortraitSize = 6.24f;

    private readonly Vector3 _cameraLandscapePostion = new Vector3(3f, 4, -10);
    private readonly float _cameraLandscapeSize = 5;

    private void OnEnable()
    {
        SetOrientation();
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
