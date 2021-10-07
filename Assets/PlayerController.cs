using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject wayPointPrefab;
    public Transform target;
    public PathRequestManager pathRequestManager;
    private void Start()
    {
        //타겟이 있다면
        if (target)
		{
            //pathRequestManager에 요청서를 넣음
            pathRequestManager.RequestPath(transform.position, target.position, FindPath);
        }
        
    }

   


    //계산이 끝나고 호출될 함수
    void FindPath(Stack<Vector3> path, bool success)
	{
        if (success)
		{

            for (int i = 0; i < path.Count; i++)
            {
                Instantiate(wayPointPrefab, path.Pop(), Quaternion.identity);
            }
        }


	}
    
}
