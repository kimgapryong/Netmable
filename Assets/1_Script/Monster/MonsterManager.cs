using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterManager : MonoBehaviour
{
    private Transform player;
    public List<Monster> monsters = new List<Monster>();

    private void Start()
    {
        player = GameObject.Find("Player")?.GetComponent<Transform>();
        FindAndAddMonsters();

        // 씬 전환 시 MonsterManager를 다시 초기화하기 위해 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 씬 전환 후 이벤트 중복 호출을 방지하기 위해 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        isAttackTrue();
    }

    private void isAttackTrue()
    {
        foreach (Monster monster in monsters)
        {
            if (monster != null && player != null)
            {
                monster.isAttack = Vector2.Distance(player.position, monster.transform.position) < 35f;
            }
        }
    }

    private void FindAndAddMonsters()
    {
        // 현재 활성화된 모든 Monster 오브젝트를 가져옴
        monsters.Clear();
        monsters.AddRange(FindObjectsOfType<Monster>());
    }

    // 씬이 새로 로드될 때 호출되는 메서드
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드될 때마다 리스트와 플레이어를 다시 초기화
        player = GameObject.Find("Player")?.GetComponent<Transform>();
        FindAndAddMonsters();
    }
}
