using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : MonoBehaviour
{
    private const float CAMSIZE = 22;
    private Camera main;

    private void Start()
    {
        main = Camera.main;
        main.orthographicSize = CAMSIZE;    
    }

    
}
