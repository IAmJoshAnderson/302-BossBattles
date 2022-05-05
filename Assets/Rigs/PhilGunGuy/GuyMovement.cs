using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GuyMovement : MonoBehaviour
{

    enum Mode
    {
        Idle,
        Walk,
    }


    public float speed = 2;
    private CharacterController pawn;
    public float walkSpeed = 1;

    //falling
    float velocityY = 0;
    public float gravity = 40;

    private Quaternion targetRotation;
    private Mode mode = Mode.Idle;
    private Vector3 input;
    public float turnRate;

    private Camera cam;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        pawn = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    void Update()
    {
        //Upon releasing WADS keys, imput goes from 1 to 0, resetting rotation?
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();
        Vector3 camRight = Vector3.Cross(Vector3.up, camForward);

        float threshold = .1f;

        input = camForward * v + camRight * h;

        mode = (input.sqrMagnitude > threshold * threshold) ? Mode.Walk : Mode.Idle;

        // this makes his body turn
        // this bit of code seems to also be a culprit
        if (mode == Mode.Walk) targetRotation = Quaternion.LookRotation(input, Vector3.up);

        Vector2 getAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        transform.Rotate(getAxis.x * turnRate * Time.deltaTime, getAxis.y * turnRate * Time.deltaTime, 0, Space.Self);
        


        if (input.sqrMagnitude > 1) input.Normalize();
        pawn.SimpleMove(input * speed);

        transform.position += transform.forward * v * Time.deltaTime * walkSpeed;
        transform.position += transform.right * h * Time.deltaTime * walkSpeed;

        //Switch to walking
        animator.SetFloat("speed", Mathf.Abs(v * walkSpeed));

        if (walkSpeed >= .3f)
        {
            mode = Mode.Walk;
        }
        else
        {
            mode = Mode.Idle;
        }
        AnimateStates();
        velocityY += gravity * Time.deltaTime;
        if (pawn.isGrounded)
        {
            velocityY = 0;
        }
    }

    void AnimateStates()
    {
        transform.rotation = AnimMath.Ease(transform.rotation, targetRotation, .01f);

        switch(mode)
        {
            case Mode.Idle:
                break;
            case Mode.Walk:
                break;
        }
    }
}
