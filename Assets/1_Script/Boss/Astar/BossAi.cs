using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAi : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 3f;
    public float stopDistance = 1f;  // 플레이어와 가까워지면 멈추는 거리
    public float nodeCreationInterval = 1f;  // 노드 생성 주기
    public LayerMask groundLayer;
    public PathFinding path; 

    private Rigidbody2D rb;
    private List<Node> availableNodes = new List<Node>();
    private float timeSinceLastNodeCreation = 0f;

    private void Start()
    {
        path = new PathFinding();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").transform;
        StartFollowingPlayer();
    }

    private void Update()
    {
        // 일정 시간마다 플레이어 위치를 노드로 추가
        timeSinceLastNodeCreation += Time.deltaTime;
        if (timeSinceLastNodeCreation >= nodeCreationInterval)
        {
            CreateNodeAtPlayerPosition();
            timeSinceLastNodeCreation = 0f;
        }

        // 플레이어가 가까우면 멈춤
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= stopDistance)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            FollowPath();
        }
    }

    private void CreateNodeAtPlayerPosition()
    {
        Vector3 playerPosition = player.position;
        if (availableNodes.Count == 0 || Vector3.Distance(playerPosition, availableNodes[availableNodes.Count - 1].position) > 1f)
        {
            availableNodes.Add(new Node(playerPosition, true));  // 플레이어의 현재 위치를 새로운 노드로 추가
        }
    }

    private void FollowPath()
    {
        List<Vector3> path = PathFinding.instance.FindPath(transform.position, player.position, availableNodes);
        if (path != null && path.Count > 1)
        {
            Vector3 nextPosition = path[1];
            Vector2 moveDir = (nextPosition - transform.position).normalized;
            rb.velocity = moveDir * followSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;  // 경로가 없을 때 멈춤
        }
    }

    private void StartFollowingPlayer()
    {
        availableNodes.Clear();  // 노드 리스트 초기화
        CreateNodeAtPlayerPosition();
    }
}
