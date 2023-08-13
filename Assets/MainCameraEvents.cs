using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera instance;

    public MapTransitionManager transitionManager;


    private void Start()
    {
        if (instance)
            Destroy(gameObject);

        instance = this;

        DontDestroyOnLoad(instance.gameObject);
    }


    public void ZoomIn()
    {

    }
    public void ZoomOut()
    {

    }
}
