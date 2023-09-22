using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponColliderScript : MonoBehaviour
{
    public GameObject player; //loosing my mind

    // Use OnDrawGizmos to draw the collider in the Scene view
    private void OnDrawGizmos()
    {
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();

        if (collider != null)
        {
            Gizmos.color = Color.yellow;

            // Calculate the world position of the capsule collider's center
            Vector3 center = transform.TransformPoint(collider.offset);

            // Calculate the world positions of the top and bottom ends of the capsule
            Vector3 top = center + new Vector3(0f, collider.size.y / 2f, 0f);
            Vector3 bottom = center - new Vector3(0f, collider.size.y / 2f, 0f);

            // Draw the capsule using Gizmos
            Gizmos.DrawWireSphere(top, collider.size.x / 2f);
            Gizmos.DrawWireSphere(bottom, collider.size.x / 2f);
            Gizmos.DrawLine(top, bottom);
        }
    }

    private CapsuleCollider2D capsuleCollider;
    private float initialWeaponX; // Store the initial weaponX value

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        initialWeaponX = capsuleCollider.offset.x; // Store the initial X offset
    }

    private void Update()
    {
        if (capsuleCollider != null)
        {
            // Update the collider's position relative to the player
            Vector3 flippedOffset = capsuleCollider.offset;
            Vector2 flippedSize = capsuleCollider.size;

            if (!player.GetComponent<CharacterScript>().turnedRight)
            {
                // If the player is facing left, adjust the offset and size to account for the flip
                flippedOffset.x = -initialWeaponX; // Use the initial value of weaponX
                flippedSize.x = -capsuleCollider.size.x;
            }

            // Adjust the local position and size based on the player's orientation
            capsuleCollider.offset = flippedOffset;
            capsuleCollider.size = flippedSize;
        }
    }

}
