using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class CameraTouchMovement : MonoBehaviour
{
    private CameraZoom camZoom;

    private bool moveEnabled;
    public bool draglock = false;
    bool isDragging = false;

    public float dragSpeed = 250;
    public float angleSoftLimit = 10;
    public float angleHardLimit = 20;
    [SerializeField]float lerpSpeed;

    private float inputx,inputy;

    [SerializeField]private Vector3 defaultRot;

    private void Start()
    {        
        camZoom = GetComponent<CameraZoom>();
        moveEnabled = GameManager.instance.sceneInfo.cameraMovementEnabled;

        defaultRot = transform.root.localEulerAngles;
    }
    private void Update()
    {
        isDragging = Input.GetMouseButton(0);
    }
    private void FixedUpdate()
    {
        inputx = Input.GetAxis("Mouse X");
        inputy = Input.GetAxis("Mouse Y");

        if (!draglock)
            CameraMove(inputx,inputy);

        AngleLerpToSoftLimit();
    }
    private void CameraMove(float inX, float inY)
    {
        if (!moveEnabled)
            return;


        if (!isDragging) return;


        float oldX = transform.root.localEulerAngles.x;
        float oldY = transform.root.localEulerAngles.y;



        float y =  inX * -dragSpeed * camZoom.zoomScale * Time.fixedDeltaTime;
        float x =  inY * dragSpeed * camZoom.zoomScale * Time.fixedDeltaTime;


        //unity funny eulerangle has no -values
        float newX = MathG.AngleClampNegative(oldX + x, -angleHardLimit + defaultRot.x, angleHardLimit + defaultRot.x);
        float newY = MathG.AngleClampNegative(oldY + y, -angleHardLimit + defaultRot.y, angleHardLimit + defaultRot.y);


        Vector3 newRot = new Vector3(newX, newY, 0);


        transform.root.localEulerAngles = newRot;
    }

    private void AngleLerpToSoftLimit()
    {
        float x = transform.root.localEulerAngles.x;
        float y = MathG.AngleCorrectionNegative(transform.root.localEulerAngles.y);


        //is beyond lerpLimit shit dont work
        bool lerpX = x > angleSoftLimit+defaultRot.x || x < -angleSoftLimit+defaultRot.x;
        bool lerpY = y > angleSoftLimit+defaultRot.y || y < -angleSoftLimit+defaultRot.y;



        if (!lerpY && !lerpX)
            return;

        lerpSpeed = new Vector2(x, y).magnitude / new Vector2(defaultRot.x+x, defaultRot.y+y).magnitude;
        //lerpSpeed += angleSoftLimit / angleHardLimit;

        transform.root.rotation = Quaternion.Lerp(Quaternion.Euler(transform.root.localEulerAngles), Quaternion.Euler(defaultRot), lerpSpeed);


        //transform.root.localEulerAngles = new Vector3 (newX, newY, 0);

    }

}
