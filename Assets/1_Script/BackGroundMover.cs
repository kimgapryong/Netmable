using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMover : MonoBehaviour
{
    public Transform cameraTransform; // 카메라의 Transform
    public float parallaxEffectMultiplier; // 패럴랙스 효과 배율

    private Vector3 lastCameraPosition;

    private void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }

    private void Update()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition; // 카메라의 움직임 차이 계산
        transform.position += deltaMovement * parallaxEffectMultiplier; // 배경의 위치를 조정

        lastCameraPosition = cameraTransform.position; // 마지막 카메라 위치 업데이트
    }
}
