using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Car : MonoBehaviour
{

    private Rigidbody rb;
    public Wheel[] wheels;

    public bool controllable;
    public float steeringAngle;

    [Header("Turning Curves")]
    public AnimationCurve steeringAngleLimit;
    public AnimationCurve steeringSpeed;
    public AnimationCurve steeringResetSpeed;

    [Header("Speed Curves")]
    public AnimationCurve speedCurve;

    private float accelerationForce;

    public float timePressed;

    public Vector3 CentreOfMass;

    public int numOfPoweredWheels;

    private float force;
    private float t = 0f;


    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        rb.centerOfMass = CentreOfMass;
        foreach (Wheel i in wheels){
            if (i.wantsToBePowered) numOfPoweredWheels += 1;
        }
    }

    void FixedUpdate()
    {
        AckermannSteering();
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.TransformPoint(CentreOfMass), 0.1f);
    }

    
    void Update()
    {
        keyPressedTime();
        UpdateInputs();

        accelerationForce = AccelerationForce();
        foreach(Wheel i in wheels)
        {
            if (i.isPowered)
            {
                i.accelerationFORCE = accelerationForce / numOfPoweredWheels;
            }
        }
    }

    float AccelerationForce()
    {
        if (Input.GetKey(KeyCode.W))
        {
            float wantedSpeed = speedCurve.Evaluate(timePressed);
            float currentSpeed = calculateSpeed();
            float currentSpeedKPH = Mathf.Abs(currentSpeed) * 3.6f;
            float accel = (wantedSpeed - currentSpeedKPH);
            accel /= 3.6f;
            force = rb.mass * accel;
            return force;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            float wantedSpeed = speedCurve.Evaluate(timePressed);
            float currentSpeed = calculateSpeed();
            float currentSpeedKPH = currentSpeed * 3.6f;
            float accel = (wantedSpeed - currentSpeedKPH);
            accel /= 3.6f;
            force = rb.mass * accel;
            return force;
        }
        else{
            force = Mathf.Lerp(force, 0, t);
            t += 0.001f * Time.deltaTime;
            if (t >= 1) t = 0f;
            return force;
        }
    }

    void keyPressedTime()
    {
        if (controllable){
            if (Input.GetKey(KeyCode.W))
            {
                timePressed += Time.deltaTime;
                if (timePressed >= speedCurve.keys[2].time) timePressed = speedCurve.keys[2].time;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (timePressed > 0) timePressed -= 10f * Time.deltaTime;
                else timePressed -= Time.deltaTime;
                if (timePressed <= speedCurve.keys[0].time) timePressed = speedCurve.keys[0].time;
            }
            else if (timePressed > 0) timePressed -= 5 * Time.deltaTime;

            else if (timePressed < 0) timePressed += 5 * Time.deltaTime;

            else timePressed = 0;
        }
    }

    void UpdateInputs()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if (!controllable)
        {
            v = 0;
            h = 0;
        }
    
        float speed = calculateSpeed();

        if (Math.Abs(h) > 0 && controllable)
        {
            float speedKPH = Math.Abs(speed) * 3.6f;
            float steerSpeed = steeringSpeed.Evaluate(speedKPH);
            float steeringAngleNow = steeringAngle + (h * steerSpeed);
            float sign = Mathf.Sign(steeringAngleNow);
            float steeringLimit = getMaxSteeringAngle(speed);
            steeringAngleNow = Mathf.Min(Mathf.Abs(steeringAngleNow), steeringLimit) * sign;
            steeringAngle = steeringAngleNow;
        }
        else
        {
            float speedKPH = Math.Abs(speed) * 3.6f;
            float angleResetSpeed = steeringResetSpeed.Evaluate(speedKPH);
            angleResetSpeed = Mathf.Lerp(0f, angleResetSpeed ,Mathf.Clamp01(speedKPH/2));
            steeringAngle = Mathf.Lerp(steeringAngle, 0f, angleResetSpeed * Time.fixedDeltaTime);
        }
    }

    void AckermannSteering()
    {
        //Calculating the axle seperation
        Vector3 midFront = (wheels[0].transform.position + wheels[1].transform.position) / 2;
        Vector3 midRear = (wheels[2].transform.position + wheels[3].transform.position) / 2;
        Vector3 axleDiff = midFront - midRear;
        float axleSeperation = axleDiff.magnitude;

        //Calculating the wheel seperation
        Vector3 wheelDiff = wheels[0].transform.position - wheels[1].transform.position;
        float wheelSeperation = wheelDiff.magnitude;

        float turningRadius = axleSeperation / Mathf.Tan(steeringAngle * Mathf.Deg2Rad);
        float steerAngleLeft = Mathf.Atan(axleSeperation / (turningRadius + wheelSeperation / 2));
        float steerAngleRight = Mathf.Atan(axleSeperation / (turningRadius - wheelSeperation / 2));

        foreach (Wheel i in wheels)
        {
            if (i.FLWheel) i.currentSteeringAngle = steerAngleLeft * Mathf.Rad2Deg;
            if (i.FRWheel) i.currentSteeringAngle = steerAngleRight * Mathf.Rad2Deg;
        }
    }

    float getMaxSteeringAngle(float speedmps)
    {
        float speedKMH = speedmps * 3.6f;
        float degressLimit = steeringAngleLimit.Evaluate(speedKMH);
        return degressLimit;
    }

    public float calculateSpeed()
    {
        Vector3 velocity = rb.velocity;
        Vector3 forward = rb.transform.rotation * Vector3.forward;
        float forwardV = Vector3.Dot(velocity, forward);
        Vector3 Vforward = forwardV * forward;
        float speed = Vforward.magnitude * Mathf.Sign(forwardV);
        return speed;
    }
}