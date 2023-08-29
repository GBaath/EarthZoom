using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera instance;

    public MapTransitionManager transitionManager;
    [SerializeField] private ParticleSystem cloudsIn;
    [SerializeField] private Animator animator;


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
