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
    bool isRunning;
    bool isGround;
    //bool isCrouch = false; // ���� ���� x
    bool isIdle;
    bool isOnLadder; // ��ٸ��� ��Ҵ��� �ƴ��� E�� ������ ��ȣ�ۿ��ϱ� ���� �߰���
    bool isLadder = false; // ��ٸ� ���·� �ٲ۴�


    [Header("�÷��̾� ����ĳ��Ʈ ����")]
    public float rayDistance = 5f; // ����ĳ��Ʈ ���� �ִ�   ����
    public Vector3 boxSize = new Vector3(2f, 2f, 2f);  // ����ĳ��Ʈ �ڽ��� ũ��




    //������ �ݱ� ��ȣ�ۿ�



    private bool collideItem = false; // ������ �浹üũ
    private GameObject currentItem; // ���� �浹 ���� ������
    public Transform RightHand; // ���� �������� ���� ��ġ , Player�� �ڽĿ�����Ʈ��  right hand�� �巡���ؼ� �ִ´�.
    private GameObject holdItem; // ���� ��� �ִ� �������� ǥ��

    public Inventory inventory; // �κ��丮 ��ũ��Ʈ ����



    CharacterController cc;

   

    //public Animator animator;

    public PlayerState currentState; // �÷��̾ �����ϴ� ó�� ����
    private object playerScan;

    public enum PlayerState // �÷��̾��� ���� ���¸ӽ�
    {
        Idle,
        Attack,
        OnDamaged,
        Dead,
        Scan
    }



    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined; // Ŀ�� ���α�
        Cursor.lockState = CursorLockMode.Locked; // Ŀ�� ���α�
        Cursor.visible = false;

        cc = GetComponent<CharacterController>(); // cc ������Ʈ

        //animator = GetComponent<Animator>();  // animator controller �� �� �ִ� �÷��̾� �𵨸��� �̰��� ���� �ִ´�.

        gravityPower = Physics.gravity;  // �߷� �ʱ�ȭ 

        //holdItem = null; // ����մ� �������� �ʱ�ȭ



        // ü�� ���¹̳� �ʱ�ȭ
        currentHp = maxHp;
        currentStamina = maxStamina;



    }

    void Update()
    {

        PickupItem(); // ������ �ݱ�
        DropItem(); // ������ ������
        Rotate();
        RegenStamina(5);


        if ((isOnLadder) && Input.GetKeyDown(KeyCode.E))  // ��ٸ��� ����ְ� e�� ��������
        {
            isLadder = !isLadder; // isladder �� true �� �ȴ�.

            if (isLadder) // isLadder ���¿�����
            {
                gravityPower = Physics.gravity; // �߷��� �ʱ�ȭ�Ѵ�.
                
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


    // OverlapBox �� ����� ���� ����
    private void PlayerScan() // ��Ŭ���ϸ� ĳ���� �������� ��ĵ 
    {
        if(Input.GetMouseButtonDown(1))
        {
            Vector3 boxCenter = (Camera.main.transform.position + Camera.main.transform.forward); // ���� ī�޶��� ����������� box�� �׸���.
            Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity, 1 << 6); // ������ �ݶ��̴��� ���� �迭�� �����.



            if(hitColliders.Length > 0) // ����, ������ �ݶ��̴��� ������ 0���� ���ٸ�
            {
                for(int i = 0; i < hitColliders.Length; i++) // �迭�� �ε����� ����� ����Ѵ�
                {
                    Collider collider = hitColliders[i]; // i ��°�� �ݶ��̴��� ��������
                    Item item = collider.GetComponent<Item>(); // �� �ݶ��̴� �ȿ� �մ� item ������Ʈ�� �����´�
                    
                    if(item != null)
                    {
                        //print("" + item.itemName);
                        //print("" + item.itemValue);

                        // Item ������Ʈ�� name�� value�� ����մϴ�.
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
        if(Camera.main == null)
        {
            return;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);  // �������� ����
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * rayDistance);

        // �������� ���� BoxCast�� �ڽ��� �׸��ϴ�.
        Vector3 halfExtents = boxSize / 2; // �ڽ������� ��
        Vector3 castEnd = ray.origin + ray.direction * rayDistance; // ray�� ���� ��ġ���� ray�� ���� * �ִ�Ÿ�
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(castEnd, boxSize);

    }

    private void PlayerAttak() //���߿� ���Ϳ� ��ȣ�ۿ��� �Ű����� �ֱ�
    {
        //�÷��̾� ������ ���� �÷��̾ Shover�� ������ ��������
        
        if(holdItem.CompareTag("Shover")) // �÷��̾ shover �±��� ���� ������Ʈ�� ����ְ�
        {
            if(Input.GetMouseButtonDown(0)) // ��Ŭ���� �ߴٸ�
            {

            }
        }
    }

    private void PlayerDead()
    {
        if(currentHp <= 0) // ����, ���� ü���� 0���� �۰ų� ���ٸ�
        {
            //�÷��̾� �𵨸�(�ƹ�Ÿ�� fbx����) ragdoll wizard �߰�
        }

    }
    private void PlayerOnDamaged()
    {

     }







    #region ĳ���� �̵�/ ����

    void Move() // ĳ���� �ȱ�, �޸���, ����, ���̱�(Crouch)
    {


        // ���� ���ͷ� �����̱�
        x = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 movedir = new Vector3(x, 0f, v);

        movedir = transform.TransformDirection(movedir); // ���� ��ǥ �̵�
        movedir.Normalize(); // �ӷ�



        // ������ ����
        isIdle = movedir.magnitude == 0; // �̵��Ÿ��� 0�̸� Idle ����
        isMoving = movedir.magnitude > 0; // ĳ���Ͱ� �����̴� �Ÿ��� 0���� ũ�� ismoving 
        isRunning = Input.GetKey(KeyCode.LeftShift); // ����Ʈ�� ������ ������



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



        // ĳ���� ����
        yPos += gravityPower.y * gravityVelocity * Time.deltaTime; // �߷��� y�ప * �߷¼ӵ� * �ð� ���� 

        // ��� ĳ���Ϳ��� y�� �Ʒ��� ����Ǵ� ��.
        isGround = cc.collisionFlags == CollisionFlags.CollidedBelow;


        if (isGround) // ���ٴ��̸�
        {
            yPos = 0; // ���ٴڿ� ������ y�� ypos���� 0���� �ٲ㼭 �߷°��� �ʱ�ȭ ��Ŵ;
            maxJump = 1;
        }

        if (Input.GetButtonDown("Jump") && maxJump > 0)  // �����̽��� ������ �����Ѵ�
        {
            if (currentStamina > 15) // ����� ���� 
            {
                yPos = jumpHeight; // ������ �Ͻ������� �߷°��� ���� ���̷� ġȯ
                maxJump--;
                UseStamina(15);
            }

        }

        movedir.y = yPos;

        cc.Move(movedir * currentSpeed * Time.deltaTime);  // ������

    }
    #endregion
    #region ĳ���� ȸ��

    // ���콺 ���⿡ ���� ĳ������ ȸ��
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

    #endregion



    #region hp stamina ����
    // ���¹̳� ���� �Լ�
    private void UseStamina(int amount)
    {
        currentStamina -= amount * Time.deltaTime; // ���¹̳� ����

        if (currentStamina <= 0)
        {
            currentStamina = 0;

            isRunning = false;


        }
    }

    private void RegenStamina(int amount) // ���¹̳� ���
    {
        if (currentStamina < maxStamina)
        {
            if (isregenStamina) // isregenStamina �� true ���
            {
                currentStamina += amount * Time.deltaTime; // ���¹̳ʸ� ����Ѵ�.
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
        // ���¹̳� �����̵�

        float targetStamina = currentStamina / maxStamina; // Ÿ�� ���¹̳� �������� max���¹̳� ������ currentstamina���� ����


        staminaSlider.value = Mathf.Lerp(currentStamina, targetStamina, Time.deltaTime * 0.5f);

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

















    //��ٸ��� �����Ѵ�
    //�÷��̾ Ladder Tag�� ���� Collider�� stay �Ұ��]
    //�÷��̾��� z,x�� �������� �����ϰ�
    // w, s�� �������� �� �� transform.up , -transform.up �� �������� walkSpeed �� �̵��Ѵ�.
    // ��ٸ� �ȿ����� �������θ� �����̰� ����
    // �׸��� ��ٸ� �ݶ��̴��� ������� E �� ȭ�鿡 ��µǰ� �ϱ� - �߰��Ұ�


    public void Laddermove()
    {
        float y = Input.GetAxis("Vertical"); // vertical �Է��� �ϸ� y ���� �޴´�.

        Vector3 ladderDir = new Vector3(0, y, -0.05f);  // ������ y���̴�.

        cc.Move(ladderDir * walkSpeed * Time.deltaTime); // ladderdir ��������  walk���ǵ�� �����δ�.

        gravityPower = Physics.gravity; 

    }


    private void OnTriggerStay(Collider other) // ��ٸ��� ��������� isOnladder �� true �� �ϰ�
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            print("��ٸ��� ��ҽ��ϴ�.");
            isOnLadder = true;
        }
    }

    private void OnTriggerEnter(Collider other)  // �����ۿ� ����
    {

        if (other.CompareTag("Item")) // item �±��� �ݶ��̴��� �浹�ߴٸ�
        {
            Debug.Log("�����ۿ� ��ҽ��ϴ�.");

            collideItem = true; // �����ۿ� ����
            currentItem = other.gameObject; // ���� ����ִ� �������� ���� ������ ������Ʈ�̴�.
            //currentItem = other.gameObject; // ���� �浹 ���� �������� currentitem������ �ֱ�
        }

       
    }

    private void OnTriggerExit(Collider other) // ��ٸ����� ������,  �����ۿ��� ������
    {

        // ��ٸ����� ����������
        if (other.gameObject.CompareTag("Ladder"))
        {
            print("��ٸ����� ���Խ��ϴ�.");
            isOnLadder = false;
            if (isLadder)
            {
                isLadder = false;
                gravityPower = Physics.gravity;
            }

        }

        // �����۰��� �浹�� ����� ���, ���� �������� null�� ����
        if (other.CompareTag("Item")) //&& other.gameObject == currentItem)
        {
            Debug.Log("�����ۿ��� ���������ϴ�.");

            collideItem = false;
            currentItem = null;
            //currentItem = null;
        }

    }






    // ������ �ݱ� ��Ŀ���� 1


    // �÷��̾ item tag�� game object�� �浹�ϰ�

    // �÷��̾ e �� ������ 

    // �������� �������(Destroy)

    // �������� �÷��̾� transform�� �ڽ� ��ġ�� �����ȴ�.

    // �������� player�� �ڽ��� �ڽ� ������Ʈ�� �����(������ �ٴҼ��ְ�)


    public void PickupItem() //������ �ݱ�  
    {
        // // E�� ������ ���� �����ۿ� ����ִٸ� 
        if (Input.GetKeyDown(KeyCode.E) && (collideItem))
        {
            if (currentItem != null) // �׸��� �ֿ� �������� null ���� �ƴ϶��
            {

                if (holdItem == null) // ���� ����ִ� �������� null ���̶��
                {
                    holdItem = Instantiate(currentItem, RightHand.position, RightHand.rotation); //  Player-RightHand.Position�� currentItem �� �����Ѵ�. // ���� �������� Ȧ������� ������ �ִ´�.
                    holdItem.transform.SetParent(RightHand); // player�� righthand�� �ֿ� ������Ʈ�� �θ�� �Ѵ�.


                    Rigidbody rb = holdItem.GetComponent<Rigidbody>(); // item �� rigidbody ������Ʈ�� �����ͼ�
                    if (rb != null)
                    {
                        rb.isKinematic = true; // �������� �ʰ� iskineitc �� üũ
                    }

                    Destroy(currentItem); // ������ �����
                    currentItem = null; // currentItem �ʱ�ȭ
                    Debug.Log("�������� �ֿ����ϴ�.");

                }
            }
        }

    }

    public void DropItem() // ������ ������
    {

        if (Input.GetKeyDown(KeyCode.G) && holdItem != null)  // G�� ������ ��� �ִ� �������� null�� �ƴ϶��
        {
            Debug.Log("GŰ�� �Է���ϴ�.");

            holdItem.transform.SetParent(null); // Righthand���� ���
            Vector3 dropPos = transform.position + transform.forward * 3f; // �÷��̾��� ���� 3f�� �Ÿ��տ��� ����߸� ������ ����
            holdItem.transform.position = dropPos; // hold �������� ��ġ�� droppos�� �̵�



            Rigidbody rb = holdItem.GetComponent<Rigidbody>(); // ���߿��� ������ �������� ����߸��� ���� item �� rigidbody�� �߰�
            if (rb != null)
            {
                rb.isKinematic = false; // �߷��� �ۿ��� �ޱ� ���� iskinetic�� üũ����
            }
            else
            {
                Debug.Log("Rigidbody �� �����ϴ�.");
            }

            holdItem = null; // hold�������� �ʱ�ȭ

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("G Ű�� �������� holdItem�� null�Դϴ�.");
            }
        }

    }
}



    // ������ �ݱ� ��Ŀ���� 2 - �κ��丮�� ����, ������ Gameobject�� ���� Inventory�� ����Ѵ�.

    // holditem != null �̰�
    // inventory[i] == null �̶��
    // inventory[i] �� �����ϰ�
    // holditem �� Destroy �Ѵ�
    // holdItem == null �� �ʱ�ȭ �Ѵ�.

  

    






    //������ �ݱ�
    //public void ItemPickUp()
    //{
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















