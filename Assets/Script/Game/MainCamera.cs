using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera instance;

    public MapTransitionManager transitionManager;
    [SerializeField] private ParticleSystem cloudsIn;
    [SerializeField] private Animator animator;
    private CameraZoom camZoom;

    private bool moveEnabled;
    public bool draglock = false;
    bool isDragging = false;
    float dragSpeed = 250;

    const string zoomIn = "CameraZoomIn";
    const string zoomInFinish = "CameraFinishZoomIn";
    const string zoomOut = "CameraZoomOut";
    const string zoomOutFinish = "CameraFinishZoomOut";


    private void Start()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }

        instance = this;
        ApplyStartValues();

    }
    private void ApplyStartValues()
    {

        switch (GameManager.cameraProperties.beginSceneZoomFX)
        {
            case GameManager.CameraProperties.BeginSceneZoomEffect.no:
                break;
            case GameManager.CameraProperties.BeginSceneZoomEffect.zoomInEnd:
                FinishZoomiIn();
                break;
            case GameManager.CameraProperties.BeginSceneZoomEffect.zoomOutEnd:
                FinishZoomOut();
                break;
        }

        camZoom = GetComponent<CameraZoom>();
        moveEnabled = GameManager.instance.sceneInfo.cameraMovementEnabled;
    }

    private void Update()
    {
        isDragging = Input.GetMouseButton(0);
    }
    private void FixedUpdate()
    {
        if(!draglock)
            CameraMove();
    }
    private void CameraMove()
    {
        if (!moveEnabled)
            return;


        if (!isDragging) return;

        float x = Input.GetAxis("Mouse X") * -dragSpeed * camZoom.zoomScale * Time.fixedDeltaTime;
        float y = Input.GetAxis("Mouse Y") * -dragSpeed * camZoom.zoomScale * Time.fixedDeltaTime;

        transform.root.position += new Vector3(x, y, 0);
    }





    //Set properties in manager to play on scene enter & play outro cam anim
    public void ZoomInToPoint()
    {
        cloudsIn.Play();
        animator.Play(zoomIn);
        GameManager.cameraProperties.beginSceneZoomFX = GameManager.CameraProperties.BeginSceneZoomEffect.zoomInEnd;
    }
    void FinishZoomiIn()
    {
        cloudsIn.Play();
        animator.Play(zoomInFinish);
        GameManager.cameraProperties.beginSceneZoomFX = GameManager.CameraProperties.BeginSceneZoomEffect.no;
    }
    public void ZoomOutMap()
    {
        animator.Play(zoomOut);
        GameManager.cameraProperties.beginSceneZoomFX = GameManager.CameraProperties.BeginSceneZoomEffect.zoomOutEnd;
    }
    void FinishZoomOut()
    {
        animator.Play(zoomOutFinish);
        GameManager.cameraProperties.beginSceneZoomFX = GameManager.CameraProperties.BeginSceneZoomEffect.no;
    }
}
