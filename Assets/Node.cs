using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : IHeapNode<Node>
{
    //부모 노드
    public Node parent;

    public bool walkable;
    public Vector3 worldPosition;
    public Vector2Int gridPosition;

    public int Gcost { get; set; }
    public int Hcost { get; set; } 
    public int Fcost { get => Gcost + Hcost; }

    //지가 배열에서 몇번째 인지
    public int HeapIndex { get; set; }

    public Node(bool walkable, Vector3 pos, int x, int y)
    {
        this.walkable = walkable;
        worldPosition = pos;
        gridPosition.x = x;
        gridPosition.y = y;
    }

    public int CompareTo(Node other)
    {
        //int 값이 작은 경우가 우선순위가 높음
        //Fcost < other.Fcost => -1
        //Fcost == other.Fcost => 0
        //Fcost > other.Fcost => 1
        int compare = Fcost.CompareTo(other.Fcost);

        //Fcost가 같을경우
        if (compare == 0)
        {
            //Hcost < other.Hcost => -1
            //Hcost == other.Hcost => 0
            //Hcost > other.Hcost => 1
            compare = Hcost.CompareTo(other.Hcost);
        }

        return compare;
    }
}
