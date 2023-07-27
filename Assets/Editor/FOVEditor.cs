using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//EnemyFOV 스크립트를 활용하기 위한 커스텀 에디터임을 명시
[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyFOV fov = (EnemyFOV)target;
        //시야각(원주) 시작점의 좌표를 계산(주어진 각도의 1/2 지점부터)
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);

        Handles.color = Color.white;
        Handles.DrawWireDisc(fov.transform.position, //원점 좌표
                             Vector3.up, //노말 벡터
                             fov.viewRange); //원의 반지름 사이즈

        Handles.color = new Color(1, 1, 1, 0.2f);
        Handles.DrawSolidArc(fov.transform.position,
                             Vector3.up,
                             fromAnglePos,//부채꼴 시작 각도
                             fov.viewAngle, //부채꼴 각도
                             fov.viewRange); //부채꼴 반지름 

        Handles.Label(fov.transform.position + (fov.transform.forward * 2f),
                      fov.viewAngle.ToString());
    }
}
