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
    public float currentSpeed = 20f; // 현재 걷는/뛰는 속도

    public float rotSpeed = 200f;

    bool isCrouch = false; // 앉기 상태

    public Vector3 moveDir;


    Vector3 gravityPower; // 중력은 vector값이다 ( x, y, z)


    public float jumpHeight;
           
    float yPos; // 
    public float gravityVelocity = 100f; // 낙하속도

    float rotX; // x축 마우스 회전 속도값을 담을 변수
    float rotY; // 

    CharacterController cc;

    Animator animator; 

    void Start()
    {
        cc = GetComponent<CharacterController>();
               
        gravityPower = Physics.gravity;  // 중력 초기화 

    }

    void Update()
    {
        Rotate();

        Move();
    }

    void Move()
    {
        // 로컬 벡터로 움직이기
        x = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 movedir = new Vector3(x, 0f, v);

        movedir = transform.TransformDirection(movedir); // 로컬 좌표로 이동
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


        // 캐릭터 점프

        yPos += gravityPower.y * gravityVelocity * Time.deltaTime; // 중력의 y축값 * 중력속도 * 시간 보간


        if (cc.collisionFlags == CollisionFlags.CollidedBelow)
        {
            yPos = 0; // 땅바닥에 닿으면 y축 ypos에 누적되는 중력값은 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            yPos = jumpHeight;
        }

        movedir.y = yPos;

        cc.Move(movedir * currentSpeed * Time.deltaTime);
    }


    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotX += mouseY * rotSpeed * Time.deltaTime; // 좌우로 움직일때 축은 y에 있고
        rotY += mouseX * rotSpeed * Time.deltaTime; // 상하로 움직일때 축은 x에 있다.

        // 상하 회전각을 60도로 제한

        if(rotX > 60) // rotx가 60도 크다면 60도로 (Maximum)
        {
            rotX = 60;
        }
        else if(rotX < -60) // rotx가 -60도 보다 작다면 -60도로 (Maximum)
        {
            rotX = -60;
        }

        transform.eulerAngles = new Vector3(rotX * -1f, rotY, 0f);


    }

    //void Crouch()
    //{
    //    //c 누르면 앉기 유지 isCrouch
    //    if(Input.GetKeyDown(KeyCode.C)) // c를 누르면 앉기 on
    //    {
    //        isCrouch = true;

            

    //    }
    //      cc.Move(moveDir * walkSpeed * Time.deltaTime);
    //}
    
}

   

   


