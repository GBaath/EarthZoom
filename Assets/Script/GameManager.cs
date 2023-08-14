using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public static GameManager instance;

    private void Start()
    {
        if (GameManager.instance)
            Destroy(gameObject);

        instance = this;

        DontDestroyOnLoad(instance.gameObject);
    }

}
