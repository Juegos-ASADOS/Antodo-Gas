using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;

    // S�lo se tiene en cuenta la Y y la Z.
    // Como en Unity, cuanto m�s Y, m�s arriba del objeto est� la cam
    // Y cuanto m�s Z, m�s por delante del objeto

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
