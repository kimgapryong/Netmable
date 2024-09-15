using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : MonoBehaviour
{
    private const float CAMSIZE = 22;
    public BossScriptable BossScript;
    private Camera main;
    private Boss boss;
    private void Start()
    {
        main = Camera.main;
        main.orthographicSize = CAMSIZE;
        boss = GameObject.Find("Boss").GetComponent<Boss1>();
        boss.GetBossData(BossScript.bossName, BossScript.maxHp, BossScript.damage);
    }

    
}
