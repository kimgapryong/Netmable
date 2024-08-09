using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMagic : MonoBehaviour
{
    public int damage = 20;
    private GameObject player;
    public float bulletSpeed = 17;
    public Vector2 bulletVec;

    private void Start()
    {
        player = PlayerManager.Instance.player;
    }
    void Update()
    {
        transform.Translate(bulletVec * bulletSpeed * Time.deltaTime);
        BulletDelete();
    }

    private void BulletDelete()
    {
        if(player.transform.position.x + 20 < transform.position.x || player.transform.position.x - 20 > transform.position.x)
        {
            Destroy(gameObject);
        }
        
    }
}
