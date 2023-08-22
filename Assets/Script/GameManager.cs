using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public static GameManager instance;

    public struct CameraProperties
    {
        public enum BeginSceneZoomEffect
        {
            no, zoomInEnd, zoomOutEnd
        }
        public BeginSceneZoomEffect beginSceneZoomFX;

    }
    public static CameraProperties cameraProperties;

    private void Start()
    {
        if (instance)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }


    }

}
