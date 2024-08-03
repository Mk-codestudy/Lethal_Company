using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CurcuitBees : MonoBehaviour
{
    //회로 말벌

    //벌집과 함께 스폰된다.
    //벌집에 붙어있는 모드 / 플레이어를 쫓는 모드 2가지.

    //플레이어가 근처로 다가오면 회로벌이 플레이어를 추격하기 시작한다.

    //플레이어가 벌집을 버리고 멀리 도망가면 집으로 돌아간다.
    //벌집이 일정 거리 이상 멀어지거나 다른 공간으로 들어가 집을 잃는다면 무차별 공격 상태로 진입한다.


    //CheckSight 함수 훔쳐와서 갖다 붙여보기.
    

    //enum을 써보자.

    public enum BeeState
    {
        Idle = 0, //벌이 벌집에 있는 상태
        Chase = 1, //플레이어를 인식하고 쫓는 상태
        Attack = 2, //빠지직
        AttackDelay = 4, //빠지직 쉬기
        ReturnHome = 8, //벌집으로 돌아가기
        Mad = 16 //벌집을 잃은 상태
    }
    public BeeState beeState = BeeState.Idle;

    CharacterController cc;
    [Header("Enemytarget 게임오브젝트")]
    public Transform playertarget; //플레이어 위치
    [Header("BeeHive(벌집) 게임오브젝트")]
    public Transform hivetarget; //벌집 위치

    //벌의 인식
    [Header("벌 시야")]
    [Range(5.0f, 15.0f)]
    public float sight = 10.0f; //플레이어 인식 시야
    public bool showSightGizmo;

    [Range(15.0f, 30.0f)]
    public float sightHome = 20.0f; //벌집 찾는 시야
    public bool showHomeGizmo;

    //벌의 이동
    [Header("벌 속도")]
    [Range(1.0f, 10.0f)]
    public float chaseSpeed = 2.0f; //추격 속도
    [Range(0.5f, 10.0f)]
    public float returnSpeed = 1.0f; //귀가 속도
    [Range(1.0f, 10.0f)]
    public float madspd = 1.5f; //미친 벌 속도


    //벌의 랜덤 이동(Mad)
    Vector3 patrolNext; //랜덤 이동 지점
    Vector3 patrolCenter; //랜덤 이동 반경의 정중앙
    [Header("MAD 상태: 랜덤 이동 반경")]
    [Range(5f, 30f)]
    public float patrolRadius = 10.0f; //벌 랜덤 이동 반경


    //벌의 공격
    //[Range(1.0f, 2.5f)]
    float attackRange = 1.0f; //공격 범위 (벌의 크기랑 똑같음)
    [Header("공격 관련 수치")]
    public float attackDelayTime = 2.0f; //딜레이 타임
    /*public*/ bool showAttackRangeGizmo;
    float currentTime = 0;
    [Header("N초마다 피격")]
    [Range(0.5f, 2.0f)]
    public float damageTime = 1.0f;

    //플레이어 피격 코루틴 관련 변수
    bool isPlayerInZone;
    private Coroutine damageCoroutine;


    void Start()
    {
        if (playertarget == null)
        {
            Debug.LogWarning("Bee:: 플레이어 게임오브젝트 변수에 부여되지 않음!");
        }
        if (hivetarget == null)
        {
            Debug.LogWarning("Bee:: 벌집 게임오브젝트 변수에 부여되지 않음!");
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

    #region Enum 관련 함수
    private void Idle()
    {
        //벌집과 벌의 위치가 0.1f 이하일 때 //리턴에서 설정
        //인식범위 근처로 플레이어가 다가오면 공격하러 온다.

        //플레이어와 벌 사이의 거리 구하기

        if (Vector3.Distance(transform.position, playertarget.position) < sight) //플레이어가 시야 내에 있으면
        {
            //cc.enabled = true;
            beeState = BeeState.Chase; //쫓아가기 시작
            print("Bee: Idle >>> Chase");
        }
        else
        {
            //벌집에 붙어있는다.
            transform.position = hivetarget.position;
        }


    }

    private void Chase()
    {
        //플레이어가 추격 거리를 벗어났을 때...
        if (Vector3.Distance(transform.position, playertarget.position) > sight)
        {
            beeState = BeeState.ReturnHome;
            print("Bee: Chase >>> ReturnHome");
            return; //체이스 끝
        }

        Vector3 dir = playertarget.position - transform.position;
        if (dir.magnitude > attackRange)
        {
            //타겟을 향해 추격 이동한다.
            cc.Move(dir.normalized * chaseSpeed * Time.deltaTime);
        }
        else
        {
            //공격 범위 이내로 들어가면 상태를 Attack로 전환한다.
            beeState = BeeState.Attack;
                      //어택 딜레이 타임 여기서 미리 초기화 할거야 기억해
            print("Bee: Chase >>> Attack");
        }

    }
    private void Attack()
    {
        //빠지직한다.
        print("빠지직!");
        //파티클 한번 활성화하기
        //빠지직 한번 틀어주기

        //빠지직하고 딜레이로 간다.
        beeState = BeeState.AttackDelay;
        print("Bee: Attack >>> AttackDelay");
    }

    private void AttackDelay()
    {
        //만약 타겟과 거리가 공격 가능한 범위를 벗어났다면...
        if (Vector3.Distance(transform.position, playertarget.position) > attackRange)
        {
            //추격 상태로 전환
            beeState = BeeState.Chase;
            print("Bee: Attack >>> Chase");
            //딜레이 타임 초기화
            return; //끝!
        }

        //일정 시간 대기한다.
        currentTime += Time.deltaTime;

        // 일정 시간이 지났다면 상태를 다시 공격 상태로 전환한다.
        if (currentTime > attackDelayTime)
        {
            currentTime = 0;
            beeState = BeeState.Attack;
            print("Bee : AttackDelay >>>> Attack");
        }
    }
    private void ReturnHome()
    {
        if (Vector3.Distance(transform.position, playertarget.position) < sight) //플레이어가 시야 내에 있으면
        {
            beeState = BeeState.Chase; //쫓아가기 시작
            print("Bee: ReturnHome >>> Chase");
            return; //끝!
        }
        //hive 위치가 거리 내에 있다면...
        if (Vector3.Distance(transform.position, hivetarget.position) < sightHome)
        {
            Vector3 dir = hivetarget.position - transform.position; //집 방향이 어딘데
            cc.Move(dir.normalized * returnSpeed * Time.deltaTime); //집에 가라

            if (dir.magnitude < 0.6f)
            {
                //cc.enabled = false; //캐릭터 무브 컴포넌트 뺏기
                beeState = BeeState.Idle;//기본 상태로 돌아옴.
                print("Bee : ReturnHome >>> Idle");
            }
        }
        else //없다면...
        {
            beeState = BeeState.Mad;//미칫다.
            print("Bee : ReturnHome >>> Mad"); 
        }

    }

    private void Mad()
    {
        //벌의 시야 내에 플레이어가 있을 때 
        //chase로 바꾸고

        if (Vector3.Distance(playertarget.position, transform.position) < sight)
        {
            beeState = BeeState.Chase; //쫓아가기 시작
            print("Bee: Mad >>> Chase");
        }

        //그렇지 않으면
        Vector3 patdir = patrolNext - transform.position; //랜덤 이동 시 작 //설명은 하이그로디어에 있음
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

    #region collider & 코루틴 데미지 부여 함수들

    IEnumerator ApplyDamage()
    {
        while (isPlayerInZone)
        {
            yield return new WaitForSeconds(damageTime);
            GameManager_Proto.gm.PlayerOnDamaged();
            GameManager_Proto.gm.AnemHit();
            Debug.Log("Bee :: 플레이어 데미지 입음!");
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
            Gizmos.DrawWireSphere(transform.position, sightHome); //집 인식 범위 기즈모
        }
        if (showSightGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sight); //플레이어 인식 범위 기즈모
        }

        if (showAttackRangeGizmo)
        {
            Gizmos.color= Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange); //공격 범위 기즈모
        }
    }
}
