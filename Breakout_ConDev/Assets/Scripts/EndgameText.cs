using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndgameText : MonoBehaviour
{
    [SerializeField] public Text egText;

    void Start()
    {
        egText.text = "You ran out of time to escape!\nYou completed " + GameManager.score + " levels and collected " + (TimerManager.countDownMultiplier - 1) + " large pellets!";
    }
}
