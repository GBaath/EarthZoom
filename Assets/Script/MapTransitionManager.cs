using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MapTransitionManager : MonoBehaviour, IRecieveEvents
{

    Camera cam;

    float normalizedZoomLimit = .3f; //what is need normalized point to middle to zoom in? 0,0 -> 1,1


    //transform info for backtransition

    //need scene reference

    //zoom in avaliable?


    public Transform[] zoomPoints;

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

        foreach (Transform point in zoomPoints)
        {
            Vector2 normalizedPoint = 2*(Camera.main.WorldToScreenPoint(point.transform.position) / CamCords) - Vector2.one; //how close to screen middle (0,0)

            if (normalizedPoint.magnitude < normalizedZoomLimit) //magnitude near of 0,0
            {
                Debug.Log("zoomtoggle" + point);
            }
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
