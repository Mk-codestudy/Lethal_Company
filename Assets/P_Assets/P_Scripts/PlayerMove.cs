using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;

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

    public Animator animator; // �ִϸ�����


    [Header("�÷��̾� �̵� ���� ����")]
    float x;
    float v;
    public float walkSpeed = 5f;
    public float runSpeed = 15f;
    public float currentSpeed = 20f; // ���� �ȴ�/�ٴ� �ӵ�
    public float rotSpeed = 300f;
    public Vector3 moveDir;

    [Header("�÷��̾� �������ͽ�")] // ĳ���� ü��, ���¹̳� 
    public float maxHp; // �׹� ������ ����
    public float maxStamina = 100; // lerp����
    public float currentHp;
    public float currentStamina;
    // ü�� ���¹̳� �����̴�
    public Image hpBar;
    public Slider staminaSlider;
    bool isregenStamina = true;


    [Header("�÷��̾� ���� ����")]
    Vector3 gravityPower; // �߷��� vector���̴� ( x, y, z)
    public float jumpHeight = 10;
    public float maxJump = 1f;
    float yPos; // 
    public float gravityVelocity = 5f; // ���ϼӵ�


    float rotX; // x�� ���콺 ȸ�� �ӵ����� ���� ����
    float rotY; // 

    bool isMoving;
    //bool isRunning;
    bool isGround;
    //bool isCrouch = false; // ���� ���� x
    bool isIdle;
    //bool isOnLadder; // ��ٸ��� ��Ҵ��� �ƴ��� E�� ������ ��ȣ�ۿ��ϱ� ���� �߰���
    //bool isLadder = false; // ��ٸ� ���·� �ٲ۴�

    bool Climb = false; // ��ٸ��� ��Ҵ��� �ƴ���

    [Header("�÷��̾� ����ĳ��Ʈ ����")]
    public float rayDistance = 5f; // ����ĳ��Ʈ ���� �ִ�   ����
    public Vector3 boxSize = new Vector3(2f, 2f, 2f);  // ����ĳ��Ʈ �ڽ��� ũ��



    //������ �ݱ� ��ȣ�ۿ�
    [Header("������ �ݱ� ����")]
    private bool collideItem = false; // ������ �浹üũ
    public GameObject currentItem; // ���� �浹 ���� ������
    public Transform RightHand; // ���� �������� ���� ��ġ , Player�� �ڽĿ�����Ʈ��  right hand�� �巡���ؼ� �ִ´�.
    public GameObject selectedItem; // ���� ������ ������
    public bool holdItem; // ���� ��� �ִ¤��� Ȯ��
    public GameObject newItem;  // ����ġ�� ������
    private int selectedIndex; // ���õ� �������� �ε���
    public Inventory inventory; // �κ��丮 ��ũ��Ʈ ����
    public bool grabItem; // pickup item �� ���� ����



    public Item_Flashlight flashlightItem; // �÷��� ����Ʈ ����
   

    CharacterController cc;


    public PlayerState currentState; // �÷��̾ �����ϴ� ó�� ����

    private object playerScan;



    public enum PlayerState // �÷��̾��� ���� ���¸ӽ�
    {
        Normal,
        Walk,
        Run,
        Ladder, // ��ٸ��� Ÿ��(collider�� �浹������) ������
        Attack, // �� ���� �����������       
        OnDamaged,
        Dead,
        Cinematic,
        // ����, ��ĵ�� ������
    }







    void Start()
    {


        //Cursor.lockState = CursorLockMode.Confined; // Ŀ�� ���α�
        Cursor.lockState = CursorLockMode.Locked; // Ŀ�� ���α�
        Cursor.visible = false;

        currentState = PlayerState.Normal; // ���� ���´� normal

        animator = GetComponent<Animator>();  // animator controller �� �� �ִ� �÷��̾� �𵨸��� �̰��� ���� �ִ´�.


        cc = GetComponent<CharacterController>(); // cc ������Ʈ



        gravityPower = Physics.gravity;  // �߷� �ʱ�ȭ 

        //holdItem = null; // ����մ� �������� �ʱ�ȭ



        // ü�� ���¹̳� �ʱ�ȭ
        currentHp = maxHp;
        currentStamina = maxStamina;



    }



    public void Update()
    {

        if(currentState == PlayerState.Cinematic)
        {
            return;
        }


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
            ClimbLadder(); // ��ٸ� Ÿ��
        }

        PickupItem(); // ���ſ� ������ �ݱ�

        DropItem(); // ������ ������
        StoreItemInventory(); // ������ �κ��丮�� �߰��ϱ�
        //EquipItem(num);

        Rotate(); // ���콺 �������� ȸ��

        Jump(); // ĳ���� ����

        RegenStamina(); // ���¹̳� ���(15 ������)
        UpdateUI(); // ui�� �ǽð� ������Ʈ
        Scan();
       




        if (Input.GetKeyDown(KeyCode.Alpha1)) // 1���� ������ 
        {
            SwitchItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // 2���� ������ 
        {
            SwitchItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) // 3���� ������ 
        {
            SwitchItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) // 4���� ������ 
        {
            SwitchItem(3);
        }




        if(Input.GetKeyDown(KeyCode.Q))
        {

            Debug.Log("�������� ŵ�ϴ�");
            Handlight();
        }
        



    }






    void Normal()
    {
        // ���� ���ͷ� �����̱�
        x = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 movedir = new Vector3(x, 0f, v);
        movedir = transform.TransformDirection(movedir); // ���� ��ǥ �̵�
        movedir.Normalize(); // 0 ~ 1 ������ ���� ���´�.

        // movedir.y = yPos; // ����ؼ� �Ʒ��� �߷� ���ӵ��� ����    
        if (cc != null)
        {
            cc.Move(movedir * walkSpeed * Time.deltaTime);  // �ȴ� �ӵ��� ĳ���� ������
        }


        if (movedir.magnitude > 0.5f)
        {
            currentState = PlayerState.Walk;
        }

       
    }


    void Walk()
    {
        // ���� ���ͷ� �����̱�
        x = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 movedir = new Vector3(x, 0f, v);
        movedir = transform.TransformDirection(movedir); // ���� ��ǥ �̵�
        movedir.Normalize(); // �ӷ�

        yPos += gravityPower.y * gravityVelocity * Time.deltaTime; // �߷��� y�ప * �߷¼ӵ� * �ð� ���� 
        movedir.y = yPos; // ĳ������ y�࿡ �߷�����

        cc.Move(movedir * walkSpeed * Time.deltaTime);  // �ȴ� �ӵ��� ĳ���� ������

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
            // �̵��� ���� ���� ����
            float x = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 movedir = new Vector3(x, 0f, v);
            movedir = transform.TransformDirection(movedir); // ���� ��ǥ �̵�
            movedir.Normalize(); // 

            yPos += gravityPower.y * gravityVelocity * Time.deltaTime; // �߷��� y�ప * �߷¼ӵ� * �ð� ���� 
            movedir.y = yPos; // ĳ������ y�࿡ �߷�����


            cc.Move(movedir * runSpeed * Time.deltaTime);  // �޸��� �ӵ��� ĳ���� ������

            UseStamina(15);
        }
        else
        {
            Walk();
        }
    }

    void Jump()
    {
        // ĳ���� ����
        yPos += gravityPower.y * gravityVelocity * Time.deltaTime; // �߷��� y�ప * �߷¼ӵ� * �ð� ����  // ��� ĳ���Ϳ��� y�� �Ʒ��� ����Ǵ� ��.


        isGround = cc.collisionFlags == CollisionFlags.CollidedBelow; // �ٴ�üũ


        if (isGround) // ���ٴ��̸�
        {
            yPos = 0; // ���ٴڿ� ������ y�� ypos���� 0���� �ٲ㼭 �߷°��� �ʱ�ȭ ��Ŵ;
            maxJump = 1;
        }

        if (Input.GetButtonDown("Jump") && maxJump > 0)  // �����̽��� ������ �����Ѵ�
        {
            if (currentStamina > 15) // ����� ���� 
            {
                yPos = jumpHeight; // ������ �Ͻ������� �߷°��� ���� ���̷� �ٲ�
                maxJump--;
                // ���� ��� 
                UseStamina(15);
            }

        }

    }



    void Ladder()
    {

        float y = Input.GetAxis("Vertical"); // vertical �Է��� �ϸ� y ���� �޴´�.

        Vector3 ladderDir = new Vector3(0, y, -0.1f);  // �� �Ʒ� �θ� ������ �� �ִ�.

        cc.Move(ladderDir * walkSpeed * Time.deltaTime); // ladderdir ��������  walk���ǵ�� �����δ�.

        gravityPower = Physics.gravity; // �߷°��� �ʱ�ȭ

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentState = PlayerState.Walk;
            Climb = false;
        }

    }

    void ClimbLadder()
    {
        if (Input.GetKeyDown(KeyCode.E)) // ��ٸ� �ݶ��̴��� ��� e�� ������
        {
            Debug.Log("��ٸ��� ��ҽ��ϴ�.");
            currentState = PlayerState.Ladder;
        }
    }



    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotX += mouseY * rotSpeed * Time.deltaTime; // �¿�� �����϶� ���� y�� �ְ�
        rotY += mouseX * rotSpeed * Time.deltaTime; // ���Ϸ� �����϶� ���� x�� �ִ�.

        // ���� ȸ������ 60���� ����

        //if (rotX > 60) // rotx�� 60�� ũ�ٸ� 60���� (Maximum)
        //{
        //    rotX = 60;
        //}
        //else if (rotX < -60) // rotx�� -60�� ���� �۴ٸ� -60���� (Maximum)   
        //{
        //    rotX = -60;
        //}


        // �� mathf.clamp�� �����ϰ� ����

        rotX = Mathf.Clamp(rotX, -60f, 60f);

        transform.eulerAngles = new Vector3(0, rotY, 0f);

        if (Camera.main != null)
        {
            Camera.main.transform.GetComponent<Follow_Cam>().rotX = rotX; // ī�޶� �ִ� followcam�� ��ũ��Ʈ�� rotx ������ �޾ƿ�
        }


    }

    void Attack() //�÷��̾� ������ ���� �÷��̾ Shover�� ������ ��������
    {

        if (selectedItem.name.Contains("Shover")) // �÷��̾ shover �±��� ���� ������Ʈ�� ����ְ�
        {
            if (Input.GetMouseButtonDown(0)) // ��Ŭ���� �ߴٸ�
            {
                
            }
        }
    }

    public void Handlight() // �ڵ����Ʈ�� ��� ��������
    {
        flashlightItem = null;

        if (selectedItem != null && selectedItem.name.Contains("Flashlight"))
        {
            flashlightItem = selectedItem.GetComponent<Item_Flashlight>();
        }
        else if (newItem != null && newItem.name.Contains("Flashlight"))
        {
            flashlightItem = newItem.GetComponent<Item_Flashlight>();
        }

        if (flashlightItem != null)
        {
            if ((selectedItem.name.Contains("Flashlight")) || (newItem != null && newItem.name.Contains("Flashlight")))
            {

                flashlightItem.LightOnOff();

            }
            else
            {
                Debug.Log("tlqkfwls");
            }
        }

        
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



    #region BoxCast�� ���� ù��° ������Ʈ�� �����ϱ⿡ OverlapBox�� ����� ��
    //private void PlayerScan() // ��Ŭ���ϸ� ĳ���� �������� ��ĵ
    //{
    //    if(Input.GetMouseButtonDown(1)) // ��Ŭ���� �ϸ�
    //    {
    //        RaycastHit hitInfo; // �浹ü�� ������ ���� ����
    //        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // ī�޶��� ��ġ���� ī�޶��� foward��������

    //        bool isHit = Physics.BoxCast(ray.origin, boxSize / 2, ray.direction, out hitInfo, transform.rotation, rayDistance, 1 << 6);

    //        if(isHit) // �ڽ� ���̿� ��Ҵٸ�
    //        {               
    //           Debug.Log("�ڽ�ĳ��Ʈ�� ��ҽ��ϴ�. " + hitInfo.collider.name);

    //        }



    //    }
    //}
    #endregion



    private void Scan() // // OverlapBox �� ����� ���� ���� /  ��Ŭ���ϸ� ĳ���� �������� ��ĵ 
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 boxCenter = (Camera.main.transform.position + Camera.main.transform.forward); // ���� ī�޶��� ����������� box�� �׸���.

            Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity, 1 << 6); // ������ �ݶ��̴��� ���� �迭�� �����.



            if (hitColliders.Length > 0) // ����, ������ �ݶ��̴��� ������ 0���� ���ٸ�
            {
                for (int i = 0; i < hitColliders.Length; i++) // �迭�� �ε����� ����� ����Ѵ�
                {
                    Collider collider = hitColliders[i]; // i ��°�� �ݶ��̴��� ��������
                    Item item = collider.GetComponent<Item>(); // �� �ݶ��̴� �ȿ� �մ� item ������Ʈ�� �����´�

                    if (item != null)
                    {
                       
                        // Item ������Ʈ�� name�� value�� ����Ѵ�.
                        Debug.Log("Item Name: " + item.itemName);
                        Debug.Log("Item Value: " + item.itemValue);

                        item.ShowItemInfo(); // UI �ؽ�Ʈ�� Ȱ��ȭ�Ͽ� ������ ǥ���մϴ�.



                    }
                    else
                    {
                        Debug.Log("�ƹ� ������Ʈ�� ã�� ���߽��ϴ�." + collider.name);
                    }
                }
            }
            else
            {
                Debug.Log("�ƹ��͵� �������� ���߽��ϴ�.");
            }


        }
    }

    private void OnDrawGizmos() // �� �信�� Box Cast�� �׷���Ȯ���Ѵ�.
    {
        if (Camera.main != null)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);  // �������� ����
            Gizmos.color = Color.red;
            Gizmos.DrawRay(ray.origin, ray.direction * rayDistance);

            // �������� ���� BoxCast�� �ڽ��� �׸��ϴ�.
            Vector3 halfExtents = boxSize / 2; // �ڽ������� ��
            Vector3 castEnd = ray.origin + ray.direction * rayDistance; // ray�� ���� ��ġ���� ray�� ���� * �ִ�Ÿ�
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(castEnd, boxSize);
        }
    }



    #region ü�� ���¹̳� / ü�¹� ���¹̳ʹ�
    // ���¹̳� ���� �Լ�
    private void UseStamina(int amount)
    {
        currentStamina -= amount * Time.deltaTime; // ���¹̳� ����

        if (currentStamina <= 0)
        {
            currentStamina = 0;


        }
    }

    private void RegenStamina() // ���¹̳� ���
    {
        if (currentStamina < maxStamina)
        {
            if (isregenStamina) // isregenStamina �� true ���
            {
                currentStamina += 5 * Time.deltaTime; // ���¹̳ʸ� ����� �����
            }
        }
        else if (currentStamina <= 0) // ���� ���¹̳ʰ� 0�̰ų� 0���� ���ٸ�
        {
            isregenStamina = false; // ���� ���¹̳��� false�� �ϰ�

            StopStaminaTimer(3f); // ��ž ���� �ڷ�ƾ�� �����Ѵ�.
        }


    }

    private void StopStaminaTimer(float time) // ���¹̳ʰ� 0�� �Ǹ� time �ʰ� ���¹̳� ����� �����Ѵ�.
    {
        StartCoroutine("StaminaCoroutine");
    }

    IEnumerator StaminaCoroutine()
    {
        yield return new WaitForSeconds(3f);

        isregenStamina = true;
    }



    // ���¹̳� ü�� ��,���Ҹ� �����̴�, �̹����� ������Ʈ
    private void UpdateUI()  // HP stamina �����̴��� �� �����Ӹ��� ������Ʈ,  Mathf.Lerp�� ����ؼ� �����̴��� �ε巴�� 
    {
        // ���¹̳� ĭ
        float targetStamina = currentStamina / maxStamina; // Ÿ�� ���¹̳� �������� max���¹̳� ������ currentstamina���� ����


        staminaSlider.value = Mathf.Lerp(currentStamina, targetStamina, Time.deltaTime * 0.2f);

        // ���� �ε巴�� �����ǰ� �����ʿ�
        //staminaSlider.value = Mathf.Lerp(staminaSlider.value, currentStamina / maxStamina, Time.deltaTime * 10f);
        //hpSlider.value = currentHp;



        // ü�� ĭ
        float targetHp = currentHp / maxHp;

        Color hpBarAlpha = hpBar.color; // hp�� �̹����� �÷���, �÷� Ŭ������ ������ �־ ��Ʈ��


        if (currentHp == 100)
        {
            currentHp = 0;
            hpBarAlpha.a = 0f; // ü���� 0 �϶� hp�̹����� ������ 0 , // ü�� slider alpha �� ���� ����
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
            hpBarAlpha.a = 1f; // ü����  �϶� hp�̹����� ������ 1
        }

    }

    #endregion







    // �浹 ����
    private void OnTriggerEnter(Collider other)  // ó�� �浹��
    {

        if (other.CompareTag("Item") || other.CompareTag("Doublehand")) // item �±��� �ݶ��̴��� �浹�ߴٸ�
        {
            //Debug.Log("�����ۿ� ��ҽ��ϴ�.");

            collideItem = true; // �����ۿ� ��Ҵ��� üũ

            if (currentItem == null)
            {
                currentItem = other.gameObject; // ���� �浹�� �������� currentItem
                Debug.Log(" currentitem�� �߰��߽��ϴ�.");
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

    private void OnTriggerExit(Collider other) // ��ٸ����� ������,  �����ۿ��� ������
    {
        if (other.gameObject.CompareTag("Ladder")) // ��ٸ� �ݶ��̴����� ����������
        {
            print("��ٸ����� ���Խ��ϴ�.");
            currentState = PlayerState.Normal;
            Climb = false; // ��ٸ� �ݶ��̴��� ����� climb �� false

        }
        
        // �����۰��� �浹�� ����� ���, ���� �������� null�� ����
        if (other.CompareTag("Item") && other.gameObject == currentItem)
        {
            Debug.Log("�����ۿ��� ���������ϴ�.");

            collideItem = false;
            currentItem = null;
            Debug.Log("currentitem �� null�Դϴ�");
            //currentItem = null;
        }

    }




    

    //public void PickupItem() //������ �ݱ�  
    //{
    //    // // E�� ������ ���� �����ۿ� ����ִٸ� 
    //    if (Input.GetKeyDown(KeyCode.E) && (collideItem))
    //    {
    //        if (currentItem != null) // �׸��� �ֿ� �������� null ���� �ƴ϶��
    //        {

    //            if (holdItem == false) // ���� ����ִ� �������� null ���̶��
    //            {
    //                selectedItem = Instantiate(currentItem, RightHand.position, RightHand.rotation); //  Player-RightHand.Position�� currentItem �� �����Ѵ�. // ���� �������� Ȧ������� ������ �ִ´�.
    //                selectedItem.transform.SetParent(RightHand); // player�� righthand�� �ֿ� ������Ʈ�� �θ�� �Ѵ�.


    //                Rigidbody rb = selectedItem.GetComponent<Rigidbody>(); // item �� rigidbody ������Ʈ�� �����ͼ�
    //                if (rb != null)
    //                {
    //                    rb.isKinematic = true; // �������� �ʰ� iskineitc �� üũ
    //                }

    //                Destroy(currentItem); // ������ �����
    //                currentItem = null; // currentItem �ʱ�ȭ
    //                Debug.Log("�������� �ֿ����ϴ�.");

    //            }
    //        }
    //    }

    //}

    public void PickupItem() // �̰� ��� �������� �鶧�� �κ��� ������ �ȵǵ���
    {
        if(Input.GetKeyDown(KeyCode.E) && (collideItem))
        {
            if (currentItem.CompareTag("Doublehand") && currentItem != null) 
            {
                Debug.Log("�����ڵ��Դϴ�.");
                currentItem.transform.SetParent(RightHand);
                currentItem.transform.position = RightHand.transform.position;
                currentItem.transform.rotation = RightHand.transform.rotation;

                 Rigidbody rb = currentItem.GetComponent<Rigidbody>(); // item �� rigidbody ������Ʈ�� �����ͼ�
                if (rb != null)
                {
                    rb.isKinematic = true; // �������� �ʰ� iskineitc �� üũ
                }

                // �� ��� ������Ʈ�� ��� �ٸ� �����۰��� ��ȣ�ۿ����� ���ϵ���

                grabItem = true;
                
            }

                        
           
        }
    }

    public void DropItem() // ������ ������
    {

        if (Input.GetKeyDown(KeyCode.G) && holdItem)  // G�� ������ ��� �ִ� �������� ����ִٸ�
        {
            Debug.Log("GŰ�� �Է���ϴ�.");

            Vector3 dropPos = transform.position + transform.forward * 3f; // �÷��̾��� ���� 3f�� �Ÿ��տ��� ����߸� ������ ����

            if (selectedItem != null)
            {
                selectedItem.transform.SetParent(null); // Righthand���� ���
                selectedItem.transform.position = dropPos; // hold �������� ��ġ�� droppos�� �̵�
                selectedItem.SetActive(true);


                Rigidbody rb = selectedItem.GetComponent<Rigidbody>(); // ���߿��� ������ �������� ����߸��� ���� item �� rigidbody�� �߰�

                if (rb != null)
                {
                    rb.isKinematic = false; // �߷��� �ۿ��� �ޱ� ���� iskinetic�� üũ����
                }
                else
                {
                    Debug.Log("Rigidbody �� �����ϴ�.");
                }

                inventory.RemoveItem(selectedIndex);

               
                holdItem = false; // ���� �տ� �� �������� null�� ����
                selectedItem = null; // ���� ���õ� �������� null�� ����
                currentItem = null;

            }
            
        }

        // �̰� ��� �������̳� ������ ���� �������� - inven�� ��������ʴ� ���ӿ�����Ʈ

        if (Input.GetKeyDown(KeyCode.G) && (currentItem.CompareTag("Doublehand") && grabItem))
        {
            Vector3 dropPos = transform.position + transform.forward * 3f; // �÷��̾��� ���� 3f�� �Ÿ��տ��� ����߸� ������ ����
            currentItem.transform.SetParent(null); // Righthand���� ���
            currentItem.transform.position = dropPos; // hold �������� ��ġ�� droppos�� �̵�
            currentItem.SetActive(true);

            Rigidbody rb = currentItem.GetComponent<Rigidbody>(); // ���߿��� ������ �������� ����߸��� ���� item �� rigidbody�� �߰�

            if (rb != null)
            {
                rb.isKinematic = false; // �߷��� �ۿ��� �ޱ� ���� iskinetic�� üũ����
            }
            else
            {
                Debug.Log("Rigidbody �� �����ϴ�.");
            }

            grabItem = false;
            holdItem = false; // ���� �տ� �� �������� null�� ����
            currentItem = null; // ���� ���õ� �������� null�� ����


        }
       

        




    }

      //else if (Input.GetKeyDown(KeyCode.G))
      //  {            
      //      {
      //          Debug.Log("G Ű�� �������� selectedItem�� null�Դϴ�.");
      //      }
      //  }




    public void StoreItemInventory() // ó�� ������ �������� �ֿ����� �κ��丮�� �����ϴ� �Լ�
    {
        if (currentItem != null && collideItem) // ����, �����۰� �浹�ϰ�, ���� �������� null �� �ƴ϶���
        {
            if (Input.GetKeyDown(KeyCode.E) && currentItem.CompareTag("Item")) // e�� ������
            {
                inventory.AddItem(currentItem);
                currentItem.SetActive(false);
                currentItem = null;
            }
            //else if(currentItem.CompareTag("Doublehand"))
            //{
            //    return;              
            //}
          

        }

    }

    public void SwitchItem(int index) // �̰� �������� �迭�� ������ �������� ��ġ���ʰ� �ٲܼ� �ְ����ִ� �Լ�
    {
       
        GameObject newItem = inventory.SelectItem(index); // �迭 index ���� �ִ� �����͸� newItem ������ �ִ´�.

        // ����, �迭�� �������� �ְ� �տ� ���� ������
        if (newItem != null)
        {
            if (holdItem)

            {
                // �� �Լ��� ����
                RemoveHandItem();

            }


            // ���� �迭�� �������� �������
            Debug.Log("Ȧ��������� ������ϴ�.");

            newItem.transform.SetParent(RightHand); // Righthand �� �θ���ϰ�
            newItem.transform.position = RightHand.transform.position;           
            newItem.transform.forward = RightHand.transform.forward;
            //newItem.transform.position = Vector3.zero;
            //newItem.transform.rotation = Quaternion.identity;


            if (newItem != null && newItem.name.Contains("Shover"))
            {
                // ���� ȸ����
                Vector3 shoverVector = new Vector3(3.047f, -240.752f, 1.705f);

                Quaternion shoverRotation = Quaternion.Euler(shoverVector);
         
                newItem.transform.position = RightHand.transform.position;
                newItem.transform.rotation = shoverRotation;
            }
            else
            {
                newItem.transform.forward = RightHand.transform.forward;
            }

            if (newItem != null && newItem.name.Contains("Flashlight"))
            {
                // ���������� ȸ����
                Vector3 handlightRV = new Vector3(-17.667f, -90.994f, -183.146f);
             
                Quaternion HandlightRotation = Quaternion.Euler(handlightRV);
              
                // �������� ȸ������ ���� ��ǥ �����̼����� �ٲ㼭 �����Ѵ�.
                newItem.transform.position = RightHand.position;
                newItem.transform.localRotation = HandlightRotation;
            }
            else
            {
                newItem.transform.forward = RightHand.transform.forward;
            }





            // ���߿��� ������ �������� ����߸��� ���� item �� rigidbody�� �߰�
            Rigidbody rb = newItem.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = true; // ��������� �߷� �ȹޱ�
            }
            else
            {
                Debug.Log("Rigidbody �� �����ϴ�.");
            }

            selectedItem = newItem; // �� �������� selected item �� �ȴ�
            newItem.SetActive(true);
            holdItem = true;
            selectedIndex = index; // ���õ� �������� �ε��� ����
        }
        else
        {
            Debug.Log("���õ� �������� �����ϴ�.");
            RemoveHandItem();
        }
    }
    

    public void RemoveHandItem() // ���� �տ� �� ������ ��Ȱ��ȭ�ϴ� �Լ�
    {    // �տ� ������ �ְ� ��ȣ�� �Է������� 
        if (selectedItem != null)
        {            
                selectedItem.SetActive(false); // ���� ��� �ִ� ������ ��Ȱ��ȭ
                selectedItem.transform.SetParent(null); // �������� �θ� �����Ͽ� ���忡 ���������� ��ġ

            //�����ڸ� �ֿ�� �������� scale���� 0.6f�� 
            if (selectedItem.name.Contains("Kettle"))
            {
                selectedItem.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            }
            else
            {
                return;
            }

                Rigidbody rb = selectedItem.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.isKinematic = false; // �߷��� ������ �޵��� ����
                }

                holdItem = false;

            }
        }
    }









   



//    RaycastHit hit;

//    Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // ������ �߻���ġ�� ���� ī�޶�

//    Color rayColor = Color.red;

//    Debug.DrawRay(ray.origin, ray.direction * rayDistance, rayColor); // ���̸� ���� ���̰�

//    // "Item" ���̾ Ž���ϵ��� ���̾� ����ũ ����
//    int layerMask = LayerMask.GetMask("Item");

//    if (Physics.Raycast(ray, out hit, rayDistance, layerMask)) // ���� ���̰� item layer�� �����ߴٸ�
//        {
//        Debug.Log($"Hit object: {hit.collider.name}");

//        // �浹�� ������Ʈ�� �±װ� "Item"���� Ȯ��
//        if (hit.collider.CompareTag("Item"))
//        {
//            // ����Ʈ�� GameObject �߰�
//            collectedItems.Add(hit.collider.gameObject);
//            Debug.Log($"Collected item: {hit.collider.name}");


//            // ������ GameObject ����
//            Destroy(hit.collider.gameObject);
//        }
//    }
//}















