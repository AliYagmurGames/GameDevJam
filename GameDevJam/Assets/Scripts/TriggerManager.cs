using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriggerManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string Message;
    [SerializeField] TextMeshPro messagePosition;
    [SerializeField] float showTime;
    GameObject playerTracker;
    void Awake()
    {
        playerTracker = GameObject.Find("/PlayerTracker");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
