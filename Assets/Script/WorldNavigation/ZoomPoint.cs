using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoomPoint : MonoBehaviour
{


    public string zoomInSceneName = "0";


    private void Start()
    {
        if(!MainCamera.instance.transitionManager.zoomPoints.Contains(this))
            MainCamera.instance.transitionManager.zoomPoints.Add(this);
    }
}
