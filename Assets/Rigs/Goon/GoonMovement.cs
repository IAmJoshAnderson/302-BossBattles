using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // don't end up with runtime errors
public class GoonMovement : MonoBehaviour
{


    enum Mode
    {
        Idle,
        Walk,
        InAir,
    }

    public FootRaycast footLeft;
    public FootRaycast footRight;

    public float speed = 2;
    public float footSeparateAmount = .2f;
    public float walkSpreadX = .2f;
    public float walkSpreadY = .4f;
    public float walkSpreadZ = .8f;
    public float walkFootSpeed = 4;



    private CharacterController pawn;

    //meters per second, these are only made to allow for jumping.
    float velocityY = 0;
    public float gravity = 40;
    public float jumpImpulse = 10;

    private Mode mode = Mode.Idle;
    private Vector3 input;
    private float walkTime = 0;

    private Camera cam;

    private Quaternion targetRotation;


    void Start()
    {
        pawn = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = Vector3.Cross(Vector3.up, camForward);

        input = camForward * v + camRight * h; // effects left or right movement too
        if (input.sqrMagnitude > 1) input.Normalize();
        // set movement mode based on movement input:
        float threshold = .1f;
        mode = (input.sqrMagnitude > threshold * threshold) ? Mode.Walk : Mode.Idle;

        if (mode == Mode.Walk) targetRotation = Quaternion.LookRotation(input, Vector3.up); // turning the player back and forth, no strafing.

        velocityY += gravity * Time.deltaTime;

        pawn.Move((input * speed + Vector3.down * velocityY) * Time.deltaTime); //pawn.SimpleMove(Input * speed) is for no jump

       // if (pawn.isGrounded)
        //{
            //if (Input.GetButtonDown("Jump"))
            //{
                //velocityY = -jumpImpulse;
            //}
        //}


        // animate the feet
        Animate();
    }

    void Animate() // the state machine to switch between different procedural animations
    {
        transform.rotation = AnimMath.Ease(transform.rotation, targetRotation, .01f); // ease into the player rotation? If we're not strafing.

        switch(mode)
        {
            case Mode.Idle:
                AnimateIdle();
                break;
            case Mode.Walk:
                AnimateWalk();
                break;
            case Mode.InAir:
                AnimateInAir();
                break;
        }
    }
    void AnimateInAir()
    {
        //TODO
        // lift legs
        // lift hands
        // use vertical velocity
    }

    void AnimateIdle()
    {
        footLeft.SetPositionHome();
        footRight.SetPositionHome();
    }

    delegate void MoveFoot(float time, FootRaycast foot); // A valid data type to store matching delegates
    void AnimateWalk()
    {
        // Don't repeat yourself

        MoveFoot moveFoot = (t, foot) => { // is called on both feet at the same time, so we don't need seperate lines

            float y = Mathf.Cos(t) * walkSpreadY; // vertical movement

            // lateral movement
            float lateral = Mathf.Sin(t) * walkSpreadZ;

            foot.transform.InverseTransformDirection(input);

            Vector3 localDir = foot.transform.parent.InverseTransformDirection(input);

            float x = lateral * localDir.x;
            float z = lateral * localDir.z;

            //exclusively for strafing, which cannot work if we're turning the character.
            float alignment = Mathf.Abs(Vector3.Dot(localDir, Vector3.forward));
            // 1 = forward and backward, 0 equals strafing

            if (y < 0) y = 0; // so the foot doesn't hit into the ground

            foot.SetPositionOffset(new Vector3(x, y, z), footSeparateAmount * alignment);
        };

        walkTime += Time.deltaTime * input.sqrMagnitude * walkFootSpeed;

        moveFoot.Invoke(walkTime, footLeft) ;
        moveFoot.Invoke(walkTime + Mathf.PI, footRight) ; // offset of the left foot, make it a half circle behind.
    }
}
