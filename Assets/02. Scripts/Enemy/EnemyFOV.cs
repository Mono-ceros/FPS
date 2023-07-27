using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRange = 15f;
    [Range(0, 360)]
    public float viewAngle = 120f;

    Transform enemyTr;
    Transform playerTr;
    int playerLayer;
    int obstacleLayer;
    int layerMask;

    private void Start()
    {
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        playerLayer = LayerMask.NameToLayer("Player");
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");
        layerMask = 1 << playerLayer | 1 << obstacleLayer;
    }

    public bool isTracePlayer()
    {
        bool isTrace = false;

        //설정된 반경만큼 OverlapSphere 메서드를 활용하여 플레이어 위치 탐지
        Collider[] colls = Physics.OverlapSphere(enemyTr.position,
                                                 viewRange,
                                                 1 << playerLayer);
        if(colls.Length == 1)
        {
            Vector3 dir = (playerTr.position - enemyTr.position).normalized;
            //적의 시야각에 플레이어가 존재하는지 판단
            if(Vector3.Angle(enemyTr.forward, dir) < viewAngle * 0.5)
            {
                isTrace = true;
            }
        }
        return isTrace;
    }

    public bool isViewPlayer()
    {
        bool isView = false;
        RaycastHit hit;

        Vector3 dir = (playerTr.position - enemyTr.position).normalized;
        if(Physics.Raycast(enemyTr.position, dir, out hit, viewRange, layerMask))
        {
            isView = hit.collider.CompareTag("Player");
        }
        return isView;
    }

    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
