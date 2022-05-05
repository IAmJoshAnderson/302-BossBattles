using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionCube : MonoBehaviour
{
    // This code is made for the cube to detect where the player is and to attack properly. The Cube's AI

    public CubeMovement player;

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
    }
}
