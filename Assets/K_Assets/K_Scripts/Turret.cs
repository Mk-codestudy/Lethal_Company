using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;

public class Turret : MonoBehaviour
{

    Transform target;


    [Header("�ͷ� ���� ����")]
    [Range(0.0f, 90.0f)]
    public float sightRange = 30.0f; //�þ� ����
    [Range(2.0f, 30.0f)]
    public float sightDistance = 15.0f; //�þ� �Ÿ�

    public bool drawTurretGizmo;

    [Header("�ͷ� �����Ŭ��")]
    public AudioClip[] turretaudio;
    AudioSource audioSource;


    //Searching ���� ����
    float searchingtime = 0;

    //Searching ����� ���� ����
    bool alreadyplaySearch;

    //fire
    //float shootTime = 0;
    float shootDelay = 0.3f;
    //fire ����� ���� ����
    bool alreadyplayFire;

    public enum TurretState
    {
        Idle,
        Turning,
        Searching,
        Fire,
    }

    public TurretState turretstate = TurretState.Idle;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        switch (turretstate)
        {
            case TurretState.Idle:
                Idle();
                break;
            case TurretState.Turning:
                Turning();
                break;
            case TurretState.Searching:
                Searching();
                break;
            case TurretState.Fire:
                Fire();
                break;
        }
    }

    private void Idle()
    {
        CheckSight(sightRange, sightDistance);
        //1.5�� �ڿ� Turning�Ѵ�.
    }

    private void Turning()
    {
     
    }

    private void Searching()
    {
        if (!alreadyplaySearch)
        {
            Sound(0, false);//����� ���
            alreadyplaySearch = true;
        }
        //����� Ʋ���ش�.

        //0.7�� ����Ѵ�.

        searchingtime += Time.deltaTime;
        if (searchingtime > 0.7f)
        {
            //fire�� �Ѿ��.
            turretstate = TurretState.Fire;
            print("TurretState : Searching >>> Fire");
            
            alreadyplaySearch = false; //����� ����
            searchingtime = 0;
        }
    }

    private void Fire()
    {
        if (!alreadyplayFire)
        {
            Sound(1, true);
            alreadyplayFire = true;
            Invoke("GoIdle", 5f);//�κ�ũ�� �̶� �ѹ��� ó�־�
        }

        if (shootDelay > 0)
        {
            shootDelay -= Time.deltaTime;
        }
        else
        {
            print("����!");
            //ShootingTurret(); //0.5�ʸ��� ���� �� �澿 ���.
            //m_BulletSound.FireSound(); //�Ҹ�
            shootDelay = 0.3f; //0.3�ʸ��� ���̸� ó��.
        }

    }


    void CheckSight(float degree, float maxDistance)
    {

        target = null; //�þ� üũ �Ҷ����� Ÿ�� ���.

        // �þ� ���� �ȿ� ���� ����� �ִٸ� �� ����� Ÿ������ �����ϰ� �ʹ�.
        // �þ� ����(�þ߰� �¿� 30��, ����, �þ� �Ÿ�: 15����)
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
                float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg; //Acos = �������� ���� ���

                // 4-1. ���� ������ ��� ���� 0���� ũ��
                // 4-2. ���� ���հ��� ���� 30���� ������
                if (cosTheta > 0 && theta < degree)
                {
                    //target = players[i].transform;

                    //���¸� Searching ���·� ��ȯ�Ѵ�.
                    turretstate = TurretState.Searching;
                    print("TurretState : Idle/Turning >>>> Searching");

                }
            }
        }

    }

    void GoIdle()
    {
        turretstate = TurretState.Idle;
        print("TurretState : Fire >>>> Idle");
        alreadyplayFire = false;
        audioSource.Pause();
    }


    public void Sound(int num, bool loop)
    {
        audioSource.clip = turretaudio[num];
        audioSource.volume = 0.7f;
        audioSource.loop = loop;
        audioSource.Play();
    }


    #region ����� �׸���
    private void OnDrawGizmos()
    {
        if (drawTurretGizmo)//�þ߰� �׸���
        {
            float rightDegree = 90 - sightRange;
            float leftDegree = 90 + sightRange;

            Vector3 rightpos = (transform.right * Mathf.Cos(rightDegree * Mathf.Deg2Rad) +
                               transform.forward * MathF.Sin(rightDegree * Mathf.Deg2Rad)) * sightDistance
                              + transform.position;


            Vector3 leftpos = (transform.right * Mathf.Cos(leftDegree * Mathf.Deg2Rad) +
                              transform.forward * MathF.Sin(leftDegree * Mathf.Deg2Rad)) * sightDistance
                              + transform.position;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, rightpos);
            Gizmos.DrawLine(transform.position, leftpos);
        }
    }
    #endregion
}
