using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackGroundMover : MonoBehaviour
{
    //public Transform cameraTransform; // 카메라의 Transform
    //public float parallaxEffectMultiplier; // 패럴랙스 효과 배율

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

        // 모든 자식 오브젝트를 반복하여 배열에 담습니다.
        for (int i = 0; i < childCount; i++)
        {
            backImage[i] = background.transform.GetChild(i);
        }
        centerBack = GetPlayerBack();
    }

    private void Update()
    {
        //Vector3 deltaMovement = cameraTransform.position - lastCameraPosition; // 카메라의 움직임 차이 계산
        //transform.position += deltaMovement * parallaxEffectMultiplier; // 배경의 위치를 조정

        //lastCameraPosition = cameraTransform.position; // 마지막 카메라 위치 업데이트
        MoveBack();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("나가졌다");
        if (collision.CompareTag("MainCamera") && !DialogueManager.Instance.isChat)
        {
            Debug.Log("실행줌");
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
        float minDistance = Vector3.Distance(player.position, backImage[0].position); // 캐릭터와 첫 번째 이미지 사이의 거리로 초기화

        for (int i = 1; i < childCount; i++)
        {
            float distance = Vector3.Distance(player.position, backImage[i].position); // 캐릭터와 현재 이미지 사이의 거리 계산

            if (distance < minDistance) // 최소 거리보다 짧으면 업데이트
            {
                minDistance = distance;
                closestBackImage = backImage[i];
            }
        }
        return closestBackImage;
    }

}
