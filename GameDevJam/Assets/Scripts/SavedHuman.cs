using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedHuman : MonoBehaviour
{
    public Rigidbody rb;
    bool isStart = false;
    bool isSuffering = true;
    [SerializeField] GameObject playerTracker;
    [SerializeField] GameObject savedHuman;
    [SerializeField] GameObject ragdollHuman;

    private void Start()
    {
        StartCoroutine(swingingMan());
    }
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
        StopCoroutine(swingingMan());
        isSuffering = false;

    }

    IEnumerator swingingMan()
    {
        while(isSuffering)
        {
            rb.AddForce(this.transform.forward * 30, ForceMode.Impulse);
            //add some chocking sound
            yield return new WaitForSeconds(5);
        }
    }
}
