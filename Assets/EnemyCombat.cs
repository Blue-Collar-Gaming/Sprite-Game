using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float health = 1;
    public float defense = 1;

    bool isAlive = true;

    [Space(10)]
    public SpriteRenderer rendererToHide;
    public SpriteRenderer secondaryRendererToHide;
    public SpriteRenderer deathRenderer;
    public Animator deathAnimator;

    private void Start()
    {
        deathRenderer.enabled = false;
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(amount);
        if(health <= 0)
        {
            // Enemy Dies
            KillEnemy();
        }
    }

    void KillEnemy()
    {
        if (isAlive)
        {
            rendererToHide.enabled = false;
            if (secondaryRendererToHide != null) secondaryRendererToHide.enabled = false;
            deathRenderer.enabled = true;
            deathAnimator.SetTrigger("Die");

            isAlive = false;

            // Temporary destroy for death
            Destroy(gameObject, 1.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "PlayerWeapon":

                break;
        }
    }

    public bool IsEnemyAlive()
    {
        return isAlive;
    }
}
