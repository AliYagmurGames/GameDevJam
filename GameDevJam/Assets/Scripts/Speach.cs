using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Speach : MonoBehaviour
{
    public TextMeshPro speak;

    public void StartSpeaking(string text, float time)
    {
        speak.gameObject.SetActive(true);
        speak.text = text;
        StartCoroutine(waitForReset(time));
    }

    IEnumerator waitForReset(float time)
    {
        yield return new WaitForSeconds(time);
        speak.gameObject.SetActive(false);
    } 
}
