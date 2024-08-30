using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    [SerializeField] TMP_Text score;
    private float seconds;
    private int minutes;
    public float totalScore;

    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimerUI();
    }
    //call t$$anonymous$$s on update
    public void UpdateTimerUI()
    {
        //set timer UI
        seconds += Time.deltaTime;
        if (seconds >= 60)
        {
            minutes++;
            seconds = seconds % 60;
        }
        score.text = minutes.ToString("00") + ":" + ((int)seconds).ToString("00");
        totalScore = minutes * 60 + seconds;
    }

    public void SetSaveTime()
    {
        PlayerPrefs.SetFloat("curr", minutes * 60 + seconds);
    }

    public int GetScore()
    {
        return minutes * 60 + (int)seconds;
    }
}