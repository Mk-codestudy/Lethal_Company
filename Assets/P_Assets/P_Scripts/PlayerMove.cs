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
using static Turret;
//using UnityEngine.UIElements;



//using static UnityEditor.Progress;

public class PlayerMove : MonoBehaviour
{

    public Animator animator; // 애니메이터


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
    //bool isRunning;
    bool isGround;
    //bool isCrouch = false; // 앉은 상태 x
    bool isIdle;
    //bool isOnLadder; // 사다리에 닿았는지 아닌지 E를 눌러서 상호작용하기 위해 추가함
    //bool isLadder = false; // 사다리 상태로 바꾼다

    bool Climb = false; // 사다리에 닿았는지 아닌지

    [Header("플레이어 레이캐스트 변수")]
    public float rayDistance = 5f; // 레이캐스트 레이 최대   길이
    public Vector3 boxSize = new Vector3(2f, 2f, 2f);  // 레이캐스트 박스의 크기



    //아이템 줍기 상호작용
    [Header("아이템 줍기 변수")]
    private bool collideItem = false; // 아이템 충돌체크
    private GameObject currentItem; // 현재 충돌 중인 아이템
    public Transform RightHand; // 주을 아이템이 있을 위치 , Player의 자식오브젝트인  right hand를 드래그해서 넣는다.
    public GameObject selectedItem; // 현재 선택한 아이템
    public bool holdItem; // 현재 들고 있는ㄴ지 확인
    private int selectedIndex; // 선택된 아이템의 인덱스
    public Inventory inventory; // 인벤토리 스크립트 참조

    public int num; // 아이템 슬롯의 번호 ( 1~4 )

    CharacterController cc;


    public PlayerState currentState; // 플레이어가 시작하는 처음 상태

    private object playerScan;



    public enum PlayerState // 플레이어의 유한 상태머신
    {
        Normal,
        Walk,
        Run,
        Ladder, // 사다리에 타고(collider가 충돌됫을때) 있을때
        Attack, // 은 삽을 들고있을때만
        Handlight, // 손전등 들고있을때만
        OnDamaged,
        Dead,
        Cinematic,
        // 점프, 스캔은 공통요소
    }







    void Start()
    {


        //Cursor.lockState = CursorLockMode.Confined; // 커서 가두기
        Cursor.lockState = CursorLockMode.Locked; // 커서 가두기
        Cursor.visible = false;

        currentState = PlayerState.Normal; // 시작 상태는 normal

        animator = GetComponent<Animator>();  // animator controller 가 들어가 있는 플레이어 모델링을 이곳에 집어 넣는다.


        cc = GetComponent<CharacterController>(); // cc 컴포넌트



        gravityPower = Physics.gravity;  // 중력 초기화 

        //holdItem = null; // 들고잇는 아이템을 초기화



        // 체력 스태미너 초기화
        currentHp = maxHp;
        currentStamina = maxStamina;



    }



