using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skil : MonoBehaviour
{
    protected Transform player;
    public PlayerSkils playerSkils; 
    public PlayerStatus status;

    public GameObject skilPrefab;
    public string skilName;
    public int damage;
    public int speed;
    public int SkilLevel;

    public KeyCode key;

    private void Awake()
    {
        StartCoroutine(InitializeSkil());
    }

    private IEnumerator InitializeSkil()
    {
        while (PlayerManager.Instance == null || PlayerManager.Instance.player == null)
        {
            yield return null;
        }
        status = PlayerManager.Instance.playerStatus;
        player = PlayerManager.Instance.player.transform;
        playerSkils = GameObject.Find("Player").GetComponent<PlayerSkils>();
        

    }
    public void ResetSkil(SkilData skil)
    {
        skilName = skil.SkilName;
        skilPrefab = skil.SkilPrefab;
        damage = skil.damage;
        speed = skil.speed;
        SkilLevel = skil.nextSkilLevel;
        key = skil.keyCode;
    }

    public virtual void SkilSpeed() { }


    protected virtual void OnTriggerEnter2D(Collider2D collision) { }



}
