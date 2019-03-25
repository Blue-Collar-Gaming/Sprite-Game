using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facing
{
    None,
    NorthWest, North, NorthEast, East,
    SouthWest, South, SouthEast, West
}

public enum CameraFacing
{
    North, South, East, West
}

public class PlayerMovement : MonoBehaviour {
    public float playerSpeed = 3.6f;
    public float jumpPower = 5.0f;
    public float gravity = 20.0f;

    bool jumped = false;
    Vector3 moveDirection = Vector3.zero;
    CharacterController controller;

    CameraFacing cameraDirection = CameraFacing.North;
    Facing direction = Facing.South;
    public Animator thisAnimator;

    bool cameraOrbitCCW = false;
    bool cameraOrbitCW = false;
    float goalOrbitRotation = 0.0f;
    float cameraOrbitTime = 0.5f;

    public float groundedYHeight = 0.0f;
    bool jumpReady = true;

	// Use this for initialization
	void Start () {
        thisAnimator = GetComponentInChildren<Animator>();
        thisAnimator.SetBool("FacingSouth", true);

        controller = GetComponent<CharacterController>();
	}

    /// <summary>
    ///     This is used by other player mechanic functions to procedurally determine the direction the player is facing and modify the behavior accordingly
    ///      - SwordAnimator.cs calls this for the direction-ambiguous SwordSwing() function calls
    /// </summary>
    /// <returns></returns>
    public Facing GetDirection()
    {
        return direction;
    }

    public void PlayerMovementUpdate(int playerNumber)
    {
        //RunCounterClockWiseCameraOrbit();
        //RunClockWiseCameraOrbit();

        if (controller.isGrounded)
        {
            
            if (Input.GetAxis("P" + playerNumber + " Button 0") > .1f)
            {
                if (jumpReady)
                {
                    groundedYHeight = transform.position.y;
                    jumped = true;
                    jumpReady = false;
                }
            } else
            {
                jumpReady = true;
            }
        }
    }

    void RunClockWiseCameraOrbit()
    {
        if (Input.GetButtonDown("Left Bumper"))
        {
            if (cameraDirection == CameraFacing.North) cameraDirection = CameraFacing.West;
            else if (cameraDirection == CameraFacing.West) cameraDirection = CameraFacing.South;
            else if (cameraDirection == CameraFacing.South) cameraDirection = CameraFacing.East;
            else if (cameraDirection == CameraFacing.East) cameraDirection = CameraFacing.North;

            cameraOrbitCW = true;
            cameraOrbitCCW = false;
            goalOrbitRotation += -90;

        }

        if (cameraOrbitCW)
        {
            float rotationValue = (-90 * Time.deltaTime) / cameraOrbitTime;
            if (goalOrbitRotation < rotationValue)
            {
                transform.Rotate(0, rotationValue, 0, Space.World);
                goalOrbitRotation -= rotationValue;
            }
            else
            {
                transform.Rotate(0, goalOrbitRotation, 0, Space.World);
                goalOrbitRotation = 0;
                cameraOrbitCW = false;
            }
        }
    }

    void RunCounterClockWiseCameraOrbit()
    {
        if (Input.GetButtonDown("Right Bumper"))
        {
            if (cameraDirection == CameraFacing.North) cameraDirection = CameraFacing.East;
            else if (cameraDirection == CameraFacing.East) cameraDirection = CameraFacing.South;
            else if (cameraDirection == CameraFacing.South) cameraDirection = CameraFacing.West;
            else if (cameraDirection == CameraFacing.West) cameraDirection = CameraFacing.North;

            cameraOrbitCCW = true;
            cameraOrbitCW = false;
            goalOrbitRotation += 90;

        }

        if (cameraOrbitCCW)
        {
            float rotationValue = (90 * Time.deltaTime) / cameraOrbitTime;
            if (goalOrbitRotation > rotationValue)
            {
                transform.Rotate(0, rotationValue, 0, Space.World);
                goalOrbitRotation -= rotationValue;
            }
            else
            {
                transform.Rotate(0, goalOrbitRotation, 0, Space.World);
                goalOrbitRotation = 0;
                cameraOrbitCCW = false;
            }
        }
    }

