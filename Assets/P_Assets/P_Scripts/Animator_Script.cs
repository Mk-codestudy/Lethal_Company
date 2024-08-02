using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_Script : MonoBehaviour
{
    public float x;
    public float v;

    public float walkSpeed = 5f;
    public float runSpeed = 15f;
    public float currentSpeed = 20f; // ÇöÀç °È´Â/¶Ù´Â ¼Óµµ
    public float rotSpeed = 300f;

    Animator animator;

    bool isMoving;
    bool isRunning;
    bool isCrouch = false; // ¾ÉÀº »óÅÂ x
    bool isIdle;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (isMoving && isRunning)
        {
            currentSpeed = runSpeed;

            animator.SetBool("Run", true);
            animator.SetBool("Walk", false);

        }
        else if (isMoving)
        {
            currentSpeed = walkSpeed;


            animator.SetBool("Walk", true);
            animator.SetBool("Run", false);

        }
        else
        {
            currentSpeed = 0;

            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);

        }

    }





}
