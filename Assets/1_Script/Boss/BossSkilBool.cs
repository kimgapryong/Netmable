using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkilBool : MonoBehaviour
{
    private float time = 0f;
    private float setTime = 1.5f;
    private float speed = 300f;
    public IEnumerator MoveBool(Vector2 vec)
    {
        while(time < setTime)
        {
            transform.Translate(vec * speed * Time.deltaTime);
            yield return null;
            time += Time.deltaTime;
        }
        Destroy(gameObject);
    }
}