    // Update is called once per frame
    public void PlayerMovementFixedUpdate (int playerNumber) {
        RunPlayerFacingDirection(playerNumber);
        if (controller.isGrounded)
        {
            moveDirection = GetMovementVectorBasedOnInput(playerNumber);
            

            if (jumped)
            {
                moveDirection.y += (jumpPower);
                // Jump will be possible again, once the player is grounded.
                jumped = false;
            }

            
        }
        moveDirection.y -= gravity * Time.fixedDeltaTime;
        controller.Move(moveDirection * Time.fixedDeltaTime * playerSpeed);
    }

    bool IsAllowedToMove()
    {
        bool allowed = true;

        if (jumped) allowed = false;

        return allowed;
    }

    void SetAnimatorDirection(Facing trueDirection)
    {
        // This catches the ninth case of no direction being pressed on the axes.
        if (trueDirection == Facing.None)
            return;

        // Set all 8 flags to false
        thisAnimator.SetBool("FacingNorth", false);
        thisAnimator.SetBool("FacingNorthEast", false);
        thisAnimator.SetBool("FacingNorthWest", false);
        thisAnimator.SetBool("FacingSouth", false);
        thisAnimator.SetBool("FacingSouthEast", false);
        thisAnimator.SetBool("FacingSouthWest", false);
        thisAnimator.SetBool("FacingEast", false);
        thisAnimator.SetBool("FacingWest", false);
        // Set the one true flag based on which Facing direction was sent
        if (trueDirection == Facing.North) thisAnimator.SetBool("FacingNorth", true);
        if (trueDirection == Facing.NorthEast) thisAnimator.SetBool("FacingNorthEast", true);
        if (trueDirection == Facing.NorthWest) thisAnimator.SetBool("FacingNorthWest", true);
        if (trueDirection == Facing.South) thisAnimator.SetBool("FacingSouth", true);
        if (trueDirection == Facing.SouthEast) thisAnimator.SetBool("FacingSouthEast", true);
        if (trueDirection == Facing.SouthWest) thisAnimator.SetBool("FacingSouthWest", true);
        if (trueDirection == Facing.East) thisAnimator.SetBool("FacingEast", true);
        if (trueDirection == Facing.West) thisAnimator.SetBool("FacingWest", true);
        // Flag to animator to respond to the change in direction
        thisAnimator.SetTrigger("NewDirection");
    }

    private Vector3 GetMovementVectorBasedOnInput()
    {
        Vector3 translation = new Vector3();

        switch (cameraDirection) {
            case CameraFacing.North:
                translation = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                Debug.Log("North");
                break;
            case CameraFacing.East:
                translation = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal"));
                Debug.Log("East");
                break;
            case CameraFacing.South:
                translation = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
                Debug.Log("South");
                break;
            case CameraFacing.West:
                translation = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
                break;
        }

        return translation;
    }

    private Vector3 GetMovementVectorBasedOnInput(int playerNumber)
    {
        Vector3 translation = new Vector3();

        switch (cameraDirection)
        {
            case CameraFacing.North:
                translation = new Vector3(Input.GetAxis("P" + playerNumber.ToString() + " Horizontal"), 0, Input.GetAxis("P" + playerNumber.ToString() + " Vertical"));
                //Debug.Log("North");
                break;
            case CameraFacing.East:
                translation = new Vector3(Input.GetAxis("P" + playerNumber + " Vertical"), 0, -Input.GetAxis("P" + playerNumber + " Horizontal"));
                //Debug.Log("East");
                break;
            case CameraFacing.South:
                translation = new Vector3(-Input.GetAxis("P" + playerNumber + " Horizontal"), 0, -Input.GetAxis("P" + playerNumber + " Vertical"));
                //Debug.Log("South");
                break;
            case CameraFacing.West:
                translation = new Vector3(-Input.GetAxis("P" + playerNumber + " Vertical"), 0, Input.GetAxis("P" + playerNumber + " Horizontal"));
                break;
        }

        return translation;
    }

