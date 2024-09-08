using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }


    public PlayerStatus playerStatus;
    public GameObject player;
    private MovePlayer movePlayer;
    public BulletMagic bulletMagic;
    public Vector2 bulletVec;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

     
            if (player != null)
            {
                DontDestroyOnLoad(player);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        movePlayer = GameObject.Find("Player").GetComponent<MovePlayer>();
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();

        StartCoroutine(PlayerHpPlus());
    }
    void Update()
    {
        if (movePlayer.facingRight)
        {
            bulletVec = Vector2.right;
        }
        else
        {
            bulletVec = Vector2.left;
        }
        
        PlayerDie();
    }

    private IEnumerator PlayerHpPlus()
    {
        while (true)
        {
            if(playerStatus.currentHp < playerStatus.maxHp)
            {
                playerStatus.currentHp++;
                yield return new WaitForSeconds(1f);
            }
            if(playerStatus.currentMp < playerStatus.maxMp)
            {
                playerStatus.currentMp++;
                yield return new WaitForSeconds(1.2f);
            }
            yield return null;
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        if(playerStatus.currentHp > 0)
        {
            playerStatus.currentHp -= damage;
            StartCoroutine(palyerDamage());

        }
        
    }
    
    private IEnumerator palyerDamage()
    {
        Renderer ren = player.GetComponent<Renderer>();

        ren.enabled = false;
        yield return new WaitForSeconds(0.3f);
        ren.enabled = true;
    }

    //죽음
    private void PlayerDie()
    {
        if(playerStatus.currentHp <= 0)
        {
            
            playerStatus.currentHp = 0;
            UiManager.Instance.hp.text = $"{playerStatus.currentHp}  / {playerStatus.maxHp}";
            UiManager.Instance.hpSlider.value = 0;
            //에니메이션
            Destroy(Instance);
            Destroy(GameManager.Instance);
            Destroy(UiManager.Instance);
            Destroy(player, 0.3f);
        }
    }
}
