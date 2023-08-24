using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager: MonoBehaviour, IRecieveEvents
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

    public static Vector3 currentPlanetOrientation;

    public SceneInfo sceneInfo;


    private void Start()
    {
        if (instance)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }

        EventSubscribe();

    }

    //SCENE INFO
    void SaveSceneInfo()
    {

        if(sceneInfo.findPlanet)
            UpdatePlanetRotation();
    }
    public void GetCurrentSceneInfo(out SceneInfo sceneInfo)
    {
        sceneInfo = this.sceneInfo;
    }
    private void UpdatePlanetRotation()
    {
        if(sceneInfo.planet)
            currentPlanetOrientation = sceneInfo.planet.eulerAngles;
    }

    public void EventSubscribe()
    {
        EventHandler.instance.AddReciever(this);
    }

    public void EventCall(EventHandler.EventType type)
    {
        switch (type)
        {
            case EventHandler.EventType.sceneSwitch:
                SaveSceneInfo();
                break;
            case EventHandler.EventType.zoomIn:
                UpdatePlanetRotation();
                break;
            default:
                break;
        }
    }
}
