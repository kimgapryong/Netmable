using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    public delegate void PlayerTrigger(int damage);
    public event PlayerTrigger OnPlayerTrigger;


    public State bossState;
    public BossState bossAtction;
    public FSM fsm;
    public Dictionary<State, BossState> stateDiction;

    public List<Action> bossSkils = new List<Action>();
    public bool bossAttack;
    public int skilCount;

    public bool isAttack = true;
    public bool isIdle = true;

    public Boss1Ui ui;

    public Animator animator;

    public GameObject player;
    public MovePlayer movePlayer;

    public Rigidbody2D rb;
    public Collider2D col;

    public string bossName;
    public CameraMove cam;

    public int maxHp;
    public int currentHp;

    public int damage;
    public float speed;

    public Animator animators;

    public enum State
    {
        Idle,
        Attack
    }


    protected virtual void Start()
    {

        bossAtction = new BossState();
        stateDiction = new Dictionary<State, BossState>() {
            { State.Idle, new IdleState(this) },
            { State.Attack, new AttackState(this) }
        };
        fsm = new FSM(State.Idle, stateDiction);

        

        player = GameObject.Find("Player");
        movePlayer = player.GetComponent<MovePlayer>();
        rb = GetComponent<Rigidbody2D>();
        col = rb.GetComponent<Collider2D>();
        ui = GameObject.Find("BossUi").GetComponent<Boss1Ui>();
    }
    public void GetBossData(string bossName, int maxHp, int damage, float speed)
    {
        this.bossName = bossName;
        this.maxHp = maxHp;
        this.damage = damage;
        this.speed = speed; 
    }
    public IEnumerator GetCam()
    {
        yield return new WaitForSeconds(1);
        GameObject cameraObj = GameObject.Find("Main Camera"); 
        if (cameraObj != null)
        {
            cam = cameraObj.GetComponent<CameraMove>(); 
            Debug.Log(cam.name);
        }
    }
    public void TakeDamage(int dam)
    {
        currentHp -= dam;
    }
    public void PlayerDamageTirgger()
    {
        Debug.Log(1);
        OnPlayerTrigger?.Invoke(damage);
    }
    public IEnumerator IdleBoss(float time)
    {
        transform.position = Vector3.zero;
        yield return new WaitForSeconds(time);
    }
    public abstract IEnumerator Attack1();
    public abstract IEnumerator Attack2();
    public abstract IEnumerator Attack3();
    public abstract void BossIdle();

    private float coolTime = 5f;
    public IEnumerator BossAttackCool(IEnumerator waitSkill)
    {
        yield return StartCoroutine(waitSkill);
        bossAttack = false;
        yield return new WaitForSeconds(coolTime);
        bossAttack = true;
    }
}
