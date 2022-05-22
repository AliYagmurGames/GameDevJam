using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    public CharController playerChar;
    [SerializeField] float movementSpeed;
    public bool dead = false;
    void Start()
    {
        
    }

    
    void Update()
    {
        
        if (dead == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                playerChar.attack();
            }

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            playerChar.move(moveX, moveZ);

        }
        
    }

    private void LateUpdate()
    {
        Vector3 destination = playerChar.transform.position;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, destination, 4f * Time.deltaTime);
        transform.position = smoothPosition;
    }

    public void transferSoul(GameObject newBody)
    {
        playerChar.playerUnit = false;
        playerChar = newBody.GetComponent<CharController>();
        playerChar.playerUnit = true;
        playerChar.stopAIMovement();
        newBody.layer = 11;
        dead = false;
    }
}
