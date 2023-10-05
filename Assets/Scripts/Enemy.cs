using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public float range;
    public Transform target;
    public float minDistance = 5f;
    public float maxDistance = 10f;
    private bool targetCollision = false;
    private float speed = 2f;

    public SimpleAnimationsScript animationsScript;

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
    }

    void Update()
    {
        FollowTarget();
    }

    public void TakeDamage(int damage) {

        currentHealth -= damage;

        //Hurt animation

        if(currentHealth <= 0) 
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");
        //die animation

        SimpleAnimationsScript.FadeSprite(GetComponent<SpriteRenderer>(),4000.0f);

        //diable enemy
    }

    public int GetHealth() { return currentHealth; }

    public void FollowTarget()
    {
        range = Vector2.Distance(transform.position, target.position);

        if (range < maxDistance)
        {
            if (range < minDistance)
            {
                if (!targetCollision)
                {
                    // Calculate the direction vector from the enemy to the target
                    Vector3 moveDirection = (target.position - transform.position).normalized;

                    // Calculate speed based on the distance to the target
                    //float adjustedSpeed = Mathf.Lerp(0, speed, range / maxDistance);

                    // Move the enemy in the calculated direction with adjusted speed
                    //transform.Translate(moveDirection * adjustedSpeed * Time.deltaTime, Space.World);
                    transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
                }
            }
        }
    }

    public int damageAmount = 20; // Amount of damage to deal to the target
    public float backwardForce = 5f; // Force to apply to the enemy when colliding with the target


    //Not detecting collision fix later
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    // Check if the collision is with the target
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        // Get the CharacterScript component from the target GameObject
    //        CharacterScript targetScript = collision.gameObject.GetComponent<CharacterScript>();

    //        // Check if the targetScript is not null
    //        if (targetScript != null)
    //        {
    //            // Deal damage to the target using the CharacterScript
    //            DealDamage(targetScript);

    //            // Calculate the backward force direction (away from the target)
    //            Vector2 backwardDirection = (transform.position - collision.transform.position).normalized;

    //            // Apply the backward force to the enemy
    //            GetComponent<Rigidbody2D>().AddForce(backwardDirection * backwardForce, ForceMode2D.Impulse);
    //        }
    //    }
    //}

    //private void DealDamage(CharacterScript target)
    //{
    //    // Deal damage to the target using the CharacterScript
    //    target.TakeDamage(damageAmount);
    //}


}
