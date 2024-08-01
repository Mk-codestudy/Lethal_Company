using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class OpenableDoor : MonoBehaviour
{
    //다시 e키를 꾹 누르면 문이 닫힘.

    //특정 몬스터들은 문을 열 수 있음.

    public Animator animator;

    public Transform player;

    //플레이어와 문 간의 거리 재기
    [Range(1.0f, 6.0f)]
    public float interactionDistance = 2.0f; //상호작용 가능한 거리

    #region 기즈모로 거리 확인하기
    public Color gizmoColor = Color.green;
    #endregion

    //E키 홀딩 시간
    public float doorHoldTime = 1.5f;
    public float currentHoldTime = 0;

    //문 여닫힘 여부 확인
    bool isDoorOpen = false;

    //UI
    public Slider progressSlider; // 진척도를 표시할 Slider

    void Start()
    {
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
            return;
        }
        else if (Input.GetKey(KeyCode.E)) //문 앞에서 e키를 꾹 누른 채 대기하면 문이 열림.
        {
            currentHoldTime += Time.deltaTime; //얼마나 누르고 있나 시간 측정...

            if (progressSlider != null)
            {
                progressSlider.value = currentHoldTime; // 진행도를 Slider에 반영
                progressSlider.gameObject.SetActive(true); // Slider 활성화
            }

            if (currentHoldTime > doorHoldTime) //내가 정한 시간을 넘어가면!
            {
                //문이 닫혔느냐 열렸느냐에 따라 문이 열리고 닫힘

                if (!isDoorOpen)
                {
                    OpenTheDoor();
                    //사운드 재생
                    currentHoldTime = 0;
                    SliderReset();

                }
                else if (isDoorOpen)
                {
                    CloseTheDoor();
                    //사운드 재생
                    currentHoldTime = 0;
                    SliderReset();
                }

            }
        }
        else
        {
            currentHoldTime = 0;
            SliderReset();
        }
    }

    public void OpenTheDoor()
    {
        //문 열기
        print("Door Open!");
        animator.SetTrigger("Openning");
        isDoorOpen = true;
    }

    public void CloseTheDoor()
    {
        //문 닫기
        print("Door Closed!");
        animator.SetTrigger("Closing");
        isDoorOpen = false;
    }

    public void DestroyDoor()
    {
        print("Door Destroy!");
        //문 파괴 사운드 삽입
        gameObject.SetActive(false); //문 비활성화하기
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
        Gizmos.color = gizmoColor;

        // 문과 플레이어 사이의 선 그리기
        Gizmos.DrawLine(transform.position, player.position);

        // 상호작용 가능한 거리 표시
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
    #endregion

}
