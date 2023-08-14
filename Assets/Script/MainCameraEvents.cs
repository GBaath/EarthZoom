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
            Destroy(gameObject);

        instance = this;

        DontDestroyOnLoad(instance.gameObject);
    }


    public void ZoomIn()
    {
        cloudsIn.Play();
        animator.Play("CameraZoomIn");
    }
    public void FinishZoomiIn()
    {
        cloudsIn.Play();
        animator.Play("CameraFinishZoomIn");
    }
    public void ZoomOut()
    {
        cloudsOut.Play();
    }
}
