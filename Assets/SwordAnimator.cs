using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAnimator : MonoBehaviour
{
    Animator swordAnimator;

    // This component is used to retrieve the facing direction of the player for use in the direction-ambiguous PlaySwordSwing# functions
    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        swordAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /**
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlaySwordSwing2();
        } **/
    }

    /// <summary>
    /// This function retrieves the player's Facing direction from playerMovement
    /// then calls the appropriate Swing1 animation trigger function
    /// </summary>
    public void PlaySwordSwing1()
    {
        switch (playerMovement.GetDirection())
        {
            case Facing.East:
                PlaySwordSwingE1();
                break;

            case Facing.SouthEast:
                PlaySwordSwingSE1();
                break;

            case Facing.South:
                PlaySwordSwingS1();
                break;

            case Facing.SouthWest:
                PlaySwordSwingSW1();
                break;

            case Facing.West:
                PlaySwordSwingW1();
                break;

            case Facing.NorthWest:
                PlaySwordSwingNW1();
                break;

            case Facing.North:
                PlaySwordSwingN1();
                break;

            case Facing.NorthEast:
                PlaySwordSwingNE1();
                break;
        }
    }
    /// <summary>
    /// This function retrieves the player's Facing direction from playerMovement
    /// then calls the appropriate Swing2 animation trigger function
    /// </summary>
    public void PlaySwordSwing2()
    {
        switch (playerMovement.GetDirection())
        {
            case Facing.East:
                PlaySwordSwingE2();
                break;

            case Facing.SouthEast:
                PlaySwordSwingSE2();
                break;

            case Facing.South:
                PlaySwordSwingS2();
                break;

            case Facing.SouthWest:
                PlaySwordSwingSW2();
                break;

            case Facing.West:
                PlaySwordSwingW2();
                break;

            case Facing.NorthWest:
                PlaySwordSwingNW2();
                break;

            case Facing.North:
                PlaySwordSwingN2();
                break;

            case Facing.NorthEast:
                PlaySwordSwingNE2();
                break;
        }
    }

    public void PlaySwordSwingE1()
    {
        swordAnimator.SetTrigger("SwordSwingE1");
    }
    public void PlaySwordSwingE2()
    {
        swordAnimator.SetTrigger("SwordSwingE2");
    }
    public void PlaySwordSwingSE1()
    {
        swordAnimator.SetTrigger("SwordSwingSE1");
    }
    public void PlaySwordSwingSE2()
    {
        swordAnimator.SetTrigger("SwordSwingSE2");
    }
    public void PlaySwordSwingS1()
    {
        swordAnimator.SetTrigger("SwordSwingS1");
    }
    public void PlaySwordSwingS2()
    {
        swordAnimator.SetTrigger("SwordSwingS2");
    }
    public void PlaySwordSwingSW1()
    {
        swordAnimator.SetTrigger("SwordSwingSW1");
    }
    public void PlaySwordSwingSW2()
    {
        swordAnimator.SetTrigger("SwordSwingSW2");
    }
    public void PlaySwordSwingW1()
    {
        swordAnimator.SetTrigger("SwordSwingW1");
    }
    public void PlaySwordSwingW2()
    {
        swordAnimator.SetTrigger("SwordSwingW2");
    }
    public void PlaySwordSwingNW1()
    {
        swordAnimator.SetTrigger("SwordSwingNW1");
    }
    public void PlaySwordSwingNW2()
    {
        swordAnimator.SetTrigger("SwordSwingNW2");
    }
    public void PlaySwordSwingN1()
    {
        swordAnimator.SetTrigger("SwordSwingN1");
    }
    public void PlaySwordSwingN2()
    {
        swordAnimator.SetTrigger("SwordSwingN2");
    }
    public void PlaySwordSwingNE1()
    {
        swordAnimator.SetTrigger("SwordSwingNE1");
    }
    public void PlaySwordSwingNE2()
    {
        swordAnimator.SetTrigger("SwordSwingNE2");
    }
}
