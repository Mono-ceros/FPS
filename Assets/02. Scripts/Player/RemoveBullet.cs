using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect; //스파크 프리팹

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            //스파크 호출
            ShowEffect(collision);
            //충돌한 오브젝트 삭제
            //Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }

    void ShowEffect(Collision coll)
    {
        //Collision정보를 토대로 충돌지점의 정보를 추출
        //여러 충돌점중 첫번째 충돌 지점
        ContactPoint contact = coll.contacts[0];
        //법선 벡터가 이루는 회전값 추출
        //FromToRotation은 기준벡터랑 타겟벡터를 일치시킴.
        //이펙트의 -z축과 총알의 진행방향 z축방향을 일치시킴
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        //sparkEffect 동적 생성
        GameObject spark = Instantiate(sparkEffect, contact.point + (-contact.normal * 0.05f), rot);
        //생성된 스파크 이펙트의 부모 오브젝트 설정
        spark.transform.SetParent(this.transform);
    }
}
