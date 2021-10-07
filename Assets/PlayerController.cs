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
        //Ÿ���� �ִٸ�
        if (target)
		{
            //pathRequestManager�� ��û���� ����
            pathRequestManager.RequestPath(transform.position, target.position, FindPath);
        }
        
    }

   


    //����� ������ ȣ��� �Լ�
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
