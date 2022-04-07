using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionStickyFeet : MonoBehaviour
{

    public Transform rayCastSource;

    public float maxDistanceBeforeMove = 1;

    public AnimationCurve curveStepVertical;

    public float raycastLength = 2;

    private Quaternion startingRotation;

    private Vector3 previousGroundPosition;

    private Vector3 groundPosition;

    private Quaternion previousGroundRotation;

    private Quaternion groundRotation;

    private float animationLength = .25f;

    private float animationCurrentTime = 0;

    private bool isAnimating
    {
        get
        {
            return (animationCurrentTime < animationLength);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.localPosition;
        startingRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimating)
        {
            animationCurrentTime += Time.deltaTime;
            float p = Mathf.Clamp(animationCurrentTime / animationLength, 0, 1);

            float y = curveStepVertical.Evaluate(p);

            // move position
            transform.position = AnimMath.Lerp(previousGroundPosition, groundPosition, p) + new Vector3(0, y, 0);


            //rotation
            transform.rotation = AnimMath.Lerp(previousGroundRotation, groundRotation, p);
        }
        else
        {
            //keep feet planted
            transform.position = groundPosition;
            transform.rotation = groundRotation;

            //check distance to starting position, trigger animation
            Vector3 vToStarting = transform.position - transform.parent.TransformPoint(startingPosition);
            if (vToStarting.sqrMagnitude > maxDistanceBeforeMove * maxDistanceBeforeMove)
            {
                FindGround();
            }
        }
    }
    private void FindGround()
    {
        Vector3 origin = rayCastSource.position + Vector3.up * raycastLength / 2;
        Vector3 direction = Vector3.down;

        // Draw the Raycast
        Debug.DrawRay(origin, direction * raycastLength, Color.blue); // direction is the magnitude of the ray, so it's multiplied by maxDistance.



        if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, raycastLength)) // if we hit the ground with the raycast
        {
            //prepare animation values
            animationCurrentTime = 0;
            previousGroundPosition = groundPosition;
                previousGroundRotation = groundRotation;

            //finds ground position
            groundPosition = hitInfo.point;

            // convert starting 
            Quaternion worldNeutral = transform.parent.rotation * startingRotation;

            groundRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * worldNeutral;


        }
    }
}
