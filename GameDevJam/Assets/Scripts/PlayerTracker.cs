using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    public CharController playerChar;
    [SerializeField] float movementSpeed;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerChar.attack();
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        playerChar.move(moveX, moveZ);


        transform.position = playerChar.transform.position;
    }
}
