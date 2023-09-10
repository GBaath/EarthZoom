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

    float xAngleMaxZoom = 20;
    float defaultZoomAngle;


    float oldAngle;

    [SerializeField] List<Camera> cams;
    private void Start()
    {

        instance = this;

        defaultZoomAngle = transform.root.localEulerAngles.x;
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


            }
        }
        else
        {

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Zoom(-scroll, MouseZoomSpeed);
        }

        //clamping
        if (cams[0].fieldOfView < ZoomMinBound)
        {
            foreach(Camera cam in cams)
            {
                cam.fieldOfView = ZoomMinBound;
            }
        }
        else
        if (cams[0].fieldOfView > ZoomMaxBound)
        {
            foreach (Camera cam in cams)
            {
                cam.fieldOfView = ZoomMaxBound;
            }
        }

        zoomScale = cams[0].fieldOfView / ZoomMaxBound;


        if (!GameManager.instance.sceneInfo.cameraMovementEnabled)
            return;

    }
    void Zoom(float deltaMagnitudeDiff, float speed)
    {
        foreach (Camera cam in cams)
        {
            cam.fieldOfView += deltaMagnitudeDiff * speed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, ZoomMinBound, ZoomMaxBound);
        }



        if (deltaMagnitudeDiff < 0)
            EventHandler.instance.Call(EventHandler.EventType.zoomIn);
        else
            EventHandler.instance.Call(EventHandler.EventType.zoomOut);

        //SetZoomAngle(); BROKEN
    }
    void SetZoomAngle()
    {
        Vector3 v = MainCamera.instance.cameraTouchMovement.defaultPos;

        float newXAngle = defaultZoomAngle - (zoomScale * xAngleMaxZoom) + v.x;
        Debug.Log(newXAngle);

        transform.root.localEulerAngles = new Vector3 (newXAngle, v.y, v.z);
        MainCamera.instance.cameraTouchMovement.ResetDefaultRotation();
    }
}
