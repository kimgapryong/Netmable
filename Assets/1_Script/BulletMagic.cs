using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMagic : MonoBehaviour
{
    public int damage = 20;
    private GameObject player;
    public float bulletSpeed = 17;
    

    private void Start()
    {
        player = PlayerManager.Instance.player;
    }
    void Update()
    {
        transform.Translate(PlayerManager.Instance.bulletVec * bulletSpeed * Time.deltaTime);
        BulletDelete();
    }

    private void BulletDelete()
    {
   
       Destroy(gameObject, 1.4f);
    }
}
