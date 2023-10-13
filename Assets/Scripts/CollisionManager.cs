using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    //public List<Enemy> enemies;

    //public PlayerMovement player;

    //public Vector3 PlayerPosition()
    //{
    //    return player.transform.position;
    //}

    //public void HandlePlayerCollisionWithEnemy(PlayerMovement player)
    //{
    //    if (player.CanTakeDamage())
    //    {
    //        player.StartCoroutine(player.DamageCooldown());
    //        player.TakeDamage(1);
    //    }
    //}

    //public void HandleWeaponCollisionWithEnemies(Collider other)
    //{
    //    // Check if the collision is with a weapon
    //    if (weaponCollider.CompareTag("Weapon"))
    //    {
    //        // Iterate through enemies and apply damage if they collide with the weapon
    //        foreach (var enemy in enemies)
    //        {
    //            if (IsColliding(enemy.transform.position, weaponCollider.transform.position))
    //            {
    //                enemy.TakeDamage(1);
    //            }
    //        }
    //    }
    //}

    //public void HandleProjectileCollisionWithEnemy(GameObject enemyObject)
    //{
    //    // You may want to handle the specific behavior when a projectile hits an enemy
    //    // For now, we'll just destroy the enemy

    //    // Assuming your Enemy script has a method to handle damage and destruction
    //    Enemy enemy = enemyObject.GetComponent<Enemy>();
    //    if (enemy != null)
    //    {
    //        enemy.TakeDamage(1);
    //    }
    //}

    //private bool IsColliding(Vector3 positionA, Vector3 positionB)
    //{
    //    // You can implement your collision detection logic here.
    //    // For simplicity, we'll use a basic distance check.
    //    float distance = Vector3.Distance(positionA, positionB);
    //    return distance < 1.0f; // Adjust the threshold as needed.
    //}
}
