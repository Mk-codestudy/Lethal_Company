using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Hygrodere : MonoBehaviour
{
    //하이그로디어


    //플레이어가 안 보이면 마지막으로 감지된 플레이어의 위치 근처를 떠돌도록 한다.
    //무적.
    //문을 열 줄 모른다.
    //문을 통과도 못한다.
    //그렇지만 문 너머를 인지할 수 있다. (벽에 계~속 붙어있음)

    [Header("애너미 타겟")]
    public Transform target;

    [Header("슬라임 스피드")]
    [Range(0.5f, 4.0f)]
    public float spd = 1.0f; //슬라임 스피드
    CharacterController cc; //슬라임 이동

    public float sight = 10.0f; //슬라임 인식 거리

    Vector3 patrolNext; //랜덤 이동 지점
    Vector3 patrolCenter; //랜덤 이동 반경의 정중앙
    public float patrolRadius = 8.0f; //슬라임 랜덤 이동 반경


    //피격 판정 관련
    bool isPlayerInZone;
    private Coroutine damageCoroutine;
    [Header("N초마다 피격")]
    public float damageTime = 1.0f;


    void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("Slime(Hygrodere): 타겟 변수 누락!!");
        }

        //패트롤
        patrolCenter = transform.position; //중간 지점을 캐싱
        patrolNext = patrolCenter; //시작 위치가 다음 위치가 되도록 한다.
        
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        //플레이어를 따라다닌다. 끝없이...

        //Vector3 dir = targe.position - transform.position;
        //응? 근데 Y축 제외해야함 

        //조각내서 재조립해야겠다.
        float h = target.position.x - transform.position.x;
        float v = target.position.z - transform.position.z;

        Vector3 dir = new Vector3(h, 0, v);
        
        if (dir.magnitude < sight) //슬라임의 시야 내에 플레이어가 있을 때
        {
            cc.Move(dir.normalized * spd * Time.deltaTime); //플레이어를 향해 슬라임이 이동한다.
        }
        else //없을 때
        {
            Vector3 patdir = patrolNext - transform.position; //랜덤 이동 시 작
            if (patdir.magnitude > 0.1f)
            {
                cc.Move(patdir.normalized * spd * Time.deltaTime);
            }
            else
            {
                Vector2 newPos = Random.insideUnitCircle * patrolRadius; //반지름 8인 circle
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
            Debug.Log("Slime :: 플레이어 데미지 입음!");
            }
        }

}
