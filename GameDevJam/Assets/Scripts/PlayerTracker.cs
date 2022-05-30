using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTracker : MonoBehaviour
{
    public CharController playerChar;
    public Camera _camera;
    [SerializeField] float movementSpeed;
    public bool dead = false;
    public RectTransform playerOnMap;
    public Slider staminaBar;
    public Slider healthBar;
    public Slider sanityBar;
    float startingX;
    float startingZ;
    Transform cameraStartPos;
    public Transform cameraAimedPos;
    bool cameraMovTimer = true;

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
        dead = true;
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
        if(sanityBar.value > 0)
        {
            sanityBar.value -= 1;
            playerChar.playerUnit = false;
            playerChar = newBody.GetComponent<CharController>();
            playerChar.playerUnit = true;
            staminaBar.maxValue = playerChar.maxStamina;
            healthBar.maxValue = playerChar.startingHealth;
            playerChar.stopAIMovement();
            newBody.layer = 11;
            dead = false;
        }
        else
        {
            //GameOver
            Debug.Log("GameOver");
        }
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

    public void raiseSanity()
    {
        sanityBar.value += 1;
    }

    public void StartGame()
    {
        dead = false;
        cameraStartPos = _camera.transform;
        StartCoroutine(cameraReset());
    }

    IEnumerator isCameraMove()
    {
        yield return new WaitForSeconds(5);
        cameraMovTimer = false;
    }

    IEnumerator cameraReset()
    {
        while(cameraMovTimer == true)
        {
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, cameraAimedPos.position, Time.deltaTime * 2);
            _camera.transform.rotation = Quaternion.Lerp(cameraStartPos.rotation, cameraAimedPos.rotation, Time.deltaTime * 2);
            yield return new WaitForFixedUpdate();
        }
    }
    
}
