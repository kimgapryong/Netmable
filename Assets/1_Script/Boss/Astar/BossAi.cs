using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAi : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 3f;
    public float stopDistance = 1f;  // �÷��̾�� ��������� ���ߴ� �Ÿ�
    public float nodeCreationInterval = 1f;  // ��� ���� �ֱ�
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
        // ���� �ð����� �÷��̾� ��ġ�� ���� �߰�
        timeSinceLastNodeCreation += Time.deltaTime;
        if (timeSinceLastNodeCreation >= nodeCreationInterval)
        {
            CreateNodeAtPlayerPosition();
            timeSinceLastNodeCreation = 0f;
        }

        // �÷��̾ ������ ����
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
            availableNodes.Add(new Node(playerPosition, true));  // �÷��̾��� ���� ��ġ�� ���ο� ���� �߰�
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
            rb.velocity = Vector2.zero;  // ��ΰ� ���� �� ����
        }
    }

    private void StartFollowingPlayer()
    {
        availableNodes.Clear();  // ��� ����Ʈ �ʱ�ȭ
        CreateNodeAtPlayerPosition();
    }
}
