using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SceneInfo : ScriptableObject
{
    public bool cameraMovementEnabled;

    public string zoomOutSceneName;
    [HideInInspector]public Vector3 currentPlanetOrientation;


     public Transform planet;
    public bool findPlanet;


}
