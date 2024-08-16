using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using UnityEngine.UIElements;



//using static UnityEditor.Progress;

public class PlayerMove : MonoBehaviour
{

   


    [Header("플레이어 이동 관련 변수")]
    float x;
    float v;
    public float walkSpeed = 5f;
    public float runSpeed = 15f;
    public float currentSpeed = 20f; // 현재 걷는/뛰는 속도
    public float rotSpeed = 300f;
    public Vector3 moveDir;

    [Header("플레이어 스테이터스")] // 캐릭터 체력, 스태미너 
    public float maxHp; // 네번 맞으면 죽음
    public float maxStamina = 100; // lerp감소
    public float currentHp;
    public float currentStamina;
    // 체력 스태미너 슬라이더
    public Image hpBar;
    public Slider staminaSlider;
    bool isregenStamina = true;


    [Header("플레이어 점프 변수")]
    Vector3 gravityPower; // 중력은 vector값이다 ( x, y, z)
    public float jumpHeight = 10;
    public float maxJump = 1f;
    float yPos; // 
    public float gravityVelocity = 5f; // 낙하속도

    
    float rotX; // x축 마우스 회전 속도값을 담을 변수
    float rotY; // 

    bool isMoving;
    bool isRunning;
    bool isGround;
    //bool isCrouch = false; // 앉은 상태 x
    bool isIdle;
    bool isOnLadder; // 사다리에 닿았는지 아닌지 E를 눌러서 상호작용하기 위해 추가함
    bool isLadder = false; // 사다리 상태로 바꾼다


    [Header("플레이어 레이캐스트 변수")]
    public float rayDistance = 5f; // 레이캐스트 레이 최대   길이
    public Vector3 boxSize = new Vector3(2f, 2f, 2f);  // 레이캐스트 박스의 크기




    //아이템 줍기 상호작용



    private bool collideItem = false; // 아이템 충돌체크
    private GameObject currentItem; // 현재 충돌 중인 아이템
    public Transform RightHand; // 주을 아이템이 있을 위치 , Player의 자식오브젝트인  right hand를 드래그해서 넣는다.
    private GameObject holdItem; // 현재 들고 있는 아이템을 표시

    public Inventory inventory; // 인벤토리 스크립트 참조



    CharacterController cc;

   

    //public Animator animator;

    public PlayerState currentState; // 플레이어가 시작하는 처음 상태
    private object playerScan;

    public enum PlayerState // 플레이어의 유한 상태머신
    {
        Idle,
        Attack,
        OnDamaged,
        Dead,
        Scan
    }



    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined; // 커서 가두기
        Cursor.lockState = CursorLockMode.Locked; // 커서 가두기
        Cursor.visible = false;

        cc = GetComponent<CharacterController>(); // cc 컴포넌트

        //animator = GetComponent<Animator>();  // animator controller 가 들어가 있는 플레이어 모델링을 이곳에 집어 넣는다.

        gravityPower = Physics.gravity;  // 중력 초기화 

        //holdItem = null; // 들고잇는 아이템을 초기화



