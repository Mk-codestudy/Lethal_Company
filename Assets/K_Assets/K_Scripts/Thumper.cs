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
    float currentTime = 1;
    float idletime;


    //Patrol 관련 변수
    public float patrolRadius = 6.0f;
    Vector3 patrolCenter;
    Vector3 patrolNext;
    [Header("덤퍼 Patrol 속도")]
    [Range(2.0f, 8.0f)]
    public float patrolSpd = 3.0f;

    //캐싱 관련 변수
    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();

        //패트롤
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
        
        if (dir.magnitude > 0.1f)
        {
            cc.Move(dir.normalized * patrolSpd * Time.deltaTime);
        }
        else
        {
            Vector2 newPos = UnityEngine.Random.insideUnitCircle * patrolRadius; //반지름이 patrolRadius인 circle 함수
            
            patrolNext = patrolCenter + new Vector3(newPos.x, 0, newPos.y); //다음 패트롤 위치 정해 주고

            idletime = UnityEngine.Random.Range(1.0f, 5.0f); //아이들 시간만 랜덤으로 지정해주면 됨
            print("Debug_Idletime : " + idletime);
            thpstate = ThpState.Idle; //아이들로 바꾸고
            print("ThpState : Patrol >>>> Idle");

        }

    }

    private void Scream()
    {
        //소리 꿕 지르면서
        //1초 시간주기

        //일정 거리 이상이면 Rush
        //일정 거리 이하면 Trace
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
    }

    private void Attack()
    {
        //공격 애니메이션
        //공격 사운드
        //공격 영역 콜라이더 inable (닿으면 한 번만 HP가 줄어들도록)
        //어택딜레이로 넘어가기
    }

    private void AttackDelay()
    {
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

    //얘는 시야체크를 해보자...
    void CheckSight(float degree, float maxDistance)
    {

        target = null; //시야 체크 할때마다 타겟 비워.

        // 시야 범위 안에 들어온 대상이 있다면 그 대상을 타겟으로 설정하고 싶다.
        // 시야 범위(시야각 좌우 5도, 전방, 시야 거리: 10미터)
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
                float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg; //Acos는 각도값을 라디안으로 출력한대

                // 4-1. 만약 내적의 결과 값이 0보다 크면(나보다 앞쪽에 있다는 뜻)...
                // 4-2. 만약 사잇각의 값이 30보다 작으면(전방 좌우 30도 이내)
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

    private void OnDrawGizmos()
    {
        #region 시야각
        //시야각
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

        #region 패트롤 거리
        //Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(transform.position, patrolRadius);
        #endregion

    }


}
