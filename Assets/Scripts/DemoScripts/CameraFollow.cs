using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, pos, smoothSpeed);

        transform.position = smoothPos;

        transform.LookAt(target);
    }
}
