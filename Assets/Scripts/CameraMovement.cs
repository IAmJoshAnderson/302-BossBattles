using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform target;

    public Transform fpsPosition;

    public Vector3 targetOffset;

    private Camera cam;

    float pitch = 0;
    float yaw = 0;

    public float lookSensitivityX = 1;

    public Vector3 transition { get; private set; }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
    }

    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        yaw += mx * lookSensitivityX;

        pitch = Mathf.Clamp(pitch, 0, 30);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        if (target != null) // references camera
        {
            transform.position = AnimMath.Ease(transform.position, target.position + targetOffset, .01f);
        }
       // new Vector3 transition = AnimMath.Lerp(cam.transform.position, fpsPosition.position, 0);
    }
    private void OnDrawGizmos()
    {
        if (!cam) cam = GetComponentInChildren<Camera>();
        if (!cam) return;

        Gizmos.DrawWireCube(transform.position, Vector3.one / 2);
        Gizmos.DrawLine(transform.position, cam.transform.position);
    }
}
