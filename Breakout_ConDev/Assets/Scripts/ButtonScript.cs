using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{ 
    public int sceneLoaded;

    public void LoadScene()
    { // resets all scores and variables when exiting the main menu or game over screen
        TimerManager.myTimer = 999;
        TimerManager.countDownMultiplier = 1;
        TimerManager.timeUp = false;
        SceneManager.LoadScene(sceneLoaded);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
