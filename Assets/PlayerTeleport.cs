using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    // These help manage the timing of the teleport sequence 
    //   and outside queries as to the state of teleportation
    enum TeleportState { NotTeleporting, TeleportingOut, TeleportingIn }
    TeleportState teleportState = TeleportState.NotTeleporting;

    // The driver class
    PlayerController player;

    // The teleport sprite is much taller than the normal movement sprites
    //  - As a result, the normal sprite is hidden during the teleportation process
    //  - and a second sprite is enabled and used instead
    public SpriteRenderer playerSprite;
    public SpriteRenderer playerTeleportSprite;
    public Animator playerTeleportAnimator;

    // This is the time between phases of the teleportation animation
    float teleportOutTime = 1.5f;
    // The timer to track timings
    float timer = 0.0f;

    // The target location to teleport to
    Vector3 targetLocation = Vector3.zero;
    // The target waypoint - used to trigger the Reverse Activation animation
    Waypoint targetTeleportWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        // Hide the teleportation sprite until it's time to teleport
        playerTeleportSprite.enabled = false;
    }

    /// <summary>
    ///  This function is called by the Update method in PlayerController
    ///  - Only when the playerState is Teleporting
    /// </summary>
    public void PlayerTeleportUpdate()
    {
        // Add the deltaTime to the timer
        if(teleportState != TeleportState.NotTeleporting)   timer += Time.deltaTime;

        // Once time is up on the current phase, move to the next phase
        if (timer > teleportOutTime) {
            switch (teleportState)
            {
                // When the Teleport Out phase is complete, 
                //   flip (hidden) player to new waypoint and Teleport In
                case TeleportState.TeleportingOut:
                    TeleportIn();
                    break;
                // When the player has fully teleported in, return to normal controls
                case TeleportState.TeleportingIn:
                    ExitTeleportStatus();
                    break;
            }
        }
    }

    /// <summary>
    /// This function is called by the PlayerController method "WaypointTeleport()"
    ///  - This function initializes and begins the teleport process
    /// </summary>
    /// <param name="targetWaypoint"></param>
    /// <param name="heightOffset"></param>
    public void WaypointTeleport(Waypoint targetWaypoint, float heightOffset)
    {
        // Used to trigger the Reverse Activation animation in TeleportIn()
        targetTeleportWaypoint = targetWaypoint;
        // Where the player will ultimately end up - just above the target waypoint
        targetLocation = (targetWaypoint.transform.position + (Vector3.up * heightOffset));
        // Initialize and begin the visuals and timer aspects of the teleportation process
        TeleportOut();
    }

    /// <summary>
    /// This is a simple query class, used by the Waypoint class to check if the player is teleporting in
    ///  when entering the trigger.  If the player enters via teleport, the waypoint menu will not display
    /// </summary>
    /// <returns></returns>
    public bool IsTeleporting()
    {
        bool teleporting = true;
        if (teleportState == TeleportState.NotTeleporting) teleporting = false;
        return teleporting;
    }

    /// <summary>
    /// TeleportOut is the process of teleporting away from the waypoint that triggered your menu
    ///  - This hides the movement sprite, and shows the teleport sprite
    ///  - This triggers the teleport sprite to teleport out and starts the timer.
    /// </summary>
    void TeleportOut()
    {
        // Hide the movement sprite,
        playerSprite.enabled = false;
        // and show the teleport sprite,
        playerTeleportSprite.enabled = true;
        // and trigger the teleportation animation
        playerTeleportAnimator.SetTrigger("Teleport Out");
        // Reset the timer
        timer = 0.0f;
        // This effectively starts the timer, as it modifies the behavior in PlayerTeleportUpdate()
        teleportState = TeleportState.TeleportingOut;
    }
    /// <summary>
    /// TeleportIn is the process of arriving at your destination waypoint
    /// - The player is moved to the destination waypoint, and the teleport in animation is triggered
    /// </summary>
    void TeleportIn()
    {
        // Flag for PlayerTeleportUpdate() to indicate the next phase
        teleportState = TeleportState.TeleportingIn;
        // Move the player to the destination waypoint
        transform.position = targetLocation;
        // Trigger the teleport-in animation
        playerTeleportAnimator.SetTrigger("Teleport In");
        // Start an arrival animation on the destination waypoint
        targetTeleportWaypoint.ReverseActivateWaypoint();
        // Reset the timer variable
        timer = 0.0f;
    }

    /// <summary>
    /// At the end of the teleport-in process, returns the player to normal operation
    /// </summary>
    void ExitTeleportStatus()
    {
        // Make the waypoint menus show again
        teleportState = TeleportState.NotTeleporting;
        // Switch the sprites back
        playerSprite.enabled = true;
        playerTeleportSprite.enabled = false;
        // Re-enable movement controls
        player.SetPlayerStateToNormal();
    }
}
