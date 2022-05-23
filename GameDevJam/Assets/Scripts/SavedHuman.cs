using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedHuman : MonoBehaviour
{
    bool isStart = false;
    [SerializeField] GameObject playerTracker;
    [SerializeField] GameObject savedHuman;
    [SerializeField] GameObject ragdollHuman;
    void Update()
    {
        if(isStart)
        {
            if((transform.position - playerTracker.transform.position).magnitude > 60)
            {
                savedHuman.SetActive(true);
                ragdollHuman.SetActive(false);
                isStart = false;
            }
        }
    }

    public void setStartActive()
    {
        isStart = true;
    }
}
