using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public float damage = 1;

    public bool knockback = false;
    public float knockbackPower = 5.0f;

    public void InitializeEnemyWeapon(float newDamage, bool newKnockback, float newKnockbackPower)
    {
        damage = newDamage;
        knockback = newKnockback;
        knockbackPower = newKnockbackPower;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If this weapon hits a player's trigger collider
        if(other.gameObject.tag == "Player")
        {
            // Deal damage to the player via their PlayerCombat component
            other.gameObject.GetComponent<PlayerCombat>().DealDamage(damage);

            // If this weapon is set to have knockback properties
            if (knockback)
            {
                // Run the knockback code
            }
            
        }
    }
}
