using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTriggers : MonoBehaviour
{
    public GameObject roomToBeActive;
    bool firstTime = true;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "PlayerTracker" && firstTime == true)
        {
            roomToBeActive.SetActive(true);
            firstTime = false;
        }
    }
}
