using UnityEngine;

[RequireComponent(typeof(OrientationChangeEvent))]
public class ScreenModifier : MonoBehaviour
{
    private OrientationChangeEvent _orientationChangeEvent;
    [SerializeField] private float _landscapeScreenSize;
    [SerializeField] private float _portraitScreenSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _orientationChangeEvent = GetComponent<OrientationChangeEvent>();
        _orientationChangeEvent.OrientationPortrait += IsPortrait;
    }

    private void IsPortrait(bool state)
    {
        Camera.main.orthographicSize = state ? _portraitScreenSize : _landscapeScreenSize;
    }

    void OnDestroy()
    {
        _orientationChangeEvent.OrientationPortrait -= IsPortrait;
    }
}
