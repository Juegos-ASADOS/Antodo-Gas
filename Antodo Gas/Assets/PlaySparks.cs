using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySparks : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<DemoCameraBezier>() != null)
        {
            GetComponent<ParticleSystem>().Play();
        }
    }
}
