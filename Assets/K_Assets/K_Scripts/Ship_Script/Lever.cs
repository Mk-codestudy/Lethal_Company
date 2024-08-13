using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    [Header("플레이어 게임오브젝트")]
    public Transform player;

    [Header("레버 당기는 위치 transform")]
    public Transform leverGetPoint;

    //플레이어와 문 간의 거리 재기
    [Header("플레이어 상호작용 가능 거리")]
    [Range(1.0f, 6.0f)]
    public float interactionDistance = 2.0f; //상호작용 가능한 거리
    public bool showGizmoSphare;
    public bool showGizmoLine;

    //E키 홀딩 시간
    [Header("E 홀드 시간")]
    [Range(0.5f, 3.0f)]
    public float doorHoldTime = 1.5f;
    float currentHoldTime = 0;

    //UI
    [Header("진척도 슬라이더 UI")]
    public Slider progressSlider; // 진척도를 표시할 Slider

    [Header("E : 이륙하기 안내 UI")]
    public GameObject holdText; //안내 텍스트

    [Header("함선 연출용 카메라쉐이킹:메인카메라에 CaneraShake할당")]
    public CameraShake camshake;

    [Header("레버를 땅기면 n번 씬으로 넘어가기")]
    public int sceneNumber;

    void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("Door :: Player 게임오브젝트를 변수에 할당하지 않음!");
        }
        //player = GameObject.FindGameObjectWithTag("Player").transform; //캐싱~

        if (progressSlider != null)
        {
            progressSlider.maxValue = doorHoldTime;
            progressSlider.value = 0f;
            progressSlider.gameObject.SetActive(false); // 초기에는 비활성화
        }
        holdText.SetActive(false);

    }

    void Update()
    {
        //문과 플레이어의 거리를 잰다.
        float distanceToPlayer = Vector3.Distance(leverGetPoint.transform.position, player.position);

        if (distanceToPlayer > interactionDistance)
        {
            holdText.SetActive(false);
            return;
        }
        else
        {
            holdText.SetActive(true); //안내 텍스트 켜기

            if (Input.GetKey(KeyCode.E)) //문 앞에서 e키를 꾹 누른 채 대기하기.
            {
                currentHoldTime += Time.deltaTime; //얼마나 누르고 있나 시간 측정...

                if (progressSlider != null)
                {
                    progressSlider.value = currentHoldTime; // 진행도를 Slider에 반영
                    progressSlider.gameObject.SetActive(true); // Slider 활성화
                }

                if (currentHoldTime > doorHoldTime) //내가 정한 시간을 넘어가면!
                {
                    //함선이 흔들리며 뭔가가 일어난다.
                    //함선 흔들어제끼기
                    camshake.letsShake = true;

                    //인보크로 1초 뒤에 씬넘어가기
                    Invoke("LoadScene", 1.5f);
                }
            }
            else
            {
                currentHoldTime = 0;
                SliderReset();
            }
        }
    }

    void SliderReset()
    {
        if (progressSlider != null)
        {
            progressSlider.value = 0f;
            progressSlider.gameObject.SetActive(false); // 완료 후 비활성화
        }
        else
        {
            Debug.Log("슬라이더가 안 들어옴!");
        }
    }

    public void LoadScene()
    {

        if (sceneNumber == 1) //타이머 리셋 관련 코드
        {
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

        SceneManager.LoadScene(sceneNumber); //이게 진짜 로드씬
    }


    #region 기즈모로 거리 그리기
    private void OnDrawGizmos()
    {
        // Gizmos 색상 설정
        Gizmos.color = Color.green;

        if (showGizmoLine)
        {
            Gizmos.color = Color.yellow;
            // 문과 플레이어 사이의 선 그리기
            Gizmos.DrawLine(leverGetPoint.transform.position, player.position);
        }

        if (showGizmoSphare)
        {
            Gizmos.color = Color.green;
            // 상호작용 가능한 거리 표시
            Gizmos.DrawWireSphere(leverGetPoint.transform.position, interactionDistance);
        }
    }
    #endregion

}
