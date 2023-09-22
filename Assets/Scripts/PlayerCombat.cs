using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Transform attackPoint;
    public float attackRange = .5f;

    public int attackDamage = 40;

    public LayerMask enemyLayers;

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Space key detected");
            Attack();
        }
    }

    void Attack() {
        //Play Attack animation

        // Detect Enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        if (hitEnemies.Length > 0)
        {
            Debug.Log($"{hitEnemies.Length} enemies detected.");
        }

        // Damage Enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            Debug.Log($"{enemy.name} is hit for {attackDamage} damage | Remaining Health: {enemy.GetComponent<Enemy>().GetHealth()}");
        }
    }

    private void OnDrawGizmosSelected() {
        
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
