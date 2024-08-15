using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Thumper : MonoBehaviour
{
    //����

    // �̵��� ������ ���尡 �鸰��.
    // ��ó�� ������ �̵��Ѵ�.

    // �÷��̾ �þ߰����� �ν��Ѵ�.
    // �÷��̾��� �Ҹ��� ���� ���Ѵ�.

    // �÷��̾ �ν��ϸ� ������ ������.
    // �÷��̾ ���� �������� ���ӵ��� ������ �޷����� �����Ѵ�.

    //�÷��̾ ���������� �ν��� ��ҿ��� ��ġ�� �� ��ó�� Ž��.

    // �ٽ� �Ϲ� Ž��.

    // ��� ���� ������ ������ ���� �μ� �� �ִ�.
    // �� 4�� ������ ����Ѵ�.

    public enum ThpState
    {
        Idle,
        Patrol,
        Scream,
        Rush,
        Trace,
        Attack,
        AttackDelay,
        Damaged,
        Dead
    }
    [Header("���� Enum")]
    public ThpState thpstate;
    public Animator animator;
    public NavMeshAgent smith;

    [Header("���ʹ�Ÿ��")]
    public Transform target;

    [Header("���� �þ߰�")]
    [Range (5.0f, 15.0f)]
    public float sightRot = 5f;
    public float sightDis = 15.0f;
    [Header("���� �þ� Gizmo")]
    public bool drawsight;

    //Idle ���� ����
    public float currentTime = 0;
    public float idletime;


    //Patrol ���� ����
    [Header("���� Patrol ����")]
    [Range (3.0f, 15.0f)]
    public float patrolRadius = 6.0f;
    Vector3 patrolCenter;
    Vector3 patrolNext;
    [Header("���� ���� Gizmo")]
    public bool drawrange;
    [Header("���� Patrol �ӵ�")]
    [Range(2.0f, 8.0f)]
    public float patrolSpd = 3.0f;


    //Scream ���� ����
    bool isalreadyScream;

    //Trace ���� ����
    [Header("n�ʰ� �þ߿� �÷��̾ ���� ������ Idle")]
    [Range(1.0f, 5.0f)]
    public float setsearchingTime = 3;
    float searchingTime;
    Transform tracetarget; //Ÿ���� ���̴��� üũ�ϴ� ��
    [Header("�߰� �ӵ�")]
    [Range(1.0f, 20.0f)]
    public float traceSpd = 10.0f;

    [Header("ȸ�� �ӵ�")]
    [Range(100.0f, 400.0f)]
    public float rotSpd = 300.0f;

    [Header("���� ����")]
    public float attackRange = 1.5f;

    //attackDelay���� ����
    [Header("���� ������")]
    public float attackDelaytime = 1.0f;

    [Header("���� ����Ŭ��")]
    public AudioClip roar;
    public AudioClip attackSound;
    public AudioClip hitSound;
    public AudioClip dieSound;

    //ĳ�� ���� ����
    CharacterController cc;

    //���� �����
    AudioSource audioSource;


    void Start()
    {
        cc = GetComponent<CharacterController>(); //ĳ���� ��Ʈ�ѷ�
        audioSource = GetComponent<AudioSource>(); //������ҽ�
        smith = GetComponent<NavMeshAgent>(); //AI

        if (smith != null)
        {
            smith.speed = patrolSpd;
            smith.angularSpeed = rotSpd;
            smith.autoBraking = true;
            smith.autoTraverseOffMeshLink = false;
            smith.stoppingDistance = 0.4f;
            smith.acceleration = 3.0f;
        }

        // ��Ʈ�� �ʱ�ȭ
        patrolCenter = transform.position;
        Vector2 newPos = UnityEngine.Random.insideUnitCircle * patrolRadius;
        patrolNext = patrolCenter + new Vector3(newPos.x, 0, newPos.y);

        idletime = UnityEngine.Random.Range(1.0f, 4.0f); // �ʱ� idletime ����

        searchingTime = setsearchingTime; //teace ���� ���� �ʱ�ȭ

    }

    void Update()
    {
        switch (thpstate)
        {
            case ThpState.Idle:
                Idle();
                break;
            case ThpState.Patrol:
                Patrol();
                break;
            case ThpState.Scream:
                Scream();
                break;
            case ThpState.Rush:
                //Rush();
                break;
            case ThpState.Trace:
                Trace();
                break;
            case ThpState.Attack:
                Attack();
                break;
            case ThpState.AttackDelay:
                AttackDelay();
                break;
            case ThpState.Damaged:
                //Damaged();
                break;
            case ThpState.Dead:
                //Dead();
                break;
        }
    }


    private void Idle()
    {
        //ó����������
        //random 3~6������
        //�÷��̾ ���̸� ��ũ��

        CheckSight(sightRot, sightDis); //�÷��̾� ���̳�?

        currentTime += Time.deltaTime; //�ð� ������

        if (currentTime > idletime) //�ð��� ������...
        {
            currentTime = 0;
            thpstate = ThpState.Patrol;
            print("ThpState : Idle >>> Patrol");
            // ��Ʈ�� ���� ����
            animator.SetBool("IsPatrol", true);

        }

    }

    private void Patrol()
    {
        //�������� ���ƴٴϸ鼭 Ž��
        //���ư��� ���� ������ �����Ͱ���
        //idle�� �ݺ�
        //�÷��̾ ���̸� ��ũ��
        CheckSight(sightRot, sightDis); //�÷��̾� ���̳�?

        //���õ� �������� �̵��Ѵ�.
        Vector3 dir = patrolNext - transform.position;
        //dir.y = 0;

        if (dir.magnitude > 0.5f)
        {
            //cc.Move(dir.normalized * patrolSpd * Time.deltaTime);
            //SmoothRotateToYou(dir);

            //Nav�� �����̱�
            smith.SetDestination(patrolNext);

            //��ֹ� �����ؼ� ��Ʈ�� ������
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 1.0f))
            {
                SetNewPatrolPoint();
            }

        }
        else
        {
            SetNewPatrolPoint();
        }
    }

    void CheckSight(float degree, float maxDistance)
    {

        target = null; //�þ� üũ �Ҷ����� Ÿ�� ���.

        // 1. ���� �ȿ� ��ġ�� ������Ʈ �߿� Tag�� "Player"�� ������Ʈ�� ��� ã�´�.
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++) //���� ���� ������Ʈ ����
        {
            float distance = Vector3.Distance(players[i].transform.position, transform.position); //�����Լ� ��� ���

            if (distance <= maxDistance)
            {
                // 3. ã�� ������Ʈ�� �ٶ󺸴� ���Ϳ� ���� ���� ���͸� �����Ѵ�.
                //���� ���� ���ʹ� Ʈ������.forward.
                Vector3 lookvector = players[i].transform.position - transform.position; //������Ʈ�� �ٶ󺸴� ����
                lookvector.Normalize();

                float cosTheta = Vector3.Dot(transform.forward, lookvector); //<�����ϴ� �Լ�
                float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;

                if (cosTheta > 0 && theta < degree)
                {
                    target = players[i].transform;

                    //scream ���·� ��ȯ�Ѵ�.
                    thpstate = ThpState.Scream;
                    print("MyState : Idle/Patrol >>>> Scream");
                    animator.SetTrigger("PlayerIsHere");
                }
            }
        }

    }

    // ���ο� ��Ʈ�� ������ �����ϴ� �Լ�
    void SetNewPatrolPoint()
    {
        Vector2 newPos = UnityEngine.Random.insideUnitCircle * patrolRadius;
        patrolNext = patrolCenter + new Vector3(newPos.x, 0, newPos.y);

        thpstate = ThpState.Idle; // ���̵� ���·� ��ȯ
        idletime = UnityEngine.Random.Range(1.0f, 3.0f); // ���ο� ���̵� �ð� ����
        print("Debug_Idletime : " + idletime);
        print("ThpState : Patrol >>>> Idle");
        animator.SetBool("IsPatrol", false);
        smith.isStopped = true;
        smith.ResetPath();
    }



    private void Scream()
    {
        if (!isalreadyScream)
        {
            //nav ���߰�
            smith.isStopped = true;
            smith.ResetPath();
            //�����
            audioSource.clip = roar;
            audioSource.Play(); //�Ҹ� �O �����鼭
            Invoke("ScreamWhatnext", 1.5f); //1�� �ð��ֱ�
            isalreadyScream = true;
        }
        Vector3 dir = target.position - transform.position;
        dir.y = 0; //�𵨸� �ü��� �������� �����
        SmoothRotateToYou(dir);
    }

    void ScreamWhatnext()
    {
        //���� �Ÿ� �̻��̸� Rush
        //���� �Ÿ� ���ϸ� Trace

        thpstate = ThpState.Trace;
        print("MyState : Scream >>>> Trace");
        animator.SetTrigger("StartRunning");

        isalreadyScream = false;
    }

    private void Rush()
    {
        //�÷��̾ �ν��� ��ġ�� �� �ѹ� ��ǥ�����鼭
        //���� ����
        //�ð��� ���� �ӵ��� ������ ����
        //������ ������ Trace
    }

    private void Trace()
    {
        //�÷��̾ ã�� �þ� �̵�
        //�� �þ߿� �÷��̾ �� ���̴� ���°� 3�� �̻� ���ӵǸ� patrol
        //�� �þ� �ȿ� �÷��̾ ���� �� �÷��̾ ����
        //���ù��� �ȿ� �÷��̾ ������ ����

        traceSight(sightRot, sightDis);

        if (tracetarget == null)
        {
            searchingTime -= Time.deltaTime;
            if (searchingTime < 0)
            {
                //nav ���߰�
                smith.isStopped = true;
                smith.ResetPath();
                //�ӵ� �����ϰ�...
                smith.speed = patrolSpd;
                smith.acceleration = 3.0f;

                //���� Idle�� ��ȯ.
                thpstate = ThpState.Idle;
                print("MyState : Trace >>> Idle");
                animator.SetTrigger("LostPlayer");
                searchingTime = setsearchingTime;
                return;
            }
        }
        else
        {
            searchingTime = setsearchingTime;
        }

        Vector3 dir = target.position - transform.position;
        dir.y = 0;
        //cc.Move(dir.normalized * traceSpd * Time.deltaTime);
        //SmoothRotateToYou(dir);

        //�߰� ����...
        smith.SetDestination(target.position);
        smith.speed = traceSpd;
        smith.acceleration = 35.0f; //���ӵ� ��������

        if (dir.magnitude < attackRange)
        {
            currentTime = 0;
            searchingTime = setsearchingTime;
            //���߰�
            smith.isStopped = true;
            smith.ResetPath();

            //������!
            thpstate = ThpState.Attack;
            print("MyState : Trace >>> Attack");
            animator.SetTrigger("Attack");
        }

    }

    void traceSight(float degree, float maxDistance)
    {

        tracetarget = null; //�þ� üũ �Ҷ����� Ÿ�� ���.

        // 1. ���� �ȿ� ��ġ�� ������Ʈ �߿� Tag�� "Player"�� ������Ʈ�� ��� ã�´�.
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++) //���� ���� ������Ʈ ����
        {
            float distance = Vector3.Distance(players[i].transform.position, transform.position); //�����Լ� ��� ���

            if (distance <= maxDistance)
            {
                // 3. ã�� ������Ʈ�� �ٶ󺸴� ���Ϳ� ���� ���� ���͸� �����Ѵ�.
                //���� ���� ���ʹ� Ʈ������.forward.
                Vector3 lookvector = players[i].transform.position - transform.position; //������Ʈ�� �ٶ󺸴� ����
                lookvector.Normalize();

                float cosTheta = Vector3.Dot(transform.forward, lookvector); //<�����ϴ� �Լ�
                float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;

                if (cosTheta > 0 && theta < degree)
                {
                    tracetarget = players[i].transform;
                }
            }
        }
    }

    private void Attack()
    {
        //���� �ִϸ��̼�
        audioSource.clip = attackSound;
        audioSource.Play();
     
        //���õ����̷� �Ѿ��
        thpstate = ThpState.AttackDelay;
        print("MyState : Attack >>> AttackDelay");
    }

    private void AttackDelay()
    {
        currentTime += Time.deltaTime;
        searchingTime -= Time.deltaTime;
        if (attackDelaytime < currentTime)
        {
            //�Ÿ� �ѹ� ��߰ڴ�...
            Vector3 dir = target.position - transform.position;
            dir.y = 0;

            if (dir.magnitude < attackRange) //�Ÿ��� ���ݹ����� ����
            {
                currentTime = 0;
                searchingTime = setsearchingTime;
                thpstate = ThpState.Attack;
                print("MyState : Trace >>> Attack");
                animator.SetTrigger("Attack");
            }
            else // �ƴϸ� �߰���
            {
                thpstate = ThpState.Trace;
                print("MyState : AttackDelay >>> Trace");
                animator.SetTrigger("StartRunning");
                return;
            }
        }
        else
        {
            Vector3 dir = target.position - transform.position;
            dir.y = 0;
            SmoothRotateToYou(dir);
        }

        //���� �ݶ��̴� Disable
        //1������ ���
        //�Ÿ� �缭 Trace�� Rush��
    }

    public void Damaged()//GameMAnager���� �����ϱ�
    {
        thpstate = ThpState.Damaged;
        print("MyState : >>>Damaged");
        //ó�´� �ִϸ��̼�
        animator.SetTrigger("Ishitted");
        //ó�´� �Ҹ�
        audioSource.clip = hitSound;
        audioSource.Play();

        //target �־��ֱ�
        target = null;
        GameObject targetobj = GameObject.Find("EnemyTarget");
        target = targetobj.transform;

        //�� ������
        Vector3 dir = target.position - transform.position;
        dir.y = 0;
        //cc.Move(dir.normalized * traceSpd * Time.deltaTime);
        SmoothRotateToYou(dir);

        //ó�°� trace ����
        Invoke("ScreamWhatnext", 0.7f);

    }

    public void Dead() //GameMAnager���� �����ϱ�
    {
        thpstate = ThpState.Dead;
        animator.SetTrigger("IsDead");
        audioSource.clip = dieSound;
        audioSource.Play();
        //���
        //��κ��� ��� Disable
        cc.enabled = false;
        smith.enabled = false;
    }


    
    void SmoothRotateToYou(Vector3 dir)
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(dir.normalized);
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, 3f * Time.deltaTime);
    }


    private void OnDrawGizmos()
    {
        #region �þ߰�
        //�þ߰�
        if (drawsight)
        {
            Gizmos.color = Color.red;
            float rightDegree = 90 - sightRot;
            float leftDegree = 90 + sightRot;

            Vector3 rightpos = (transform.right * Mathf.Cos(rightDegree * Mathf.Deg2Rad) +
                               transform.forward * MathF.Sin(rightDegree * Mathf.Deg2Rad)) * sightDis
                              + transform.position;


            Vector3 leftpos = (transform.right * Mathf.Cos(leftDegree * Mathf.Deg2Rad) +
                              transform.forward * MathF.Sin(leftDegree * Mathf.Deg2Rad)) * sightDis
                              + transform.position;

            Gizmos.DrawLine(transform.position, rightpos);
            Gizmos.DrawLine(transform.position, leftpos);
        }
        #endregion

        #region ��Ʈ�� �Ÿ�
        if (drawrange)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, patrolRadius);
        }
        #endregion

    }


}
