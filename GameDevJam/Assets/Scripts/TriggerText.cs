using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerText : MonoBehaviour
{
    public string text;
    public PlayerTracker thePlayerTracker;
    bool firstTime = true;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "PlayerTracker" && firstTime == true)
        {
            thePlayerTracker.startSpeaking(text, 4f);
            firstTime = false;
        }
        
    }
}
