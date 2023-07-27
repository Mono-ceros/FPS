using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    float iniHp = 100f;
    public float currHp;

    public delegate void PlayerDieHandler(); //델리게이트 생성
    public static event PlayerDieHandler OnPlayerDieEvent; //델리게이트를 사용해 event 함수
    //static을 사용해 객체를 통하지 않고 전역적으로 사용가능

    public Image bloodScreen;

    public Image hpBar;
    readonly Color initColor = new Vector4(0, 1f, 0f, 1f);
    Color currColor;

    void Start()
    {
        currHp = iniHp;

        hpBar.color = initColor;
        currColor = initColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BULLET"))
        {
            Destroy(other.gameObject);


            //플레이어 피격 효과 코루틴 함수 호출
            StartCoroutine(ShowBloodScreen());

            currHp -= 5f;

            //체력바 게이지의 색상 및 크기 변경
            DisplayHpBar();

            if(currHp <= 0f)
            {
                //플레이어 사망 함수 호출
                PlayerDie();
            }
        }
    }

    void DisplayHpBar()
    {
        float currHpPercent = currHp / iniHp; //현재 체력의 비율
        
        if(currHpPercent > 0.5f)    //HP 50% 보다 많을 떄
        {
            //빨강 비율 낮아짐
            currColor.r = (1 - currHpPercent) * 2f;
        }
        else //50% 미만
        {
            //초록 비율 높아짐
            currColor.g = currHpPercent * 2f;
        }

        hpBar.color = currColor; //현재 색상 적용
        hpBar.fillAmount = currHpPercent; //체력 게이지 변경
    }

    IEnumerator ShowBloodScreen()
    {
        //알파값을 조정하여 화면에 보이게함
        bloodScreen.color = new Color(1f, 0, 0, Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);
        //이후에 다시 clear(0,0,0,0)값을 지니도록 하여 보이지 않도록
        bloodScreen.color = Color.clear;
    }

    void PlayerDie()
    {
        //생성한 이벤트 호출
        OnPlayerDieEvent();
        GameManager.Instance.isGameOver = true;

        //print("플레이어 사망");
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");

    }
    
}
