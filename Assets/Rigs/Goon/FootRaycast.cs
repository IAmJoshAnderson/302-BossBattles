using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootRaycast : MonoBehaviour
{

    public float raycastLength = 2;
    private float distanceBetweenGroundAndIK;
    private Quaternion startingRot; // has to be local rotation to the player, since the player can influence its rotation. If it's global, the feet will never rotate.
    

    /// <summary>
    /// The world-space position of the ground above/below the foot IK.
    /// </summary>
    // Foot Movement
    private Vector3 groundPosition;
    /// <summary>
    /// The world-space rotation for the foot to be aligned with the ground.
    /// </summary>
    private Quaternion groundRotation;

    void Start()
    {
        startingRot = transform.localRotation;
        distanceBetweenGroundAndIK = transform.localPosition.y;
    }

    void Update()
    {
        //FindGround();
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
            groundPosition = hitInfo.point + Vector3.up * distanceBetweenGroundAndIK; // change the position of the IK constraint

            //Rotation
            Quaternion worldNeutral = transform.parent.rotation * startingRot; // turns back from local into global for the fromtorotation function
            groundRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * worldNeutral;

        }
    }
}
