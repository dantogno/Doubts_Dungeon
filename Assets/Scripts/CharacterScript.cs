using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterScript : MonoBehaviour
{
    public int maxHealth = 200;
    public int currentHealth;
    public float speed = 5;

    CharacterController controller;
    SpriteRenderer spriteRenderer;

    public bool turnedRight = false; 

    void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<CharacterController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction based on the input
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0); // Adjust the Z component to 0
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        
        // Flip the sprite when turning right
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = true; // Flip back to the original direction
            turnedRight = true;
            Debug.Log("Turned right");
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = false; // Flip the sprite when turning right
            turnedRight = false;
            Debug.Log("Turned left");
        }

        // Move the character controller
        controller.Move(moveDirection * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {

        currentHealth -= damage;

        //Hurt animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died");
        //die animation

        SimpleAnimationsScript.FadeSprite(GetComponent<SpriteRenderer>(), 4000.0f);

        //diable enemy
    }
}

