using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoomPoint : MonoBehaviour
{


    public string zoomInSceneName = "0";


    private void Start()
    {
        MainCamera.instance.transitionManager.zoomPoints.Add(this);
    }
}
