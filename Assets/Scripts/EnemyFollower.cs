using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollower : MonoBehaviour
{
    public LayerMask whatIsPlayer;
    private Rigidbody2D rb;
    public Animator anim;

    private bool inPlayerRange;
    private bool inAttackRange = false;
    private bool isAttacking = false;
    private bool isHurting = false;
    private bool canMove = true;

    private bool isLookingRight = false;

    public Transform playerCheck;
    public float playerCheckRadius;

    private float distanceToPlayer;

    Transform target;
    GameObject Player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Player = GameObject.FindWithTag("Player"); 
        target = Player.transform;
    }


    // Update is called once per frame
    void Update()
    {
        inPlayerRange = Physics2D.OverlapCircle(playerCheck.position, playerCheckRadius, whatIsPlayer);

        if (inPlayerRange)
        {
            FollowPlayer();
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    void FollowPlayer()
    {

        distanceToPlayer = target.position.x - transform.position.x;
        float direction;

        if (distanceToPlayer > 0)
        {
            if (!isLookingRight)
            {
                isLookingRight = true;
                transform.Rotate(0f, 180, 0f);
            }
            direction = 1;
        }
        else 
        {
            if (isLookingRight)
            {
                isLookingRight = false;
                transform.Rotate(0f, 180, 0f);
            }
            direction = -1;
        }

        if (Mathf.Abs(distanceToPlayer) < 3.5)
        {
            anim.SetBool("isRunning", false);
            AttackPlayer();
        }
        else
        {
            if (!isAttacking && canMove)
            {
                rb.velocity = new Vector2(direction * 8, rb.velocity.y);
                anim.SetBool("isRunning", true);
            }
            
        }
    }

    void CanMove()
    {
        canMove = true;
    }

    void CantMove()
    {
        canMove = false;
    }

    void AttackPlayer()
    {
        isAttacking = true;
        canMove = false;
        anim.SetBool("isAttacking", isAttacking);
        // Play attack animation
        //anim.SetTrigger("Attack");
    }

    // Function called by the animator of the enemy in the last frame of attack and the last frame of the hurt animation (to not attack if im already out of range)
    void StopAttacking()
    {
        isAttacking = false;
        canMove = true;
        anim.SetBool("isAttacking", isAttacking);
    }

    void OnDrawGizmos()
    {
        if (playerCheck == null)
            return;

        Gizmos.DrawWireSphere(playerCheck.position, playerCheckRadius);
    }
}
