using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkilBool : MonoBehaviour
{
    private float time;
    private float setTime = 1.5f;
    private float speed = 35f;
    public IEnumerator MoveBool(Vector2 vec)
    {
        while(time >= setTime)
        {
            transform.Translate(vec * speed * Time.deltaTime);
            yield return null;
            time += Time.deltaTime;
        }
    }
}
