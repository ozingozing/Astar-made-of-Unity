using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;

    [SerializeField] private Node[,] grid;

    public float nodeRadius; 
    [SerializeField] private float nodeDiameter;
    [SerializeField] Vector2Int gridCount;

    //³ëµåÀÇ ÃÑ °¹¼ö
    public int GridCount { get => gridCount.x * gridCount.y; }

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2; 

        gridCount.x = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridCount.y = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridCount.y, gridCount.x];

        Vector3 worldPosBottomLeft = transform.position - new Vector3((gridWorldSize.x * 0.5f), 0f, (gridWorldSize.y * 0.5f));

        for (int y = 0; y < gridCount.y; y++)
        {
            for (int x = 0; x < gridCount.x; x++)
            {
                Vector3 worldPosition = worldPosBottomLeft + new Vector3((x * nodeDiameter + nodeRadius), 0f, (y * nodeDiameter + nodeRadius));
                bool walkable = !(Physics.CheckSphere(worldPosition, nodeRadius, unwalkableMask));
                grid[y, x] = new Node(walkable, worldPosition, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node currentNode)
    {
        List<Node> neighbours = new List<Node>();

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = currentNode.gridPosition.x + x;
                int checkY = currentNode.gridPosition.y + y;

                if ((0 <= checkX && checkX < gridCount.x) && (0 <= checkY && checkY < gridCount.y))
                {
                    Node checkNode = grid[checkY, checkX];
                    if (checkNode.walkable)
                    {
                        neighbours.Add(checkNode);
                    }
                }
            }
        }

        return neighbours;
    }

    public Node GetNodeFromWorldPosition(Vector3 pos)
    {
        float percentX = ((pos.x - nodeRadius) / gridWorldSize.x + 0.5f);
        float percentY = ((pos.z - nodeRadius) / gridWorldSize.y + 0.5f);

        int x = Mathf.RoundToInt((gridWorldSize.x) * percentX);
        int y = Mathf.RoundToInt((gridWorldSize.y) * percentY);

        x = Mathf.Clamp(x, 0, gridCount.x - 1);
        y = Mathf.Clamp(y, 0, gridCount.y - 1);

        return grid[y, x];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 0f, gridWorldSize.y));

        if (grid != null)
        {
            foreach (var node in grid)
            {
                Gizmos.color = node.walkable ? Color.white : Color.black;
                Gizmos.DrawCube(node.worldPosition, new Vector3(0.95f, 0.1f, 0.95f) * nodeDiameter);
            }
        }
    }

}
