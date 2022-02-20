using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //Countdown
    public int countdownTime;
    public Text countdownDisplay;
    public Wheel[] wheels;
    public Car car;
    private Rigidbody carRB;
    public LapTimeManager lapTimeManager;

    void Start() {
        StartCoroutine(Countdown());
        carRB = car.GetComponent<Rigidbody>();
    }
    IEnumerator Countdown()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }
        countdownDisplay.text = "GO!!!";
        foreach (Wheel i in wheels){
            if (i.wantsToBePowered) i.isPowered = true;
        }
        car.controllable = true;
        lapTimeManager.isStarted = true;
        

        yield return new WaitForSeconds(1f);

        countdownDisplay.gameObject.SetActive(false);
        
    }
    private void Update() {
        if (countdownTime > 0) {
            carRB.constraints = RigidbodyConstraints.FreezeAll;
        }
        else carRB.constraints = RigidbodyConstraints.None;
    }
}
