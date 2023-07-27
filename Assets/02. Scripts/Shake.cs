
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
        float passTime = 0f;//시간누적변수

        while(passTime < duration)
        {
            //반지름이 1인 구형의 영역에서 랜덤한 위치값
            Vector3 shakePos = Random.insideUnitSphere;
            //위에서 추출한 값을 기반으로 카메라 흔들기
            shakeCamera.localPosition = shakePos * magnitudePos;

            //불규칙 회전 사용할 경우
            if (shokaRotate)
            {
                //Random.Range는 일반 난수 생성기
                //PerlinNoise는 0에서 1사이에서 연속성이 있는 난수 생성기
                Vector3 shakeRot = new Vector3(0,0,Mathf.PerlinNoise(Time.time, magnitudeRot));

                shakeCamera.localRotation = Quaternion.Euler(shakeRot);
            }

            passTime += Time.deltaTime;

            yield return null;
        }
        
        //흔들고 나서 카메라 원상복귀
        shakeCamera.localPosition = originPos;
        shakeCamera.localRotation = originRot;
    }
}
