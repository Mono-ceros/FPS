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
        //예약 폴더인 Resources 폴더에 있는 리소스(프리팹, 이미지등)
        //에 접근하여 해당 리소스를 가져옴.
        //만약 리소시스 안에 다른 폴더 속에 있으면 폴더명/파일명 으로 표시해야함 
        bloodEffect = Resources.Load<GameObject>("Blood");

        //체력바 생성 및 초기화 함수 호출
        SetHpBar();
    }

    void SetHpBar()
    {
        uiCanvas = GameObject.Find("UI_Canvas").GetComponent<Canvas>();
        GameObject hpBar = Instantiate(hpBarPrefab, uiCanvas.transform);
        //체력바 프리팹의 자식으로 있는 Image를 말함
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];

        var _hpBar = hpBar.GetComponent<EnemyHpBar>();
        _hpBar.targetTr = this.gameObject.transform;
        _hpBar.offset = hpBarOffset;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("BULLET"))
        {
            //혈흔 효과 생성 함수 호출
            ShowBloodEffect(collision);

            //Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);

            hp -= collision.gameObject.GetComponent<Bullet>().damage;

            hpBarImage.fillAmount = hp / iniHp;

            //실수는 0으로 딱 나눠 떨어지지않을수도 있음
            //정확하게 하기위해 이하로함.
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
