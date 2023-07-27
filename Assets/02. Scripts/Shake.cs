
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Transform shakeCamera;
    public bool shokaRotate = false;

    Vector3 originPos;
    Quaternion originRot;


    void Start()
    {
        originPos = shakeCamera.position;
        originRot = shakeCamera.rotation; 
    }

    public IEnumerator ShakeCamera(float duration = 0.05f,
                                   float magnitudePos = 0.03f,
                                   float magnitudeRot = 0.1f)
    {
        float passTime = 0f;//�ð���������

        while(passTime < duration)
        {
            //�������� 1�� ������ �������� ������ ��ġ��
            Vector3 shakePos = Random.insideUnitSphere;
            //������ ������ ���� ������� ī�޶� ����
            shakeCamera.localPosition = shakePos * magnitudePos;

            //�ұ�Ģ ȸ�� ����� ���
            if (shokaRotate)
            {
                //Random.Range�� �Ϲ� ���� ������
                //PerlinNoise�� 0���� 1���̿��� ���Ӽ��� �ִ� ���� ������
                Vector3 shakeRot = new Vector3(0,0,Mathf.PerlinNoise(Time.time, magnitudeRot));

                shakeCamera.localRotation = Quaternion.Euler(shakeRot);
            }

            passTime += Time.deltaTime;

            yield return null;
        }
        
        //���� ���� ī�޶� ���󺹱�
        shakeCamera.localPosition = originPos;
        shakeCamera.localRotation = originRot;
    }
}
