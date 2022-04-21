using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateMachineAttacks : MonoBehaviour
{

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        print(animator);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            animator.SetBool("leftclick", true);
        }
        else
        {
            animator.SetBool("leftclick", false);
        }
    }
}
