using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{

    enum Mode
    {
        Idle,
        WalkSide,
        WalkForward,
        JumptoPlayer,
    }


    //The Gun Guy
    public Transform player;
    public Transform playerHeight; // a game object above the player

    //The variables for attacks to change.
    Animator bossAnimations;

    //FOV pieces
    public float radius;
    [Range (0, 360)]
    public float angle;
    private bool isJumping; // keep track of the jumping state
    [Range (0, 10)]
    public float heightLimit; // the height of which the cube goes up, max 10

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public float speed; // the speed at which the slime moves during its jump
    private Quaternion targetRotation;
    private Mode mode = Mode.Idle;
    private CharacterController pawn;

    // CanAttack
    public bool canSeePlayer;

    private float gravity = 40;

    // Start is called before the first frame update
    private void Start()
    {

        bossAnimations = GetComponent<Animator>();
        StartCoroutine(FOVRoutine());
        pawn = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        AttackStates();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            bossAnimations.SetBool("Jump", true);
        }
    }

    private void AttackStates()
    {
        transform.rotation = AnimMath.Ease(transform.rotation, targetRotation, .01f);

        switch (mode)
        {
            case Mode.Idle:
                break;
            case Mode.WalkSide:
                break;
        }
    }

    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    if (isJumping != true)
                    canSeePlayer = true;
                    bossAnimations.SetBool("Proximity", true); // canSeePlayer and Proximity are the same
                    
                }
                else // this should be when the slime jumps to the player!
                {
                    canSeePlayer = false;
                    bossAnimations.SetBool("Proximity", false);
                }
            }
            else
            {
                canSeePlayer = false;
                bossAnimations.SetBool("Proximity", false);
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
            bossAnimations.SetBool("Proximity", false);
            // now we are jumping, not in proximity
            isJumping = true;
            bossAnimations.SetBool("Jump", true);
        }
        if (isJumping == true)
        {
            Vector3 a = transform.position;
            Vector3 b = playerHeight.position;
            if (a.y <= heightLimit)
            transform.position = Vector3.Lerp(a, b, Mathf.SmoothStep(0, 1, speed));
            if (a.y >= heightLimit)
            {
                isJumping = false;
                bossAnimations.SetBool("Jump", false);
            }
        }
    }
    void Attract()
    {

    }
}
