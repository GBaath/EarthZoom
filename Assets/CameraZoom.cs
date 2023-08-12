using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public static CameraZoom instance;

    public float zoomScale;

    float MouseZoomSpeed = 15.0f;
    float TouchZoomSpeed = 0.1f;
    float ZoomMinBound = 13f;
    float ZoomMaxBound = 70f;

    Camera cam;
    private void Start()
    {
        cam = GetComponent<Camera>();
        instance = this;
    }
    private void Update()
    {
        if (Input.touchSupported)
        {
            // Pinch to zoom
            if (Input.touchCount == 2)
            {

                // get current touch positions
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);

                // get touch position from the previous frame
                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

                // get offset value
                float deltaDistance = oldTouchDistance - currentTouchDistance;
                Zoom(deltaDistance, TouchZoomSpeed);




                if (cam.fieldOfView < ZoomMinBound)
                {
                    cam.fieldOfView = 0.1f;
                }
                else
                if (cam.fieldOfView > ZoomMaxBound)
                {
                    cam.fieldOfView = 179.9f;
                }
            }
        }
        else
        {

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Zoom(-scroll, MouseZoomSpeed);
        }

        zoomScale = cam.fieldOfView / ZoomMaxBound;
    }
    void Zoom(float deltaMagnitudeDiff, float speed)
    {
        cam.fieldOfView += deltaMagnitudeDiff * speed;

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, ZoomMinBound, ZoomMaxBound);


        if (deltaMagnitudeDiff < 0)
            EventHandler.instance.Call(EventHandler.EventType.zoomIn);
        else
            EventHandler.instance.Call(EventHandler.EventType.zoomOut);
    }
}
