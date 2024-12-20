using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    public static NextStage Instance {  get; private set; }
    public Transform player;
    public List<TriggerPoint> triggerPoints = new List<TriggerPoint>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {

        for (int i = triggerPoints.Count - 1; i >= 0; i--)
        {
            TriggerPoint trigger = triggerPoints[i];
            if (trigger != null && trigger.CheckTrigger(player))
            {
                trigger.onTrigger.Invoke();
                triggerPoints.RemoveAt(i);  // 현재 인덱스에서 안전하게 삭제
            }
        }
    }
    
    public void MoveBoss()
    {
        SceneManager.LoadScene("Boss1");
    }

}

[Serializable]
public class TriggerPoint
{
    public Transform targetObject;  
    public UnityEvent onTrigger;  

    public bool CheckTrigger(Transform player)
    {
        return player.position.x >= targetObject.position.x;
    }

}
