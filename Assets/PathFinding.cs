using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics; //���

public class PathFinding : MonoBehaviour
{
    public Transform start, target;
    [SerializeField] private Grid grid;
    [SerializeField] private PathRequestManager pathRequestManager;
    Node startNode, targetNode;
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        //FindPath �ڷ�ƾ ����
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //�⺻���� ��� ����
        Stack<Vector3> wayPoints = new Stack<Vector3>();
        //�⺻���� false
        bool pathSuccess = false;


        startNode = grid.GetNodeFromWorldPosition(startPos);
        targetNode = grid.GetNodeFromWorldPosition(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.GridCount);
        HashSet<Node> closeSet = new HashSet<Node>();

        openSet.Push(startNode);

        while (openSet.Count > 0)
        {
            

            Node currentNode = openSet.Pop();
            closeSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                //��ã�� ����
                pathSuccess = true;
                break;
            }

            foreach (var neighbourNode in grid.GetNeighbours(currentNode))
            {
                if (closeSet.Contains(neighbourNode))
                {
                    continue;
                }

                int newGCost = currentNode.Gcost + GetDistance(currentNode, neighbourNode);

                if (openSet.Contains(neighbourNode))
                {

                    if (newGCost < neighbourNode.Gcost)
                    {
                        neighbourNode.parent = currentNode;
                        neighbourNode.Gcost = newGCost;

                    }

                }
                else
                {
                    int newHCost = GetDistance(neighbourNode, targetNode);

                    neighbourNode.parent = currentNode;
                    neighbourNode.Gcost = newGCost;

                    neighbourNode.Hcost = newHCost;
                    //openSet.Add(neighbourNode);
                    openSet.Push(neighbourNode);
                }
            }
        }
        //�� ã�� ����
        if (pathSuccess)
		{
            //wayPoints(���)�� target���� start���� �־���
            wayPoints = GetPath(startNode, targetNode);
        }
        //�����ٰ� ȣ��
        pathRequestManager.FinishProcess(wayPoints, pathSuccess);

        yield return null;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int distY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }



    //��ǥ�� ��Ƽ� ��ȯ
    Stack<Vector3> GetPath(Node startNode, Node targetNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = targetNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);

            currentNode = currentNode.parent;
        }

        path.Add(currentNode);

        return SimplifyPath(path);

    }

    //��θ� �ܼ��ϰ�
    Stack<Vector3> SimplifyPath(List<Node> path)
	{
        //�ܼ��� ��δ��� Stack
        Stack<Vector3> wayPoints = new Stack<Vector3>();
        //���� ���� ����
        Vector2Int directionOld = Vector2Int.zero;

        //List�ȿ� �ִ� ��� node ��
        for (int i = 1; i < path.Count; i++)
        {
            //���� ���ο� ����    = �� ��� ��ǥ - ���� ��� ��ǥ
            Vector2Int directionNew = new Vector2Int(path[i - 1].gridPosition.x - path[i].gridPosition.x, path[i - 1].gridPosition.y - path[i].gridPosition.y);
            //���� ����� ���ο� ������ �ٸ����
            if (directionOld != directionNew)
            {
                //�� ����� ��ǥ�� �߰�
                wayPoints.Push(path[i - 1].worldPosition);
                //���� ���� ������Ʈ
                directionOld = directionNew;
            }
        }

        //�ܼ��� ��� ��ȯ
        return wayPoints;
    }

    private void OnDrawGizmos()
    {
        if (startNode != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(startNode.worldPosition, new Vector3(0.95f, 0.2f, 0.95f) * 1f);
        }
        if (targetNode != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(targetNode.worldPosition, new Vector3(0.95f, 0.2f, 0.95f) * 1f);
        }
    }
}
