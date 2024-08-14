using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_Proto : MonoBehaviour
{
    static public GameManager_Proto gm;
    //prototype를 위한 게임매니저.
    //알파 베타로 넘어갈때마다 필요한 함수만 복붙하기.



    #region protype에서 구현되어야 할 사항
    //플레이어 HP
    //적 HP
    //사망 가능 개체의 사망
    //플레이어 사망 연출 (사망 UI 출력이라도)
    
    //씬 넘기기 (F7, F8)
    #endregion


    //1. 플레이어 HP 감소 ~ 사망
    [Header("플레이어 스테이터스 관련")]
    public float playerHP = 100;
    public float playerSP = 100;

    [Header("삽 데미지")]
    public float damage = 15;

    //2. 적 HP 감소 ~ 사망
    //적이 여럿이라서 하나만 만들면 안되지만 프로토타입에서 구현할 몹 중 HP있는 놈은 덤퍼 뿐
    [Header("몹 스테이터스")]
    public float dumperHP = 100;
    public float enumDamage = 30; //몹 한번 공격할때마다 입는 데미지량

    [Header("사망 UI 관련 퍼블릭 변수")]
    public GameObject deadUI;

    [Header("플레이어 피격 UI 컬러 코루틴")]
    public HitUICorutine hitUICorutine;



    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        //사망 UI 뜨지 않게 꺼두기
        deadUI.SetActive(false);

    }

    void Update()
    {
        //플레이어 사망 판정 
        if (playerHP <= 0)
        {
            PlayerDead();
        }

        #region 프로토타입 사용 - 씬 넘기기
        //씬 넘기기
        if (Input.GetKeyDown(KeyCode.F7))            // F4 누르면...
        {
            SceneLethal(-1);                        // 이전 씬 스타트
        }
        if (Input.GetKeyDown(KeyCode.F5))            // F5 누르면...
        {
            SceneLethal(0);                         // 현재 씬 스타트
        }
        else if (Input.GetKeyDown(KeyCode.F8))       // F6 누르면...
        {
            SceneLethal(1);                         // 다음 씬 스타트
        }
        #endregion


    }


    //플레이어 사망 함수
    public void PlayerDead()
    {
        //플레이어 레그돌 실행
        //각종 플레이어 기능(이동, 그랩, 기타등등...) 상실
        //카메라 3인칭으로 전환

        //UI실행
        PlayerDeadUI();

        //인보크 1초 뒤에 게임오버 연출(함선 떠나기)

        Invoke("RoundOver", 1.5f);

    }
    public void PlayerDeadUI()
    {
        //Albedo 60정도의 검은 창 배경에 텍스트
        //프로토:  UI만 뜨게 하기

        //UI창 화면 지직거리듯 "-------------" 가 위 아래로 흔들리다가 > 알파
        //[생명 신호: 오프라인] 텍스트 작아졌다가 화면 전체를 채우도록 커짐 > 알파

        //UI창 켜기
        deadUI.SetActive(true);
        Text uitext = deadUI.transform.GetChild(1).GetComponent<Text>();


        #region 코루틴으로 폰트 사이즈 늘리기 > 알파에서 계속
        //UI폰트 = 18로 시작해서 170까지 늘리기!!!
        //print(uitext.fontSize);

        //float percent = 0; // 폰트 배율 변수
        //percent += Time.deltaTime;

        //float size = Mathf.Lerp(18, 170, percent); //Lerp로 살살 늘려주려고 했는데...
        //uitext.fontSize = Mathf.CeilToInt(size); //한번만 실행되는것같았다.
        #endregion
    }


    //플레이어가 때리는 함수
    //플레이어가 삽으로 때리는 스크립트에 gm.playerhit(dumperHP) 적는 식으로 사용
    public void PlayerHit()
    {
        dumperHP -= 15; //데미지 가하기

    }

    public void PlayerOnDamaged()
    {
        //카메라 뒤흔들며 아픈 연출
        //눈앞에 보이는 플레이어 모델링 손이 움직임

        //프로토타입은 대신 UI코루틴 넣기
        StartCoroutine(hitUICorutine.FadeOut());
    }

    //적이 때리는 함수
    public void AnemHit()
    {
         playerHP -= enumDamage; //적 데미지만큼 플레이어 HP 차감
    }


    //함선으로 돌아가는 씬 조절 함수
    public void RoundOver()
    {
        SceneManager.LoadScene(1);

        GameObject clock = GameObject.Find("MapUI");

        if (clock != null)
        {
            clock.SetActive(false);
        }
        else
        {
            Debug.Log("시계 안 찾아짐!");
        }
    }


    #region 씬 조절 함수 

    void SceneLethal(int num)
    {
        // 현재 씬 인덱스(순서)를 확인한 뒤
        int currentSceneindex = SceneManager.GetActiveScene().buildIndex;

        // F4 ~ F6을 통해 해당 씬 불러오기
        // F4(이전), F5(현재), F6(다음)
        SceneManager.LoadScene(currentSceneindex + num);

        // 커서 원상복구 (만약 해당 씬에서 시작할 때 커서 안보이게 설정돼 있으면 그대로 반영됨)
        Cursor.lockState = CursorLockMode.Confined;
    }

    #endregion

}
