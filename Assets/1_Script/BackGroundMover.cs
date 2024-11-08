using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackGroundMover : MonoBehaviour
{
    //public Transform cameraTransform; // ī�޶��� Transform
    //public float parallaxEffectMultiplier; // �з����� ȿ�� ����

    //private Vector3 lastCameraPosition;
    public float vecX = 120;
    private Transform player;
    private MovePlayer movePlayer;
    private Transform cooiseBack;

    private GameObject background;
    private Transform[] backImage;
    public Transform centerBack;
    private int childCount;
    private void Start()
    {
        //cameraTransform = Camera.main.transform;
        //lastCameraPosition = cameraTransform.position;

        player = GameObject.Find("Player").transform;
        movePlayer = player.GetComponent<MovePlayer>();

        background = GameObject.Find("BackGround");
        childCount = background.transform.childCount;
        backImage = new Transform[childCount];

        // ��� �ڽ� ������Ʈ�� �ݺ��Ͽ� �迭�� ����ϴ�.
        for (int i = 0; i < childCount; i++)
        {
            backImage[i] = background.transform.GetChild(i);
        }
        centerBack = GetPlayerBack();
    }

    private void Update()
    {
        //Vector3 deltaMovement = cameraTransform.position - lastCameraPosition; // ī�޶��� ������ ���� ���
        //transform.position += deltaMovement * parallaxEffectMultiplier; // ����� ��ġ�� ����

        //lastCameraPosition = cameraTransform.position; // ������ ī�޶� ��ġ ������Ʈ
        MoveBack();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("��������");
        if (collision.CompareTag("MainCamera") && !DialogueManager.Instance.isChat)
        {
            Debug.Log("������");
            if (!movePlayer.isJump)
            {
                if (movePlayer.facingRight)
                {
                    TurnRightBack();
                }
                else
                {
                    TrunLeftBack();
                }
            }
        }
    }
    private void MoveBack()
    {
        if(player.transform.position.x > centerBack.position.x + vecX / 3)
        {
            TurnRightBack();
        }
        if(player.transform.position.x < centerBack.position.x - vecX / 3)
        {
            TrunLeftBack();
        }

    }
        private void TurnRightBack()
        {
            cooiseBack = LowTrans();
        centerBack = GetPlayerBack();
            cooiseBack.position = new Vector3(cooiseBack.position.x + vecX, cooiseBack.position.y, cooiseBack.position.z);

        }

        private void TrunLeftBack()
        {
            cooiseBack = HightTrans();
        centerBack = GetPlayerBack();
        cooiseBack.position = new Vector3(cooiseBack.position.x - vecX, cooiseBack.position.y, cooiseBack.position.z);
        }


    private Transform LowTrans()
    {
        Transform low;
        low = backImage[0];
        for(int i = 1; i < childCount; i++)
        {
            if(low.position.x > backImage[i].position.x)
            {
                low = backImage[i];
            }
        }
        return low;
    }
    private Transform HightTrans()
    {
        Transform hight;
        hight = backImage[0];
        for (int i = 1; i < childCount; i++)
        {
            if (hight.position.x < backImage[i].position.x)
            {
                hight = backImage[i];
            }
        }
        return hight;
    }
    private Transform GetPlayerBack()
    {
        Transform closestBackImage = backImage[0];
        float minDistance = Vector3.Distance(player.position, backImage[0].position); // ĳ���Ϳ� ù ��° �̹��� ������ �Ÿ��� �ʱ�ȭ

        for (int i = 1; i < childCount; i++)
        {
            float distance = Vector3.Distance(player.position, backImage[i].position); // ĳ���Ϳ� ���� �̹��� ������ �Ÿ� ���

            if (distance < minDistance) // �ּ� �Ÿ����� ª���� ������Ʈ
            {
                minDistance = distance;
                closestBackImage = backImage[i];
            }
        }
        return closestBackImage;
    }

}
