using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public GameObject ThirdCam;

    public GameObject FirstCam;
    public int camMode;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2")){
            camMode = 1;
        }
        else
        {
            camMode = 0;
        }
        StartCoroutine("CamChange");
    }

    IEnumerator CamChange()
    {
        yield return new WaitForSeconds(0.01f);
        if (camMode == 0)
        {
            ThirdCam.SetActive(true);
            FirstCam.SetActive(false);
        }
        if (camMode == 1)
        {
            ThirdCam.SetActive(false);
            FirstCam.SetActive(true);
        }
    }
}
