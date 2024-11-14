using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeverMove : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform background;

    private MovePlayer movePlayer;
    private float inputDerector;

    [SerializeField, Range(10, 150)]
    private float leverPosX;
    private float leverPosY = 0;

    private void Awake()
    {
        background = GetComponent<RectTransform>();
        movePlayer = GameObject.Find("Player").GetComponent<MovePlayer>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        MoveDerection(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveDerection(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;  // ���� ��ġ�� �ǵ���
        movePlayer.horizontal = 0;
    }

    private void MoveDerection(PointerEventData eventData)
    {
        Vector2 leverPos = eventData.position - background.anchoredPosition;

        // X �� ����: leverPosX ���� �ȿ��� �¿�� ������ �� �ֵ��� ����
        float leverX = Mathf.Clamp(leverPos.x, -leverPosX, leverPosX);
        float leverY = leverPosY;
        lever.anchoredPosition = new Vector2(leverX, leverY);
        inputDerector = leverX / leverPosX;
        movePlayer.horizontal = inputDerector;
    }

}
