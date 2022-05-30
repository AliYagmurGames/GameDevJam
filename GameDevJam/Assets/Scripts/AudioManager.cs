using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider volume;
    Persistent pData;

    private void Start()
    {
        pData = GameObject.FindGameObjectWithTag("persistent").GetComponent<Persistent>();
        volume.value = pData.volume;
    }


}
