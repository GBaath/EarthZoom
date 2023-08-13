using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTransitionManager : MonoBehaviour, IRecieveEvents
{

    Camera cam;

    float normalizedZoomLimit = .3f; //what is need normalized point to middle to zoom in? 0,0 -> 1,1

    public List<ZoomPoint> zoomPoints;

    private void Start()
    {
        cam = Camera.main;

        EventSubscribe();
    }

    //call when zoomed in enough to check if scene transition
    public void ZoomToGroundCheck()
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

        //fin zoompoint closest to middle of screen
        float closest = 999;
        int index = 0;
        foreach(Vector2 point in pointsAndMagnitude) 
        {
            if(point.y < closest)
            {
                closest = point.y;
                index = (int)point.x;
            }
        }

        ZoomPoint zoomInPoint = zoomPoints[index];

        try
        {
            SceneManager.LoadSceneAsync(zoomInPoint.zoomInSceneName);

        }
        catch
        {
            SceneManager.LoadScene("WorldMap");
        }

    }

    public void EventSubscribe()
    {
        EventHandler.instance.Add(this);
    }

    public void EventCall(EventHandler.EventType type)
    {
        switch (type)
        {
            case EventHandler.EventType.zoomIn:
                ZoomToGroundCheck();
                break;

            default: break;
        }
    }
}
