using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//EnemyFOV ��ũ��Ʈ�� Ȱ���ϱ� ���� Ŀ���� ���������� ���
[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyFOV fov = (EnemyFOV)target;
        //�þ߰�(����) �������� ��ǥ�� ���(�־��� ������ 1/2 ��������)
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);

        Handles.color = Color.white;
        Handles.DrawWireDisc(fov.transform.position, //���� ��ǥ
                             Vector3.up, //�븻 ����
                             fov.viewRange); //���� ������ ������

        Handles.color = new Color(1, 1, 1, 0.2f);
        Handles.DrawSolidArc(fov.transform.position,
                             Vector3.up,
                             fromAnglePos,//��ä�� ���� ����
                             fov.viewAngle, //��ä�� ����
                             fov.viewRange); //��ä�� ������ 

        Handles.Label(fov.transform.position + (fov.transform.forward * 2f),
                      fov.viewAngle.ToString());
    }
}
