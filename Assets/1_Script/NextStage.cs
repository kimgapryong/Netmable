using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    
        foreach (TriggerPoint trigger in triggerPoints)
        {
            if (trigger.CheckTrigger(player))
            {
                trigger.onTrigger.Invoke();  
                triggerPoints.Remove(trigger); 
                break; 
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
