using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Photon.Pun;
using static Unity.Burst.Intrinsics.X86;
using System;

public class DemoCameraBezier : MonoBehaviour
{
    public PathCreator pathCreator;
    PathCreator jumpRoot;   //La raiz a la que puedo saltars
    public EndOfPathInstruction endOfPathInstruction;
    [SerializeField]
    GameObject spaceText;
    float speed = 0.0f;
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
    public float timeAfterChange = 2.0f;
    float changeTimer = float.MaxValue;
    public bool started = false;

    float distanceTravelled;
    float acumRot = 0;
    public float rotVirage = 40;
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
    public float fovAcelSpeed = 30.0f;
    public float fovSpeedBoost = 20.0f;
    private float targetFov;
    int levelBoost;
    float baseCamerapos;
    public float offSetCamera;
    float timer = 0;
    float timerBOST = 0;

    float totalDistanceTraveled; //Para controlar quien va primero en la carrera

    Fmod_Music fmodMusicManager;
    Fmod_Collisions fmodCollisionManager;
    Fmod_Engine fmodEngineManager;
    Fmod_RootChange fmodRootChangeManager;


    void cameraStart()
    {
        cam.transform.position = cam.transform.position + transform.forward * 2;
    }
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

        fmodMusicManager = GetComponent<Fmod_Music>();
        fmodCollisionManager = GetComponent<Fmod_Collisions>();
        fmodEngineManager = GetComponent<Fmod_Engine>();
        fmodRootChangeManager = GetComponent<Fmod_RootChange>();

        normalMove(); //colcar al jugador y la camara en posicion
        cameraStart(); //adelantar la camara
        //detachear la camara
        cam.transform.parent = null;

