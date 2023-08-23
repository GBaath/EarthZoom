using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTransitionManager : MonoBehaviour, IRecieveEvents
{
    public List<ZoomPoint> zoomPoints;

    Camera cam;
    ZoomPoint zoomInPoint;

    GameManager.SceneInfo sceneInfoSend;

    float normalizedZoomLimit = .3f; //what is need normalized point to middle to zoom in? 0,0 -> 1,1

    bool loadingTransition;

    public const string namoOfWorldMap = "WorldMap";
    private void Start()
    {
        cam = Camera.main;

        EventSubscribe();

        loadingTransition = false;
    }

    //call when zoomed in enough to check if scene transition
    void ZoomToGroundCheck()
    {
        if (!(CameraZoom.instance.zoomScale < .2f)) //only check transition while zoomed in
            return;

        Vector2 CamCords = new Vector2(cam.pixelWidth, cam.pixelHeight);

        List<Vector2> pointsAndMagnitude = new List<Vector2>();

        int i = 0;

        //save all acceptable zoompoints
        foreach (ZoomPoint point in zoomPoints)
        {
            Vector2 normalizedPoint = 2*(Camera.main.WorldToScreenPoint(point.transform.position) / CamCords) - Vector2.one; //how close to screen middle (0,0)
            float zDistance = point.transform.position.z - cam.transform.position.z;

            if (normalizedPoint.magnitude < normalizedZoomLimit&&zDistance<40) //magnitude near of 0,0 //hard coded depth distance, front side of planet
            {
                pointsAndMagnitude.Add(new Vector2(i, normalizedPoint.magnitude)); //addindex of point and its magnitude to list for comparing
            }
            i++;
        }

        if (pointsAndMagnitude.Count < 1)
            return;

        //find zoompoint closest to middle of screen
        float closest = float.PositiveInfinity;
        int index = 0;
        foreach(Vector2 point in pointsAndMagnitude) 
        {
            if(point.y < closest)
            {
                closest = point.y;
                index = (int)point.x;
            }
        }

        zoomInPoint = zoomPoints[index];

        if (loadingTransition)
            return;
        loadingTransition = true;

        MainCamera.instance.ZoomInToPoint();
        //start loading after animation
        Invoke(nameof(InvokeZoomIn),1);
    }

    void ZoomOutCheck()
    {
        if (!(CameraZoom.instance.zoomScale > .9f)) //only check transition while zoomed in
            return;
        if (SceneManager.GetActiveScene().name == namoOfWorldMap)
            return;

        if (loadingTransition)
            return;
        loadingTransition = true;

        MainCamera.instance.ZoomOutMap();
        Invoke(nameof(InvokeZoomOut), 1);
    }


    private void InvokeZoomIn()
    {
        sceneInfoSend.zoomOutSceneName = SceneManager.GetActiveScene().name;
        GameManager.instance.SetCurrentSceneInfo(sceneInfoSend);

        EventHandler.instance.Call(EventHandler.EventType.sceneSwitch);

        SceneManager.LoadSceneAsync(zoomInPoint.zoomInSceneName);
        zoomPoints.Clear();
        zoomInPoint = null;

    }
    private void InvokeZoomOut()
    {
        GameManager.SceneInfo info;
        GameManager.instance.GetCurrentSceneInfo(out info);

        EventHandler.instance.Call(EventHandler.EventType.sceneSwitch);

        SceneManager.LoadSceneAsync(info.zoomOutSceneName);

    }


    public void EventSubscribe()
    {
        EventHandler.instance.AddReciever(this);
    }

    public void EventCall(EventHandler.EventType type)
    {
        switch (type)
        {
            case EventHandler.EventType.zoomIn:
                ZoomToGroundCheck();
                break;
            case EventHandler.EventType.zoomOut:
                ZoomOutCheck();
                break;
            default: 
                break;
        }
    }
}
