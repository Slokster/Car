using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    private Rigidbody rb;

    public float relaxedLen;
    public float springConstant, dampingConstant;
    public float wheelRadius;

    private float currentLen, extension, prevExtension;

    public bool FLWheel;
    public bool FRWheel;
    public bool RLWheel;
    public bool RRWheel;

    public float currentSteeringAngle;
    private float totalForce;

    public float accelerationFORCE;
    public bool wantsToBePowered;
    public bool isPowered;

    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Mathf.Abs(currentSteeringAngle) < 1f) currentSteeringAngle = 0f;
        transform.localRotation = Quaternion.Euler(currentSteeringAngle * Vector3.up);
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, relaxedLen + wheelRadius))
        {
            ///////////SUSPENSION///////////
            currentLen = hit.distance - wheelRadius;
            extension = 1 - Mathf.Clamp01(currentLen / relaxedLen);

            //F = -kx, attemtpting to reset the extension back to 0
            float springForce = -springConstant * extension;

            //Damping force, attempting to reset the spring velocity to 0
            float springVel = (extension - prevExtension) / Time.fixedDeltaTime; //speed = distance/time
            prevExtension = extension;
            float dampingForce = -dampingConstant * springVel;

            //Total Force
            totalForce = springForce + dampingForce;

            //Considering only applying the force along the normal to the plane (so force doesnt act at an angle and tilt the car)
            //float normalisationMult = Vector3.Dot(hit.normal, -Vector3.down);
            //totalForce = totalForce * normalisationMult;

            //Applying the force
            Vector3 suspensionForce = -transform.up * totalForce;
            rb.AddForceAtPosition(suspensionForce, hit.point);

            rb.AddForceAtPosition(accelerationFORCE * transform.forward, hit.point - 0.2f * transform.up);
        }
    }
}
