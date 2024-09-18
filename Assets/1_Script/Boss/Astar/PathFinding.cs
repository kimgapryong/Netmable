using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    private List<Node> openList;
    private List<Node> closedList;

    public static PathFinding instance { get; private set; }

    public PathFinding()
    {
        instance = this;
    }

    // 경로 찾기: 주어진 노드 리스트에서 시작점과 끝점으로 경로를 계산
    public List<Vector3> FindPath(Vector3 start, Vector3 end, List<Node> availableNodes)
    {
        Node startNode = FindClosestNode(start, availableNodes);
        Node endNode = FindClosestNode(end, availableNodes);

        if (startNode == null || endNode == null) return null;

        openList = new List<Node> { startNode };
        closedList = new List<Node>();

        foreach (Node node in availableNodes)
        {
            node.gCost = int.MaxValue;
            node.hCost = GetHCost(node, endNode);
            node.fCost = node.GetFCost();
            node.ParentNode = null;
        }

        startNode.gCost = 0;
        startNode.hCost = GetHCost(startNode, endNode);
        startNode.fCost = startNode.GetFCost();

        while (openList.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(currentNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Node neighbor in GetNeighborNodes(currentNode, availableNodes))
            {
                if (closedList.Contains(neighbor) || !neighbor.isWalkable) continue;

                int tentativeGCost = currentNode.gCost + GetHCost(currentNode, neighbor);
                if (tentativeGCost < neighbor.gCost)
                {
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = GetHCost(neighbor, endNode);
                    neighbor.fCost = neighbor.GetFCost();
                    neighbor.ParentNode = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private List<Vector3> CalculatePath(Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;
        while (currentNode != null)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.ParentNode;
        }
        path.Reverse();
        return path;
    }

    public Node FindClosestNode(Vector3 position, List<Node> availableNodes)
    {
        Node closestNode = null;
        float minDistance = float.MaxValue;

        foreach (Node node in availableNodes)
        {
            float distance = Vector3.Distance(position, node.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestNode = node;
            }
        }

        return closestNode;
    }

    private List<Node> GetNeighborNodes(Node currentNode, List<Node> availableNodes)
    {
        List<Node> neighbors = new List<Node>();
        foreach (Node node in availableNodes)
        {
            if (Vector2.Distance(currentNode.position, node.position) <= 1.5f)
            {
                neighbors.Add(node);  // 근처 노드 추가
            }
        }
        return neighbors;
    }

    private int GetHCost(Node a, Node b)
    {
        int dx = (int)Mathf.Abs(a.position.x - b.position.x);
        int dy = (int)Mathf.Abs(a.position.y - b.position.y);
        return dx + dy;
    }

    private Node GetLowestFCostNode(List<Node> nodes)
    {
        Node lowestFCostNode = nodes[0];
        foreach (Node node in nodes)
        {
            if (node.fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = node;
            }
        }
        return lowestFCostNode;
    }
}
