using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interraction : MonoBehaviour
{
    public Transform textPlace;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "PlayerTracker")
        {
            //setActive(true)
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PlayerTracker")
        {
            //setActive(false);
        }
    }

    private void Update()
    {
        
    }

}
