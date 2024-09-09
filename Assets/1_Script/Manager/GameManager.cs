using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject ManagerObj;

    public PlayerManager playerManager;
    public MonsterManager monsterManager;
    public UiManager uiManager;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            if (ManagerObj != null)
            {
                DontDestroyOnLoad (ManagerObj); 
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
