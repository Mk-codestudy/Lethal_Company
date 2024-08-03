using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
     float x;
     float v;

    public float walkSpeed = 5f;
    public float runSpeed = 15f;
    public float currentSpeed = 20f; // 현재 걷는/뛰는 속도
    public float rotSpeed = 300f;


    // 캐릭터 체력, 스태미너 
    public float maxHp; // 네번 맞으면 죽음
    public float maxStamina = 100; // lerp감소
    public float currentHp;
    public float currentStamina;
    // 체력 스태미너 슬라이더
    public Image hpBar;
    public Slider staminaSlider;

    bool isregenStamina = true;



    public Vector3 moveDir;


    Vector3 gravityPower; // 중력은 vector값이다 ( x, y, z)

    

    public float jumpHeight;
    public float maxJump = 1f;

    float yPos; // 
    public float gravityVelocity = 100f; // 낙하속도

    float rotX; // x축 마우스 회전 속도값을 담을 변수
    float rotY; // 

    bool isMoving;
    bool isRunning;
    //bool isCrouch = false; // 앉은 상태 x
    bool isIdle;
    bool isLadder = false; // 사다리 상태인지 아닌지


    private Inventory playerInventory;  // 인벤토리 클래스 
    public Item item;

    CharacterController cc;
    Animator animator;
    





    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; // 커서 가두기

        cc = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();

        gravityPower = Physics.gravity;  // 중력 초기화 

        playerInventory = GetComponent<Inventory>();


        currentHp = maxHp;
        currentStamina = maxStamina;    
        

    }

    void Update()
    {
      
        Rotate();
        RegenStamina(5);
        
        if(isLadder)
        {
            Laddermove();
        }
        else
        {
            Move();
        }





        UpdateUI();
       

}

    void Move() // 캐릭터 걷기, 달리기, 점프, 숙이기(Crouch)
    {
        

        // 로컬 벡터로 움직이기
        x = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 movedir = new Vector3(x, 0f, v);

        movedir = transform.TransformDirection(movedir); // 로컬 좌표 이동
        movedir.Normalize();



        // 움직임 상태
        isIdle = movedir.magnitude == 0; // 이동거리가 0이면 Idle 상태
        isMoving = movedir.magnitude > 0; // 캐릭터가 움직이는 거리가 0보다 크면 ismoving 
        isRunning = Input.GetKey(KeyCode.LeftShift); // 쉬프트를 누르고 있으면



        if (isMoving)  // ismoving 
        {
            if (isRunning)
            {
                if (currentStamina > 0)
                {
                    currentSpeed = runSpeed;
                    UseStamina(15);
                    //isRunning = true;
                    //animator.SetBool("run", true);
                }
                else
                {
                    isRunning = false;
                    currentSpeed = walkSpeed;
                    // animator.SetBool("run", false);
                    // animator.SetBool("walk", true);
                }

            }
            else
            {
                currentSpeed = walkSpeed;
               
                //isMoving = true;
                // animator.SetBool("walk", true);

            }
        }
        else
        {
            isIdle = true;
          
            //  animator.SetBool("idle", true);
        }



        // 캐릭터 점프

        yPos += gravityPower.y * gravityVelocity * Time.deltaTime; // 중력의 y축값 * 중력속도 * 시간 보간
                                                                  // 상시 캐릭터에게 y축 아래로 적용되는 힘.


        if (cc.collisionFlags == CollisionFlags.CollidedBelow)
        {
            yPos = 0; // 땅바닥에 닿으면 y축 ypos값을 0으로 바꿔서 중력값을 초기화 시킴;
            maxJump = 1;
        }

        if (Input.GetButtonDown("Jump") && maxJump > 0)  // 스페이스를 누르면 점프한다
        {
            if (currentStamina > 15) // 상수로 넣음 
            {
                yPos = jumpHeight; // 점프시 일시적으로 중력값을 점프 높이로 치환
                maxJump--;
                UseStamina(15);
            }

        }

        movedir.y = yPos;

        cc.Move(movedir * currentSpeed * Time.deltaTime);  // 움직임

    }



    // 마우스 방향에 따른 캐릭터의 회전
    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotX += mouseY * rotSpeed * Time.deltaTime; // 좌우로 움직일때 축은 y에 있고
        rotY += mouseX * rotSpeed * Time.deltaTime; // 상하로 움직일때 축은 x에 있다.

        // 상하 회전각을 60도로 제한

        //if (rotX > 60) // rotx가 60도 크다면 60도로 (Maximum)
        //{
        //    rotX = 60;
        //}
        //else if (rotX < -60) // rotx가 -60도 보다 작다면 -60도로 (Maximum)   
        //{
        //    rotX = -60;
        //}


        // 를 mathf.clamp로 간단하게 제한

        rotX = Mathf.Clamp(rotX, -60f, 60f);

        transform.eulerAngles = new Vector3(0, rotY, 0f);

        Camera.main.transform.GetComponent<Follow_Cam>().rotX = rotX; // 카메라에 있는 followcam의 스크립트의 rotx 변수를 받아옴


    }
     


    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Item"))
        {

        }
    }

    
        

 
    private void UseStamina(int amount)
    {
        currentStamina -= amount * Time.deltaTime; // 스태미너 감소

        if(currentStamina <= 0 )
        {
            currentStamina = 0;

            isRunning = false;

          
        }
    }

    private void RegenStamina(int amount) // 스태미너 재생
    {        
        if (currentStamina < maxStamina)
        {
            if (isregenStamina) // isregenStamina 가 true 라면
            {
                currentStamina += amount * Time.deltaTime; // 스태미너를 재생한다.
            }
        }
        else if(currentStamina <= 0) // 현재 스태미너가 0이거나 0보다 적다면
        {
            isregenStamina = false; // 리젠 스태미나를 false로 하고

            StopStaminaTimer(3f); // 스탑 리젠 코루틴을 시작한다.
        }

        
    }

    private void StopStaminaTimer(float time) // 스태미너가 0이 되면 time 초간 스태미너 재생을 연기한다.
    {        
            StartCoroutine("StaminaCoroutine");           
    }

   IEnumerator StaminaCoroutine()
    {     
        yield return new WaitForSeconds(3f);

        isregenStamina = true;
    }


    private void UpdateUI()  // HP stamina 슬라이더를 매 프레임마다 업데이트,  Mathf.Lerp를 사용해서 슬라이더를 부드럽게 
    {
        // 스태미너 슬라이드

        float targetStamina = currentStamina / maxStamina; // 타겟 스태미나 벨류값은 max스태미너 값에서 currentstamina값의 비율
       

        staminaSlider.value = Mathf.Lerp(currentStamina, targetStamina, Time.deltaTime * 0.5f);

        // 좀더 부드럽게 증감되게 수정필요
        //staminaSlider.value = Mathf.Lerp(staminaSlider.value, currentStamina / maxStamina, Time.deltaTime * 10f);
        //hpSlider.value = currentHp;



        // 체력 칸
        #region 체력칸

        float targetHp = currentHp / maxHp;

        Color hpBarAlpha = hpBar.color; // hp바 이미지의 컬러를, 컬러 클래스의 변수로 넣어서 컨트롤
   

        if (currentHp == 100)
        {
            currentHp = 0;
            hpBarAlpha.a = 0f; // 체력이 0 일때 hp이미지의 선명도가 0 , // 체력 slider alpha 값 벨류 설정
        }
        else if (currentHp > 50 && currentHp < 75)
        {
            hpBarAlpha.a = 0.25f; 
        }
        else if (currentHp > 25 &&currentHp < 50)
        {
            hpBarAlpha.a = 0.5f;
        }
        else if (currentHp > 0 && currentHp < 25)
        {
            hpBarAlpha.a = 0.75f;
        }
        else
        {
            hpBarAlpha.a = 1f; // 체력이  일때 hp이미지의 선명도가 1
        }

    }
    #endregion


    //사다리를 구현한다
    //플레이어가 Ladder Tag가 붙은 Collider와 stay 할경우]
    //플레이어의 z,x축 움직임은 제한하고
    // w, s를 눌렀을때 각 각 transform.up , -transform.up 의 방향으로 walkSpeed 로 이동한다.

   

  

    // 사다리 안에서는 수직으로만 움직이게 제한
    public void Laddermove()
    {
        float y = Input.GetAxis("Vertical");

        Vector3 ladderDir = new Vector3(0, y, 0);

        cc.Move(ladderDir * walkSpeed * Time.deltaTime);

        gravityPower = Physics.gravity;

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            print("사다리에 닿았습니다.");
            isLadder = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            print("사다리에서 나왔습니다.");
            isLadder = false;
        }
    }




}






