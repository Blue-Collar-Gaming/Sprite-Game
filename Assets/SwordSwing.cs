using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwingType { NotSwinging, BasicSwing, SwingBack }
public class SwordSwing : MonoBehaviour
{
    // PlayerCombat will call a SwordSwing() function, and that function will set the player state to swinging via playerController.
    public PlayerController playerController;
    // swordAnimator is the conduit in which SwordSwing controls the visual swinging of the sword
    SwordAnimator swordAnimator;
    // This prefab is spawned and used for the sword's trigger interactions with other GameObjects via its PlayerWeapon tag
    public GameObject swordCollider;
    public Collider swordColliderTrigger;

    [SerializeField]
    float swing1Time = .5f;
    float swing1Timer = 0.0f;

    SwingType currentSwingType;
    PlayerMovement playerMovement;

    // This is the PlayerWeapon component that controls the combat effects of the sword object
    public PlayerWeapon swordWeaponComponent;

    [Space(10)]
    public float basicDamage = 1;
    public float currentDamage = 1;

    public bool knockBack = true;
    public float knockBackPower = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        swordAnimator = GetComponent<SwordAnimator>();
        swordColliderTrigger.enabled = false;
        currentSwingType = SwingType.NotSwinging;
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    public void RunSwordSwingUpdate()
    {
        // We rotate the sword trigger prefab by 45 degrees over the duration of Swing1Time
        switch (currentSwingType)
        {
            case SwingType.NotSwinging:
                playerController.SetPlayerStateToNormal();
                break;

            case SwingType.BasicSwing:
                swing1Timer += Time.deltaTime;
                swordCollider.transform.Rotate(0, ((Time.deltaTime * 90) / swing1Time), 0);

                if(swing1Timer > swing1Time)
                {
                    playerController.SetPlayerStateToNormal();
                    swordColliderTrigger.enabled = false;
                }
                break;

            case SwingType.SwingBack:

                break;
        }
    }

    public void SwordSwing1()
    {
        // Run the animation
        swordAnimator.PlaySwordSwing1();
        // Let the player Controller know that we are now swinging
        playerController.SetPlayerStateToSwinging();
        // Flag our current swing state for the swing update function
        currentSwingType = SwingType.BasicSwing;
        // Set the swing timer
        swing1Timer = 0.0f;
        // Prepare the sword collider to swing
        SwingSwordCollider(playerMovement.GetDirection());
        // Set the damage for Sword Swing 1
        currentDamage = 1;
        // Initialize the weapon
        swordWeaponComponent.SetWeaponStats(basicDamage, true, 1);

    }

    void SwingSwordCollider(Facing playerDirection)
    {
        swordCollider.transform.rotation = Quaternion.identity;

        switch (playerDirection)
        {
            case Facing.East:
                swordColliderTrigger.enabled = true;
                break;

            case Facing.SouthEast:
                swordCollider.transform.Rotate(0, 45, 0);
                swordColliderTrigger.enabled = true;
                break;

            case Facing.South:
                swordCollider.transform.Rotate(0, 90, 0);
                swordColliderTrigger.enabled = true;
                break;

            case Facing.SouthWest:
                swordCollider.transform.Rotate(0, 135, 0);
                swordColliderTrigger.enabled = true;
                break;

            case Facing.West:
                swordCollider.transform.Rotate(0, 180, 0);
                swordColliderTrigger.enabled = true;
                break;

            case Facing.NorthWest:
                swordCollider.transform.Rotate(0, 225, 0);
                swordColliderTrigger.enabled = true;
                break;

            case Facing.North:
                swordCollider.transform.Rotate(0, 270, 0);
                swordColliderTrigger.enabled = true;
                break;

            case Facing.NorthEast:
                swordCollider.transform.Rotate(0, 315, 0);
                swordColliderTrigger.enabled = true;
                break;
        }
    }
}
