using System;
using UnityEngine;

public class OrientationChangeEvent : MonoBehaviour
{
    public Action<bool> OrientationPortrait;
    private int _screenWidth;

    void Start()
    {
        _screenWidth = 0;
        CheckOrientaion();
    }

    void Update()
    {
        CheckOrientaion();
    }

    private void CheckOrientaion()
    {
        if (_screenWidth == Screen.width) return;

        _screenWidth = Screen.width;

        if (Screen.height >= _screenWidth)
            OrientationPortrait.Invoke(true);
        else
            OrientationPortrait.Invoke(false);
    }
}
