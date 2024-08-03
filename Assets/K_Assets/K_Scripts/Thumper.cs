using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Thumper : MonoBehaviour
{
    //덤퍼

    // 이동할 때마다 사운드가 들린다.
    // 근처를 느리게 이동한다.

    // 플레이어를 시야각으로 인식한다.
    // 플레이어의 소리는 듣지 못한다.

    // 플레이어를 인식하면 괴성을 지른다.
    // 플레이어를 향해 직선으로 가속도를 가지며 달려오기 시작한다.

    //플레이어를 마지막으로 인식한 장소에서 놓치면 그 근처를 탐색.

    // 다시 일반 탐색.

    // 어느 정도 가속이 붙으면 문을 부술 수 있다.
    // 삽에 4번 맞으면 사망한다.

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
    [Header("덤퍼 Enum")]
    public ThpState thpstate;

    [Header("에너미타겟")]
    public Transform target;

    [Header("덤퍼 시야각")]
    [Range (5.0f, 15.0f)]
    public float sightRot = 5f;
    public float sightDis = 15.0f;

    //Idle 관련 변수
    public float currentTime = 0;
    public float idletime;


    //Patrol 관련 변수
    public float patrolRadius = 6.0f;
    Vector3 patrolCenter;
    Vector3 patrolNext;
    [Header("덤퍼 Patrol 속도")]
    [Range(2.0f, 8.0f)]
    public float patrolSpd = 3.0f;


    //Scream 관련 변수
    bool isalreadyScream;

    //Trace 관련 변수
    [Header("n초간 시야에 플레이어가 들지 않으면 Idle")]
    [Range(1.0f, 5.0f)]
    public float setsearchingTime = 3;
    float searchingTime;
    Transform tracetarget; //타겟이 보이는지 체크하는 중
    [Header("추격 속도")]
    [Range(1.0f, 20.0f)]
    public float traceSpd = 10.0f;

    [Header("회전 속도")]
    [Range(1.0f, 8.0f)]
    public float rotSpd = 5.0f;

    [Header("공격 범위")]
    public float attackRange = 1.5f;

    //attackDelay관련 변수
    [Header("공격 딜레이")]
    public float attackDelaytime = 1.0f;


    //캐싱 관련 변수
    CharacterController cc;
    AudioSource audioSource;

    //덤퍼 오디오


    void Start()
    {
        cc = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();


        // 패트롤 초기화
        patrolCenter = transform.position;
        idletime = UnityEngine.Random.Range(1.0f, 4.0f); // 초기 idletime 설정

        searchingTime = setsearchingTime; //teace 관련 변수 초기화

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
        //처가만히있음
        //random 3~6초정도
        //플레이어가 보이면 스크림

        CheckSight(sightRot, sightDis); //플레이어 보이냐?

        currentTime += Time.deltaTime; //시간 보내기

        if (currentTime > idletime) //시간이 지나면...
        {
            currentTime = 0;
            thpstate = ThpState.Patrol;
            print("ThpState : Idle >>> Patrol");
            // 패트롤 지점 설정
        }

    }

    private void Patrol()
    {
        //여기저기 돌아다니면서 탐색
        //돌아가는 방향 넣으면 좋을것같음
        //idle과 반복
        //플레이어가 보이면 스크림
        CheckSight(sightRot, sightDis); //플레이어 보이냐?

        //선택된 지점으로 이동한다.
        Vector3 dir = patrolNext - transform.position;
        dir.y = 0;

        if (dir.magnitude > 0.1f)
        {
            cc.Move(dir.normalized * patrolSpd * Time.deltaTime);
            SmoothRotateToYou(dir);

        }
        else
        {
            // 다음 패트롤 위치 정하기
            Vector2 newPos = UnityEngine.Random.insideUnitCircle * patrolRadius;
            patrolNext = patrolCenter + new Vector3(newPos.x, 0, newPos.y);

            thpstate = ThpState.Idle; //아이들로 바꾸고
            idletime = UnityEngine.Random.Range(1.0f, 4.0f); //아이들 시간만 랜덤으로 지정해주면 됨
            print("Debug_Idletime : " + idletime);
            print("ThpState : Patrol >>>> Idle");

        }

    }

    void CheckSight(float degree, float maxDistance)
    {

        target = null; //시야 체크 할때마다 타겟 비워.

        // 1. 월드 안에 배치된 오브젝트 중에 Tag가 "Player"인 오브젝트를 모두 찾는다.
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++) //범위 내의 오브젝트 추출
        {
            float distance = Vector3.Distance(players[i].transform.position, transform.position); //기존함수 사용 방법

            if (distance <= maxDistance)
            {
                // 3. 찾은 오브젝트를 바라보는 벡터와 나의 전방 벡터를 내적한다.
                //나의 전방 벡터는 트랜스폼.forward.
                Vector3 lookvector = players[i].transform.position - transform.position; //오브젝트를 바라보는 벡터
                lookvector.Normalize();

                float cosTheta = Vector3.Dot(transform.forward, lookvector); //<내적하는 함수
                float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;

                if (cosTheta > 0 && theta < degree)
                {
                    target = players[i].transform;

                    //상태를 trace 상태로 전환한다.
                    thpstate = ThpState.Scream;
                    print("MyState : Idle/Patrol >>>> Scream");
                }
            }
        }

    }



    private void Scream()
    {
        if (!isalreadyScream)
        {
            audioSource.Play(); //소리 꿕 지르면서
            Invoke("ScreamWhatnext", 1.5f); //1초 시간주기
            isalreadyScream = true;
        }
        Vector3 dir = target.position - transform.position;
        dir.y = 0; //모델링 시선을 수평으로 만들기

        SmoothRotateToYou(dir);
    }

    void ScreamWhatnext()
    {
        //일정 거리 이상이면 Rush
        //일정 거리 이하면 Trace

        thpstate = ThpState.Trace;
        print("MyState : Scream >>>> Trace");

        isalreadyScream = false;
    }

    private void Rush()
    {
        //플레이어를 인식한 위치를 딱 한번 좌표찍으면서
        //직선 돌진
        //시간에 따라서 속도와 데미지 증가
        //러쉬가 끝나면 Trace
    }

    private void Trace()
    {
        //플레이어를 찾아 시야 이동
        //내 시야에 플레이어가 안 보이는 상태가 3초 이상 지속되면 patrol
        //내 시야 안에 플레이어가 있을 때 플레이어를 따라감
        //어택범위 안에 플레이어가 들어오면 어택

        traceSight(sightRot, sightDis);

        if (tracetarget == null)
        {
            searchingTime -= Time.deltaTime;
            if (searchingTime < 0)
            {
                //상태 Idle로 전환.
                thpstate = ThpState.Idle;
                print("MyState : Trace >>> Idle");
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
        cc.Move(dir.normalized * traceSpd * Time.deltaTime);

        SmoothRotateToYou(dir);

        if (dir.magnitude < attackRange)
        {
            currentTime = 0;
            searchingTime = setsearchingTime;
            thpstate = ThpState.Attack;
            print("MyState : Trace >>> Attack");
        }

    }

    void traceSight(float degree, float maxDistance)
    {

        tracetarget = null; //시야 체크 할때마다 타겟 비워.

        // 1. 월드 안에 배치된 오브젝트 중에 Tag가 "Player"인 오브젝트를 모두 찾는다.
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++) //범위 내의 오브젝트 추출
        {
            float distance = Vector3.Distance(players[i].transform.position, transform.position); //기존함수 사용 방법

            if (distance <= maxDistance)
            {
                // 3. 찾은 오브젝트를 바라보는 벡터와 나의 전방 벡터를 내적한다.
                //나의 전방 벡터는 트랜스폼.forward.
                Vector3 lookvector = players[i].transform.position - transform.position; //오브젝트를 바라보는 벡터
                lookvector.Normalize();

                float cosTheta = Vector3.Dot(transform.forward, lookvector); //<내적하는 함수
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
        GameManager_Proto.gm.AnemHit();
        GameManager_Proto.gm.PlayerOnDamaged();
        //공격 애니메이션
        //공격 사운드
        //공격 영역 콜라이더 inable (닿으면 한 번만 HP가 줄어들도록)
        //어택딜레이로 넘어가기
        thpstate = ThpState.AttackDelay;
        print("MyState : Attack >>> AttackDelay");
    }

    private void AttackDelay()
    {
        currentTime += Time.deltaTime;
        searchingTime -= Time.deltaTime;
        if (attackDelaytime < currentTime)
        {
            thpstate = ThpState.Trace;
            print("MyState : AttackDelay >>> Trace");
            return;
        }
        else
        {
            Vector3 dir = target.position - transform.position;
            dir.y = 0;
            SmoothRotateToYou(dir);
        }

        //공격 콜라이더 Disable
        //1초정도 대기
        //거리 재서 Trace냐 Rush냐
    }

    private void Damaged()
    {
        //처맞는 애니메이션
        //처맞는 소리
        //HP감소
    }

    private void Dead()
    {
        //쥬금
        //대부분의 기능 Disable
    }


    
    void SmoothRotateToYou(Vector3 dir)
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(dir.normalized);
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, rotSpd * Time.deltaTime);
    }


    private void OnDrawGizmos()
    {
        #region 시야각
        //시야각
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
        #endregion

        #region 패트롤 거리
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
        #endregion

    }


}
