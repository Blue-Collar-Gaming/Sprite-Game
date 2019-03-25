using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float damage = 1;

    public bool knockBack = true;
    public float knockBackPower = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// This sets the weapon's current damage, knockback boolean, and knockback power stats
    ///  - It should be used to accurately initialize an attack whenever a weapon attack is started
    /// </summary>
    /// <param name="currentDamage"></param>
    /// <param name="currentKnockback"></param>
    /// <param name="currentKnockbackPower"></param>
    public void SetWeaponStats(float currentDamage, bool currentKnockback, float currentKnockbackPower)
    {
        damage = currentDamage;
        knockBack = currentKnockback;
        knockBackPower = currentKnockbackPower;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                if(gameObject.tag == "Player Weapon")
                {
                    if(other.gameObject.GetComponent<EnemyCombat>() != null)
                    {
                        other.gameObject.GetComponent<EnemyCombat>().TakeDamage(damage);

                        if (knockBack)
                        {
                            if (other.gameObject.GetComponent<Rigidbody>() != null)
                            {
                                Vector3 hitbackVector = transform.position - other.ClosestPoint(transform.position);
                                hitbackVector.Normalize();

                                other.gameObject.GetComponent<Rigidbody>().AddForce(hitbackVector);
                            } else
                            {
                                Debug.LogError("No Rigidbody on Enemy, skipping knockback effects");
                            }
                        }
                    } else
                    {
                        Debug.LogError("Player weapon hit an enemy without an EnemyCombat component");
                    }
                }
                break;
        }
    } 
}
