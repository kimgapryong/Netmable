using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }


    public PlayerStatus playerStatus;
    public GameObject player;
    private MovePlayer movePlayer;
    public BulletMagic bulletMagic;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        movePlayer = GameObject.Find("Player").GetComponent<MovePlayer>();
        if (bulletMagic != null)
        {
            Debug.Log(bulletMagic);
        }
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movePlayer.facingRight)
        {
            bulletMagic.bulletVec = Vector2.up;
        }
        else
        {
            bulletMagic.bulletVec = Vector2.down;
        }
    }
}
