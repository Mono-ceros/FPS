using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;

public class EnemyAi : MonoBehaviour
{

    public enum State   //상태에 대한 정의
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    public State state = State.PATROL; //초기 상태는 순찰 상태로

    Transform playerTr;
    Transform enemyTr;

    [Range(1f, 10f)]
    public float attackDist = 5f;
    [Range(5f, 20f)]
    public float traceDist = 10f;
    public bool isDie = false;

    WaitForSeconds ws; //코루틴에서 사용할 시간 지연 변수
    //업데이트문쓰는 대신 코루틴을 써서 리소스를 조금이라도 줄이는것

    MoveAgent moveAgent;
    Animator animator;
    EnemyFire enemyFire;


    //애니메이터 컨트롤러의 파라미터를 int 변수에 미리 등록해 성능 향상
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
        //태크를 이용해서 플레이어 오브젝트 찾음
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


        //시간 지연 변수는 new 키워드로 초기화 하며
        //설정된 시간 만큼 잠시 대기(지연)
        ws = new WaitForSeconds(0.3f);

        animator.SetFloat(hashOffset, Random.Range(0f, 1f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1f, 2f));
    }

    private void OnEnable()
    {
        //상태 체크 코루틴 함수 호출
        StartCoroutine(CheckState());
        StartCoroutine(Action());

        //이벤트 메소드에 호출할 메소드 연결
        //Damage(이벤트) += EnemyAi(호출될 메소드)

        Damage.OnPlayerDieEvent += this.OnPlayerDie;
    }

    private void OnDisable()
    {
        Damage.OnPlayerDieEvent -= this.OnPlayerDie;
    }

    IEnumerator CheckState()
    {
        //다른 스크립트 초기화를 위한 대기시간
        yield return new WaitForSeconds(1);

        //플레이어와 적 사이의 거리 계산
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
            yield return ws;    //코루틴 반환
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

                    //죽고나서 콜라이더 비활성화
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
