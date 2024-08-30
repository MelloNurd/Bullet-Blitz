using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreScript : MonoBehaviour
{
    [SerializeField] TMP_Text highSore, currScore;
    private int bestMin, bestSec, bestInt;
    // Start is called before the first frame update
    void Start()
    {
        bestSec = 0;
        bestMin = 0;

        //score = GetComponent<Text>();
        if (PlayerPrefs.HasKey("hiScore"))
        {
            bestInt = PlayerPrefs.GetInt("hiScore");
            bestSec = bestInt % 60;
            bestMin = (bestInt - bestSec) / 60;
        }
        highSore.text = bestMin.ToString("00")+ ":" + bestSec.ToString("00");

        if (PlayerPrefs.HasKey("curr"))
        {
            bestInt = (int)PlayerPrefs.GetFloat("curr");
            bestSec = bestInt % 60;
            bestMin = (bestInt - bestSec) / 60;
        }
        currScore.text = bestMin.ToString("00") + ":" + bestSec.ToString("00");
    }

}
