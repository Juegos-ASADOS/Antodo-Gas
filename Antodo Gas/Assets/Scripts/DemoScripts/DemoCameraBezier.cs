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

    public float speed = 5;
    public float offset = 5;
    public float pinchoPunch = 200;
    public float acelSpeed = 15;
    public float baseSpeed = 5;
    public float rebSpeed = 25;
    public float deceleration = 0.2f;
    public float aceleration = 0.2f;
    public bool manejable = true;
    public float stunTime = 0.5f;
    public float cd_Lerp = 0.5f;
    float timerLerp = 0.0f;
    public float rayCastRange = 8.0f;

    float distanceTravelled;
    float acumRot = 0;
    float lateralAcceleration = 0;


    private bool lerping = false;
    private float lerpStartTime = 0;
    bool colision = false;
    bool stunned = false;
    float timeStunned = 0;
    float basePov;

    PhotonView view;

    public int acumulatedInput = 0; //Para las colisiones

    //Camera Fov
    public Camera cam;
    float baseFOV;
    public float fovSpeed = 20.0f;
    private float targetFov;
    int levelBoost;
    float baseCamerapos;
    public float offSetCamera;

    float totalDistanceTraveled; //Para controlar quien va primero en la carrera

    void Start()
    {
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
            baseFOV = cam.fieldOfView;
            targetFov = baseFOV;
            levelBoost = 0;
            baseCamerapos = cam.transform.localPosition.z;
        }
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        if (!manejable) distanceTravelled = 50;
    }

    void normalMove()
    {

        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

        totalDistanceTraveled += distanceTravelled;

        if (manejable && !stunned)
        {
            if (speed < acelSpeed && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.JoystickButton0)) speed += aceleration;
            if (speed > baseSpeed && Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.JoystickButton1)) speed -= deceleration;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                acumRot++; 
                acumulatedInput++;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                acumRot--;
                acumulatedInput--;
            }

            acumRot -= Input.GetAxis("Horizontal");
        }

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
        else 
            levelBoost = 0;
    }


    void CheckPathChange()
    {
        //raycast
        RaycastHit raycast;
        Ray ray = new Ray(transform.position, transform.up);
        Debug.DrawLine(transform.position, transform.position + (transform.up * rayCastRange), Color.red);


        if (Physics.Raycast(ray, out raycast, rayCastRange))
        {
            if (raycast.collider.GetComponentInParent<PathCreator>() != null && raycast.collider.GetComponentInParent<PathCreator>() != pathCreator)
            {
                //punto de choque
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    jumpRoot = raycast.collider.gameObject.GetComponentInParent<PathCreator>();
                    //objeto spline al que vas a cambiar
                    distanceTravelled = jumpRoot.path.GetClosestDistanceAlongPath(transform.position);
                    pathCreator = jumpRoot;
                    jumpRoot = null;

                    //this.transform.position = sample.location;
                    //this.transform.rotation = sample.Rotation;

                    ////TODO esto de la rotation no esta workeando;
                    float sum = acumRot % 360;
                    acumRot += (sum < 0)?180:-180;
                    //transform.Rotate(0, 0, acumRot);
                    //transform.position = transform.position + transform.up * (sampleToJump.scale.x);
                    //angulo, rotation
                    //lo unico que tienes que hace re strasformar el vector up del player a la normal del choque del raycast

                    //sampleToJump.location //centor de la curva
                    timerLerp = 0;
                    lerping = true;
                }
            }
        }

        //punto de choque del raycast



    }

    void changeFOVSpeed()
    {
        if (levelBoost > 0)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV + fovSpeed * levelBoost, 3f * Time.deltaTime);
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition,
                new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, baseCamerapos + offSetCamera * -levelBoost), 3f * Time.deltaTime);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV, 2f * Time.deltaTime);
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition,
                new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, baseCamerapos), 2f * Time.deltaTime);
        }
        
    }

    void lerpingMove()
    {
        Debug.Log("Lerping...");
        distanceTravelled += speed * Time.deltaTime;
        Vector3 final = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        Quaternion finRot = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

        totalDistanceTraveled += distanceTravelled;

        if (Mathf.Abs(lateralAcceleration) > 0)
        {
            if (lateralAcceleration < 0) lateralAcceleration += 0.1f;
            else lateralAcceleration -= 0.1f;
        }
        else
            lateralAcceleration = 0;


        //GameObject aux = new GameObject();
        //aux.transform.position = final;
        //aux.transform.rotation = finRot;


        //Debug.Log(acumRot);
        acumRot += lateralAcceleration;
        //aux.transform.Rotate(0, 0, acumRot);
        ////transform.Rotate(0, 0, acumRot);
        //transform.rotation = Quaternion.Lerp(transform.rotation, aux.transform.rotation, timerLerp/cd_Lerp);
        //transform.position = Vector3.Lerp(transform.position, aux.transform.position + aux.transform.up * offset, timerLerp / cd_Lerp);
        ////transform.position + transform.up * offset;

        float auxTime = timerLerp / cd_Lerp;

        auxTime = (auxTime > 1) ? 1 : auxTime;

        transform.rotation = Quaternion.Lerp(transform.rotation, finRot, auxTime);
        transform.Rotate(0, 0, Mathf.Lerp(transform.rotation.z, acumRot, auxTime));
        transform.position = Vector3.Lerp(transform.position, final + transform.up * offset, auxTime);


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

    }

    void Update()
    {
        if (view.IsMine && pathCreator != null)
        {

            if (Mathf.Abs(acumRot) > 360)
            {
                acumRot = (Mathf.Abs(acumRot) % 360) * ((acumRot > 0) ? 1.0f : -1.0f);
            }

            CheckPathChange();



            changeFOVSpeed();
            if (stunned) timeStunned += Time.deltaTime;

            if (timeStunned >= stunTime)
            {
                stunned = false;
                timeStunned = 0;
            }
            if (!lerping)
                normalMove();
            else
            {
                timerLerp += Time.deltaTime;
                lerpingMove();
                if (timerLerp >= cd_Lerp)
                    lerping = false;
            }
        }
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        if (view.IsMine)
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

                speed = baseSpeed;
                colision = true;

                GetComponent<ParticleSystem>().Play();//Homer homer homer homer homer homer homer bart lisa homer marge homer otto homer homer otto homer
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
                speed = rebSpeed;
                if(levelBoost<3)levelBoost++;
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

        if (otherPlayer != null && !stunned)
        {
            Debug.Log(acumulatedInput);
            Debug.Log(otherPlayer.acumulatedInput);

            if (Mathf.Abs(acumulatedInput) > Mathf.Abs(otherPlayer.acumulatedInput))
            {
                if (acumulatedInput > 0) lateralAcceleration -= 3;
                else lateralAcceleration += 3;

            }
            else
            {
                if (acumulatedInput > 0) lateralAcceleration -= 10;
                else lateralAcceleration += 10;

                stunned = true;
            }


        }
    }

    public float getDistance() { return totalDistanceTraveled; } //Para que el gameManager los ordene en la carrera
}
