using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FollowCam : MonoBehaviourPun
{
    public float moveDamping = 15f; //이동 속도 계수
    public float rotateDaming = 10f; //회전 속도 계수

    public float distance = 5f; //추적대상과 거리
    public float height = 4f; //대상과 높이
    public float targetOffset = 2f; //추적 좌표의 오프셋

    Transform tr; //CameraRig의 트랜스폼 컴포넌트


    [Header("벽 장애물 설정")]
    public float heightAboveWall = 7f;
    public float colliderRadius = 1f;
    public float overDamping = 5f;
    float originHeight;

    [Header("기타 장애물 설정")]
    public float heightAboveObstacle = 12f;
    public float castOffset = 1f;


    void Start()
    {
        tr = GameObject.FindGameObjectWithTag("CAMERA").GetComponent<Transform>();
        originHeight = height;
    }

    private void Update()
    { 
        //구체 형채의 콜라이더를 통해서 충돌체크
        if(Physics.CheckSphere(tr.position, colliderRadius))
        {
            height = Mathf.Lerp(height,
                                heightAboveWall,
                                Time.deltaTime * overDamping);
        }
        else
        {
            height = Mathf.Lerp(height,
                                originHeight,
                                Time.deltaTime * overDamping);
        }

        Vector3 castTarget = transform.position + (transform.up * castOffset);
        Vector3 castDir = (castTarget - tr.position).normalized;
        RaycastHit hit;

        if(Physics.Raycast(tr.position, castDir, out hit, Mathf.Infinity))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                height = Mathf.Lerp(height,
                                    heightAboveObstacle,
                                    Time.deltaTime * overDamping);
            }
            else
            {
                height = Mathf.Lerp(height,
                                    originHeight,
                                    Time.deltaTime * overDamping);
            }
        }
    }

    private void LateUpdate()
    {
        //카메라 움직임은 보통 LateUpdate 에 작성하는 경우가 많다
        //타겟이 되는 오브젝트가 먼저 움직이고 후에 카메라가 움직이기 때문
        
        var Campos = transform.position - (transform.forward * distance) + (transform.up * height);

        //이동할때 이동 속도 계수적용
        //Slerp = 구면선형 보간함수
        //Slerp(출발지점, 도착지점, 계수)
        tr.position = Vector3.Slerp(tr.position, Campos, Time.deltaTime * moveDamping);
        
        tr.rotation = Quaternion.Slerp(tr.rotation, transform.rotation, Time.deltaTime * rotateDaming);
        //position위치만 보면 모델 발부분을 처다봄
        //지정한 offset만큼 위를 보도록 조정
        tr.LookAt(transform.position + (transform.up * targetOffset));
    }
 
}
