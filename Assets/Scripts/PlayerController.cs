using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Normal,
    Swinging,
    Teleporting,
    Dead
}

// The PlayerController class is meant to the the primary driver class of the player - The main loop
//  - All other components and sub-systems related to the player should be run through the playerController
//  - This needs to be the only class on the player with an active Update call.
//  --  Behavior is determined by a switch(playerState) in Update() and FixedUpdate()
public class PlayerController : MonoBehaviour
{
    // This identifies the player and is used to modify the strings for input Axis checks
    //  - This means playerID 1 works the the first gamepad plugged in, and playerID 2 with the second
    public int playerID = 1;
    
    // These three variables are components and are the drivers of their given mechanics.
    // This component controls your player's movement around the immediate game world.
    PlayerMovement playerMovement;
    // This component is attached to a world space canvas that is a child of the Player object
    //  - The waypoint menu triggers PlayerTeleport via WaypointTeleport() in this class
    PlayerWaypointSystem playerWaypointSystem;
    // This component is used to trigger the teleport animations during Waypoint activity.
    PlayerTeleport playerTeleport;
    // This component is used to handle the Death mechanics
    PlayerDeath playerDeath;
    // PlayerCombat is used for controlling weapon attacks and handling damage calculations
    PlayerCombat playerCombat;
    // This player state is used to determine how the player can be controlled by the player at any given time
    PlayerState playerState = PlayerState.Normal;
    // This is used for all things concerned with sword mechanics
    SwordSwing swordSwing;

    // Start is called before the first frame update - Collect the components
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerWaypointSystem = GetComponentInChildren<PlayerWaypointSystem>();
        playerTeleport = GetComponent<PlayerTeleport>();
        playerDeath = GetComponent<PlayerDeath>();
        playerCombat = GetComponent<PlayerCombat>();
        swordSwing = GetComponentInChildren<SwordSwing>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerState)
        {
            case PlayerState.Normal:
                NormalStateUpdate();
            break;

            case PlayerState.Teleporting:
                playerTeleport.PlayerTeleportUpdate();
            break;

            case PlayerState.Dead:
                playerDeath.PlayerDeathUpdate();
            break;

            case PlayerState.Swinging:
                swordSwing.RunSwordSwingUpdate();
            break;
        }
        
    }

    private void FixedUpdate()
    {
        switch (playerState)
        {
            case PlayerState.Normal:
                NormalStateFixedUpdate();
            break;

            case PlayerState.Teleporting:
               
            break;

            case PlayerState.Swinging:
                SwingingStateFixedUpdate();
            break;
        }
    }

    void NormalStateUpdate()
    {
        if (playerWaypointSystem.IsWaypointMenuOpen())
        {
            // Runs the waypoint menu controls
            playerWaypointSystem.RunWaypointMenuUpdate();
        }
        else
        {
            // This reads Button 0 for jumping
            playerMovement.PlayerMovementUpdate(playerID);
            // This reads button 1 for sword attacking
            playerCombat.RunPlayerCombatUpdate(playerID);
        }
    }
    void NormalStateFixedUpdate()
    {
        // This reads the X and y axes on the joystick
        playerMovement.PlayerMovementFixedUpdate(playerID);
    }

    void SwingingStateUpdate()
    {

    }
    void SwingingStateFixedUpdate()
    {
        // Move like this was the normal state
        NormalStateFixedUpdate();
    }

    // These are used as initialization functions when the PlayerManager instantiates player prefabs
    public void SetAsPlayer1()
    {
        playerID = 1;
    }
    public void SetAsPlayer2()
    {
        playerID = 2;
    }

    /// <summary>
    /// This function sets the player state back to normal
    /// </summary>
    public void SetPlayerStateToNormal()
    {
        playerState = PlayerState.Normal;
    }
    
    public void SetPlayerStateToSwinging()
    {
        playerState = PlayerState.Swinging;
    }

    // This is a pass-through command that flags the central PlayerController to run the teleport sequence
    //   and initializes the PlayerTeleport mechanics with the received Waypoint information
    public void WaypointTeleport(Waypoint targetWaypoint, float heightOffset)
    {
        playerState = PlayerState.Teleporting;
        playerTeleport.WaypointTeleport(targetWaypoint, heightOffset);
    }
    // This is a pass-through command that flags the PlayerController to run the death mechanic update
    //   and triggers the death mechanics via the PlayerDeath component
    public void KillPlayer()
    {
        // Flags PlayerController to run playerDeath.PlayerDeathUpdate()
        playerState = PlayerState.Dead;
        // Initializes the death sequence
        playerDeath.KillPlayer();
    }

    public PlayerState GetPlayerState()
    {
        return playerState;
    }
}