        //if (!GameManager.instance.isMultiplayer)
            startButton();
    }

    public void startButton()
    {
        //comineza la cuneta atras, 2 segundos voy a decir.
        //cunado pasen los dos segundos la camara hacer el lerpeo a la posicion del jugador a la velocidad que tenga quedara guapo trusteen
        timer = 0.5f;
        timerBOST = 1.0f;
        levelBoost = 3;
        speed = baseSpeed;
        started = true;
    }

    public void setVel()
    {
        speed = baseSpeed;
    }
    public float getVel()
    {
        return speed;
    }
    void Update()
    {
        if (!view.IsMine)
            return;

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Musica");
            raceStart();
        }
        if (Input.GetKeyDown(KeyCode.P)) fmodEngineManager.playEngine();


        //como no tengo boton pues con la x se hace el comienzo
        //if (Input.GetKeyDown(KeyCode.X) && !started)

        if (pathCreator != null)
        {

            if (Mathf.Abs(acumRot) > 360)
            {
                acumRot = (Mathf.Abs(acumRot) % 360) * ((acumRot > 0) ? 1.0f : -1.0f);
            }

            CheckPathChange();
            fmodEngineManager.updateBoostMusic((speed * 1) / rebSpeed);

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

    void normalMove()
    {

        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

        totalDistanceTraveled += distanceTravelled;

        if (manejable && !stunned && timer <= 0 && started)
        {
            if (speed < acelSpeed && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel") > 0)) speed += aceleration * Time.deltaTime;
            if (speed > baseSpeed && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("Mouse ScrollWheel") < 0)) speed -= deceleration * Time.deltaTime;

            acumRot -= Input.GetAxis("Horizontal") * rotVirage * Time.deltaTime;
            acumulatedInput -= (int)Input.GetAxis("Horizontal");
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

        if (speed > acelSpeed)
        {
            //Debug.Log(speed);
            speed -= deceleration * Time.deltaTime;
        }
        else
            if (timerBOST <= 0)
            levelBoost = 0;
    }


    void CheckPathChange()
    {
        if (changeTimer < timeAfterChange)
        {
            changeTimer += Time.deltaTime;
            Debug.Log(changeTimer);
            return;
        }


        //Debug.Log("Raycast");
        //raycast
        RaycastHit raycast;
        Ray ray = new Ray(transform.position, transform.up);
        Debug.DrawLine(transform.position, transform.position + (transform.up * rayCastRange), Color.red);


        if (Physics.Raycast(ray, out raycast, rayCastRange))
        {
            if (raycast.collider.GetComponentInParent<PathCreator>() != null && raycast.collider.GetComponentInParent<PathCreator>() != pathCreator)
            {
                spaceText.SetActive(true);
                //punto de choque
                if (!lerping && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0)))
                {
                    spaceText.SetActive(false);
                    jumpRoot = raycast.collider.gameObject.GetComponentInParent<PathCreator>();
                    //objeto spline al que vas a cambiar
                    distanceTravelled = jumpRoot.path.GetClosestDistanceAlongPath(transform.position);
                    pathCreator = jumpRoot;
                    jumpRoot = null;

                    //this.transform.position = sample.location;
                    //this.transform.rotation = sample.Rotation;

                    ////TODO esto de la rotation no esta workeando;
                    float sum = acumRot % 360;
                    acumRot += (sum < 0) ? 180 : -180;
                    //transform.Rotate(0, 0, acumRot);
                    //transform.position = transform.position + transform.up * (sampleToJump.scale.x);
                    //angulo, rotation
                    //lo unico que tienes que hace re strasformar el vector up del player a la normal del choque del raycast

                    //sampleToJump.location //centor de la curva
                    timerLerp = 0;
                    lerping = true;
                    changeTimer = 0;

                    fmodRootChangeManager.playChangeRoot();
                }
            }
        }
        else
        {
            spaceText.SetActive(false);
            //Debug.Log("La vieja de lau");
        }
        //punto de choque del raycast



    }

    void changeFOVSpeed()
    {
        if (started && timer <= 0)
            if (levelBoost > 0)
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV + fovAcelSpeed + fovSpeedBoost * levelBoost, 3f * Time.deltaTime);
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition,
                    new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, baseCamerapos + offSetCamera * -levelBoost), 3f * Time.deltaTime);
            }
            else
            {
                float currentFov = baseFOV + (speed - baseSpeed) * (fovAcelSpeed / (acelSpeed - baseSpeed));
                if (currentFov < baseFOV) currentFov = baseFOV;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, currentFov, 5f * Time.deltaTime);
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition,
                    new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, baseCamerapos), 2f * Time.deltaTime);
            }
        else if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }

        if (timerBOST >= 0)
        {
            timerBOST -= Time.deltaTime;
        }

        if (timer <= 0 && started)
        {
            cam.transform.parent = this.transform;
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

        if (speed > baseSpeed)
        {
            //Debug.Log(speed);
            speed -= deceleration * Time.deltaTime;
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
            Debug.Log("Trigger");
            PinchoTrigger pincho = other.GetComponent<PinchoTrigger>();

            if (pincho != null && !colision)
            {
                if (pincho.derecha) lateralAcceleration -= pinchoPunch;
                else lateralAcceleration += pinchoPunch;

                speed = baseSpeed;
                colision = true;

                GetComponentInChildren<ParticleSystem>().Play();
                fmodMusicManager.resetBoostMusic();
                fmodCollisionManager.playCollision(1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (view.IsMine)
        {
            Debug.Log("Rebufo");
            rebufo reb = other.GetComponent<rebufo>();

            if (reb != null && !colision)
            {
                speed = rebSpeed;
                if (levelBoost < 3) levelBoost++;

                fmodMusicManager.updateBoostMusic(1);
                fmodCollisionManager.playCollision(2);
            }
            else if (reb != null)
            {
                colision = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //DemoCameraBezier otherPlayer = collision.gameObject.GetComponent<DemoCameraBezier>();

        //if (otherPlayer != null && !stunned)
        //{
        //    Debug.Log(acumulatedInput);
        //    Debug.Log(otherPlayer.acumulatedInput);
        //    cols.playCollision(0);

        //    if (Mathf.Abs(acumulatedInput) > Mathf.Abs(otherPlayer.acumulatedInput))
        //    {
        //        if (acumulatedInput > 0) lateralAcceleration -= 3;
        //        else lateralAcceleration += 3;

        //    }
        //    else
        //    {
        //        if (acumulatedInput > 0) lateralAcceleration -= 10;
        //        else lateralAcceleration += 10;

        //        stunned = true;
        //    }


        //}
    }

    public float getDistance() { return totalDistanceTraveled; } //Para que el gameManager los ordene en la carrera

    public void raceStart()
    {
        fmodMusicManager.playMusic();
        fmodMusicManager.updateStartedMusic(true);
    }
}
