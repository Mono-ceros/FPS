using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    Camera uiCamera;
    Canvas canvas;
    RectTransform rectParent;
    RectTransform rectHp;

    //hpBar 이미지의 위치 조절 오프셋
    [HideInInspector] //public 인스펙터에서 숨김
    public Vector3 offset = Vector3.zero;
    [HideInInspector] //public 인스펙터에서 숨김
    public Transform targetTr; //추적 대상



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
        //월드 좌표를 스크린의 좌표로 변환
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

        //카메라의 뒷쪽일때 좌표값 보정
        if(screenPos.z < 0f)
        {
            screenPos *= -1f;
        }
        var localPos = Vector2.zero;
        
        //스크린 좌표를 RectTransform 기준의 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, 
                                                                screenPos, 
                                                                uiCamera,
                                                                out localPos);
        
        //HpBar의 위치를 계산된 RectTransform 좌표로 설정
        rectHp.localPosition = localPos;
    }
}
