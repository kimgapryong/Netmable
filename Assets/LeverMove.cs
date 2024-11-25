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
        lever.anchoredPosition = Vector2.zero;  // 원래 위치로 되돌림
        movePlayer.horizontal = 0;
    }

    private void MoveDerection(PointerEventData eventData)
    {
        Vector2 _eventData = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 0));
        Vector2 leverPos = _eventData - (Vector2)background.transform.position;
        Debug.Log(background.anchoredPosition);
        Debug.Log(_eventData);
        Debug.Log(leverPos.x);
        // X 값 제한: leverPosX 범위 안에서 좌우로 움직일 수 있도록 수정
        float leverX = Mathf.Clamp(leverPos.x * 10, -leverPosX, leverPosX);
        float leverY = leverPosY;
        lever.anchoredPosition = new Vector2(leverX, leverY);
        inputDerector = leverX / leverPosX;
        movePlayer.horizontal = inputDerector;
    }

}
