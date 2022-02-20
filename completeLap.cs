using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class completeLap : MonoBehaviour
{
    public GameObject lapCompletedTrigger;
    public GameObject halfwayTrigger;

    public GameObject minDisplay;
    public GameObject secDisplay;
    public GameObject msDisplay;

    void OnTriggerEnter() {
        if(LapTimeManager.secCount <= 9) secDisplay.GetComponent<Text>().text = "0" + LapTimeManager.secCount + ".";
        else secDisplay.GetComponent<Text>().text = "" + LapTimeManager.secCount + ".";

        if(LapTimeManager.minCount <= 9) minDisplay.GetComponent<Text>().text = "0" + LapTimeManager.minCount + ".";
        else minDisplay.GetComponent<Text>().text = "" + LapTimeManager.minCount + ".";

        msDisplay.GetComponent<Text>().text = "" + LapTimeManager.msCount;

        LapTimeManager.minCount = 0;
        LapTimeManager.secCount = 0;
        LapTimeManager.msCount = 0;

        lapCompletedTrigger.SetActive(false);
        halfwayTrigger.SetActive(true);
    }
}
