using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Hygrodere : MonoBehaviour
{
    //���̱׷ε��


    //�÷��̾ �� ���̸� ���������� ������ �÷��̾��� ��ġ ��ó�� �������� �Ѵ�.
    //����.
    //���� �� �� �𸥴�.
    //���� ����� ���Ѵ�.
    //�׷����� �� �ʸӸ� ������ �� �ִ�. (���� ��~�� �پ�����)

    [Header("�ֳʹ� Ÿ��")]
    public Transform target;

    [Header("������ ���ǵ�")]
    [Range(0.5f, 4.0f)]
    public float spd = 1.0f; //������ ���ǵ�
    CharacterController cc; //������ �̵�

    public float sight = 10.0f; //������ �ν� �Ÿ�

    Vector3 patrolNext; //���� �̵� ����
    Vector3 patrolCenter; //���� �̵� �ݰ��� ���߾�
    public float patrolRadius = 8.0f; //������ ���� �̵� �ݰ�


    //�ǰ� ���� ����
    bool isPlayerInZone;
    private Coroutine damageCoroutine;
    [Header("N�ʸ��� �ǰ�")]
    public float damageTime = 1.0f;


    void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("Slime(Hygrodere): Ÿ�� ���� ����!!");
        }

        //��Ʈ��
        patrolCenter = transform.position; //�߰� ������ ĳ��
        patrolNext = patrolCenter; //���� ��ġ�� ���� ��ġ�� �ǵ��� �Ѵ�.
        
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        //�÷��̾ ����ٴѴ�. ������...

        //Vector3 dir = targe.position - transform.position;
        //��? �ٵ� Y�� �����ؾ��� 

        //�������� �������ؾ߰ڴ�.
        float h = target.position.x - transform.position.x;
        float v = target.position.z - transform.position.z;

        Vector3 dir = new Vector3(h, 0, v);
        
        if (dir.magnitude < sight) //�������� �þ� ���� �÷��̾ ���� ��
        {
            cc.Move(dir.normalized * spd * Time.deltaTime); //�÷��̾ ���� �������� �̵��Ѵ�.
        }
        else //���� ��
        {
            Vector3 patdir = patrolNext - transform.position; //���� �̵� �� ��
            if (patdir.magnitude > 0.1f)
            {
                cc.Move(patdir.normalized * spd * Time.deltaTime);
            }
            else
            {
                Vector2 newPos = Random.insideUnitCircle * patrolRadius; //������ 8�� circle
                patrolNext = patrolCenter + new Vector3(newPos.x, 0, newPos.y);
            }
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

        IEnumerator ApplyDamage()
        {
            while (isPlayerInZone)
            {
                yield return new WaitForSeconds(damageTime);
            GameManager_Proto.gm.PlayerOnDamaged();
            GameManager_Proto.gm.AnemHit();
            Debug.Log("Slime :: �÷��̾� ������ ����!");
            }
        }

}
