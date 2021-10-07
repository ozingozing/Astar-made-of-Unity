using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics; //사용

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
        //FindPath 코루틴 실행
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //기본값은 경로 없음
        Stack<Vector3> wayPoints = new Stack<Vector3>();
        //기본값은 false
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
                //길찾기 성공
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
        //길 찾기 성공
        if (pathSuccess)
		{
            //wayPoints(경로)에 target부터 start까지 넣어줌
            wayPoints = GetPath(startNode, targetNode);
        }
        //끝났다고 호출
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



    //좌표만 담아서 반환
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

    //경로를 단순하게
    Stack<Vector3> SimplifyPath(List<Node> path)
	{
        //단순한 경로담을 Stack
        Stack<Vector3> wayPoints = new Stack<Vector3>();
        //비교할 옛날 방향
        Vector2Int directionOld = Vector2Int.zero;

        //List안에 있는 모든 node 비교
        for (int i = 1; i < path.Count; i++)
        {
            //비교할 새로운 방향    = 전 노드 좌표 - 현재 노드 좌표
            Vector2Int directionNew = new Vector2Int(path[i - 1].gridPosition.x - path[i].gridPosition.x, path[i - 1].gridPosition.y - path[i].gridPosition.y);
            //옛날 방향과 새로운 방향이 다를경우
            if (directionOld != directionNew)
            {
                //전 노드의 좌표를 추가
                wayPoints.Push(path[i - 1].worldPosition);
                //옛날 방향 업데이트
                directionOld = directionNew;
            }
        }

        //단순한 경로 반환
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
