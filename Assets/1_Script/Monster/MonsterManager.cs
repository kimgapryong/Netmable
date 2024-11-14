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

        // �� ��ȯ �� MonsterManager�� �ٽ� �ʱ�ȭ�ϱ� ���� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // �� ��ȯ �� �̺�Ʈ �ߺ� ȣ���� �����ϱ� ���� �̺�Ʈ ����
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
        // ���� Ȱ��ȭ�� ��� Monster ������Ʈ�� ������
        monsters.Clear();
        monsters.AddRange(FindObjectsOfType<Monster>());
    }

    // ���� ���� �ε�� �� ȣ��Ǵ� �޼���
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� �ε�� ������ ����Ʈ�� �÷��̾ �ٽ� �ʱ�ȭ
        player = GameObject.Find("Player")?.GetComponent<Transform>();
        FindAndAddMonsters();
    }
}
