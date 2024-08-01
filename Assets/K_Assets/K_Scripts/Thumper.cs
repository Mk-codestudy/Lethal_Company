using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
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

    [Header("���ʹ�Ÿ��")]
    public Transform target;

    [Header("���� �þ߰�")]
    [Range (5.0f, 15.0f)]
    public float sightRot = 5f;
    public float sightDis = 15.0f;

    //Idle ���� ����
    float currentTime = 1;
    float idletime;


    //Patrol ���� ����
    public float patrolRadius = 6.0f;
    Vector3 patrolCenter;
    Vector3 patrolNext;
    [Header("���� Patrol �ӵ�")]
    [Range(2.0f, 8.0f)]
    public float patrolSpd = 3.0f;

    //ĳ�� ���� ����
    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();

        //��Ʈ��
        patrolCenter = transform.position;
        patrolNext = patrolCenter;

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
                Rush();
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
                Damaged();
                break;
            case ThpState.Dead:
                Dead();
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
        
        if (dir.magnitude > 0.1f)
        {
            cc.Move(dir.normalized * patrolSpd * Time.deltaTime);
        }
        else
        {
            Vector2 newPos = UnityEngine.Random.insideUnitCircle * patrolRadius; //�������� patrolRadius�� circle �Լ�
            
            patrolNext = patrolCenter + new Vector3(newPos.x, 0, newPos.y); //���� ��Ʈ�� ��ġ ���� �ְ�

            idletime = UnityEngine.Random.Range(1.0f, 5.0f); //���̵� �ð��� �������� �������ָ� ��
            print("Debug_Idletime : " + idletime);
            thpstate = ThpState.Idle; //���̵�� �ٲٰ�
            print("ThpState : Patrol >>>> Idle");

        }

    }

    private void Scream()
    {
        //�Ҹ� �O �����鼭
        //1�� �ð��ֱ�

        //���� �Ÿ� �̻��̸� Rush
        //���� �Ÿ� ���ϸ� Trace
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
    }

    private void Attack()
    {
        //���� �ִϸ��̼�
        //���� ����
        //���� ���� �ݶ��̴� inable (������ �� ���� HP�� �پ�鵵��)
        //���õ����̷� �Ѿ��
    }

    private void AttackDelay()
    {
        //���� �ݶ��̴� Disable
        //1������ ���
        //�Ÿ� �缭 Trace�� Rush��
    }

    private void Damaged()
    {
        //ó�´� �ִϸ��̼�
        //ó�´� �Ҹ�
        //HP����
    }

    private void Dead()
    {
        //���
        //��κ��� ��� Disable
    }

    //��� �þ�üũ�� �غ���...
    void CheckSight(float degree, float maxDistance)
    {

        target = null; //�þ� üũ �Ҷ����� Ÿ�� ���.

        // �þ� ���� �ȿ� ���� ����� �ִٸ� �� ����� Ÿ������ �����ϰ� �ʹ�.
        // �þ� ����(�þ߰� �¿� 5��, ����, �þ� �Ÿ�: 10����)
        // ��� ������ ���� �±�(Player) ����

        // 1. ���� �ȿ� ��ġ�� ������Ʈ �߿� Tag�� "Player"�� ������Ʈ�� ��� ã�´�.
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // 2. ã�� ������Ʈ�� �߿��� �Ÿ��� maxDistance �̳��� ������Ʈ�� ã�´�.

        for (int i = 0; i < players.Length; i++) //���� ���� ������Ʈ ����
        {
            //float distance = (players[i].transform.position - transform.position).magnitude; //������ ���
            float distance = Vector3.Distance(players[i].transform.position, transform.position); //�����Լ� ��� ���

            if (distance <= maxDistance)
            {
                // 3. ã�� ������Ʈ�� �ٶ󺸴� ���Ϳ� ���� ���� ���͸� �����Ѵ�.
                //���� ���� ���ʹ� Ʈ������.forward.
                Vector3 lookvector = players[i].transform.position - transform.position; //������Ʈ�� �ٶ󺸴� ����
                lookvector.Normalize();

                float cosTheta = Vector3.Dot(transform.forward, lookvector); //<�����ϴ� �Լ�
                float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg; //Acos�� �������� �������� ����Ѵ�

                // 4-1. ���� ������ ��� ���� 0���� ũ��(������ ���ʿ� �ִٴ� ��)...
                // 4-2. ���� ���հ��� ���� 30���� ������(���� �¿� 30�� �̳�)
                if (cosTheta > 0 && theta < degree)
                {
                    target = players[i].transform;

                    //���¸� trace ���·� ��ȯ�Ѵ�.
                    thpstate = ThpState.Scream;
                    print("MyState : Idle/Patrol >>>> Scream");
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        #region �þ߰�
        //�þ߰�
        Gizmos.color = Color.red;
        float rightDegree = 90 - sightRot;
        float leftDegree = 90 + sightRot;

        Vector3 rightpos = new Vector3(Mathf.Cos(rightDegree * Mathf.Deg2Rad),
                                                  0,
                                                  MathF.Sin(rightDegree * Mathf.Deg2Rad)) * sightDis
                                                + transform.position;

        Vector3 leftpos = new Vector3(Mathf.Cos(leftDegree * Mathf.Deg2Rad),
                                                  0,
                                                  MathF.Sin(leftDegree * Mathf.Deg2Rad)) * sightDis
                                                + transform.position;

        Gizmos.DrawLine(transform.position, rightpos);
        Gizmos.DrawLine(transform.position, leftpos);
        #endregion

        #region ��Ʈ�� �Ÿ�
        //Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(transform.position, patrolRadius);
        #endregion

    }


}
