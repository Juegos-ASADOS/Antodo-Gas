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
    public float aceleration = 0.2f;
    public float maxAce = 0.2f;

    float distanceTravelled;
    float acumRot = 0;
    float lateralAcceleration = 0;


    private bool lerping = false;
    private float lerpStartTime = 0;
    bool colision = false;
    float basePov;
    float distanceTraveledBeforeExit = 0;   //Para saber en que momento salto de raiz

    PhotonView view;

    public int acumulatedInput = 0; //Para las colisiones
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


    void normalMove()
    {

        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) acumRot += 1; acumulatedInput++;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) acumRot -= 1; acumulatedInput--;

        acumRot -= Input.GetAxis("Horizontal");

        acumRot += lateralAcceleration;
        transform.Rotate(0, 0, acumRot);
        transform.position = transform.position + transform.up * offset;

        if (Mathf.Abs(lateralAcceleration) > 0)
        {
            if (lateralAcceleration < 0) lateralAcceleration += 0.1f;
            else lateralAcceleration -= 0.1f;
        }
        else
            lateralAcceleration = 0;

        if (speed > baseSpeed)
        {
            //Debug.Log(speed);
            speed -= deceleration;
        }

        cam.fieldOfView = basePov + speed;
    }

    void lerpingMove()
    {

        distanceTravelled += speed * Time.deltaTime;
        Vector3 final = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        Quaternion finRot = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

        if (Mathf.Abs(lateralAcceleration) > 0)
        {
            if (lateralAcceleration < 0) lateralAcceleration += 0.1f;
            else lateralAcceleration -= 0.1f;
        }
        else
            lateralAcceleration = 0;


        GameObject aux = new GameObject();
        aux.transform.position = final;
        aux.transform.rotation = finRot;


        //Debug.Log(acumRot);
        acumRot += lateralAcceleration;
        aux.transform.Rotate(0, 0, acumRot);
        //transform.Rotate(0, 0, acumRot);
        transform.position = Vector3.Lerp(transform.position, aux.transform.position + aux.transform.up * offset, (1 - lerpStartTime));
        transform.rotation = Quaternion.Lerp(transform.rotation, aux.transform.rotation, (1 - lerpStartTime));
        //transform.position + transform.up * offset;


        Destroy(aux);
        if (Mathf.Abs(lateralAcceleration) > 0)
        {
            if (lateralAcceleration < 0) lateralAcceleration += 0.1f;
            else lateralAcceleration -= 0.1f;
        }
        else
            lateralAcceleration = 0;

        if (speed > baseSpeed)
        {
            //Debug.Log(speed);
            speed -= deceleration;
        }

        cam.fieldOfView = basePov + speed;

        if (lerpStartTime <= 0)
        {
            lerping = false;
        }
        else
        {
            lerpStartTime -= Time.deltaTime;
        }

    }

    void Update()
    {
        if (view.IsMine && pathCreator != null)
        {
            //Debug.Log("hola!");

            if (speed < maxAce && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.JoystickButton0)) speed += aceleration;
            if (speed > baseSpeed && Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.JoystickButton1)) speed -= deceleration;
            if (Input.GetKeyDown(KeyCode.Z) && jumpRoot != null && jumpRoot != pathCreator)
            {
                Debug.Log("saltado!");
                pathCreator = jumpRoot;
                //OnPathChanged();
                distanceTravelled = jumpRoot.path.GetClosestDistanceAlongPath(transform.position);
                lerping = true;
                lerpStartTime = 1;

                jumpRoot = null;
            }

            if (!lerping)
                normalMove();
            else
                lerpingMove();
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

        //Debug.Log("hay un trigger");
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
                //distanceTraveledBeforeExit = distanceTravelled;
                jumpRoot = sal.GetComponentInParent<PathCreator>();


                //a pelo pecho


                //Debug.Log("saltado!");
                //pathCreator = jumpRoot;
                //OnPathChanged();
                ////distanceTravelled = distanceTravelled - distanceTraveledBeforeExit;
                //lerping = true;
                //lerpStartTime = 1;

                //jumpRoot = null;



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
                Debug.Log("bufo");
                speed = rebSpeed;
            }
            else if (reb != null) colision = false;

            Salida sal = other.GetComponent<Salida>();
            if (sal != null)
            {
                jumpRoot = null;
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        DemoCameraBezier otherPlayer = collision.gameObject.GetComponent<DemoCameraBezier>();

        if (otherPlayer != null)
        {
            if (Mathf.Abs(acumulatedInput) > Mathf.Abs(otherPlayer.acumulatedInput))
            {
                if (acumulatedInput > 0) lateralAcceleration -= 10;
                else lateralAcceleration += 10;

            }
            else
            {
                if (acumulatedInput > 0) lateralAcceleration -= 50;
                else lateralAcceleration += 50;
            }


        }
    }
}
