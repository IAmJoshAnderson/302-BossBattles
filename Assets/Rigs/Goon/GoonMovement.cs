using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // don't end up with runtime errors
public class GoonMovement : MonoBehaviour
{

    public FootRaycast footLeft;
    public FootRaycast footRight;

    public float speed = 2;
    public float walkSpreadX = .2f;
    public float walkSpreadY = .4f;
    public float walkSpreadZ = .8f;
    public float walkFootSpeed = 4;


    private CharacterController pawn;

    void Start()
    {
        pawn = GetComponent<CharacterController>();
    }


    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 move = transform.forward * v + transform.right * h; // effects left or right movement too
        if (move.sqrMagnitude > 1) move.Normalize();


        pawn.SimpleMove(move * speed);

        // animate the feet
        AnimateWalk();
    }


    delegate void MoveFoot(float time, float x, FootRaycast foot); // A valid data type to store matching delegates
    void AnimateWalk()
    {
        // Don't repeat yourself

        MoveFoot moveFoot = (t, x, foot) => { // is called on both feet at the same time, so we don't need seperate lines

            float y = Mathf.Cos(t) * walkSpreadY;
            float z = Mathf.Sin(t) * walkSpreadZ; //oscillates back and forth with time. Controls the lift back and forth of the feet.

            if (y < 0) y = 0; // so the foot doesn't hit into the ground

            y += .1687f;

            foot.transform.localPosition = new Vector3(x, y, z);


        };
        float t = Time.time * walkFootSpeed;

        moveFoot.Invoke(t, -walkSpreadX, footLeft) ;
        moveFoot.Invoke(t + Mathf.PI, walkSpreadX, footRight) ; // offset of the left foot, make it a half circle behind.
    }

}
