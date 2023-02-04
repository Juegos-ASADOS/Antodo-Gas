using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class DemoCameraBezier : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed = 5;
    public float offset = 5;
    public float pinchoPunch = 200;
    public float acelSpeed = 15;
    public float baseSpeed = 5;

    float distanceTravelled;
    float acumRot = 0;
    float lateralAcceleration = 0;

    void Start()
    {
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
        }
    }

    void Update()
    {
        if (pathCreator != null)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.JoystickButton0)) speed = acelSpeed;
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.JoystickButton0)) speed = baseSpeed;

            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) acumRot += 1;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) acumRot -= 1;

            acumRot -= Input.GetAxis("Horizontal");

            acumRot += lateralAcceleration;
            transform.Rotate(0, 0, acumRot);
            transform.position = transform.position + transform.up * offset;

            if (Mathf.Abs(lateralAcceleration) > 0)
            {
                if(lateralAcceleration < 0) lateralAcceleration += 0.1f;
                else lateralAcceleration -= 0.1f;
            }         
            else
                lateralAcceleration = 0;
        }
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        PinchoTrigger pincho = other.GetComponent<PinchoTrigger>();

        if (pincho != null)
        {
            if (pincho.derecha) lateralAcceleration = -pinchoPunch;
            else lateralAcceleration = pinchoPunch;
        }
    }
}
