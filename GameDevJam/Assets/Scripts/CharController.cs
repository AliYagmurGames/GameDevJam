using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    // This script is shared between all the characters, if the character is controlled by the player, it uses the inputs trough playerTracjer script.

    public Animator animator;
    [SerializeField] Camera mainCam;
    [SerializeField] GameObject playerTracker;
    public bool playerUnit = false;
    string currentAnim = "idle";
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float detectRange;
    [SerializeField] float attackDelay = 2;
    bool waitForAttack = false;
    bool dead = false;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    public LayerMask enemyLayer;
    public LayerMask playerLayer;
    [SerializeField] float damage;
    [SerializeField] float power;
    [SerializeField] float health;
    Vector3 startPosition;

    void Awake()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        if (playerUnit == false)
        {
            aiMovement();
        }
    }

    void aiMovement()
    {
        if(dead == false)
        {
            if((playerTracker.transform.position - startPosition).magnitude <= detectRange)
            {
                //move towards player and attack
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
                transform.forward = new Vector3(mX, 0, mZ).normalized;
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
                setAnimation("ToDie");
                this.gameObject.layer = 13;
                dead = true;
                StartCoroutine(reviveWithTime());
            }
        }
    }

    IEnumerator reviveWithTime()
    {
        yield return new WaitForSeconds(5);
        setAnimation("ToRevive");
        yield return new WaitForSeconds(2);
        dead = false;
        this.gameObject.layer = 12;
    }
}
