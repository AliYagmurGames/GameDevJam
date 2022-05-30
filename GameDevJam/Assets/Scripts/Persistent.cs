using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Persistent : MonoBehaviour
{
    [Range(0f, 1f)]
    public float volume;
    public AudioMixer masterMixer;
    bool changed = false;
    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("persistent").Length > 1)
        {
            if (changed == false)
            {
                Destroy(this.gameObject);
            }
        }

        Object.DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        changed = true;
    }

    public void changeVolume(float vol)
    {
        volume = vol;
        setVolume();
    }

    private void Update()
    {
        
    }

    public void setVolume()
    {
        masterMixer.SetFloat("MasterVolume", (Mathf.Log10(volume) * 20) + 6);
    }

}
