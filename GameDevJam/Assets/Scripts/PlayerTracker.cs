using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTracker : MonoBehaviour
{
    public CharController playerChar;
    [SerializeField] float movementSpeed;
    public bool dead = false;
    public RectTransform playerOnMap;
    public Slider staminaBar;
    public Slider healthBar;
    float startingX;
    float startingZ;

    public bool KeyCollected = false;

    private void Awake()
    {
        startingX = transform.position.x;
        startingZ = transform.position.z;
        staminaBar.maxValue = playerChar.maxStamina;
        healthBar.maxValue = playerChar.health;
    }

    private void Start()
    {
        StartCoroutine(mapTracker());
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
            staminaBar.value = playerChar.stamina;
            healthBar.value = playerChar.health;


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
        staminaBar.maxValue = playerChar.maxStamina;
        healthBar.maxValue = playerChar.startingHealth;
        playerChar.stopAIMovement();
        newBody.layer = 11;
        dead = false;
    }



    IEnumerator mapTracker()
    {
        while(true)
        {
            float differenceX = transform.position.x - startingX;
            float differenceZ = transform.position.z - startingZ;
            playerOnMap.anchoredPosition = new Vector2(147.28f - (differenceX * -0.87f), -120.79f - (differenceZ * -0.87f));
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void collectKey()
    {
        KeyCollected = true;
    }

    
}
