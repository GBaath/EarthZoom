using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatable : MonoBehaviour
{
    public float rotspeed = 100f;
    float rotSpeedScale = 1;
    bool draglock = false;
    bool isDragging = false;
    Rigidbody rb;

    float MouseZoomSpeed = 15.0f;
    float TouchZoomSpeed = 0.1f;
    float ZoomMinBound = 13f;
    float ZoomMaxBound = 70f;
    Camera cam;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

        if (!Input.touchSupported)
        {
            rotspeed *= 2;
        }
    }

    void Update()
    {
        isDragging = Input.GetMouseButton(0);

        rotSpeedScale = cam.fieldOfView / ZoomMaxBound;

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
    private void FixedUpdate()
    {
        if(!draglock)
            Rotate();
    }
    void Rotate()
    {
        if (!isDragging) return;

        float x = Input.GetAxis("Mouse X") * rotspeed * rotSpeedScale * Time.fixedDeltaTime;
        float y = Input.GetAxis("Mouse Y") * rotspeed * rotSpeedScale * Time.fixedDeltaTime;

        rb.AddTorque(Vector3.down * x);
        rb.AddTorque(Vector3.right * y);
    }

    void Zoom(float deltaMagnitudeDiff, float speed)
    {

        cam.fieldOfView += deltaMagnitudeDiff * speed;
        // set min and max value of Clamp function upon your requirement
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, ZoomMinBound, ZoomMaxBound);
    }
}
