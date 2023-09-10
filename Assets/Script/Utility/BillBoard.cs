using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public bool constantScale;
    public bool lockRotationXZ = false;
    public bool fliptoPlayer = false;

    private Vector3 startScale;


    private SpriteRenderer sr;

    private void Start()
    {

        sr = GetComponent<SpriteRenderer>();
        if(!sr)
            sr = gameObject.AddComponent<SpriteRenderer>();

        if (!constantScale)
            return;

        startScale = transform.localScale;
        
    }

    void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        if (!lockRotationXZ)
        {
            transform.rotation = rotation;
        }
        else
        {
            transform.rotation = rotation;
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }

        if (fliptoPlayer)
        {
            Vector2 posOnScreen = Camera.main.WorldToViewportPoint(transform.position);
            try
            {
                if (posOnScreen.x > 0.6)
                    sr.flipX = true;
                else if (posOnScreen.x < 0.4)
                    sr.flipX = false;
            }
            catch
            {

            }
        }

        if (constantScale)
        {
            transform.localScale = (startScale * (1+CameraZoom.instance.defaultScalingFactor)) * (CameraZoom.instance.zoomScale);
        }
    }
}
