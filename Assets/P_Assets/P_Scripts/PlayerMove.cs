using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    public float x;
    public float v;

    public float walkSpeed = 5f;
    public float runSpeed = 15f;
    public float currentSpeed = 20f; // ���� �ȴ�/�ٴ� �ӵ�
    public float rotSpeed = 200f;

    bool isCrouch = false; // ���� ���� x
    bool isRun; // �޸��� �������� �ƴ���

    public Vector3 moveDir;


    Vector3 gravityPower; // �߷��� vector���̴� ( x, y, z)


    public float jumpHeight;
    public float maxJump = 1f;
           
    float yPos; // 
    public float gravityVelocity = 100f; // ���ϼӵ�

    float rotX; // x�� ���콺 ȸ�� �ӵ����� ���� ����
    float rotY; // 

    CharacterController cc;

    Animator animator; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; // Ŀ�� ���α�

       
        cc = GetComponent<CharacterController>();
               
        gravityPower = Physics.gravity;  // �߷� �ʱ�ȭ 



    }

    void Update()
    {
        Rotate();

        Move();
    }

    void Move()
    {
        // ���� ���ͷ� �����̱�
        x = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 movedir = new Vector3(x, 0f, v);

        movedir = transform.TransformDirection(movedir); // ���� ��ǥ�� �̵�
        movedir.Normalize();


        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;

            //animator.SetBool("Runanime", true);
        }
        else
        {
            currentSpeed = walkSpeed;

            //animator.SetBool("Walkanime", false);
        }


        // ĳ���� ����

        yPos += gravityPower.y * gravityVelocity * Time.deltaTime; // �߷��� y�ప * �߷¼ӵ� * �ð� ����
        // ��� ĳ���Ϳ��� y�� �Ʒ��� ����Ǵ� ��.


        if (cc.collisionFlags == CollisionFlags.CollidedBelow)
        {
            yPos = 0; // ���ٴڿ� ������ y�� ypos�� �����Ǵ� �߷°��� 0;
            maxJump = 1;
        }

        if (Input.GetButtonDown("Jump") && maxJump > 0)  // �����̽��� ������ �����Ѵ�
        {
            yPos = jumpHeight; // ������ ����Ǵ� ���� ���� ���� ���̷� ġȯ
            maxJump--;
        }

        movedir.y = yPos;

        cc.Move(movedir * currentSpeed * Time.deltaTime);





        if(Input.GetKeyDown(KeyCode.C))
        {
            Crouch();
        }
    }


    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotX += mouseY * rotSpeed * Time.deltaTime; // �¿�� �����϶� ���� y�� �ְ�
        rotY += mouseX * rotSpeed * Time.deltaTime; // ���Ϸ� �����϶� ���� x�� �ִ�.

        // ���� ȸ������ 60���� ����

        if(rotX > 60) // rotx�� 60�� ũ�ٸ� 60���� (Maximum)
        {
            rotX = 60;
        }
        else if(rotX < -60) // rotx�� -60�� ���� �۴ٸ� -60���� (Maximum)
        {
            rotX = -60;
        }

        transform.eulerAngles = new Vector3(0 , rotY, 0f);

        Camera.main.transform.GetComponent<Follow_Cam>().rotX = rotX;


    }

    void Crouch()
    {
        isCrouch = !isCrouch;
        if(isCrouch)
        {
            // �ɾ����� �ݶ��̴� ũ�� , ���� ���߱�
        }
        if(!isCrouch)
        {
            // �������� �ݶ��̴� ũ�� , ���� ���߱�
        }
    }
   
}






