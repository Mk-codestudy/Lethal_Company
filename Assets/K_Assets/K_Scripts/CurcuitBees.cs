using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CurcuitBees : MonoBehaviour
{
    //ȸ�� ����

    //������ �Բ� �����ȴ�.
    //������ �پ��ִ� ��� / �÷��̾ �Ѵ� ��� 2����.

    //�÷��̾ ��ó�� �ٰ����� ȸ�ι��� �÷��̾ �߰��ϱ� �����Ѵ�.

    //�÷��̾ ������ ������ �ָ� �������� ������ ���ư���.
    //������ ���� �Ÿ� �̻� �־����ų� �ٸ� �������� �� ���� �Ҵ´ٸ� ������ ���� ���·� �����Ѵ�.


    //CheckSight �Լ� ���Ŀͼ� ���� �ٿ�����.
    

    //enum�� �Ẹ��.

    public enum BeeState
    {
        Idle = 0, //���� ������ �ִ� ����
        Chase = 1, //�÷��̾ �ν��ϰ� �Ѵ� ����
        Attack = 2, //������
        AttackDelay = 4, //������ ����
        ReturnHome = 8, //�������� ���ư���
        Mad = 16 //������ ���� ����
    }
    public BeeState beeState = BeeState.Idle;

    CharacterController cc;
    [Header("Enemytarget ���ӿ�����Ʈ")]
    public Transform playertarget; //�÷��̾� ��ġ
    [Header("BeeHive(����) ���ӿ�����Ʈ")]
    public Transform hivetarget; //���� ��ġ

    //���� �ν�
    [Header("�� �þ�")]
    [Range(5.0f, 15.0f)]
    public float sight = 10.0f; //�÷��̾� �ν� �þ�
    public bool showSightGizmo;

    [Range(15.0f, 30.0f)]
    public float sightHome = 20.0f; //���� ã�� �þ�
    public bool showHomeGizmo;

    //���� �̵�
    [Header("�� �ӵ�")]
    [Range(1.0f, 10.0f)]
    public float chaseSpeed = 2.0f; //�߰� �ӵ�
    [Range(0.5f, 10.0f)]
    public float returnSpeed = 1.0f; //�Ͱ� �ӵ�
    [Range(1.0f, 10.0f)]
    public float madspd = 1.5f; //��ģ �� �ӵ�


    //���� ���� �̵�(Mad)
    Vector3 patrolNext; //���� �̵� ����
    Vector3 patrolCenter; //���� �̵� �ݰ��� ���߾�
    [Header("MAD ����: ���� �̵� �ݰ�")]
    [Range(5f, 30f)]
    public float patrolRadius = 10.0f; //�� ���� �̵� �ݰ�


    //���� ����
    //[Range(1.0f, 2.5f)]
    float attackRange = 1.0f; //���� ���� (���� ũ��� �Ȱ���)
    [Header("���� ���� ��ġ")]
    public float attackDelayTime = 2.0f; //������ Ÿ��
    /*public*/ bool showAttackRangeGizmo;
    float currentTime = 0;
    [Header("N�ʸ��� �ǰ�")]
    [Range(0.5f, 2.0f)]
    public float damageTime = 1.0f;

    //�÷��̾� �ǰ� �ڷ�ƾ ���� ����
    bool isPlayerInZone;
    private Coroutine damageCoroutine;


    void Start()
    {
        if (playertarget == null)
        {
            Debug.LogWarning("Bee:: �÷��̾� ���ӿ�����Ʈ ������ �ο����� ����!");
        }
        if (hivetarget == null)
        {
            Debug.LogWarning("Bee:: ���� ���ӿ�����Ʈ ������ �ο����� ����!");
        }

        cc = GetComponent<CharacterController>();
        beeState = BeeState.Idle;
    }

    void Update()
    {
        switch (beeState)
        {
            case BeeState.Idle:
                Idle();
                break;
            case BeeState.Chase:
                Chase();
                break;
            case BeeState.Attack:
                Attack();
                break;
            case BeeState.AttackDelay:
                AttackDelay();
                break;
            case BeeState.ReturnHome:
                ReturnHome();
                break;
            case BeeState.Mad:
                Mad();
                break;
        }

    }

    #region Enum ���� �Լ�
    private void Idle()
    {
        //������ ���� ��ġ�� 0.1f ������ �� //���Ͽ��� ����
        //�νĹ��� ��ó�� �÷��̾ �ٰ����� �����Ϸ� �´�.

        //�÷��̾�� �� ������ �Ÿ� ���ϱ�

        if (Vector3.Distance(transform.position, playertarget.position) < sight) //�÷��̾ �þ� ���� ������
        {
            //cc.enabled = true;
            beeState = BeeState.Chase; //�Ѿư��� ����
            print("Bee: Idle >>> Chase");
        }
        else
        {
            //������ �پ��ִ´�.
            transform.position = hivetarget.position;
        }


    }

    private void Chase()
    {
        //�÷��̾ �߰� �Ÿ��� ����� ��...
        if (Vector3.Distance(transform.position, playertarget.position) > sight)
        {
            beeState = BeeState.ReturnHome;
            print("Bee: Chase >>> ReturnHome");
            return; //ü�̽� ��
        }

        Vector3 dir = playertarget.position - transform.position;
        if (dir.magnitude > attackRange)
        {
            //Ÿ���� ���� �߰� �̵��Ѵ�.
            cc.Move(dir.normalized * chaseSpeed * Time.deltaTime);
        }
        else
        {
            //���� ���� �̳��� ���� ���¸� Attack�� ��ȯ�Ѵ�.
            beeState = BeeState.Attack;
                      //���� ������ Ÿ�� ���⼭ �̸� �ʱ�ȭ �Ұž� �����
            print("Bee: Chase >>> Attack");
        }

    }
    private void Attack()
    {
        //�������Ѵ�.
        print("������!");
        //��ƼŬ �ѹ� Ȱ��ȭ�ϱ�
        //������ �ѹ� Ʋ���ֱ�

        //�������ϰ� �����̷� ����.
        beeState = BeeState.AttackDelay;
        print("Bee: Attack >>> AttackDelay");
    }

    private void AttackDelay()
    {
        //���� Ÿ�ٰ� �Ÿ��� ���� ������ ������ ����ٸ�...
        if (Vector3.Distance(transform.position, playertarget.position) > attackRange)
        {
            //�߰� ���·� ��ȯ
            beeState = BeeState.Chase;
            print("Bee: Attack >>> Chase");
            //������ Ÿ�� �ʱ�ȭ
            return; //��!
        }

        //���� �ð� ����Ѵ�.
        currentTime += Time.deltaTime;

        // ���� �ð��� �����ٸ� ���¸� �ٽ� ���� ���·� ��ȯ�Ѵ�.
        if (currentTime > attackDelayTime)
        {
            currentTime = 0;
            beeState = BeeState.Attack;
            print("Bee : AttackDelay >>>> Attack");
        }
    }
    private void ReturnHome()
    {
        if (Vector3.Distance(transform.position, playertarget.position) < sight) //�÷��̾ �þ� ���� ������
        {
            beeState = BeeState.Chase; //�Ѿư��� ����
            print("Bee: ReturnHome >>> Chase");
            return; //��!
        }
        //hive ��ġ�� �Ÿ� ���� �ִٸ�...
        if (Vector3.Distance(transform.position, hivetarget.position) < sightHome)
        {
            Vector3 dir = hivetarget.position - transform.position; //�� ������ ���
            cc.Move(dir.normalized * returnSpeed * Time.deltaTime); //���� ����

            if (dir.magnitude < 0.6f)
            {
                //cc.enabled = false; //ĳ���� ���� ������Ʈ ����
                beeState = BeeState.Idle;//�⺻ ���·� ���ƿ�.
                print("Bee : ReturnHome >>> Idle");
            }
        }
        else //���ٸ�...
        {
            beeState = BeeState.Mad;//��ĩ��.
            print("Bee : ReturnHome >>> Mad"); 
        }

    }

    private void Mad()
    {
        //���� �þ� ���� �÷��̾ ���� �� 
        //chase�� �ٲٰ�

        if (Vector3.Distance(playertarget.position, transform.position) < sight)
        {
            beeState = BeeState.Chase; //�Ѿư��� ����
            print("Bee: Mad >>> Chase");
        }

        //�׷��� ������
        Vector3 patdir = patrolNext - transform.position; //���� �̵� �� �� //������ ���̱׷ε� ����
        if (patdir.magnitude > 0.1f)
        {
            cc.Move(patdir.normalized * madspd * Time.deltaTime);
        }
        else
        {
            Vector2 newPos = UnityEngine.Random.insideUnitCircle * patrolRadius;
            patrolNext = patrolCenter + new Vector3(newPos.x, 0, newPos.y);
        }
    }

    #endregion

    #region collider & �ڷ�ƾ ������ �ο� �Լ���

    IEnumerator ApplyDamage()
    {
        while (isPlayerInZone)
        {
            yield return new WaitForSeconds(damageTime);
            GameManager_Proto.gm.PlayerOnDamaged();
            GameManager_Proto.gm.AnemHit();
            Debug.Log("Bee :: �÷��̾� ������ ����!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            isPlayerInZone = true;
            damageCoroutine = StartCoroutine(ApplyDamage());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            isPlayerInZone = false;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (showHomeGizmo)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, sightHome); //�� �ν� ���� �����
        }
        if (showSightGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sight); //�÷��̾� �ν� ���� �����
        }

        if (showAttackRangeGizmo)
        {
            Gizmos.color= Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange); //���� ���� �����
        }
    }
}
