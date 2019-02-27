using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Stats playerStats = new Stats(10, 1);
    PlayerController playerController;

    [Space(10)]
    // This Sprite Renderer is rapidly enabled and disabled for the temporary invulnerability effect
    [Tooltip("This Sprite Renderer is rapidly enabled and disabled for the temporary invulnerability effect")]
    public SpriteRenderer playerSprite;
    // These variables handle the invulnerability mechanic
    public float invulnerabilityTimeAfterGettingHit = 1.5f;
    bool invulnerable = false;
    float invulnerabilityTimer = 0.0f;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // This is only called to visually flicker the player sprite in the event that he is invulnerable after recently being hit
    private void Update()
    {
        // If the player was recently hit, this will be true
        if (invulnerable)
        {
            // This only runs if the player is in a Normal state, as different animations take place in different states
            if (playerController.GetPlayerState() == PlayerState.Normal) {
                // Flicker the player sprite
                playerSprite.enabled = !playerSprite.enabled;
                // Increment the countdown timer
                invulnerabilityTimer += Time.deltaTime;
                // If the timer is up
                if (invulnerabilityTimer > invulnerabilityTimeAfterGettingHit)
                {
                    // Ensure the player sprite is visible
                    playerSprite.enabled = true;
                    // Disable invulnerability
                    invulnerable = false;
                }
            }
        }
    }

    public void DealDamage(float damage)
    {
        if (PlayerIsDamageAble())
        {
            // Calculate the damage taken after the defense stat is applied
            damage = CalculateDamageAfterDefenses(damage);

            playerStats.hitPoints -= damage;
            if(playerStats.hitPoints <= 0)
            {
                playerController.KillPlayer();
            }

            invulnerabilityTimer = 0.0f;
            invulnerable = true;
        }
    }

    float CalculateDamageAfterDefenses(float initialDamage)
    {
        float modifiedDamage = initialDamage;
        float modifier = 1.0f;

        if(playerStats.defense < 10)
        {
            modifier = 1 - (.05f * playerStats.defense);
            modifiedDamage = initialDamage * modifier;
        } else
        {
            modifier = (10 / 10 + playerStats.defense);
            modifiedDamage = initialDamage * modifier;
        }

        return modifiedDamage;
    }

    /// <summary>
    /// This function determines if the player can be damage based on the player's current state and returns true if they can be damaged
    /// </summary>
    /// <returns></returns>
    bool PlayerIsDamageAble()
    {
        bool damageable = true;

        PlayerState currentState = playerController.GetPlayerState();

        switch (currentState)
        {
            case PlayerState.Dead:
                damageable = false;
                break;

            case PlayerState.Normal:
                damageable = true;
                break;

            case PlayerState.Teleporting:
                damageable = false;
                break;
        }

        // Finally, if the player was recently hit, this will be true for the time specified by the invulnerabilityTimeAfterGettingHit float
        //  - The player will be blinking and undamageable because of this line of code, regardless of player State
        if (invulnerable) damageable = false;

        return damageable;
    }
}
