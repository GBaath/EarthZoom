using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraTouchMovement : MonoBehaviour
{
    MainCamera mainCam;

    private bool moveEnabled;
    public bool draglock = false;
    bool isDragging = false;

    public float dragSpeed = 250;
    public float angleSoftLimit = 10;
    public float angleHardLimit = 20;
    [SerializeField]float lerpSpeed;

    private float inputx,inputy;

    public Vector3 defaultRot;

    private void Start()
    {
        //camZoom = MainCamera.instance.camZoom;
        mainCam = MainCamera.instance;
        moveEnabled = GameManager.instance.sceneInfo.cameraMovementEnabled;

        ResetDefaultRotation();
    }
    public void ResetDefaultRotation()
    {
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

        //AngleLerpToSoftLimit();
    }
    private void CameraMove(float inX, float inY)
    {
        if (!moveEnabled)
            return;


        if (!isDragging) return;



        float oldX = MathG.AngleCorrectionNegative(transform.root.localEulerAngles.x);
        float oldY = MathG.AngleCorrectionNegative(transform.root.localEulerAngles.y);

        Vector2 vectorFromDefRot = new Vector2(oldX - defaultRot.x, oldY - defaultRot.y);

        //if(oldY < defaultRot.x-angleSoftLimit)
        //    inX -= (1 / (angleHardLimit - angleSoftLimit));
        //else if(oldY > defaultRot.x+angleSoftLimit)
        //    inX += (1/ (angleHardLimit-angleSoftLimit));

        //Debug.Log(inX);
        

        float y =  inX * -dragSpeed * mainCam.camZoom.zoomScale * Time.fixedDeltaTime;
        float x =  inY * dragSpeed * mainCam.camZoom.zoomScale * Time.fixedDeltaTime;


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

        transform.root.rotation = Quaternion.Lerp(Quaternion.Euler(transform.root.localEulerAngles), Quaternion.Euler(defaultRot), lerpSpeed * Time.fixedDeltaTime);


        //transform.root.localEulerAngles = new Vector3 (newX, newY, 0);

    }

}
