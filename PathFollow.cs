using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    public Transform[] routes;
    private int routeToGo;
    private float t;
    private Vector3 carPos;
    public float speedMult;
    private bool coroutineAllowed;

    private void Start() {
        routeToGo = 0;
        t = 0;
        coroutineAllowed = true;
    }

    private void Update() {
        if(coroutineAllowed) StartCoroutine(GoByRoute(routeToGo));
    }

    private IEnumerator GoByRoute(int routeNum){
        coroutineAllowed = false;

        Vector3 p0 = routes[routeNum].GetChild(0).position;
        Vector3 p1 = routes[routeNum].GetChild(1).position;
        Vector3 p2 = routes[routeNum].GetChild(2).position;
        Vector3 p3 = routes[routeNum].GetChild(3).position;

        while (t < 1){
            t += speedMult * Time.deltaTime;

            carPos = Mathf.Pow(1 - t, 3) * p0 +
                3 * Mathf.Pow(1 - t, 2) * t * p1 +
                3 * (1 - t) * Mathf.Pow(t, 2) * p2 +
                Mathf.Pow(t, 3) * p3;

            transform.position = carPos;
            yield return new WaitForEndOfFrame();
        }

        t = 0;
        routeToGo ++;

        if (routeToGo > routes.Length - 1) routeToGo = 0;

        coroutineAllowed = true;
    }
}
