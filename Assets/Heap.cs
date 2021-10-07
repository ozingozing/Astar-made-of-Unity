using System;
public interface IHeapNode<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}

public class Heap<T> where T : IHeapNode<T>
{
    private readonly T[] nodes;
    private int nodeIndex;
    public int Count { get => nodeIndex - 1; }

    public Heap(int maxHeapSize)
    {
        nodeIndex = 1;
        nodes = new T[maxHeapSize + nodeIndex];
    }

    public void Push(T node)
    {
        node.HeapIndex = nodeIndex;
        nodes[nodeIndex] = node;
        SortUp(node);
        nodeIndex++;
    }

    private void SortUp(T node)
    {
        int parentIndex = 0;

        while ((parentIndex = GetParentIndex(node.HeapIndex)) != 0)
        {
            T parentNode = nodes[parentIndex];

            if (node.CompareTo(parentNode) < 0)
            {
                Swap(node, parentNode);
            }
            else 
            {
                break;
            }
        }
    }

    public T Pop()
    {
        T firstNode = nodes[1];
        nodeIndex--;
        nodes[1] = nodes[nodeIndex];
        nodes[1].HeapIndex = 1;

        SortDown(nodes[1]);

        return firstNode;
    }

    private void SortDown(T node)
    {
        int childIndex = 0;

        while ((childIndex = GetChildIndex(node.HeapIndex)) != 0)
        {
            T childNode = nodes[childIndex];

            if (node.CompareTo(childNode) > 0)
            {
                Swap(node, childNode);
            }
            else
            {
                break;
            }
        }
    }
   
    private int GetChildIndex(int selfIndex)
    {
        if (GetLeftChildIndex(selfIndex) > Count)
        {
            return 0;
        }
        else if (GetLeftChildIndex(selfIndex) == Count)
        {
            return GetLeftChildIndex(selfIndex);
        }
        else
        {
           if(nodes[GetLeftChildIndex(selfIndex)].CompareTo(nodes[GetRightChildIndex(selfIndex)]) < 0)
            {
                return GetLeftChildIndex(selfIndex);
            }
            else
            {
                return GetRightChildIndex(selfIndex);
            }
        }
      
    }
    private int GetParentIndex(int selfIndex)
    {
        return selfIndex / 2;
    }
    private int GetLeftChildIndex(int selfIndex)
    {
        return selfIndex * 2;
    }
    private int GetRightChildIndex(int selfIndex)
    {
        return selfIndex * 2 + 1;
    }

    private void Swap(T nodeA, T nodeB)
    {
        nodes[nodeA.HeapIndex] = nodeB;
        nodes[nodeB.HeapIndex] = nodeA;

        int tempIndex = nodeA.HeapIndex;
        nodeA.HeapIndex = nodeB.HeapIndex;
        nodeB.HeapIndex = tempIndex;
    }

    //nodes에 있나 없나 확인
    public bool Contains(T node)
    {
        //nodes에 들어온 node
        return Equals(nodes[node.HeapIndex], node);
    }

}
