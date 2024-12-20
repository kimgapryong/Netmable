using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public bool realDie = true;
    public CameraMove cam;
    public PlayerStatus playerStatus;
    public GameObject player;
    private MovePlayer movePlayer;
    public BulletMagic bulletMagic;
    public Vector2 bulletVec;
    public Boss boss;

    public bool okAtk = true;
    public AudioClip takeClip;
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

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        movePlayer = GameObject.Find("Player").GetComponent<MovePlayer>();
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        cam = Camera.main.GetComponent<CameraMove>();
        StartCoroutine(PlayerHpPlus());
    }

    private void Update()
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
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FindBossAfterSceneLoad());
    }

    private IEnumerator FindBossAfterSceneLoad()
    {
        yield return new WaitForSeconds(1f);  

        boss = GameObject.Find("Boss")?.GetComponent<Boss>();

        if (boss != null)
        {
            Debug.Log("Boss 찾음: 델리게이트 등록 완료");
            boss.OnPlayerTrigger += PlayerTakeDamage;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
        if(playerStatus.currentHp > 0 && okAtk)
        {
            SoundManager.Instance.SFXSound("PlayerTake", takeClip);
            playerStatus.currentHp -= damage;
            StartCoroutine(palyerDamage());

            if(cam != null)
            {
                StartCoroutine(cam.Shake(0.55f, 0.2f, 0.8f));
            }
        }
        
    }
    
    private IEnumerator palyerDamage()
    {
        Renderer ren = player.GetComponent<Renderer>();

        ren.enabled = false;
        yield return new WaitForSeconds(0.1f);
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

            if (realDie)
            {
                Destroy(Instance);
                Destroy(GameManager.Instance);
                Destroy(UiManager.Instance);
                Destroy(player, 0.3f);
            }
            else
            {
                playerStatus.currentHp = 0;
            }
        }
    }
}
