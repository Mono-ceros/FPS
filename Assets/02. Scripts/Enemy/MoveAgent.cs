using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//�ش� ��Ʈ����Ʈ�� �ʼ� ������Ʈ�� ���
//�ݵ�� NavMeshAgent�� �־����.
[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    public List<Transform> wayPoints;   //���� ���� ����� List
    public int nextIdx; //���� �������� ���� ����

    NavMeshAgent agent;

    readonly float patrolSpeed = 1.5f;
    readonly float traceSpeed = 4.0f;

    float damping = 1f; //ȸ���ӵ� ���� ���
    Transform enemyTr;

    bool patrolling;
    public bool PATROLLING
    {
        get{return patrolling; }
        set
        {
            patrolling = value;
            if(patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1f;
                MoveWayPoint();
            }
        }
    }

    Vector3 traceTarget;
    public Vector3 TRACETARGET
    {
        get { return traceTarget; }
        set
        {
            traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7f;
            //���� ��� ���� �Լ� ȣ��
            TraceTarget(traceTarget);
        }
    }

    public float SPEED
    {
        get { return agent.velocity.magnitude; }
    }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale)
            return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    public void Stop()
    {
        agent.isStopped = true;
        //���ڸ��� ���߱� ���Ͽ� �ӵ� 0����
        agent.velocity = Vector3.zero;
        patrolling = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyTr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        //�������� �����Կ� ���� �����ϴ� �ɼ� ��Ȱ��ȭ
        agent.autoBraking = false;
        agent.updateRotation = false;

        //Find�� �̸�����
        //���̾��Ű�� �ִ� ������Ʈ�� Ž���ϴ� �޼���
        //�ϳ��ϳ� Ȯ���ؼ� �������ϸ� ����Ŵ ���� ����
        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            //�ڽ� ������Ʈ���� ������ �ͼ� waypoints ����Ʈ�� �Ҵ�
            group.GetComponentsInChildren<Transform>(wayPoints);
            //����Ʈ�� ���� ó�� �׸��� ����
            //�θ������Ʈ���� ���͹����� 0�� ����
            wayPoints.RemoveAt(0);

            //���� ����Ʈ ����
            nextIdx = Random.Range(0, wayPoints.Count);
        }

        //������������ �����̴� �޼ҵ� ȣ��
        //MoveWayPoint();
        this.patrolling = true;
    }

   

    void MoveWayPoint()
    {
        //��� ������̸� �� ������ ����
        if (agent.isPathStale)
            return;
        //���� �������� ����Ʈ���� �����ؼ� ����
        agent.destination = wayPoints[nextIdx].position;
        //�׺���̼� ��� Ȱ��ȭ = ������
        agent.isStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!agent.isStopped)    //ĳ���� �̵����̸�
        {
            //NavMeshAgent�� �����ؾߵ� ���� ���͸� ȸ�������� ��ȯ
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation,
                                                rot,
                                                Time.deltaTime * damping);
        }


        //���� ���°� �ƴ� ��쿡�� �Ʒ� �ڵ� �������
        if (!patrolling)
            return;

        //�̵��ӵ��� 0.2 * 0.2 �̻��̸鼭 �����Ÿ��� 0.5����
        //sqrMagnitude�� ���ɶ����� �����ٿ����� ��
        if (agent.velocity.sqrMagnitude >- 0.2f * 0.2f &&
            agent.remainingDistance <= 0.5f)
        {
            //nextIdx++;
            //���������� �����̼��� ���ؼ� ������ ����
            // 5�� ������ 0,1,2,3,4,5=0,6=1, �̷������� ��ȯ
            //nextIdx = nextIdx % wayPoints.Count;
            nextIdx = Random.Range(0, wayPoints.Count);

            MoveWayPoint();
        }
    }
}
