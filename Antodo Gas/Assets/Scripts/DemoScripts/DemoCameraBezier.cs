using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Photon.Pun;

public class DemoCameraBezier : MonoBehaviour
{
    public PathCreator pathCreator;
    PathCreator jumpRoot;   //La raiz a la que puedo saltar
    public EndOfPathInstruction endOfPathInstruction;
    public Camera cam;

    public float speed = 5;
    public float offset = 5;
    public float pinchoPunch = 200;
    public float acelSpeed = 15;
    public float baseSpeed = 5;
    public float rebSpeed = 25;
    public float deceleration = 0.2f;

    float distanceTravelled;
    float acumRot = 0;
    float lateralAcceleration = 0;
    
    bool colision = false;
    float basePov;
    float distanceTraveledBeforeExit = 0;   //Para saber en que momento salto de raiz
    
    PhotonView view;

    void Start()
    {
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
            basePov = cam.fieldOfView;
        }
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (view.IsMine && pathCreator != null)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.JoystickButton0)) speed = acelSpeed;
            if (Input.GetKey(KeyCode.Z) && jumpRoot != null && jumpRoot != pathCreator)
            {
                pathCreator = jumpRoot;
                distanceTravelled = distanceTravelled - distanceTraveledBeforeExit;
                jumpRoot = null;
            }

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

            if (speed > baseSpeed)
            {
                Debug.Log(speed);
                speed -= deceleration;
            }

            cam.fieldOfView = basePov + speed;
        }
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        if(view.IsMine)
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (view.IsMine)
        {
            PinchoTrigger pincho = other.GetComponent<PinchoTrigger>();
            Salida sal = other.GetComponent<Salida>();

            Debug.Log("salto");
            if (pincho != null)
            {
                if (pincho.derecha) lateralAcceleration = -pinchoPunch;
                else lateralAcceleration = pinchoPunch;

                colision = true;
            }

            if (sal != null)
            {
                Debug.Log("te obligo a asaltar");
                distanceTraveledBeforeExit = distanceTravelled;
                jumpRoot = sal.GetComponentInParent<PathCreator>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (view.IsMine)
        {
            rebufo reb = other.GetComponent<rebufo>();

            if (reb != null && !colision)
            {
                speed = rebSpeed;
            }
            else if (reb != null) colision = false;
        }
    }
}
