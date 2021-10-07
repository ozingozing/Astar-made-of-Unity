using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    [SerializeField] private PathFinding pathFinding;
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentRequest;

    //지금 계산중인지 아닌지 확인용도 true(계산중), false(계산 끝 or 안함)
    //초기값은 false
    bool isProcessing = false;

    private void Awake()
    {
        pathFinding = GetComponent<PathFinding>();
    }

    public void RequestPath(Vector3 start, Vector3 end, Action<Stack<Vector3>, bool> callback)
    {
        PathRequest pathRequest = new PathRequest(start, end, callback);
        pathRequestQueue.Enqueue(pathRequest);

        //계산 시도
        TryProcessNext();
    }

    void TryProcessNext()
    {
        //계산중이 아니고 큐에 요청서가 있을 경우
        if (!isProcessing && pathRequestQueue.Count > 0)
        {
            //계산중이라고 만들어줌
            isProcessing = true;

            //큐에서 하나 빼서 currentRequest에 값 복사
            currentRequest = pathRequestQueue.Dequeue();

            //요청(현재 요청서의 시작점, 현재 요청서의 타겟)
            pathFinding.StartFindPath(currentRequest.pathStart, currentRequest.pathEnd);
        }
    }

    //PathFinding에서 계산이 끝나고 호출 해줄것
    public void FinishProcess(Stack<Vector3> path, bool success)
	{
        //진행끝
        isProcessing = false;

        //지금 요청서에 있는 callback함수를 호출(경로, 성공여부)
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