using System;
using UnityEngine;

public class InputHandler : MonoBehaviour 
{
    public Action<Vector3> MousePosition;

    public Action LeftMouseClickDown;
    
    private void Update()
    {
        MousePosition.Invoke(MouseUtilities.GetMouseWorldPositionWithoutY());

        if (Input.GetMouseButtonDown(0))
            LeftMouseClickDown.Invoke();
    }
}
