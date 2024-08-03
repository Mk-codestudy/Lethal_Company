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
    public float currentSpeed = 20f; // ���� �ȴ�/�ٴ� �ӵ�
    public float rotSpeed = 300f;


    // ĳ���� ü��, ���¹̳� 
    public float maxHp; // �׹� ������ ����
    public float maxStamina = 100; // lerp����
    public float currentHp;
    public float currentStamina;
    // ü�� ���¹̳� �����̴�
    public Image hpBar;
    public Slider staminaSlider;

    bool isregenStamina = true;



    public Vector3 moveDir;


    Vector3 gravityPower; // �߷��� vector���̴� ( x, y, z)

    

    public float jumpHeight;
    public float maxJump = 1f;

    float yPos; // 
    public float gravityVelocity = 100f; // ���ϼӵ�

    float rotX; // x�� ���콺 ȸ�� �ӵ����� ���� ����
    float rotY; // 

    bool isMoving;
    bool isRunning;
    //bool isCrouch = false; // ���� ���� x
    bool isIdle;
    bool isLadder = false; // ��ٸ� �������� �ƴ���


    private Inventory playerInventory;  // �κ��丮 Ŭ���� 
    public Item item;

    CharacterController cc;
    Animator animator;
    





    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; // Ŀ�� ���α�

        cc = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();

        gravityPower = Physics.gravity;  // �߷� �ʱ�ȭ 

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

    void Move() // ĳ���� �ȱ�, �޸���, ����, ���̱�(Crouch)
    {
        

        // ���� ���ͷ� �����̱�
        x = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 movedir = new Vector3(x, 0f, v);

        movedir = transform.TransformDirection(movedir); // ���� ��ǥ �̵�
        movedir.Normalize();



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



        // ĳ���� ����

        yPos += gravityPower.y * gravityVelocity * Time.deltaTime; // �߷��� y�ప * �߷¼ӵ� * �ð� ����
                                                                  // ��� ĳ���Ϳ��� y�� �Ʒ��� ����Ǵ� ��.


        if (cc.collisionFlags == CollisionFlags.CollidedBelow)
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

        Camera.main.transform.GetComponent<Follow_Cam>().rotX = rotX; // ī�޶� �ִ� followcam�� ��ũ��Ʈ�� rotx ������ �޾ƿ�


    }
     


    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Item"))
        {

        }
    }

    
        

 
    private void UseStamina(int amount)
    {
        currentStamina -= amount * Time.deltaTime; // ���¹̳� ����

        if(currentStamina <= 0 )
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
        else if(currentStamina <= 0) // ���� ���¹̳ʰ� 0�̰ų� 0���� ���ٸ�
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


    private void UpdateUI()  // HP stamina �����̴��� �� �����Ӹ��� ������Ʈ,  Mathf.Lerp�� ����ؼ� �����̴��� �ε巴�� 
    {
        // ���¹̳� �����̵�

        float targetStamina = currentStamina / maxStamina; // Ÿ�� ���¹̳� �������� max���¹̳� ������ currentstamina���� ����
       

        staminaSlider.value = Mathf.Lerp(currentStamina, targetStamina, Time.deltaTime * 0.5f);

        // ���� �ε巴�� �����ǰ� �����ʿ�
        //staminaSlider.value = Mathf.Lerp(staminaSlider.value, currentStamina / maxStamina, Time.deltaTime * 10f);
        //hpSlider.value = currentHp;



        // ü�� ĭ
        #region ü��ĭ

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
            hpBarAlpha.a = 1f; // ü����  �϶� hp�̹����� ������ 1
        }

    }
    #endregion


    //��ٸ��� �����Ѵ�
    //�÷��̾ Ladder Tag�� ���� Collider�� stay �Ұ��]
    //�÷��̾��� z,x�� �������� �����ϰ�
    // w, s�� �������� �� �� transform.up , -transform.up �� �������� walkSpeed �� �̵��Ѵ�.

   

  

    // ��ٸ� �ȿ����� �������θ� �����̰� ����
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
            print("��ٸ��� ��ҽ��ϴ�.");
            isLadder = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            print("��ٸ����� ���Խ��ϴ�.");
            isLadder = false;
        }
    }




}






