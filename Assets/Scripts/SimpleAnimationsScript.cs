using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimationsScript : MonoBehaviour
{
    public float squishDuration = 2.0f; // Duration of the squish animation
    public float squishAmount = 0.5f;   // Amount to squish the sprite

    private Vector3 originalScale;
    public IEnumerator SquishAnimation(SpriteRenderer spriteRenderer)
    {
        float elapsedTime = 0f;
        Vector3 originalSpriteSize = spriteRenderer.size;

        while (elapsedTime < squishDuration)
        {
            float t = elapsedTime / squishDuration;
            Vector3 newScale = originalScale;
            newScale.y -= Mathf.Lerp(0f, squishAmount, t); // Squish the Y scale

            transform.localScale = newScale;

            // Adjust the SpriteRenderer's size to match the squished scale
            spriteRenderer.size = new Vector2(originalSpriteSize.x, originalSpriteSize.y - Mathf.Lerp(0f, squishAmount, t));

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the final scale and SpriteRenderer size are set to the squished state
        Vector3 finalScale = originalScale;
        finalScale.y -= squishAmount;
        transform.localScale = finalScale;

        spriteRenderer.size = new Vector2(originalSpriteSize.x, originalSpriteSize.y - squishAmount);
    }

    public static void FadeSprite(SpriteRenderer spriteRenderer, float fadeDuration)
    {
        Color startColor = spriteRenderer.color;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
        }

        // Ensure the alpha is set to 0 when the fading is complete.
        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 0.0f);
    }
}
