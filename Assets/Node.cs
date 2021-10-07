using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : IHeapNode<Node>
{
    //�θ� ���
    public Node parent;

    public bool walkable;
    public Vector3 worldPosition;
    public Vector2Int gridPosition;

    public int Gcost { get; set; }
    public int Hcost { get; set; } 
    public int Fcost { get => Gcost + Hcost; }

    //���� �迭���� ���° ����
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
        //int ���� ���� ��찡 �켱������ ����
        //Fcost < other.Fcost => -1
        //Fcost == other.Fcost => 0
        //Fcost > other.Fcost => 1
        int compare = Fcost.CompareTo(other.Fcost);

        //Fcost�� �������
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
