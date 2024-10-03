using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBoolSkil : Skil
{

    public SkilData data;
    public GameObject firePaticle;


    private void Start()
    {
        ResetSkil(data);
        StartCoroutine(waitSkil());
    }
    public IEnumerator waitSkil()
    {
        while(status == null)
        {
            yield return null;
        }
        status.OnLevelUp += SkilConditon;
    }

    private void Update()
    {
        SkilSpeed();
    }

    public void SkilConditon()
    {
        Debug.Log("Current Level: " + status.currentLevel + ", Skill Level: " + SkilLevel);
        if (status.currentLevel >= SkilLevel)
        {
            Debug.Log("사용 가능");
            playerSkils.FireSkil = true;
            playerSkils.FireCheck(key, skilPrefab);
        }
    }


    public override void SkilSpeed()
    {
        
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        Destroy(gameObject, 1.5f);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Monster>().normalAttack = false;
            collision.gameObject.GetComponent<Monster>().TakeDamage(status.damage / 2 + damage);
            GameObject clone = Instantiate(firePaticle, collision.transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(clone, 0.3f);

        }else if (collision.CompareTag("Boss")){
            collision.gameObject.GetComponent<Boss>().TakeDamage(status.damage / 2 + damage /2 );
            GameObject clone = Instantiate(firePaticle, collision.transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(clone, 0.3f);
        }
    }
}




