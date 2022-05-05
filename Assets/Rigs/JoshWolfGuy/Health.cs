using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public float maxHealth;
    public float health;

    public GameObject healthBarUI;
    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.value = CalculateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = CalculateHealth();
        if (health <= 0)
        {
            // Input Death animation
            Destroy(gameObject);
            Debug.Log("I'm Dead!");
        }
        if (health > maxHealth) // You can have no more health than your max, sir.
        {
            health = maxHealth;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            health -= 10;
        }
    }
    float CalculateHealth()
    {
        return health / maxHealth;
    }

     void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            Debug.Log("We hit a bullet!");
            health -= 1;
        }
    }
}
