using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera instance;

    public MapTransitionManager transitionManager;
    [SerializeField] private ParticleSystem cloudsIn, cloudsOut;
    [SerializeField] private Animator animator;


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
        Debug.Log(GameManager.cameraProperties.beginSceneZoomFX);

        switch (GameManager.cameraProperties.beginSceneZoomFX)
        {
            case GameManager.CameraProperties.BeginSceneZoomEffect.no:
                break;
            case GameManager.CameraProperties.BeginSceneZoomEffect.zoomInEnd:
                FinishZoomiIn();
                break;
            case GameManager.CameraProperties.BeginSceneZoomEffect.zoomOutEnd:
                break;
        }
    }

    public void ZoomIn()
    {
        cloudsIn.Play();
        animator.Play("CameraZoomIn");
        GameManager.cameraProperties.beginSceneZoomFX = GameManager.CameraProperties.BeginSceneZoomEffect.zoomInEnd;
    }
    public void FinishZoomiIn()
    {
        cloudsIn.Play();
        animator.Play("CameraFinishZoomIn");
        GameManager.cameraProperties.beginSceneZoomFX = GameManager.CameraProperties.BeginSceneZoomEffect.no;
    }
    public void ZoomOut()
    {
        cloudsOut.Play();
    }
}
