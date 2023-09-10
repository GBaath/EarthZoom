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

    public float dragSpeed = 25;
    public float posSoftLimit = 10;
    public float posHardLimit = 20;
    [SerializeField]float lerpSpeed;

    private float inputx,inputy;

    public Vector3 defaultPos;

    private void Start()
    {
        //camZoom = MainCamera.instance.camZoom;
        mainCam = MainCamera.instance;
        moveEnabled = GameManager.instance.sceneInfo.cameraMovementEnabled;

        ResetDefaultRotation();
    }
    public void ResetDefaultRotation()
    {
        defaultPos = transform.root.position;//localEulerAngles;
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


        if (!isDragging) 
            return;



        float oldX = transform.root.position.x;//MathG.AngleCorrectionNegative(transform.root.localEulerAngles.x);
        float oldY = transform.root.position.y;//MathG.AngleCorrectionNegative(transform.root.localEulerAngles.y);

        Vector2 vectorFromDefPos = new Vector2(oldX - defaultPos.x, oldY - defaultPos.y);

        //if(oldY < defaultRot.x-angleSoftLimit)
        //    inX -= (1 / (angleHardLimit - angleSoftLimit));
        //else if(oldY > defaultRot.x+angleSoftLimit)
        //    inX += (1/ (angleHardLimit-angleSoftLimit));

        //Debug.Log(inX);
        

        float x =  inX * -dragSpeed * mainCam.camZoom.zoomScale * Time.fixedDeltaTime;
        float y =  inY * -dragSpeed * mainCam.camZoom.zoomScale * Time.fixedDeltaTime;


        //unity funny eulerangle has no -values
        float newX = Mathf.Clamp(oldX + x, -posHardLimit + defaultPos.x, posHardLimit + defaultPos.x);
        float newY = Mathf.Clamp(oldY + y, -posHardLimit + defaultPos.y, posHardLimit + defaultPos.y);



        Vector3 newPos = new Vector3(newX, newY, transform.root.position.z);


        transform.root.transform.position = newPos;
    }

    private void AngleLerpToSoftLimit()
    {
        float x = transform.root.localEulerAngles.x;
        float y = MathG.AngleCorrectionNegative(transform.root.localEulerAngles.y);


        //is beyond lerpLimit shit dont work
        bool lerpX = x > posSoftLimit+defaultPos.x || x < -posSoftLimit+defaultPos.x;
        bool lerpY = y > posSoftLimit+defaultPos.y || y < -posSoftLimit+defaultPos.y;



        if (!lerpY && !lerpX)
            return;

        lerpSpeed = new Vector2(x, y).magnitude / new Vector2(defaultPos.x+x, defaultPos.y+y).magnitude;
        //lerpSpeed += angleSoftLimit / angleHardLimit;

        transform.root.rotation = Quaternion.Lerp(Quaternion.Euler(transform.root.localEulerAngles), Quaternion.Euler(defaultPos), lerpSpeed * Time.fixedDeltaTime);


        //transform.root.localEulerAngles = new Vector3 (newX, newY, 0);

    }

}
