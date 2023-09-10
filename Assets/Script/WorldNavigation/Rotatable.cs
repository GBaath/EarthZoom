using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rotatable : MonoBehaviour
{
    public float rotspeed = 25f;
    float rotSpeedScale = 1;
    
    
    
    public bool draglock = false;
    bool isDragging = false;

    float oldAngle;
    
    Rigidbody rb;

    CameraZoom camZoom;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camZoom = Camera.main.GetComponent<CameraZoom>();
        if(!camZoom )
            Camera.main.AddComponent<CameraZoom>();

        //mousemove is slower
        if (!Input.touchSupported)
        {
            rotspeed *= 2;
        }
    }

    void Update()
    {
        isDragging = Input.GetMouseButton(0);

        //scaling with zoom level
        rotSpeedScale = camZoom.zoomScale;

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



}
