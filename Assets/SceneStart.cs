using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStart : MonoBehaviour
{
    public SceneInfo SceneInfo;

    private void Start()
    {
        if (SceneInfo.findPlanet)
        {
            try
            {
                SceneInfo.planet = GameObject.Find("planet").transform;
                SceneInfo.planet.eulerAngles = GameManager.currentPlanetOrientation;
            }
            catch { }
        }


        GameManager.instance.sceneInfo = SceneInfo;
        Debug.Log(GameManager.instance.sceneInfo.zoomOutSceneName);


    }
}
