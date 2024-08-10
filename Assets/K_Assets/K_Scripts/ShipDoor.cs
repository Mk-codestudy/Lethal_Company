using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDoor : MonoBehaviour
{

    [Header("플레이어 게임오브젝트")]
    public Transform player;

    //플레이어와 문 간의 거리 재기
    [Header("플레이어 상호작용 가능 거리")]
    [Range(1.0f, 6.0f)]
    public float interactionDistance = 2.0f; //상호작용 가능한 거리
    public bool showGizmoSphare;
    public bool showGizmoLine;

    [Header("문 여는 버튼")]
    public GameObject button;

    [Header("문-닫힘위치-열림위치")]
    public GameObject leftDoor;
    public Transform leftClose;
    public Transform leftOpen;
    public GameObject rightDoor;
    public Transform rightClose;
    public Transform rightOpen;

    [Header("문 여닫히는 속도")]
    [Range(3.0f, 25.0f)]
    public float moneSpd = 8;
    //문 여닫히는 동안 딜레이 주기
    float delayTime = 0f;

    public bool isDoorOpen;

    [Header("문 열기 UI")]
    public GameObject openUI;

    void Start()
    {
        openUI.SetActive(false);
    }

    void Update()
    {
        if (isDoorOpen)
        {
            //Invoke("Open", 0);//왜일케했징?
            Open();
        }
        else
        {
            //Invoke("Close", 0);
            Close();
        }


        //문과 플레이어의 거리를 잰다.
        float distanceToPlayer = Vector3.Distance(button.transform.position, player.position);

        if (distanceToPlayer > interactionDistance)
        {
            openUI.SetActive(false);
            return;
        }
        else
        {
            openUI.SetActive(true);
            delayTime -= Time.deltaTime;
            if (delayTime < 0 && Input.GetKey(KeyCode.E)) //문 앞에서 e키를 누르면 문이 열림.
            {
                #region 실패했지만 보존가치있는 코드
                //이렇게 하니까 조금 누르면 조금 열린다 캐신기하다
                //혹시모르니 보존
                //if (!isDoorOpen)
                //{
                //    Open();
                //    //사운드 재생

                //}
                //else if (isDoorOpen)
                //{
                //    Close();
                //    //사운드 재생
                //}
                #endregion

                if (!isDoorOpen)
                {
                    isDoorOpen = true;
                    delayTime = 1.0f;
                }
                else
                {
                    isDoorOpen = false;
                    delayTime = 1.0f;
                }

            }
        }

    }


    void Close()
    {
        leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftClose.position, moneSpd * Time.deltaTime);
        rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightClose.position, moneSpd * Time.deltaTime);
        leftDoor.GetComponentInParent<BoxCollider>().enabled = true;
        rightDoor.GetComponentInParent<BoxCollider>().enabled = true;
    }

    void Open()
    {
        leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftOpen.position, moneSpd * Time.deltaTime);
        rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightOpen.position, moneSpd * Time.deltaTime);
        leftDoor.GetComponentInParent<BoxCollider>().enabled = false;
        rightDoor.GetComponentInParent<BoxCollider>().enabled = false;
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
            Gizmos.DrawLine(button.transform.position, player.position);
        }

        if (showGizmoSphare)
        {
            Gizmos.color = Color.green;
            // 상호작용 가능한 거리 표시
            Gizmos.DrawWireSphere(button.transform.position, interactionDistance);
        }
    }
    #endregion


}
