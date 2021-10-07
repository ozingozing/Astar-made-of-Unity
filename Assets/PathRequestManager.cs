using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    [SerializeField] private PathFinding pathFinding;
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentRequest;

    //���� ��������� �ƴ��� Ȯ�ο뵵 true(�����), false(��� �� or ����)
    //�ʱⰪ�� false
    bool isProcessing = false;

    private void Awake()
    {
        pathFinding = GetComponent<PathFinding>();
    }

    public void RequestPath(Vector3 start, Vector3 end, Action<Stack<Vector3>, bool> callback)
    {
        PathRequest pathRequest = new PathRequest(start, end, callback);
        pathRequestQueue.Enqueue(pathRequest);

        //��� �õ�
        TryProcessNext();
    }

    void TryProcessNext()
    {
        //������� �ƴϰ� ť�� ��û���� ���� ���
        if (!isProcessing && pathRequestQueue.Count > 0)
        {
            //������̶�� �������
            isProcessing = true;

            //ť���� �ϳ� ���� currentRequest�� �� ����
            currentRequest = pathRequestQueue.Dequeue();

            //��û(���� ��û���� ������, ���� ��û���� Ÿ��)
            pathFinding.StartFindPath(currentRequest.pathStart, currentRequest.pathEnd);
        }
    }

    //PathFinding���� ����� ������ ȣ�� ���ٰ�
    public void FinishProcess(Stack<Vector3> path, bool success)
	{
        //���ೡ
        isProcessing = false;

        //���� ��û���� �ִ� callback�Լ��� ȣ��(���, ��������)
        currentRequest.pathCallback(path, success);
        TryProcessNext();
    }


    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Stack<Vector3>, bool> pathCallback;
        public PathRequest(Vector3 start, Vector3 end, Action<Stack<Vector3>, bool> callback)
        {
            pathStart = start;
            pathEnd = end;
            pathCallback = callback;
        }
    }
}