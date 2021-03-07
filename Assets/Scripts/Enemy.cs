using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator anim;
    public GameObject effect;
    public GameObject blood;

    private Rigidbody2D rb;

    public int maxHealth = 100;
    int currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }


    public void TakeDamage(int damage, int damageDirection)
    {
        currentHealth -= damage;
        anim.SetTrigger("Hurt");

        if (currentHealth > 0)
        {
            Vector2 forceToAdd = new Vector2(5 * damageDirection, 10);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
        
        else if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Die animation
        anim.SetBool("isDead", true);
        Instantiate(effect, transform.position, Quaternion.identity);
        Instantiate(blood, transform.position, Quaternion.identity);

        // Disable the enemy
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<EnemyFollower>().enabled = false;
        this.enabled = false;
    }

    
}
