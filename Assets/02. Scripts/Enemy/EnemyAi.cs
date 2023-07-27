using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;

public class EnemyAi : MonoBehaviour
{

    public enum State   //���¿� ���� ����
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    public State state = State.PATROL; //�ʱ� ���´� ���� ���·�

    Transform playerTr;
    Transform enemyTr;

    [Range(1f, 10f)]
    public float attackDist = 5f;
    [Range(5f, 20f)]
    public float traceDist = 10f;
    public bool isDie = false;

    WaitForSeconds ws; //�ڷ�ƾ���� ����� �ð� ���� ����
    //������Ʈ������ ��� �ڷ�ƾ�� �Ἥ ���ҽ��� �����̶� ���̴°�

    MoveAgent moveAgent;
    Animator animator;
    EnemyFire enemyFire;


    //�ִϸ����� ��Ʈ�ѷ��� �Ķ���͸� int ������ �̸� ����� ���� ���
    readonly int hashMove = Animator.StringToHash("isMove");
    readonly int hashSpeed = Animator.StringToHash("Speed");
    readonly int hashDie = Animator.StringToHash("Die");
    readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    readonly int hashOffset = Animator.StringToHash("Offset");
    readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");

    EnemyFOV enemyFOV;

    private void Awake()
    {
        //��ũ�� �̿��ؼ� �÷��̾� ������Ʈ ã��
        var player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }

        enemyTr = GetComponent<Transform>();
        moveAgent = GetComponent<MoveAgent>();
        animator = GetComponent<Animator>();
        enemyFire = GetComponent<EnemyFire>(); 
        enemyFOV = GetComponent<EnemyFOV>();


        //�ð� ���� ������ new Ű����� �ʱ�ȭ �ϸ�
        //������ �ð� ��ŭ ��� ���(����)
        ws = new WaitForSeconds(0.3f);

        animator.SetFloat(hashOffset, Random.Range(0f, 1f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1f, 2f));
    }

    private void OnEnable()
    {
        //���� üũ �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(CheckState());
        StartCoroutine(Action());

        //�̺�Ʈ �޼ҵ忡 ȣ���� �޼ҵ� ����
        //Damage(�̺�Ʈ) += EnemyAi(ȣ��� �޼ҵ�)

        Damage.OnPlayerDieEvent += this.OnPlayerDie;
    }

    private void OnDisable()
    {
        Damage.OnPlayerDieEvent -= this.OnPlayerDie;
    }

    IEnumerator CheckState()
    {
        //�ٸ� ��ũ��Ʈ �ʱ�ȭ�� ���� ���ð�
        yield return new WaitForSeconds(1);

        //�÷��̾�� �� ������ �Ÿ� ���
        while(!isDie)
        {
            if (state == State.DIE)
                   yield break;

            float dist = Vector3.Distance(playerTr.position,
                                           enemyTr.position);
            if(dist <= attackDist)
            {
                if (enemyFOV.isViewPlayer())
                    state = State.ATTACK;
                else
                    state = State.TRACE;
            }
            else if (enemyFOV.isTracePlayer())
            {
                state = State.TRACE;
            }
            else if (dist <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }
            yield return ws;    //�ڷ�ƾ ��ȯ
        }
    }

    IEnumerator Action()
    {
        while(!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.PATROL:
                    enemyFire.isFire = false;
                    moveAgent.PATROLLING = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.TRACE:
                    enemyFire.isFire = false;
                    moveAgent.TRACETARGET = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    if (!enemyFire.isFire)
                    {
                        enemyFire.isFire = true;
                    }
                    break;
                case State.DIE:
                    this.gameObject.tag = "Untagged";

                    isDie = true;
                    enemyFire.isFire = false;
                    moveAgent.Stop();
                    animator.SetInteger(hashDieIdx, Random.Range(0, 3));
                    animator.SetTrigger(hashDie);

                    //�װ��� �ݶ��̴� ��Ȱ��ȭ
                    GetComponent<CapsuleCollider>().enabled = false;

                    break;
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.SPEED); 
    }

    public void OnPlayerDie()
    {
        moveAgent.Stop();
        enemyFire.isFire = false;
        StopAllCoroutines();

        animator.SetTrigger(hashPlayerDie);
    }
}
