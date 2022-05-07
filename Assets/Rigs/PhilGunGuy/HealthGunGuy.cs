using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGunGuy : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
            //animator.SetBool("Hit", true);
            //if (currentHealth <= 0)
            //animator.SetBool("Death", true);
        }
    }
    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
    private void OnCollisionEnter(Collision collision)
    {
            Debug.Log(collision.collider.name);
    }
}
