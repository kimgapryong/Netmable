using System.Collections;
using UnityEngine;

public class ScaleUp : MonoBehaviour
{
    public float layerSpeed = 1f; // 확장 속도

    private void Start()
    {
        Destroy(gameObject, 1f);
    }
    public void AttackLeft(Transform player)
    {


            transform.localScale = new Vector2(player.localScale.x + (layerSpeed * Time.deltaTime), transform.localScale.y);
            transform.position = new Vector2(player.position.x - (layerSpeed * Time.deltaTime) * 2, transform.position.y);
        //boxCollider.size = new Vector2(transform.localScale.x, boxCollider.size.y);
        //boxCollider.offset = new Vector2(-(transform.localScale.x / 2), boxCollider.offset.y);

    }

    public void AttackRight(Transform player)
    {

            transform.localScale = new Vector2(player.localScale.x + (layerSpeed * Time.deltaTime), transform.localScale.y);
            transform.position = new Vector2(player.position.x + (layerSpeed * Time.deltaTime) * 2, transform.position.y);
        //boxCollider.size = new Vector2(transform.localScale.x, boxCollider.size.y);
        //boxCollider.offset = new Vector2(transform.localScale.x / 2, boxCollider.offset.y);



    }
}
