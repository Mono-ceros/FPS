using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public Color _color = Color.yellow; //기즈모 색상
    public float _radius = 0.1f; //기즈모 사이즈

    private void OnDrawGizmos()
    {
        Gizmos.color = _color; //사용자가 기즈모 생성할때 사용하는 콜백 메소드
        Gizmos.DrawSphere(transform.position, _radius); //구모양의 기즈모를 만드는데 DrawSphere(생성위치, 크기)
    }
}
