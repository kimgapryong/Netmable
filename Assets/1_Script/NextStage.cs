using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    public static NextStage Instance {  get; private set; }

    public Transform[] stageObj;
    public Transform player;

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
        Action();
    }
    private void Action()
    {
        if(player.transform.position.x >= stageObj[0].position.x)
        {
            SceneManager.LoadScene("Boss1");
        }
    }

}
