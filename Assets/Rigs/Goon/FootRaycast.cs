using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootRaycast : MonoBehaviour
{

    public float raycastLength = 2; // Total length of the raycast in meters

    /// <summary>
    /// The local-space position of where the IK spawned
    /// </summary>

    private Vector3 startingPosition;
    /// <summary>
    /// The local-space rotation of where the IK spawned
    /// </summary>
    private Quaternion startingRotation; // has to be local rotation to the player, since the player can influence its rotation. If it's global, the feet will never rotate.
    /// <summary>
    /// The world-space position of the ground above/below the foot IK.
    /// </summary>
    // Foot Movement
    private Vector3 groundPosition;
    /// <summary>
    /// The world-space rotation for the foot to be aligned with the ground.
    /// </summary>
    private Quaternion groundRotation;

    /// <summary>
    /// This allows us to ease the position!
    /// </summary>
    private Vector3 targetPosition;
    private Vector3 footSeparateDir;

    void Start()
    {
        startingRotation = transform.localRotation;
        startingPosition = transform.localPosition;

        footSeparateDir = (startingPosition.x > 0) ? Vector3.right : Vector3.left;
    }

    void Update()
    {
        //FindGround();

        // ease towards target:
        transform.localPosition = AnimMath.Ease(transform.localPosition, targetPosition, .01f);
    }

    public void SetPositionLocal(Vector3 p)
    {
        targetPosition = p;
    }
    public void SetPositionHome()
    {
        targetPosition = startingPosition;
    }

    public void SetPositionOffset(Vector3 p, float separateAmount = 0)
    {
        targetPosition = startingPosition + p + separateAmount * footSeparateDir;
    }


    private void FindGround()
    {
        Vector3 origin = transform.position + Vector3.up * raycastLength / 2; //Directly on the red square of the foot, plus moving up in the y axis one.
        Vector3 direction = Vector3.down;
        RaycastHit hitInfo = default;

        // Draw the Raycast
        Debug.DrawRay(origin, direction * raycastLength, Color.blue); // direction is the magnitude of the ray, so it's multiplied by maxDistance.



        if (Physics.Raycast(origin, direction, out hitInfo, raycastLength)) // if we hit the ground with the raycast
        {
            groundPosition = hitInfo.point + Vector3.up * startingPosition.y; // change the position of the IK constraint

            //Rotation
            Quaternion worldNeutral = transform.parent.rotation * startingRotation; // turns back from local into global for the fromtorotation function
            groundRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * worldNeutral;

        }
    }
}
