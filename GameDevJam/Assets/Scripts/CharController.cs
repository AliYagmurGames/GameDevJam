using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharController : MonoBehaviour
{
    // This script is shared between all the characters, if the character is controlled by the player, it uses the inputs trough playerTracjer script.

    public Animator animator;
    Camera mainCam;
    GameObject playerTracker;
    public bool playerUnit = false;
    string currentAnim = "idle";
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float detectRange;
    [SerializeField] float attackDelay = 2;
    bool waitForAttack = false;
    bool waitForNextAttackBool = false;
    bool dead = false;
    bool notCharControled = true;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    [SerializeField] float hitRange;
    public LayerMask enemyLayer;
    public LayerMask playerLayer;
    [SerializeField] float damage;
    [SerializeField] float power;
    [SerializeField] float health;
    float startingHealth;
    float turnSmoothVelocity;
    Vector3 startPosition;

    private NavMeshAgent selfAgent;

    void Awake()
    {
        startPosition = transform.position;
        playerTracker = GameObject.Find("/PlayerTracker");
        mainCam = Camera.main;
        selfAgent = this.GetComponent<NavMeshAgent>();
        startingHealth = health;
    }
    void Start()
    {
        if (playerUnit == false)
        {
            StartCoroutine(forAIMovement());
        }
    }

    IEnumerator forAIMovement()
    {
        while(notCharControled)
        {
            aiMovement();
            yield return new WaitForSeconds(0.2f);
        }
    }

    void startAIMovement()
    {
        notCharControled = true;
        selfAgent.enabled = true;
        StartCoroutine(forAIMovement());
    }

    public void stopAIMovement()
    {
        notCharControled = false;
        selfAgent.enabled = false;
    }

    void aiMovement()
    {
        if(dead == false)
        {
            if((playerTracker.transform.position - this.transform.position).magnitude <= detectRange)
            {
                if((playerTracker.transform.position - this.transform.position).magnitude >= hitRange)
                {
                    if(waitForAttack == false)
                    {
                        selfAgent.destination = playerTracker.transform.position;
                        setAnimation("ToRun");
                        if (selfAgent.desiredVelocity.magnitude != 0)
                        {
                            transform.forward = selfAgent.desiredVelocity.normalized;
                        }
                    }
                }
                else
                {
                    selfAgent.destination = transform.position;
                    if (waitForAttack == false)
                    {
                        if (waitForNextAttackBool == false)
                        {
                            aiAttack();
                        }
                        else
                        {
                            setAnimation("ToIdle");
                        }
                    }
                }
                
            }
        }
        //AI = wait till the player is in range, than attack. Return to the starting position if player is not in range.
    }


    public void setAnimation(string anim)
    {
        if (anim != currentAnim)
        {
            animator.SetTrigger(anim);
            currentAnim = anim;
        }
    }

    public void attack()
    {
        waitForAttack = true;
        StartCoroutine(delayAttack());
        setAnimation("ToAttack");
        //check for enemies and deal damage

        int layerMask = 1 << 10;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 attackDirection = hit.point - this.transform.position;
            attackDirection.y = 0;
            this.transform.forward = attackDirection;
        }
        StartCoroutine(attackWithTiming());
    }

    void aiAttack()
    {
        waitForAttack = true;
        waitForNextAttackBool = true;
        StartCoroutine(delayAttack());
        setAnimation("ToAttack");
        StartCoroutine(attackWithTiming());
        StartCoroutine(waitForNextAttack());
    }

    IEnumerator waitForNextAttack()
    {
        waitForNextAttackBool = true;
        yield return new WaitForSeconds(4);
        waitForNextAttackBool = false;
    }

    IEnumerator delayAttack()
    {
        waitForAttack = true;
        yield return new WaitForSeconds(attackDelay);
        waitForAttack = false;
    }

    IEnumerator attackWithTiming()
    {
        yield return new WaitForSeconds(attackDelay/2);
        Collider[] hitEnemies;
        if (playerUnit == true)
        {
            hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        }
        else
        {
            hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
        }

        foreach(Collider enemy in hitEnemies)
        {
            enemy.gameObject.GetComponent<CharController>().receiveDamage(this.gameObject, damage);
            enemy.attachedRigidbody.AddForce((enemy.transform.position - this.transform.position) * power, ForceMode.Impulse);
        }
    }

    public void move(float mX, float mZ)
    {
        if(waitForAttack == false)
        {
            transform.Translate(new Vector3(mX, 0, mZ).normalized * movementSpeed * Time.deltaTime, Space.World);
            if (new Vector3(mX, 0, mZ).magnitude == 0)
            {
                setAnimation("ToIdle");
            }
            else
            {
                Vector3 direction = new Vector3(mX, 0, mZ).normalized;
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                setAnimation("ToRun");
            }
        }
    }

    public void receiveDamage(GameObject hiter, float dmg)
    {
        if(health > 0)
        {
            health = health - dmg;
            if (health <= 0)
            {
                if(playerUnit == true)
                {
                    playerTracker.GetComponent<PlayerTracker>().dead = true;
                }
                setAnimation("ToDie");
                this.gameObject.layer = 13;
                dead = true;
                playerTracker.GetComponent<PlayerTracker>().transferSoul(hiter);
                this.GetComponent<CapsuleCollider>().isTrigger = true;
                StartCoroutine(reviveWithTime());
            }
        }
    }

    IEnumerator reviveWithTime()
    {
        yield return new WaitForSeconds(5);
        setAnimation("ToRevive");
        yield return new WaitForSeconds(4);
        dead = false;
        health = startingHealth;
        this.GetComponent<CapsuleCollider>().isTrigger = false;
        this.gameObject.layer = 12;
        startAIMovement();



    }
}
