using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTimeManager : MonoBehaviour
{
    public static int minCount;
    public static int secCount;
    public static float msCount;
    public static string msDisplay;

    public GameObject minBox;
    public GameObject secBox;
    public GameObject msBox;

    public bool isStarted;

    void Update() {
        if(isStarted){
            msCount += Time.deltaTime * 10;
            msDisplay = msCount.ToString("F0");
            msBox.GetComponent<Text>().text = "" + msDisplay;

            if(msCount > 9){
                msCount = 0;
                secCount ++;
            }

            if(secCount <= 9) secBox.GetComponent<Text>().text = "0" + secCount + ".";
            else secBox.GetComponent<Text>().text = "" + secCount + ".";

            
            if(secCount > 59){
                secCount = 0;
                minCount ++;
            }

            if(minCount <= 9) minBox.GetComponent<Text>().text = "0" + minCount + ":";
            else minBox.GetComponent<Text>().text = "" + minCount + ":";
        }
    }
}
