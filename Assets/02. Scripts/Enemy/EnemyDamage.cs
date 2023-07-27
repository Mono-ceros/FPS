using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    float hp = 100f;
    float iniHp = 100f;
    GameObject bloodEffect;

    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset = new Vector3 (0f, 2.2f, 0f);

    Canvas uiCanvas;
    Image hpBarImage;


    // Start is called before the first frame update
    void Start()
    {
        //���� ������ Resources ������ �ִ� ���ҽ�(������, �̹�����)
        //�� �����Ͽ� �ش� ���ҽ��� ������.
        //���� ���ҽý� �ȿ� �ٸ� ���� �ӿ� ������ ������/���ϸ� ���� ǥ���ؾ��� 
        bloodEffect = Resources.Load<GameObject>("Blood");

        //ü�¹� ���� �� �ʱ�ȭ �Լ� ȣ��
        SetHpBar();
    }

    void SetHpBar()
    {
        uiCanvas = GameObject.Find("UI_Canvas").GetComponent<Canvas>();
        GameObject hpBar = Instantiate(hpBarPrefab, uiCanvas.transform);
        //ü�¹� �������� �ڽ����� �ִ� Image�� ����
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];

        var _hpBar = hpBar.GetComponent<EnemyHpBar>();
        _hpBar.targetTr = this.gameObject.transform;
        _hpBar.offset = hpBarOffset;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("BULLET"))
        {
            //���� ȿ�� ���� �Լ� ȣ��
            ShowBloodEffect(collision);

            //Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);

            hp -= collision.gameObject.GetComponent<Bullet>().damage;

            hpBarImage.fillAmount = hp / iniHp;

            //�Ǽ��� 0���� �� ���� ���������������� ����
            //��Ȯ�ϰ� �ϱ����� ���Ϸ���.
            if(hp <= 0)
            {
                GetComponent<EnemyAi>().state = EnemyAi.State.DIE;

                hpBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
            }
        }
    }

    void ShowBloodEffect(Collision coll)
    {
        Vector3 pos = coll.contacts[0].point;
        Vector3 _normal = coll.contacts[0].normal;

        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);

        GameObject blood = Instantiate(bloodEffect, pos, rot);
        Destroy(blood, 1f);
    }
}
