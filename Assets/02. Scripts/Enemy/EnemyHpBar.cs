using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    Camera uiCamera;
    Canvas canvas;
    RectTransform rectParent;
    RectTransform rectHp;

    //hpBar �̹����� ��ġ ���� ������
    [HideInInspector] //public �ν����Ϳ��� ����
    public Vector3 offset = Vector3.zero;
    [HideInInspector] //public �ν����Ϳ��� ����
    public Transform targetTr; //���� ���



    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        //���� ��ǥ�� ��ũ���� ��ǥ�� ��ȯ
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

        //ī�޶��� �����϶� ��ǥ�� ����
        if(screenPos.z < 0f)
        {
            screenPos *= -1f;
        }
        var localPos = Vector2.zero;
        
        //��ũ�� ��ǥ�� RectTransform ������ ��ǥ�� ��ȯ
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, 
                                                                screenPos, 
                                                                uiCamera,
                                                                out localPos);
        
        //HpBar�� ��ġ�� ���� RectTransform ��ǥ�� ����
        rectHp.localPosition = localPos;
    }
}
