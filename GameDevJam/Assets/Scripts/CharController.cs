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
    PlayerTracker _thePlayerTracker;
    public bool playerUnit = false;
    public int unitType;
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
    public LayerMask breakWall;
    public LayerMask charLayers;
    [SerializeField] float damage;
    [SerializeField] float power;
    public float health;
    [HideInInspector] public int stamina;
    public int maxStamina;
    [HideInInspector] public float startingHealth;
    float turnSmoothVelocity;
    Vector3 startPosition;
    float chase = 4;

    List<CharController> friendlyEnemies;
    List<CharController> hostileEnemies;
    CharController[] enemyListTwo;
    CharController closestEnemy;

    bool shouldAttack;
    bool isAggresive = false;



    private NavMeshAgent selfAgent;

    void Awake()
    {
        startPosition = transform.position;
        playerTracker = GameObject.Find("/PlayerTracker");
        _thePlayerTracker = playerTracker.GetComponent<PlayerTracker>();
        mainCam = Camera.main;
        selfAgent = this.GetComponent<NavMeshAgent>();
        startingHealth = health;
        stamina = maxStamina;
        
    }
    void Start()
    {
        if (playerUnit == false)
        {
            StartCoroutine(forAIMovement());
        }
        
        StartCoroutine(raiseStamina());

        
    }


    IEnumerator forAIMovement()
    {
        while(notCharControled)
        {
            aiMovement();
            chase -= 0.2f;
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

            shouldAttack = true;
            if(_thePlayerTracker.playerChar.unitType == unitType)
            {
                if(isAggresive)
                {
                    shouldAttack = true;
                }
                else
                {
                    shouldAttack = false;
                }

            }

            if ((playerTracker.transform.position - this.transform.position).magnitude <= detectRange && shouldAttack)
            {
                if((playerTracker.transform.position - this.transform.position).magnitude >= hitRange)
                {
                    if(waitForAttack == false)
                    {
                        chase = 4;
                        Vector3 agentDestination = playerTracker.transform.position;

                        selfAgent.destination = agentDestination;
                        setAnimation("ToRun");
                        selfAgent.isStopped = false;
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
                            Vector3 lookDirection = playerTracker.transform.position - transform.position;
                            lookDirection.y = transform.position.y;
                            transform.forward = lookDirection;
                            aiAttack();
                        }
                        else
                        {
                            setAnimation("ToIdle");
                            selfAgent.isStopped = true;
                        }
                    }
                }
                
            }
            else
            {
                if(chase > 0)
                {
                    setAnimation("ToIdle");
                    selfAgent.isStopped = true;
                }
                else if ((transform.position - startPosition).magnitude > 0.5)
                {
                    selfAgent.destination = startPosition;
                    setAnimation("ToRun");
                    selfAgent.isStopped = false;
                }
                else
                {
                    setAnimation("ToIdle");
                    selfAgent.isStopped = true;
                }
            }
        }
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
        if(waitForAttack == false && stamina >= 20)
        {
            waitForAttack = true;
            StartCoroutine(delayAttack());
            setAnimation("ToAttack");

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

    IEnumerator raiseStamina()
    {
        while (true)
        {
            if (stamina < maxStamina)
            {
                stamina += 1;
            }
            yield return new WaitForSeconds(0.15f);
        }

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
        stamina = stamina - 20;
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
        Collider[] breakableWall;
        breakableWall = Physics.OverlapSphere(attackPoint.position, attackRange, breakWall);

        if(breakableWall.Length > 0 && power >=40)
        {
            Debug.Log("wall broken");
            breakableWall[0].gameObject.GetComponent<wallBreak>().breakWall();
        }

        if (dead == false)
        {
            foreach (Collider enemy in hitEnemies)
            {
                CharController enemyController = enemy.gameObject.GetComponent<CharController>();
                if(enemyController.unitType == unitType && playerUnit == true)
                {
                    enemyController.setAggresive();
                }
                enemyController.receiveDamage(this.gameObject, damage);
                enemy.attachedRigidbody.AddForce((enemy.transform.position - this.transform.position) * power, ForceMode.Impulse);
            }
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
                if(playerUnit == true)
                {
                    playerTracker.GetComponent<PlayerTracker>().transferSoul(hiter);
                }
                
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

    public void setAggresive()
    {
        isAggresive = true;
    }

}
