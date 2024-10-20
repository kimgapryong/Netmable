using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoolTest : MonoBehaviour
{
    public float speed = 20;
    public Camera mainCamera;
    public Vector3 originalCameraPosition;
    public Supporter sup;
    private MovePlayer movePlayer;


    private void Start()
    {
        
        movePlayer = GameObject.Find("Player").GetComponent<MovePlayer>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;  // �⺻ ī�޶� ����
        }
        mainCamera.GetComponent<CameraMove>().StopFollowing();
        originalCameraPosition = mainCamera.transform.position;  // ���� ī�޶� ��ġ�� ����
        StartCoroutine(FollowAttack());
    }
    private void Update()
    {
        MoveBool();
    }
    private void MoveBool()
    {
       transform.Translate(Vector2.right * speed * Time.deltaTime);
       Destroy(gameObject,2f);
    }
    private IEnumerator FollowAttack()
    {
        
        while (gameObject != null)
        {
            Vector3 targetPosition = new Vector3(transform.position.x + 5, transform.position.y + 5, originalCameraPosition.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        mainCamera.transform.position = originalCameraPosition;  // ������ ������ ī�޶� ���� ��ġ�� �ǵ���
        movePlayer.enabled = true;
    }

    private void OnDestroy()
    {
        // ������ �����ų� �ı��Ǹ� ī�޶� ���� ��ġ�� ��������
        if (mainCamera != null)
        {
            mainCamera.transform.position = originalCameraPosition;
        }
        movePlayer.enabled = true;
        sup.onDestroy += checkMonster;
    }

    private void checkMonster(List<Monster> monster)
    {
        if(monster != null)
        {
            foreach(Monster m in monster)
            {
                if (m != null && m.gameObject != null)
                {
                    Destroy(m.gameObject);
                }
            }
        }
        movePlayer.enabled = true;
        sup.onDestroy -= checkMonster;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}