        // 체력 스태미너 초기화
        currentHp = maxHp;
        currentStamina = maxStamina;



    }

    void Update()
    {

        PickupItem(); // 아이템 줍기
        DropItem(); // 아이템 버리기
        Rotate();
        RegenStamina(5);


        if ((isOnLadder) && Input.GetKeyDown(KeyCode.E))  // 사다리와 닿아있고 e를 눌렀을때
        {
            isLadder = !isLadder; // isladder 가 true 가 된다.

            if (isLadder) // isLadder 상태에서는
            {
                gravityPower = Physics.gravity; // 중력을 초기화한다.
                
            }

        }
        if (isLadder)
        {
            Laddermove();
        }
        else
        {
            Move();
        }


        UpdateUI();

        switch (currentState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Attack:
                PlayerAttak();
                break;
            case PlayerState.OnDamaged:
                PlayerOnDamaged();
                break;
            case PlayerState.Dead:
                PlayerDead();
                break;
            case PlayerState.Scan:
                PlayerScan();
                break;
            default:
                break;
        }


    }
    #region BoxCast는 닿은 첫번째 오브젝트만 감지하기에 OverlapBox를 쓰기로 함
    //private void PlayerScan() // 우클릭하면 캐릭터 전방으로 스캔
    //{
    //    if(Input.GetMouseButtonDown(1)) // 우클릭을 하면
    //    {
    //        RaycastHit hitInfo; // 충돌체의 정보를 담을 변수
    //        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // 카메라의 위치에서 카메라의 foward방향으로

    //        bool isHit = Physics.BoxCast(ray.origin, boxSize / 2, ray.direction, out hitInfo, transform.rotation, rayDistance, 1 << 6);

    //        if(isHit) // 박스 레이에 닿았다면
    //        {               
    //           Debug.Log("박스캐스트에 닿았습니다. " + hitInfo.collider.name);

    //        }



    //    }
    //}
    #endregion


    // OverlapBox 를 사용한 다중 감지
    private void PlayerScan() // 우클릭하면 캐릭터 전방으로 스캔 
    {
        if(Input.GetMouseButtonDown(1))
        {
            Vector3 boxCenter = (Camera.main.transform.position + Camera.main.transform.forward); // 메인 카메라의 정면방향으로 box를 그린다.
            Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity, 1 << 6); // 감지한 콜라이더를 담을 배열을 만든다.



            if(hitColliders.Length > 0) // 만약, 감지된 콜라이더의 개수가 0보다 많다면
            {
                for(int i = 0; i < hitColliders.Length; i++) // 배열의 인덱스를 사용해 출력한다
                {
                    Collider collider = hitColliders[i]; // i 번째의 콜라이더를 가져오고
                    Item item = collider.GetComponent<Item>(); // 그 콜라이더 안에 잇는 item 컴포넌트를 가져온다
                    
                    if(item != null)
                    {
                        //print("" + item.itemName);
                        //print("" + item.itemValue);

                        // Item 컴포넌트의 name과 value를 출력합니다.
                        Debug.Log("Item Name: " + item.itemName);
                        Debug.Log("Item Value: " + item.itemValue);

                        item.ShowItemInfo(); // UI 텍스트를 활성화하여 정보를 표시합니다.



                    }
                    else
                    {
                        Debug.Log("아무 오브젝트도 찾지 못했습니다." + collider.name);
                    }                    
                }
            }
            else
            {
                Debug.Log("아무것도 감지하지 못했습니다.");
            }


        }
    }

    private void OnDrawGizmos() // 씬 뷰에서 Box Cast를 그려서확인한다.
    {
        if(Camera.main == null)
        {
            return;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);  // 빨간줄은 레이
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * rayDistance);

        // 빨간줄의 끝에 BoxCast의 박스를 그립니다.
        Vector3 halfExtents = boxSize / 2; // 박스길이의 반
        Vector3 castEnd = ray.origin + ray.direction * rayDistance; // ray의 시작 위치에서 ray의 방향 * 최대거리
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(castEnd, boxSize);

    }

    private void PlayerAttak() //나중에 몬스터와 상호작용할 매개변수 넣기
    {
        //플레이어 공격은 오직 플레이어가 Shover를 가지고 있을때만
        
        if(holdItem.CompareTag("Shover")) // 플레이어가 shover 태그의 게임 오브젝트를 들고있고
        {
            if(Input.GetMouseButtonDown(0)) // 좌클릭을 했다면
            {

            }
        }
    }

    private void PlayerDead()
    {
        if(currentHp <= 0) // 만약, 현재 체력이 0보다 작거나 같다면
        {
            //플레이어 모델링(아바타나 fbx파일) ragdoll wizard 추가
        }

    }
    private void PlayerOnDamaged()
    {

     }







    #region 캐릭터 이동/ 점프

    void Move() // 캐릭터 걷기, 달리기, 점프, 숙이기(Crouch)
    {


        // 로컬 벡터로 움직이기
        x = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 movedir = new Vector3(x, 0f, v);

        movedir = transform.TransformDirection(movedir); // 로컬 좌표 이동
        movedir.Normalize(); // 속력



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
                    isRunning = true;
                    // animator.SetFloat("MoveFloat" , 0.3f );
                }
                else
                {
                    isRunning = false;
                    currentSpeed = walkSpeed;
                    // animator.SetFloat("MoveFloat", 0.1f);

                    // animator.SetBool("run", false);
                    // animator.SetBool("walk", true);
                }

            }
            else
            {
                currentSpeed = walkSpeed;
                // animator.SetFloat("MoveFloat", 0f);

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
        isGround = cc.collisionFlags == CollisionFlags.CollidedBelow;


        if (isGround) // 땅바닥이면
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
    #endregion
    #region 캐릭터 회전

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
        if (Camera.main != null)
        {
            Camera.main.transform.GetComponent<Follow_Cam>().rotX = rotX; // 카메라에 있는 followcam의 스크립트의 rotx 변수를 받아옴
        }

    }

    #endregion



    #region hp stamina 관리
    // 스태미너 관련 함수
    private void UseStamina(int amount)
    {
        currentStamina -= amount * Time.deltaTime; // 스태미너 감소

        if (currentStamina <= 0)
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
        else if (currentStamina <= 0) // 현재 스태미너가 0이거나 0보다 적다면
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





    // 스태미너 체력 증,감소를 슬라이더, 이미지에 업데이트
    private void UpdateUI()  // HP stamina 슬라이더를 매 프레임마다 업데이트,  Mathf.Lerp를 사용해서 슬라이더를 부드럽게 
    {
        // 스태미너 슬라이드

        float targetStamina = currentStamina / maxStamina; // 타겟 스태미나 벨류값은 max스태미너 값에서 currentstamina값의 비율


        staminaSlider.value = Mathf.Lerp(currentStamina, targetStamina, Time.deltaTime * 0.5f);

        // 좀더 부드럽게 증감되게 수정필요
        //staminaSlider.value = Mathf.Lerp(staminaSlider.value, currentStamina / maxStamina, Time.deltaTime * 10f);
        //hpSlider.value = currentHp;



        // 체력 칸


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
        else if (currentHp > 25 && currentHp < 50)
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
    // 그리고 사다리 콜라이더에 닿았을때 E 를 화면에 출력되게 하기 - 추가할것


    public void Laddermove()
    {
        float y = Input.GetAxis("Vertical"); // vertical 입력을 하면 y 값을 받는다.

        Vector3 ladderDir = new Vector3(0, y, -0.05f);  // 방향은 y축이다.

        cc.Move(ladderDir * walkSpeed * Time.deltaTime); // ladderdir 방향으로  walk스피드로 움직인다.

        gravityPower = Physics.gravity; 

    }


    private void OnTriggerStay(Collider other) // 사다리에 닿아있을때 isOnladder 를 true 로 하고
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            print("사다리에 닿았습니다.");
            isOnLadder = true;
        }
    }

    private void OnTriggerEnter(Collider other)  // 아이템에 닿음
    {

        if (other.CompareTag("Item")) // item 태그의 콜라이더와 충돌했다면
        {
            Debug.Log("아이템에 닿았습니다.");

            collideItem = true; // 아이템에 닿음
            currentItem = other.gameObject; // 현재 들고있는 아이템은 지금 아이템 오브젝트이다.
            //currentItem = other.gameObject; // 현재 충돌 중인 아이템을 currentitem변수에 넣기
        }

       
    }

    private void OnTriggerExit(Collider other) // 사다리에서 떨어짐,  아이템에서 떨어짐
    {

        // 사다리에서 떨어졌을때
        if (other.gameObject.CompareTag("Ladder"))
        {
            print("사다리에서 나왔습니다.");
            isOnLadder = false;
            if (isLadder)
            {
                isLadder = false;
                gravityPower = Physics.gravity;
            }

        }

        // 아이템과의 충돌이 종료된 경우, 현재 아이템을 null로 설정
        if (other.CompareTag("Item")) //&& other.gameObject == currentItem)
        {
            Debug.Log("아이템에서 떨어졌습니다.");

            collideItem = false;
            currentItem = null;
            //currentItem = null;
        }

    }






    // 아이템 줍기 메커니즘 1


    // 플레이어가 item tag인 game object와 충돌하고

    // 플레이어가 e 를 누르면 

    // 아이템은 사라지고(Destroy)

    // 아이템은 플레이어 transform의 자식 위치에 생성된다.

    // 아이템을 player의 자식의 자식 오브젝트로 만든다(가지고 다닐수있게)


    public void PickupItem() //아이템 줍기  
    {
        // // E를 누르고 현재 아이템에 닿아있다면 
        if (Input.GetKeyDown(KeyCode.E) && (collideItem))
        {
            if (currentItem != null) // 그리고 주울 아이템이 null 값이 아니라면
            {

                if (holdItem == null) // 현재 들고있는 아이템이 null 값이라면
                {
                    holdItem = Instantiate(currentItem, RightHand.position, RightHand.rotation); //  Player-RightHand.Position에 currentItem 을 생성한다. // 현재 아이템을 홀드아이템 변수로 넣는다.
                    holdItem.transform.SetParent(RightHand); // player의 righthand를 주운 오브젝트의 부모로 한다.


                    Rigidbody rb = holdItem.GetComponent<Rigidbody>(); // item 의 rigidbody 컴포넌트를 가져와서
                    if (rb != null)
                    {
                        rb.isKinematic = true; // 떨어지지 않게 iskineitc 을 체크
                    }

                    Destroy(currentItem); // 씬에서 지운다
                    currentItem = null; // currentItem 초기화
                    Debug.Log("아이템을 주웠습니다.");

                }
            }
        }

    }

    public void DropItem() // 아이템 버리기
    {

        if (Input.GetKeyDown(KeyCode.G) && holdItem != null)  // G를 누르고 들고 있는 아이템이 null이 아니라면
        {
            Debug.Log("G키가 입력됬습니다.");

            holdItem.transform.SetParent(null); // Righthand에서 벗어남
            Vector3 dropPos = transform.position + transform.forward * 3f; // 플레이어의 전방 3f의 거리앞에서 떨어뜨릴 지점을 생성
            holdItem.transform.position = dropPos; // hold 아이템의 위치를 droppos로 이동



            Rigidbody rb = holdItem.GetComponent<Rigidbody>(); // 공중에서 생성된 아이템을 떨어뜨리기 위해 item 에 rigidbody를 추가
            if (rb != null)
            {
                rb.isKinematic = false; // 중력의 작용을 받기 위해 iskinetic을 체크해제
            }
            else
            {
                Debug.Log("Rigidbody 가 없습니다.");
            }

            holdItem = null; // hold아이템을 초기화

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("G 키가 눌렸지만 holdItem이 null입니다.");
            }
        }

    }
}



    // 아이템 줍기 메커니즘 2 - 인벤토리에 저장, 별개의 Gameobject를 만들어서 Inventory로 사용한다.

    // holditem != null 이고
    // inventory[i] == null 이라면
    // inventory[i] 에 저장하고
    // holditem 을 Destroy 한다
    // holdItem == null 로 초기화 한다.

  

    






    //아이템 줍기
    //public void ItemPickUp()
    //{
    //    RaycastHit hit;

//    Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // 레이의 발사위치는 메인 카메라

//    Color rayColor = Color.red;

//    Debug.DrawRay(ray.origin, ray.direction * rayDistance, rayColor); // 레이를 눈에 보이게

//    // "Item" 레이어만 탐지하도록 레이어 마스크 설정
//    int layerMask = LayerMask.GetMask("Item");

//    if (Physics.Raycast(ray, out hit, rayDistance, layerMask)) // 만약 레이가 item layer를 감지했다면
//        {
//        Debug.Log($"Hit object: {hit.collider.name}");

//        // 충돌한 오브젝트의 태그가 "Item"인지 확인
//        if (hit.collider.CompareTag("Item"))
//        {
//            // 리스트에 GameObject 추가
//            collectedItems.Add(hit.collider.gameObject);
//            Debug.Log($"Collected item: {hit.collider.name}");


//            // 씬에서 GameObject 삭제
//            Destroy(hit.collider.gameObject);
//        }
//    }
//}















