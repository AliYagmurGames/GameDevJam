using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FullScreen : MonoBehaviour
{
    public Sprite expand;
    public Sprite shrink;
    public Image fullScreen;
    public Animator pauseMenu;
    public Slider volume;
    public Slider volumeOriginal;
    bool pauseMenuChecker = false;

    public void changeScreenSize()
    {
        Screen.fullScreen = !Screen.fullScreen;
        if(fullScreen.sprite == expand)
        {
            fullScreen.sprite = shrink;
        }
        else
        {
            fullScreen.sprite = expand;
        }
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuActivation();
        }
    }

    public void pauseMenuActivation()
    {
        if (pauseMenuChecker == false)
        {
            pauseMenu.SetTrigger("appear");
            pauseMenuChecker = true;
            volume.value = volumeOriginal.value;
            StartCoroutine(stopingTime());
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetTrigger("disappear");
            pauseMenuChecker = false;
            volumeOriginal.value = volume.value;
        }
        
    }

    IEnumerator stopingTime()
    {
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
    }

    public void restartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }
}
