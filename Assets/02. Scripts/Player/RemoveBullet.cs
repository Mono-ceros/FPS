using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect; //����ũ ������

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            //����ũ ȣ��
            ShowEffect(collision);
            //�浹�� ������Ʈ ����
            //Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }

    void ShowEffect(Collision coll)
    {
        //Collision������ ���� �浹������ ������ ����
        //���� �浹���� ù��° �浹 ����
        ContactPoint contact = coll.contacts[0];
        //���� ���Ͱ� �̷�� ȸ���� ����
        //FromToRotation�� ���غ��Ͷ� Ÿ�ٺ��͸� ��ġ��Ŵ.
        //����Ʈ�� -z��� �Ѿ��� ������� z������� ��ġ��Ŵ
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        //sparkEffect ���� ����
        GameObject spark = Instantiate(sparkEffect, contact.point + (-contact.normal * 0.05f), rot);
        //������ ����ũ ����Ʈ�� �θ� ������Ʈ ����
        spark.transform.SetParent(this.transform);
    }
}