    void Update()
    {

        switch (currentState)
        {
            case PlayerState.Normal:
                Normal();
                break;
            case PlayerState.Walk:
                Walk();
                Jump();
                break;
            case PlayerState.Run:
                Run();
                Jump();
                break;
            case PlayerState.Ladder:
                Ladder();
                break;
            case PlayerState.Attack:
                Attack();
                break;
            case PlayerState.Handlight:
                Handlight();
                break;
            case PlayerState.OnDamaged:
                OnDamaged();
                break;
            case PlayerState.Dead:
                Dead();
                break;
            case PlayerState.Cinematic:
                Cinematic();
                break;

        }

        if (Climb)
        {
            ClimbLadder(); // 사다리 타기
        }

        //PickupItem(); // 아이템 줍기
        DropItem(); // 아이템 버리기
        StoreItemInventory(); // 아이템 인벤토리에 추가하기
        //EquipItem(num);

        Rotate(); // 마우스 방향으로 회전

        Jump(); // 캐릭터 점프

        RegenStamina(); // 스태미너 재생(15 고정값)
        UpdateUI(); // ui를 실시간 업데이트
        Scan();





        if (Input.GetKeyDown(KeyCode.Alpha1)) // 1번을 누르면 
        {
            SwitchItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // 2번을 누르면 
        {
            SwitchItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) // 3번을 누르면 
        {
            SwitchItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) // 4번을 누르면 
        {
            SwitchItem(3);
        }















    }

           




    void Normal()
    {
        // 로컬 벡터로 움직이기
        x = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 movedir = new Vector3(x, 0f, v);
        movedir = transform.TransformDirection(movedir); // 로컬 좌표 이동
        movedir.Normalize(); // 0 ~ 1 사이의 값만 나온다.

        // movedir.y = yPos; // 계속해서 아래로 중력 가속도를 받음    

        cc.Move(movedir * walkSpeed * Time.deltaTime);  // 걷는 속도로 캐릭터 움직임



        if (movedir.magnitude > 0.5f)
        {
            currentState = PlayerState.Walk;
        }

    }


    void Walk()
    {
        // 로컬 벡터로 움직이기
        x = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 movedir = new Vector3(x, 0f, v);
        movedir = transform.TransformDirection(movedir); // 로컬 좌표 이동
        movedir.Normalize(); // 속력

        yPos += gravityPower.y * gravityVelocity * Time.deltaTime; // 중력의 y축값 * 중력속도 * 시간 보간 
        movedir.y = yPos; // 캐릭터의 y축에 중력적용

        cc.Move(movedir * walkSpeed * Time.deltaTime);  // 걷는 속도로 캐릭터 움직임

        if (movedir.magnitude > 0.5f && Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentState = PlayerState.Run;
        }
        else
        {
            currentState = PlayerState.Walk;
        }

        if (movedir.magnitude < 0.5f)
        {
            currentState = PlayerState.Normal;
        }
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            // 이동을 위한 로컬 변수
            float x = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 movedir = new Vector3(x, 0f, v);
            movedir = transform.TransformDirection(movedir); // 로컬 좌표 이동
            movedir.Normalize(); // 

            yPos += gravityPower.y * gravityVelocity * Time.deltaTime; // 중력의 y축값 * 중력속도 * 시간 보간 
            movedir.y = yPos; // 캐릭터의 y축에 중력적용


            cc.Move(movedir * runSpeed * Time.deltaTime);  // 달리는 속도로 캐릭터 움직임

            UseStamina(15);
        }
        else
        {
            Walk();
        }
    }

    void Jump()
    {
        // 캐릭터 점프
        yPos += gravityPower.y * gravityVelocity * Time.deltaTime; // 중력의 y축값 * 중력속도 * 시간 보간  // 상시 캐릭터에게 y축 아래로 적용되는 힘.


        isGround = cc.collisionFlags == CollisionFlags.CollidedBelow; // 바닥체크


        if (isGround) // 땅바닥이면
        {
            yPos = 0; // 땅바닥에 닿으면 y축 ypos값을 0으로 바꿔서 중력값을 초기화 시킴;
            maxJump = 1;
        }

        if (Input.GetButtonDown("Jump") && maxJump > 0)  // 스페이스를 누르면 점프한다
        {
            if (currentStamina > 15) // 상수로 넣음 
            {
                yPos = jumpHeight; // 점프시 일시적으로 중력값을 점프 높이로 바꿈
                maxJump--;
                // 점프 모션 
                UseStamina(15);
            }

        }

    }



