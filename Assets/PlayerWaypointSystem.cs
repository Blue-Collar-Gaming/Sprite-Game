using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWaypointSystem : MonoBehaviour
{
    // These are inspector-assigned objects related to the waypoint system
    public PlayerController player;
    public Text titleText;
    public Text waypoint1Text;
    public Text waypoint2Text;
    public Text waypoint3Text;
    public Text waypoint4Text;
    public Image waypointMenuBackdropImage;
    public Image waypoint1SelectionImage;
    public Image waypoint2SelectionImage;
    public Image waypoint3SelectionImage;
    public Image waypoint4SelectionImage;

    // The current selection index
    int selection = 1;
    // Is the menu open? boolean
    bool menuOpen = false;
    // Player's ID - assigned at ShowWaypointMenu - used for menu input
    int playerID = 0;
    // Key tap cooldown - the player must release their joystick to keep tapping down.
    bool rightStickReleased = true;
    // The current waypoint - Set when a player enters a waypoint trigger
    Waypoint currentWaypoint;

    void Start()
    {
        HideWaypointMenu();
        playerID = player.playerID;
    }

    /// <summary>
    /// This is meant to be run every frame from the PlayerController's NormalStateUpdate function 
    /// whenever the Waypoint System menu is open.  
    /// </summary>
    public void RunWaypointMenuUpdate()
    {
        // Right-stick or arrows navigate the waypoint menu
        RunMenuTraversal();

        // The jump button becomes a select button
        RunMenuSelection();
    }
    // This is meant to read the right joystick and modify the current selection in the menu
    //  - It is called as a part of RunWaypointMenuUpdate()
    void RunMenuTraversal()
    {
        // If the right stick is tapped up or down, move the selection in that direction
        if ((Input.GetAxis("P" + playerID + " Right Stick Vertical") < -0.2f) && rightStickReleased)
        {
            selection++;
            // Stops at the bottom selection
            if (selection > 4) selection = 4;
            HighlightSelection();
            // Prevents spam movement
            rightStickReleased = false;
        }
        else if ((Input.GetAxis("P" + playerID + " Right Stick Vertical") > 0.2f) && rightStickReleased)
        {
            selection--;
            // Stops at the top selection
            if (selection < 1) selection = 1;
            HighlightSelection();
            // Prevents spam movement
            rightStickReleased = false;
        }
        else if ((Input.GetAxis("P" + playerID + " Right Stick Vertical")) == 0)
        {
            rightStickReleased = true;
        }
    }
    // This is meant to listen for button 0.  If pressed, begins the waypoint teleport process
    //  - It is called as a part of RunWaypointMenuUpdate()
    void RunMenuSelection()
    {
        // If Button 0 is pressed
        if (Input.GetAxis("P" + playerID + " Button 0") > 0.2f)
        {
            // Track the yHeight difference between the waypoint and the player position
            // - This is important to keep the player from ending up in the ground, and using this
            // -   allows other waypoints to be at higher or lower levels of terrain
            float heightOffset = player.transform.position.y - currentWaypoint.transform.position.y;
            // This switch uses the selection to determine the target waypoint, chosen from the ley line
            switch (selection)
            {
                // The function reaches back to the PlayerController class, and triggers teleportation
                //   via the WaypointTeleport() call.
                //   -  Teleportation ultimately takes place via the PlayerTeleport component
                case 1:
                    player.WaypointTeleport(currentWaypoint.leyLine.waypoint1, heightOffset);
                    break;
                case 2:
                    player.WaypointTeleport(currentWaypoint.leyLine.waypoint2, heightOffset);
                    break;
                case 3:
                    player.WaypointTeleport(currentWaypoint.leyLine.waypoint3, heightOffset);
                    break;
                case 4:
                    player.WaypointTeleport(currentWaypoint.leyLine.waypoint4, heightOffset);
                    break;
            }
            HideWaypointMenu();

            // Triggers the waypoint animation of the waypoint that triggered this menu
            currentWaypoint.ActivateWaypoint();
        }
    }

    // Returns whether the waypoint menu is currently being displayed
    // - This is used by the PlayerController script to determine whether button 0 is for jumping or waypoint selection
    public bool IsWaypointMenuOpen()
    {
        return menuOpen;
    }

    /// <summary>
    /// This function hides all components related to the menu display
    /// </summary>
    public void HideWaypointMenu()
    {
        menuOpen = false;
        selection = 1;

        titleText.enabled = false;
        waypoint1Text.enabled = false;
        waypoint2Text.enabled = false;
        waypoint3Text.enabled = false;
        waypoint4Text.enabled = false;

        waypointMenuBackdropImage.enabled = false;
        waypoint1SelectionImage.enabled = false;
        waypoint2SelectionImage.enabled = false;
        waypoint3SelectionImage.enabled = false;
        waypoint4SelectionImage.enabled = false;
    }
    /// <summary>
    /// This function shows the core menu components, then highlights the default selection
    /// </summary>
    /// <param name="enteredWaypoint"></param>
    public void ShowWaypointMenu(Waypoint enteredWaypoint)
    {
        currentWaypoint = enteredWaypoint;

        //playerID = player.playerID;
        menuOpen = true;

        titleText.enabled = true;
        waypoint1Text.enabled = true;
        waypoint2Text.enabled = true;
        waypoint3Text.enabled = true;
        waypoint4Text.enabled = true;

        waypointMenuBackdropImage.enabled = true;
        HighlightSelection();
    }
    // This function hides all of the selection backdrops, then shows one based on the "selection" variable
    void HighlightSelection()
    {
        waypoint1SelectionImage.enabled = false;
        waypoint2SelectionImage.enabled = false;
        waypoint3SelectionImage.enabled = false;
        waypoint4SelectionImage.enabled = false;

        switch (selection)
        {
            case 1:
                waypoint1SelectionImage.enabled = true;
                break;

            case 2:
                waypoint2SelectionImage.enabled = true;
                break;

            case 3:
                waypoint3SelectionImage.enabled = true;
                break;

            case 4:
                waypoint4SelectionImage.enabled = true;
                break;
        }
    }
}
