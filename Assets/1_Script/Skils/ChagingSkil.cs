using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChagingSkil : Skil
{
    public SkilData data;
    public GameObject ChagingPaticle;
    public int chaDam = 0;

    private GameObject clone;
    public Collider2D collider2D;
    private Transform target;
    private void Start()
    {

        ResetSkil(data);
        StartCoroutine(waitSkil());
        collider2D = GetComponent<Collider2D>();
        collider2D.enabled = false;
        Destroy(gameObject, 11);
    }

    
    public IEnumerator waitSkil()
    {
        while (status == null)
        {
            yield return null;
        }
        status.OnLevelUp += SkilConditon;
    }
    private void Update()
    {
        if (playerSkils != null)
        {
            chaDam = playerSkils.GetChaDamage();
        }
        
        SkilSpeed();
        if(target != null && clone != null)
        {
            clone.transform.position = target.position;
        }
        else
        {
            Destroy(clone);
        }
        
    }
    private void SkilConditon()
    {
        if(status.currentLevel >= 10)
        {
            playerSkils.isChaging = true;
            playerSkils.ChagingCheck(key,skilPrefab);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
   
        if (collision.CompareTag("Enemy"))
        {
         
            collision.gameObject.GetComponent<Monster>().normalAttack = false;
            collision.gameObject.GetComponent<Monster>().TakeDamage(5 + chaDam);
            target = collision.transform;
            clone = Instantiate(ChagingPaticle, target.position, Quaternion.identity);
           
            Destroy(clone, 1f);
        }else if (collision.CompareTag("Boss"))
        {
           
            collision.gameObject.GetComponent<Boss>().TakeDamage(5 + chaDam);
            target = collision.transform;
            clone = Instantiate(ChagingPaticle, target.position, Quaternion.identity);

            Destroy(clone, 1f);
        }
    }

    public override void SkilSpeed()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
   
}
