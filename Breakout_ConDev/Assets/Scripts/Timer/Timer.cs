using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float countdownTime = 0f;
    float tickTime = 1f;
    [SerializeField] private Text timeText;
    [SerializeField] private Text breakerText;
    private bool isFlashing;
    // Start is called before the first frame update
    void Start()
    {
        countdownTime = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TimerManager.isCountingDown)
        {
            countdownTime = countdownTime + (Time.deltaTime * (TimerManager.countDownMultiplier + GameManager.deathMultiplier));
            if (countdownTime >= tickTime)
            {
                countdownTime = 0f;
                TimerManager.myTimer = TimerManager.myTimer - 1;
            }
        }
        else
        {
            if (Time.fixedTime % 1 < .2)
            {
                timeText.enabled = false;
            }
            else
            {
                timeText.enabled = true;
            }
        }
        timeText.text = TimerManager.myTimer.ToString();
        breakerText.text = TimerManager.countDownMultiplier.ToString();

        if (TimerManager.myTimer < 1)
        {
            TimerManager.timeUp = true;
        }
    }
}
