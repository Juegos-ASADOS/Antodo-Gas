using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;

    // Sólo se tiene en cuenta la Y y la Z.
    // Como en Unity, cuanto más Y, más arriba del objeto está la cam
    // Y cuanto más Z, más por delante del objeto

    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = target.position + target.up * offset.y + target.forward * offset.z;

        Vector3 smoothPos = Vector3.Lerp(transform.position, pos, smoothSpeed);

        transform.position = smoothPos;

        transform.LookAt(target);
    }
}
