using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;
public class ShakeCam : MonoBehaviour
{
    public ShakeData shakeData;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            CameraShakerHandler.Shake(shakeData);
        }    
    }
}
