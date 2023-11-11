using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOperation : MonoBehaviour
{


    public float CameraMoveOffset = 1f;
    public float CameraScrollOffset = 1f;
    
    
    
    private void Update()
    {
        
        MouseDrag();
        MouseScroll();
    }


    void MouseDrag()
    {

        if (Input.GetMouseButton(1))
        {

            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x - Input.GetAxis("Mouse X") * CameraMoveOffset,
                Camera.main.transform.position.y - Input.GetAxis("Mouse Y") * CameraMoveOffset,
                Camera.main.transform.position.z);
        }
        
    }

    void MouseScroll()
    {

        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * CameraScrollOffset;

    }
    
    
    
    
}
