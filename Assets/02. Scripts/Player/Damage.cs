using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    float iniHp = 100f;
    public float currHp;

    public delegate void PlayerDieHandler(); //��������Ʈ ����
    public static event PlayerDieHandler OnPlayerDieEvent; //��������Ʈ�� ����� event �Լ�
    //static�� ����� ��ü�� ������ �ʰ� ���������� ��밡��

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


            //�÷��̾� �ǰ� ȿ�� �ڷ�ƾ �Լ� ȣ��
            StartCoroutine(ShowBloodScreen());

            currHp -= 5f;

            //ü�¹� �������� ���� �� ũ�� ����
            DisplayHpBar();

            if(currHp <= 0f)
            {
                //�÷��̾� ��� �Լ� ȣ��
                PlayerDie();
            }
        }
    }

    void DisplayHpBar()
    {
        float currHpPercent = currHp / iniHp; //���� ü���� ����
        
        if(currHpPercent > 0.5f)    //HP 50% ���� ���� ��
        {
            //���� ���� ������
            currColor.r = (1 - currHpPercent) * 2f;
        }
        else //50% �̸�
        {
            //�ʷ� ���� ������
            currColor.g = currHpPercent * 2f;
        }

        hpBar.color = currColor; //���� ���� ����
        hpBar.fillAmount = currHpPercent; //ü�� ������ ����
    }

    IEnumerator ShowBloodScreen()
    {
        //���İ��� �����Ͽ� ȭ�鿡 ���̰���
        bloodScreen.color = new Color(1f, 0, 0, Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);
        //���Ŀ� �ٽ� clear(0,0,0,0)���� ���ϵ��� �Ͽ� ������ �ʵ���
        bloodScreen.color = Color.clear;
    }

    void PlayerDie()
    {
        //������ �̺�Ʈ ȣ��
        OnPlayerDieEvent();
        GameManager.Instance.isGameOver = true;

        //print("�÷��̾� ���");
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");

    }
    
}
