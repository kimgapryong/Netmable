using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position;  // ����� ��ġ
    public bool isWalkable;   // �ش� ��尡 �̵� �������� ����

    public int gCost;  // ���������� �������� ���� �Ÿ�
    public int hCost;  // ��忡�� ������������ ���� �Ÿ�
    public int fCost;  // gCost + hCost

    public Node ParentNode;  // ��� ������ ���� �θ� ���

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