    void Ladder()
    {


        float y = Input.GetAxis("Vertical"); // vertical 입력을 하면 y 값을 받는다.

        Vector3 ladderDir = new Vector3(0, y, -0.05f);  // 위 아래 로만 움직일 수 있다.

        cc.Move(ladderDir * walkSpeed * Time.deltaTime); // ladderdir 방향으로  walk스피드로 움직인다.

        gravityPower = Physics.gravity; // 중력값은 초기화

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentState = PlayerState.Walk;
            Climb = false;
        }

    }

    void ClimbLadder()
    {
        if (Input.GetKeyDown(KeyCode.E)) // 사다리 콜라이더에 닿고 e를 누르면
        {
            Debug.Log("사다리에 닿았습니다.");
            currentState = PlayerState.Ladder;
        }
    }



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

    void Attack() //플레이어 공격은 오직 플레이어가 Shover를 가지고 있을때만
    {

        if (selectedItem.CompareTag("Shover")) // 플레이어가 shover 태그의 게임 오브젝트를 들고있고
        {
            if (Input.GetMouseButtonDown(0)) // 좌클릭을 했다면
            {

            }
        }
    }

    void Handlight() // 핸드라이트를 들고 있을때만
    {

    }

    void OnDamaged()
    {

    }

    void Dead()
    {

    }

    void Cinematic()
    {

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



    private void Scan() // // OverlapBox 를 사용한 다중 감지 /  우클릭하면 캐릭터 전방으로 스캔 
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 boxCenter = (Camera.main.transform.position + Camera.main.transform.forward); // 메인 카메라의 정면방향으로 box를 그린다.

            Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity, 1 << 6); // 감지한 콜라이더를 담을 배열을 만든다.



            if (hitColliders.Length > 0) // 만약, 감지된 콜라이더의 개수가 0보다 많다면
            {
                for (int i = 0; i < hitColliders.Length; i++) // 배열의 인덱스를 사용해 출력한다
                {
                    Collider collider = hitColliders[i]; // i 번째의 콜라이더를 가져오고
                    Item item = collider.GetComponent<Item>(); // 그 콜라이더 안에 잇는 item 컴포넌트를 가져온다

                    if (item != null)
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
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);  // 빨간줄은 레이
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * rayDistance);

        // 빨간줄의 끝에 BoxCast의 박스를 그립니다.
        Vector3 halfExtents = boxSize / 2; // 박스길이의 반
        Vector3 castEnd = ray.origin + ray.direction * rayDistance; // ray의 시작 위치에서 ray의 방향 * 최대거리
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(castEnd, boxSize);

    }



    #region 체력 스태미너 / 체력바 스태미너바
    // 스태미너 관련 함수
    private void UseStamina(int amount)
    {
        currentStamina -= amount * Time.deltaTime; // 스태미너 감소

        if (currentStamina <= 0)
        {
            currentStamina = 0;


        }
    }

    private void RegenStamina() // 스태미너 재생
    {
        if (currentStamina < maxStamina)
        {
            if (isregenStamina) // isregenStamina 가 true 라면
            {
                currentStamina += 5 * Time.deltaTime; // 스태미너를 재생은 상수로
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
        // 스태미너 칸
        float targetStamina = currentStamina / maxStamina; // 타겟 스태미나 벨류값은 max스태미너 값에서 currentstamina값의 비율


        staminaSlider.value = Mathf.Lerp(currentStamina, targetStamina, Time.deltaTime * 0.2f);

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







    // 충돌 판정
    private void OnTriggerEnter(Collider other)  // 처음 충돌시
    {

        if (other.CompareTag("Item")) // item 태그의 콜라이더와 충돌했다면
        {
            //Debug.Log("아이템에 닿았습니다.");

            collideItem = true; // 아이템에 닿았느지 체크

            if (currentItem == null)
            {
                currentItem = other.gameObject; // 현재 충돌한 아이템은 currentItem
                Debug.Log(" currentitem을 추가했습니다.");
            }


        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ladder") && currentState != PlayerState.Ladder)
        {
            Climb = true;
        }

    }

    private void OnTriggerExit(Collider other) // 사다리에서 떨어짐,  아이템에서 떨어짐
    {
        if (other.gameObject.CompareTag("Ladder")) // 사다리 콜라이더에서 떨어졌을때
        {
            print("사다리에서 나왔습니다.");

        }

        // 아이템과의 충돌이 종료된 경우, 현재 아이템을 null로 설정
        if (other.CompareTag("Item") && other.gameObject == currentItem)
        {
            Debug.Log("아이템에서 떨어졌습니다.");

            collideItem = false;
            currentItem = null;
            Debug.Log("currentitem 이 null입니다");
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

                if (holdItem == false) // 현재 들고있는 아이템이 null 값이라면
                {
                    selectedItem = Instantiate(currentItem, RightHand.position, RightHand.rotation); //  Player-RightHand.Position에 currentItem 을 생성한다. // 현재 아이템을 홀드아이템 변수로 넣는다.
                    selectedItem.transform.SetParent(RightHand); // player의 righthand를 주운 오브젝트의 부모로 한다.


                    Rigidbody rb = selectedItem.GetComponent<Rigidbody>(); // item 의 rigidbody 컴포넌트를 가져와서
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

        if (Input.GetKeyDown(KeyCode.G) && holdItem)  // G를 누르고 들고 있는 아이템을 들고있다면
        {
            Debug.Log("G키가 입력됬습니다.");

            Vector3 dropPos = transform.position + transform.forward * 3f; // 플레이어의 전방 3f의 거리앞에서 떨어뜨릴 지점을 생성

            if (selectedItem != null)
            {
                selectedItem.transform.SetParent(null); // Righthand에서 벗어남
                selectedItem.transform.position = dropPos; // hold 아이템의 위치를 droppos로 이동
                selectedItem.SetActive(true);


                Rigidbody rb = selectedItem.GetComponent<Rigidbody>(); // 공중에서 생성된 아이템을 떨어뜨리기 위해 item 에 rigidbody를 추가

                if (rb != null)
                {
                    rb.isKinematic = false; // 중력의 작용을 받기 위해 iskinetic을 체크해제
                }
                else
                {
                    Debug.Log("Rigidbody 가 없습니다.");
                }

                inventory.RemoveItem(selectedIndex);

                holdItem = false; // 현재 손에 든 아이템을 null로 설정
                selectedItem = null; // 현재 선택된 아이템을 null로 설정
            
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {            
            {
                Debug.Log("G 키가 눌렸지만 selectedItem이 null입니다.");
            }
        }






    }




    public void StoreItemInventory() // 처음 씬에서 아이템을 주웠을때 인벤토리에 저장하는 함수
    {
        if (currentItem != null && collideItem) // 만약, 아이템과 충돌하고, 현재 아이템이 null 이 아니라라면
        {
            if (Input.GetKeyDown(KeyCode.E)) // e를 누르면
            {
                inventory.AddItem(currentItem);
                currentItem.SetActive(false);
                currentItem = null;
            }

        }

    }

    public void SwitchItem(int index) // 이건 아이템이 배열에 있을때 아이템이 겹치지않고 바꿀수 있게해주는 함수
    {
       
        GameObject newItem = inventory.SelectItem(index); // 배열 index 번에 있는 데이터를 newItem 변수에 넣는다.

        // 만약, 배열에 아이템이 있고 손에 아무것도 없을떄
        if (newItem != null)
        {
            if (holdItem)

            {
                RemoveHandItem();

            }

            Debug.Log("홀드아이템이 생겻습니다.");

            newItem.transform.SetParent(RightHand); // Righthand 를 부모로하고
            newItem.transform.position = Vector3.zero;
            newItem.transform.rotation = Quaternion.identity;


            // 공중에서 생성된 아이템을 떨어뜨리기 위해 item 에 rigidbody를 추가
            Rigidbody rb = newItem.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = true; // 들고있을땐 중력 안받기
            }
            else
            {
                Debug.Log("Rigidbody 가 없습니다.");
            }

            selectedItem = newItem; // 새 아이템이 selected item 이 된다
            newItem.SetActive(true);
            holdItem = true;
            selectedIndex = index; // 선택된 아이템의 인덱스 설정
        }
        else
        {
            Debug.Log("선택된 아이템이 없습니다.");
        }
    }
    





    public void RemoveHandItem() // 현재 손에 든 물건을 비활성화하는 함수
    {    // 손에 물건이 있고 번호를 입력했을때 
        if (selectedItem != null)
        {            
                selectedItem.SetActive(false); // 현재 들고 있는 아이템 비활성화
                selectedItem.transform.SetParent(null); // 아이템의 부모를 제거하여 월드에 독립적으로 배치
                Rigidbody rb = selectedItem.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.isKinematic = false; // 중력의 영향을 받도록 설정
                }

                holdItem = false;

            }
        }
    }

    //public void EquipItem(int num)  /// 수정필요
    //{
    //    GameObject equipItem = inventory.GetItem(num); // getitem의 매개변수와 equip 아이템의 매개변수를 맞춘다

    //    if (equipItem != null) // 호출한 아이템이 null이 아닌 경우
    //    {
    //        if (holdItem != null) // 현재 손에 아이템이 있을 경우
    //        {
    //            // 현재 손에 든 아이템을 비활성화하고 부모를 제거
    //            holdItem.SetActive(false);
    //            holdItem.transform.SetParent(null); // 손에서 부모 제거
    //        }

    //        // 새 아이템을 손에 장착
    //        holdItem = equipItem;
    //        holdItem.transform.position = RightHand.position;
    //        holdItem.transform.rotation = RightHand.rotation;
    //        holdItem.SetActive(true);
    //        holdItem.transform.SetParent(RightHand); // 오른손을 부모로 설정
    //    }
    //    else
    //    {
    //        // 호출한 아이템이 null일 경우
    //        if (holdItem != null)
    //        {
    //            // 현재 손에 든 아이템이 있으면 이를 비활성화하고 부모를 제거
    //            holdItem.SetActive(false);
    //            holdItem.transform.SetParent(null); // 손에서 부모 제거
    //            holdItem = null; // 손에 든 아이템 초기화
    //        }
    //    }






    //if (equipitem != null) // 호출한 물건이 null이 아니고
    //{
    //    if (holdItem == null)
    //    {
    //        holdItem = equipitem; // 배열에서 호출한 아이템이 holditem 이다

    //        holdItem.SetActive(true);
    //    }
    //}
    //else if (equipitem != null) // 호출한 물건이 null이 아니고
    //{
    //    if (holdItem != null) // 손에 무엇을 들고 있고 다른 번호를 눌러 배열에 있는 아이템을 호출했다면
    //    {
    //        holdItem.SetActive(false);// 현재 들고있는 아이템을 비활성화 시키고

    //        holdItem = equipitem; // 배열에서 호출한 아이템이 holditem 이다
    //    }
    //}
    //else if (equipitem == null)
    //{
    //    if (holdItem != null)
    //    {
    //        return;
    //    }
    //}








        //    if (equipitem != null) // 배열에서 호출한 아이템이 null 이아니고
        //{
        //    if (holdItem != null) // 빈 손이 아니라면
        //    {
        //        inventory.AddItem(holdItem); // 현재 손에 들고있는 아이템을 배열에 담는다
        //        holdItem.SetActive(false);
        //        holdItem.transform.SetParent(null);  //holditem 부모를 제거
        //    }

        //    //그리고 배열에서 호출한 아이템을 손에 든다

        //    holdItem = equipitem;
        //    holdItem.transform.position = RightHand.position;
        //    holdItem.transform.rotation = RightHand.rotation;
        //    holdItem.SetActive(true);
        //    holdItem.transform.SetParent(RightHand); // 오른손을 부모로 잡음
        //}
        //else
        //{
        //    if (holdItem != null) // 들고 있는 아이템이 있다면
        //    {
        //        inventory.AddItem(holdItem);
        //        holdItem.SetActive(false);
        //        holdItem.transform.SetParent(null); //holditem 부모를 제거
        //        holdItem = null;
        //    }
        //}
    





        //if (Input.GetKeyDown(KeyCode.Alpha1)) // 1을 누르면
        //    {
        //        if(holdItem == null) // 만약, 손에 든 아이템이 없다면
        //        {
        //           holdItem = inventory.GetItem(0); // 0번에 있는 아이템을 가져온다. 어디로?
        //            holdItem.transform.position = RightHand.transform.position; // 오른손의 위치로
        //            holdItem.transform.rotation =  RightHand.transform.rotation;
        //            holdItem.SetActive(true);

        //        }
        //        else if(holdItem != null) // 만약, 손에 item을 들고 있다면
        //        {
        //            inventory.AddItem(holdItem); // 현재, 들고 있는 물건을 다시 넣고
        //            holdItem.SetActive(false);
        //            holdItem = null;
        //        }


        //}


    





    // collideitem 으로 collide 충돌확인
    // 현재 닿은 아이템을 currentitem 변수에 복사
    // 만약, 닿은 collider의 tag 가 item 이면
    // 미리 만들어둔 인벤토리 배열에 담는다.
    // 주운 오브젝트는 월드에서 SetActive(false)


// 각 배열에 번호를 할당 - player.update
// 해당 번호를 누르면 
// holditem 이라는 변수를 만들고 그 안에 담는다
// holditem의 위치는 미리 만들어둔 오른손의 위치(Righthand Position)

// 만약, 1번을 눌렀다가 2번을 누르면
// hold item 을 빈칸을찾아(for i == null) 넣고
// 2번 오브젝트를 꺼낸다.



// trigger는 각 오브젝트에 만들어 두는게 낫다



// 아이템을 줍는다
// 4칸짜리 배열
// 아이템을 주우면
// 해당 아이템을 씬에서 비활성화 하고
// 배열에 담는다

// 배열에 0(1) 1(2) 2(3) 3(4) 번호를 할당하고
// 번호를 누르면 해당 칸에 있는 아이템을
// RIghthand position 으로 이동시키고
// 활성화 한다.
















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















