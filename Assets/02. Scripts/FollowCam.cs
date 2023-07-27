using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FollowCam : MonoBehaviourPun
{
    public float moveDamping = 15f; //�̵� �ӵ� ���
    public float rotateDaming = 10f; //ȸ�� �ӵ� ���

    public float distance = 5f; //�������� �Ÿ�
    public float height = 4f; //���� ����
    public float targetOffset = 2f; //���� ��ǥ�� ������

    Transform tr; //CameraRig�� Ʈ������ ������Ʈ


    [Header("�� ��ֹ� ����")]
    public float heightAboveWall = 7f;
    public float colliderRadius = 1f;
    public float overDamping = 5f;
    float originHeight;

    [Header("��Ÿ ��ֹ� ����")]
    public float heightAboveObstacle = 12f;
    public float castOffset = 1f;


    void Start()
    {
        tr = GameObject.FindGameObjectWithTag("CAMERA").GetComponent<Transform>();
        originHeight = height;
    }

    private void Update()
    { 
        //��ü ��ä�� �ݶ��̴��� ���ؼ� �浹üũ
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
        //ī�޶� �������� ���� LateUpdate �� �ۼ��ϴ� ��찡 ����
        //Ÿ���� �Ǵ� ������Ʈ�� ���� �����̰� �Ŀ� ī�޶� �����̱� ����
        
        var Campos = transform.position - (transform.forward * distance) + (transform.up * height);

        //�̵��Ҷ� �̵� �ӵ� �������
        //Slerp = ���鼱�� �����Լ�
        //Slerp(�������, ��������, ���)
        tr.position = Vector3.Slerp(tr.position, Campos, Time.deltaTime * moveDamping);
        
        tr.rotation = Quaternion.Slerp(tr.rotation, transform.rotation, Time.deltaTime * rotateDaming);
        //position��ġ�� ���� �� �ߺκ��� ó�ٺ�
        //������ offset��ŭ ���� ������ ����
        tr.LookAt(transform.position + (transform.up * targetOffset));
    }
 
}
