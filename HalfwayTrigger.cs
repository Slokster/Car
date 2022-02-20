using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfwayTrigger : MonoBehaviour
{
    public GameObject halfwayTrigger;
    public GameObject lapTrigger;

    void OnTriggerEnter() {
        halfwayTrigger.SetActive(false);
        lapTrigger.SetActive(true);
    }
}
