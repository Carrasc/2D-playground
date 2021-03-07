using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator anim;
    public Transform attackPoint;
    [SerializeField] private Transform wallCheck;   // A position marking where to check if the player is touching a wall

    public float attackRange = 1f;
    public int attackDamage = 40;
    public int dashDamage = 100;
    public float attackRate = 2f; // How many attacks per second
    public float dashDistance = 8f;

    bool isAttacking = false;
    float nextAttackTime = 0f;
   
    public LayerMask enemyLayers;

    private AudioSource source;
    public AudioClip swordSound;
    public AudioClip swordHitSound;
    public AudioClip dashSound;

    private static int facingDirection;

    void Start()
    {
        source = GetComponent<AudioSource>();
        facingDirection = CharacterController2D.facingDirection;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                isAttacking = true;
                anim.SetBool("isAttacking", isAttacking);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

    }

    // This function is called as an event by the animation, at the moment of the actual attack frame
    void Attack()
    {
        // Detect enemies in range of attack 
        Collider2D [] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        if (hitEnemies.Length == 0)
        {
            source.clip = swordSound;
            source.Play();
        }
        else if (hitEnemies.Length > 0)
        {
            source.clip = swordHitSound;
            source.Play();
        }

        // Apply damage to enemies in range 
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage, CharacterController2D.facingDirection);
        }
    }

    public void DashAttack()
    {
        // Detect enemies in range of attack 
        RaycastHit2D[] hitEnemies = Physics2D.RaycastAll(wallCheck.position, transform.right, dashDistance, enemyLayers);

        if (hitEnemies.Length > 0)
        {
            source.clip = dashSound;
            source.Play();
        }

        // Apply damage to enemies in range 
        foreach (RaycastHit2D enemy in hitEnemies)
        {
            enemy.collider.GetComponent<Enemy>().TakeDamage(dashDamage, CharacterController2D.facingDirection);
        }
    }

    // This function is called as an event by the animation, when the last frame is done
    private void AttackFinished()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
    }

    void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;
             
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
