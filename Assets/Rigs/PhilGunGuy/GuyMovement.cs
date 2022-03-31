using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GuyMovement : MonoBehaviour
{

    public float speed = 2;
    public LayerMask layerMask;
    private CharacterController pawn;
    public float walkSpeed = 5;

    private Animator animator;

    [Range (0, 1f)]
    public float DistanceToGround;


    void Start()
    {
        animator = GetComponent<Animator>();
        pawn = GetComponent<CharacterController>();
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 move = transform.forward * v + transform.right * h;

        if (move.sqrMagnitude > 1) move.Normalize();
        pawn.SimpleMove(move * speed);

        transform.position += transform.forward * v * Time.deltaTime * walkSpeed;
        transform.position += transform.right * h * Time.deltaTime * walkSpeed;
        animator.SetFloat("speed", Mathf.Abs(v * walkSpeed));

    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            // This applies the ability to move the foot in the animator tab. 0 means not at all, 1 means all
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IKLeftFootWeight"));
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IKLeftFootWeight"));
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IKRightFootWeight"));
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IKRightFootWeight"));

            //Applying the actual movement to the left foot
            RaycastHit hit;
            Ray ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistanceToGround +1f, layerMask)) // the "1" value may change if the leg is bent a certain way in the idle
            {
                if (hit.transform.tag == "Walkable")
                {
                    Vector3 footPosition = hit.point; //hit.point is where the raycast hits
                    footPosition.y += DistanceToGround;
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                    Vector3 forward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(forward, hit.normal));
                    //animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }



                // This applies the ability to move the foot in the animator tab. 0 means not at all, 1 means all

                //Now the movement of the right foot
                 ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
                if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layerMask)) // the "1" value may change if the leg is bent a certain way in the idle
                {
                    if (hit.transform.tag == "Walkable")
                    {
                        Vector3 footPosition = hit.point; //hit.point is where the raycast hits
                        footPosition.y += DistanceToGround;
                        animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
                        animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(forward, hit.normal));
                        //animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                    }
                }

            }

        }
    }
}
