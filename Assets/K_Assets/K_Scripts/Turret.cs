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


    [Header("터렛 공격 각도")]
    [Range(0.0f, 90.0f)]
    public float sightRange = 30.0f; //시야 각도
    [Range(2.0f, 30.0f)]
    public float sightDistance = 15.0f; //시야 거리

    public bool drawTurretGizmo;

    [Header("터렛 오디오클립")]
    public AudioClip[] turretaudio;
    AudioSource audioSource;


    //Searching 관련 변수
    float searchingtime = 0;

    //Searching 오디오 관련 변수
    bool alreadyplaySearch;

    //fire
    //float shootTime = 0;
    float shootDelay = 0.3f;
    //fire 오디오 관련 변수
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
        //1.5초 뒤에 Turning한다.
    }

    private void Turning()
    {
     
    }

    private void Searching()
    {
        if (!alreadyplaySearch)
        {
            Sound(0, false);//오디오 재생
            alreadyplaySearch = true;
        }
        //오디오 틀어준다.

        //0.7초 대기한다.

        searchingtime += Time.deltaTime;
        if (searchingtime > 0.7f)
        {
            //fire로 넘어간다.
            turretstate = TurretState.Fire;
            print("TurretState : Searching >>> Fire");
            
            alreadyplaySearch = false; //오디오 리셋
            searchingtime = 0;
        }
    }

    private void Fire()
    {
        if (!alreadyplayFire)
        {
            Sound(1, true);
            alreadyplayFire = true;
            Invoke("GoIdle", 5f);//인보크도 이때 한번만 처넣어
        }

        if (shootDelay > 0)
        {
            shootDelay -= Time.deltaTime;
        }
        else
        {
            print("빵야!");
            //ShootingTurret(); //0.5초마다 레이 한 방씩 쏜다.
            //m_BulletSound.FireSound(); //소리
            shootDelay = 0.3f; //0.3초마다 레이를 처쏴.
        }

    }


    void CheckSight(float degree, float maxDistance)
    {

        target = null; //시야 체크 할때마다 타겟 비워.

        // 시야 범위 안에 들어온 대상이 있다면 그 대상을 타겟으로 설정하고 싶다.
        // 시야 범위(시야각 좌우 30도, 전방, 시야 거리: 15미터)
        // 대상 선택을 위한 태그(Player) 설정

        // 1. 월드 안에 배치된 오브젝트 중에 Tag가 "Player"인 오브젝트를 모두 찾는다.
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // 2. 찾은 오브젝트들 중에서 거리가 maxDistance 이내인 오브젝트만 찾는다.

        for (int i = 0; i < players.Length; i++) //범위 내의 오브젝트 추출
        {
            //float distance = (players[i].transform.position - transform.position).magnitude; //원시적 방법
            float distance = Vector3.Distance(players[i].transform.position, transform.position); //기존함수 사용 방법

            if (distance <= maxDistance)
            {
                // 3. 찾은 오브젝트를 바라보는 벡터와 나의 전방 벡터를 내적한다.
                //나의 전방 벡터는 트랜스폼.forward.
                Vector3 lookvector = players[i].transform.position - transform.position; //오브젝트를 바라보는 벡터
                lookvector.Normalize();

                float cosTheta = Vector3.Dot(transform.forward, lookvector); //<내적하는 함수
                float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg; //Acos = 각도값을 라디안 출력

                // 4-1. 만약 내적의 결과 값이 0보다 크면
                // 4-2. 만약 사잇각의 값이 30보다 작으면
                if (cosTheta > 0 && theta < degree)
                {
                    //target = players[i].transform;

                    //상태를 Searching 상태로 전환한다.
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


    #region 기즈모 그리기
    private void OnDrawGizmos()
    {
        if (drawTurretGizmo)//시야각 그리기
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
