using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GuardianMovement : MonoBehaviour
{
    public float walkSpeed = 5; //scaler

    private Animator animator; // holds the animator reference

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical") ; // vertical axis

        transform.position += transform.forward * v * Time.deltaTime * walkSpeed; // pushing forward one meter, times how much the vertical axis is pushed. Time.DeltaTime is for meters per second, not meters per tick.

        animator.SetFloat("speed", Mathf.Abs(v * walkSpeed)); // set speed to be multiplied by the vertical axis and the walk speed. get the absolute value on account for walking backward.

    }
}
