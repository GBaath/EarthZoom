using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatable : MonoBehaviour
{
    public float rotspeed = 100f;
    bool draglock = false;
    bool isDragging = false;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    void Update()
    {
        isDragging = Input.GetMouseButton(0);
    }
    private void FixedUpdate()
    {
        if(!draglock)
            Rotate();
    }
    void Rotate()
    {
        if (!isDragging) return;

        float x = Input.GetAxis("Mouse X") * rotspeed * Time.fixedDeltaTime;
        float y = Input.GetAxis("Mouse Y") * rotspeed * Time.fixedDeltaTime;

        rb.AddTorque(Vector3.down * x);
        rb.AddTorque(Vector3.right * y);
    }
}
