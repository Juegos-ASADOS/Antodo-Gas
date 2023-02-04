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
    float distanceTravelled;
    float acumRot = 0;

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
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            transform.Rotate(0, 0, acumRot);

            if (Input.GetKey(KeyCode.D)) acumRot += 1;
            if (Input.GetKey(KeyCode.A)) acumRot -= 1;

            transform.position = transform.position + transform.up * offset;
        }

        if (Input.GetKey(KeyCode.W)) speed = 15;


    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
