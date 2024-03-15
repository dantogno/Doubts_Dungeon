using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSurface : MonoBehaviour
{
    [SerializeField]
    float DMGInterval;

    [SerializeField]
    int Damage;

    [SerializeField]
    float EnvSpeed;

    [SerializeField]
    bool CanTakeDamage = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && CanTakeDamage)
        {
            DamagePlayer(collision);
        }

        collision.gameObject.GetComponent<PlayerMovementNew>().Speed = EnvSpeed;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && CanTakeDamage)
        {
            DamagePlayer(collision);
        }
    }
    void DamagePlayer(Collision player)
    {
        //player.gameObject.GetComponent<Player>().TakeDamage(Damage);
        player.gameObject.GetComponent<Player>().IncreaseStress(Damage);
        CanTakeDamage = false;
        StartCoroutine(DamageInterval());
    }
    
    private IEnumerator DamageInterval()
    {
        yield return new WaitForSeconds(DMGInterval);
        CanTakeDamage = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.gameObject.GetComponent<PlayerMovementNew>().Speed = 8;
    }
}
