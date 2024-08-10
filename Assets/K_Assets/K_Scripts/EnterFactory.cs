using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterFactory : MonoBehaviour
{
    [Header("n번 씬으로 가기!")]
    public int sceneNumber = 0;


    [Header("플레이어 게임오브젝트")]
    public Transform player;

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

    [Header("UI 캔버스 제어")]
    public GameObject canvas;

    void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("Door :: Player 게임오브젝트를 변수에 할당하지 않음!");
        }
        player = GameObject.FindGameObjectWithTag("Player").transform; //캐싱~

        // Slider를 초기화
        if (progressSlider != null)
        {
            progressSlider.maxValue = doorHoldTime;
            progressSlider.value = 0f;
            progressSlider.gameObject.SetActive(false); // 초기에는 비활성화
        }
    }

    void Update()
    {
        //문과 플레이어의 거리를 잰다.
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > interactionDistance)
        {
            canvas.SetActive(false); //UI활성화
            return;
        }
        else if (Input.GetKey(KeyCode.E)) //문 앞에서 e키를 꾹 누른 채 대기하면 문이 열림.
        {
            canvas.SetActive(true);
            currentHoldTime += Time.deltaTime; //얼마나 누르고 있나 시간 측정...

            if (progressSlider != null)
            {
                progressSlider.value = currentHoldTime; // 진행도를 Slider에 반영
                progressSlider.gameObject.SetActive(true); // Slider 활성화
            }

            if (currentHoldTime > doorHoldTime) //내가 정한 시간을 넘어가면!
            {
                //씬 넘어가기
                SceneManager.LoadScene(sceneNumber);
            }
        }
        else
        {
            currentHoldTime = 0;
            SliderReset();
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
            print("슬라이더가 안 들어옴!");
        }
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
            Gizmos.DrawLine(transform.position, player.position);
        }

        if (showGizmoSphare)
        {
            Gizmos.color = Color.green;
            // 상호작용 가능한 거리 표시
            Gizmos.DrawWireSphere(transform.position, interactionDistance);
        }
    }
    #endregion


}
