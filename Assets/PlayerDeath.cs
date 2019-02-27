using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    // Death has 2 phases
    //  - First, the animation takes place
    //  - Second, the game waits for the player to press a button to respawn
    enum DeathState { NotDead, Animating, WaitingOnInput }
    DeathState deathState = DeathState.NotDead;

    // The driver class
    PlayerController player;

    // The death animation sprite is much taller than the normal movement sprites
    //  - As a result, the normal sprite is hidden during the teleportation process
    //  - and a second sprite is enabled and used instead
    public SpriteRenderer playerSprite;
    public SpriteRenderer playerDeathSprite;
    public Animator playerDeathAnimator;

    // The timer before the menu displays
    float deathAnimationTime = 3.0f;
    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        // Hide the death sprite until it's time to die
        playerDeathSprite.enabled = false;
    }

    // Update is called once per frame
    public void PlayerDeathUpdate()
    {
        switch (deathState)
        {
            case DeathState.NotDead:

                break;

            case DeathState.Animating:
                timer += Time.deltaTime;
                if (timer > deathAnimationTime)
                {
                    // Move to WaitingOnInput
                }

                break;

            case DeathState.WaitingOnInput:

                break;
        }
    }

    public void KillPlayer()
    {
        // Hide the movement sprite,
        playerSprite.enabled = false;
        // and show the death sprite,
        playerDeathSprite.enabled = true;
        // and trigger the death animation
        playerDeathAnimator.SetTrigger("Death Trigger");
        // Reset the timer
        timer = 0.0f;
        // This effectively starts the timer, as it modifies behavior in PlayerrDeathUpdate()
        deathState = DeathState.Animating;
    }

    void DisplayRespawnMenu()
    {

    }
}
