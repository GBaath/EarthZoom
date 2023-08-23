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

    public struct SceneInfo
    {
        public string zoomOutSceneName;
        public Vector3 currentPlanetOrientation;
        public Transform planet;
    }
    private static SceneInfo currentSceneInfo;

    //save through scenes
    private static string zoomOutName;
    private static Vector3 currentPlanetOrientation;


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
        SetSceneInfoVariables();
    }
    private void SetSceneInfoVariables()
    {
        LoadSceneInfoToStruct();

        //TODO REPLACE WHEN CHANGE PLANET TO A CLASS
        currentSceneInfo.planet = GameObject.Find("planet").transform;
        if(currentSceneInfo.currentPlanetOrientation == null)
            currentSceneInfo.currentPlanetOrientation = Vector3.zero;
        if(currentSceneInfo.zoomOutSceneName == null)
            currentSceneInfo.zoomOutSceneName = MapTransitionManager.namoOfWorldMap;

        if (currentSceneInfo.planet != null)
            currentSceneInfo.planet.eulerAngles = currentSceneInfo.currentPlanetOrientation;
    }

    //SCENE INFO
    public void GetCurrentSceneInfo(out SceneInfo sceneInfo)
    {
        sceneInfo = currentSceneInfo;
    }
    public void SetCurrentSceneInfo(SceneInfo sceneInfo)
    {
        Debug.Log(sceneInfo.currentPlanetOrientation);
        Debug.Log(currentSceneInfo.currentPlanetOrientation);
        if(sceneInfo.planet != null)
            currentSceneInfo.planet = sceneInfo.planet;

        if (sceneInfo.zoomOutSceneName != null)
            currentSceneInfo.zoomOutSceneName = sceneInfo.zoomOutSceneName;

        if (sceneInfo.currentPlanetOrientation != Vector3.zero)
            currentSceneInfo.currentPlanetOrientation = sceneInfo.currentPlanetOrientation;

    }
    private void UpdatePlanetRotation()
    {
        if(currentSceneInfo.planet)
            currentSceneInfo.currentPlanetOrientation = currentSceneInfo.planet.eulerAngles;
    }
    private void SaveSceneInfoToStaticVars()
    {
        zoomOutName = currentSceneInfo.zoomOutSceneName;
        currentPlanetOrientation = currentSceneInfo.currentPlanetOrientation;
    }
    private void LoadSceneInfoToStruct()
    {
        currentSceneInfo.zoomOutSceneName = zoomOutName;
        currentSceneInfo.currentPlanetOrientation = currentPlanetOrientation;
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
                SaveSceneInfoToStaticVars();
                break;
            case EventHandler.EventType.zoomIn:
                UpdatePlanetRotation();
                break;
            default:
                break;
        }
    }
}
