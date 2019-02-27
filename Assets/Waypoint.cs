using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    // This name will be for descriptive flavor names in the Waypoint Menu
    public string waypointName;
    // The Waypoint Ley Line tracks which waypoints are connected to each other
    public WaypointLeyLine leyLine;
    // This is for triggering the waypoint's activation effects
    Animator waypointAnimator;

    private void Start()
    {
        waypointAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// Activates one round of the waypoint activation animation
    /// </summary>
    public void ActivateWaypoint()
    {
        waypointAnimator.SetTrigger("Activate");
    }

    /// <summary>
    /// Activate one round of the waypoint reverse activation animation
    /// </summary>
    public void ReverseActivateWaypoint()
    {
        waypointAnimator.SetTrigger("Reverse Activate");
    }

    // Upon entering the waypoint center, the Waypoint System Menu shows itself
    private void OnTriggerEnter(Collider other)
    {
        // Only perform this on players
        if(other.gameObject.tag == "Player")
        {
            // If the player isn't teleporting, then display the waypoint menu
            //  - This prevents the menu from showing as you port in to a new waypoint
            if (!other.gameObject.GetComponent<PlayerTeleport>().IsTeleporting())
            {
                // Initializes and shows the Waypoint menu for the player that entered this trigger
                other.gameObject.GetComponentInChildren<PlayerWaypointSystem>().ShowWaypointMenu(this);
            }
        }
    }
    // Upon leaving the waypoint, the Waypoint System Menu hides itself
    private void OnTriggerExit(Collider other)
    {
        // Only perform this on players
        if (other.gameObject.tag == "Player")
        {
            // Hide the waypoint menu for the player that exited this trigger
            other.gameObject.GetComponentInChildren<PlayerWaypointSystem>().HideWaypointMenu();
        }
    }
}
