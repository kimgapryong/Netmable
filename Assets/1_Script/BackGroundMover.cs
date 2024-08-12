using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMover : MonoBehaviour
{
    public Transform cameraTransform; // ī�޶��� Transform
    public float parallaxEffectMultiplier; // �з����� ȿ�� ����

    private Vector3 lastCameraPosition;

    private void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }

    private void Update()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition; // ī�޶��� ������ ���� ���
        transform.position += deltaMovement * parallaxEffectMultiplier; // ����� ��ġ�� ����

        lastCameraPosition = cameraTransform.position; // ������ ī�޶� ��ġ ������Ʈ
    }
}