    void RunPlayerFacingDirection(int playerNumber)
    {
        if ((Input.GetAxis("P" + playerNumber + " Horizontal") == 0) && (Input.GetAxis("P" + playerNumber + " Vertical") == 0))
        {
            // Flag the walking animation as false, triggering idle
            thisAnimator.SetBool("Walking", false);
        }
        else
        {


            // If the joystick is tilted to the right at all
            if (Input.GetAxis("P" + playerNumber + " Horizontal") > 0)
            {
                // If the joystick is tilted up at all
                if (Input.GetAxis("P" + playerNumber + " Vertical") > 0)
                {
                    if (direction != Facing.NorthEast)
                    {
                        direction = Facing.NorthEast;
                        SetAnimatorDirection(Facing.NorthEast);
                    }
                    thisAnimator.SetBool("Walking", true);
                }
                // If the joystick is tilted down at all
                if (Input.GetAxis("P" + playerNumber + " Vertical") < 0)
                {
                    if (direction != Facing.SouthEast)
                    {
                        direction = Facing.SouthEast;
                        SetAnimatorDirection(Facing.SouthEast);
                    }
                    thisAnimator.SetBool("Walking", true);
                }
                // If the joystick is not tilted vertically
                if (Input.GetAxis("P" + playerNumber + " Vertical") == 0)
                {
                    if (direction != Facing.East)
                    {
                        direction = Facing.East;
                        SetAnimatorDirection(Facing.East);
                    }
                    thisAnimator.SetBool("Walking", true);
                }
            }
            // If the joystick is tilted to the left at all
            if (Input.GetAxis("P" + playerNumber + " Horizontal") < 0)
            {
                // If the joystick is tilted up at all
                if (Input.GetAxis("P" + playerNumber + " Vertical") > 0)
                {
                    if (direction != Facing.NorthWest)
                    {
                        direction = Facing.NorthWest;
                        SetAnimatorDirection(Facing.NorthWest);
                    }
                    thisAnimator.SetBool("Walking", true);
                }
                // If the joystick is tilted down at all
                if (Input.GetAxis("P" + playerNumber + " Vertical") < 0)
                {
                    if (direction != Facing.SouthWest)
                    {
                        direction = Facing.SouthWest;
                        SetAnimatorDirection(Facing.SouthWest);
                    }
                    thisAnimator.SetBool("Walking", true);
                }
                // If the joystick is not tilted vertically
                if (Input.GetAxis("P" + playerNumber + " Vertical") == 0)
                {
                    if (direction != Facing.West)
                    {
                        direction = Facing.West;
                        SetAnimatorDirection(Facing.West);
                    }
                    thisAnimator.SetBool("Walking", true);
                }
            }
            // If the joystick is not tilted horizontally
            if (Input.GetAxis("P" + playerNumber + " Horizontal") == 0)
            {
                // If the joystick is tilted up at all
                if (Input.GetAxis("P" + playerNumber + " Vertical") > 0)
                {
                    if (direction != Facing.North)
                    {
                        direction = Facing.North;
                        SetAnimatorDirection(Facing.North);
                    }
                    thisAnimator.SetBool("Walking", true);
                }
                // If the joystick is tilted down at all
                if (Input.GetAxis("P" + playerNumber + " Vertical") < 0)
                {
                    if (direction != Facing.South)
                    {
                        direction = Facing.South;
                        SetAnimatorDirection(Facing.South);
                    }
                    thisAnimator.SetBool("Walking", true);
                }
            }
        }
    }
}
