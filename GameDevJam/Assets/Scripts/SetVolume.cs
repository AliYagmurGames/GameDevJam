using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVolume : MonoBehaviour
{
    Persistent pData;
    void Start()
    {
        pData = GameObject.FindGameObjectWithTag("persistent").GetComponent<Persistent>();
    }

    public void changeVolume(float vol)
    {
        pData.changeVolume(vol);
    }
}
