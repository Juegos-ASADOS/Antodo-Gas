using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fmod_Collisions : MonoBehaviour
{
    [Header("FMOD Settings")]
    [SerializeField] public EventReference CollisionEventPath;

    [SerializeField] public string CollisionType;                         

    public void playCollision(int collisionType) {
        FMOD.Studio.EventInstance CollideEvent = FMODUnity.RuntimeManager.CreateInstance(CollisionEventPath);
        CollideEvent.setParameterByName(CollisionType, collisionType);
        CollideEvent.start();
        CollideEvent.release();
    }
}
