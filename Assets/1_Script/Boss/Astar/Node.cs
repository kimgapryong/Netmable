using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position;  // 노드의 위치
    public bool isWalkable;   // 해당 노드가 이동 가능한지 여부

    public int gCost;  // 시작점부터 노드까지의 실제 거리
    public int hCost;  // 노드에서 목적지까지의 예상 거리
    public int fCost;  // gCost + hCost

    public Node ParentNode;  // 경로 추적을 위한 부모 노드

    public Node(Vector3 position, bool isWalkable)
    {
        this.position = position;
        this.isWalkable = isWalkable;
    }

    public int GetFCost()
    {
        return gCost + hCost;
    }
}
