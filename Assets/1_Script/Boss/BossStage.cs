using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : MonoBehaviour
{
    private const float CAMSIZE = 22;
    public BossScriptable BossScript;
    private Camera main;
    private Boss boss;
    private GameObject pla;
    public AudioClip bgClip;
    private void Start()
    {
        main = Camera.main;
        main.orthographicSize = CAMSIZE;
        boss = GameObject.Find("Boss").GetComponent<Boss1>();
        boss.GetBossData(BossScript.bossName, BossScript.maxHp, BossScript.damage, BossScript.speed);
        pla = GameObject.Find("Player");
        UiManager.Instance.monsterUi.SetActive(false);
        SoundManager.Instance.BGSound(bgClip);

        pla.transform.position = new Vector3(-40, -4.3f,0);
    }

    
}
