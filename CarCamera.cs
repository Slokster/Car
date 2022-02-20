using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    public GameObject car;
    public float distance;
    public float targetHeightOffset;
    public float cameraHeightOffset;

    Camera cameraComponent;
    Car carComponent;

    Vector3 currentPos;

    public AnimationCurve fovCurve;

    void Start()
    {
        cameraComponent = GetComponent<Camera>();
        if (car != null)
        {
            carComponent = car.GetComponent<Car>();
        }

        currentPos = transform.position;
    }

    void Update() {
        Vector3 tempPos = currentPos;
        Vector3 carPos = car.transform.position;

        carPos.y = 0;
        tempPos.y = 0;

        Vector3 direction = tempPos - carPos;
        float length = direction.magnitude;
        direction.Normalize();

        Vector3 camPos = tempPos;
        if (length > distance) camPos = carPos + direction * distance;

        camPos.y = car.transform.position.y + cameraHeightOffset;
        transform.position = camPos;

        Vector3 carPoint = car.transform.position;
        carPoint.y += targetHeightOffset;

        Vector3 lookDirection = carPoint - camPos;
        Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        transform.rotation = rotation;

        float speedKPH = carComponent.calculateSpeed() * 3.6f;
        float fov = fovCurve.Evaluate(speedKPH);

        cameraComponent.fieldOfView = fov;

        currentPos = transform.position;
        transform.RotateAround(carPoint, Vector3.up, 0);
    }
}
