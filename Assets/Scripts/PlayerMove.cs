using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum MoveDirectionEnum{
	Forward, Back, Left, Right,
	ForwardLeft, ForwardRight, BackLeft, BackRight,
	Idle
}

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour {
	public float moveSpeed = 10.0f;
	public float rotationSpeed = 140.0f;
	public float jumpPower = 5.0f;
	public float gravity = 20.0f;

	bool jumped = false;
	Vector3 moveDirection = Vector3.zero;
	CharacterController controller;
	// The displayed model.
	public GameObject fighterModel;
	// The animator component for the model object
	public Animator fighterAnimator;
	// The CameraController script hides these meshes when in firstPerson mode.
	public SkinnedMeshRenderer fighterRenderer;
	public SkinnedMeshRenderer fighterClothRenderer;

	// Manipulated by the CameraController to communicate to this script that the camera is in first person mode
	MoveDirectionEnum moveDirectionEnum = MoveDirectionEnum.Forward;

	// Boolean flag for conditional behaviours throughout the script
	public bool firstPersonMode = false;

	void Start(){
		// Find the attached(and required) CharacterController component
		controller = GetComponent<CharacterController>();

		// Link this player up with the main camera
		//if(Camera.main.GetComponent<CameraControl>() == null)
			//Camera.main.gameObject.AddComponent<CameraControl>();
		//Camera.main.GetComponent<CameraControl>().SetFollowTarget(gameObject);
	}

	// Runs every frame
	void Update ()
	{
		// Jump is in Update() because we use GetKeyDown().
		//  -Sometimes FixedUpdate() misses GetKeyDown(). 
		if (controller.isGrounded) {
			if (Input.GetKeyDown (KeyCode.Space))
				jumped = true;
		}
	}

	// Runs every physics frame
	void FixedUpdate ()
	{
		float moveSpeedModifier = 1;

		//CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded) {
			// The keyboard presses will be converted into a Vector3 indicating direction, and multiplied by speed in GetMovementVectorBasedOnKeys()
			moveDirection = GetMovementVectorBasedOnKeys ();
			// Convert the movement vector to relate to the player's orientation
			//moveDirection = transform.TransformDirection (moveDirection);

			// If holding left shift, sprint
			if (Input.GetKey (KeyCode.LeftShift)) {
				// Increase speed modifier to go faster
				moveSpeedModifier = 2;
				// Set this float higher to blend the animations to a run
				fighterAnimator.SetFloat ("Speed", .5f);
			} else {
				// Set this float lower to blend the animations to a walk
				fighterAnimator.SetFloat ("Speed", 0);
			}

			// Modify how far the player moves this frame
			moveDirection *= moveSpeedModifier;

			// Jump was flagged in Update(), but is executed here in FixedUpdate()
			if (jumped) {
				moveDirection.y += (jumpPower * moveSpeedModifier);
				// Jump will be possible again, once the player is grounded.
				jumped = false;
			}
		}

		// IF in third person camera mode, do rotation on A&D
		if (!firstPersonMode) {
			float rotation = 0;
			if (Input.GetKey (KeyCode.A) && Input.GetMouseButton (1) == false)
				rotation -= 1;
			if (Input.GetKey (KeyCode.D) && Input.GetMouseButton (1) == false)
				rotation += 1;

			rotation = rotation * Time.fixedDeltaTime * rotationSpeed;
			transform.Rotate (0, rotation, 0);
		}

		// This is pseudo-gravity to avoid the need for a Rigidbody component
		moveDirection.y -= gravity * Time.fixedDeltaTime;
		// Move the CharacterController with math, utilizing the magic of the engine's algorithms to keep you out of other collidable objects
		controller.Move (moveDirection * Time.fixedDeltaTime);
	}

	/**
	//[ClientCallback]
	public void RunPlayerMoveFixedUpdate ()
	{
		if (!isLocalPlayer)
			return;

		if (!chatManager.IsChatfocused ()) {
			Vector3 translation = new Vector3 ();
			float rotation = 0.0f;

			if (grounded == true) {
				translation = GetMovementVectorBasedOnKeys ();
				transform.Translate (translation * Time.fixedDeltaTime);


			} else {
				Quaternion facingRotation = transform.rotation;
				transform.rotation = inertiaAngle;
				inertiaVector += GetMovementVectorBasedOnKeys().normalized * Time.fixedDeltaTime;
				transform.Translate (inertiaVector * Time.fixedDeltaTime);
				transform.rotation = facingRotation;
			}

			if (Input.GetKey (KeyCode.A) && Input.GetMouseButton (1) == false)
				rotation -= 1;
			if (Input.GetKey (KeyCode.D) && Input.GetMouseButton (1) == false)
				rotation += 1;

			rotation = rotation * Time.fixedDeltaTime * rotationSpeed;
			transform.Rotate (0, rotation, 0);
		} else if (grounded == false){
			Quaternion facingRotation = transform.rotation;
			transform.rotation = inertiaAngle;
			transform.Translate (inertiaVector * Time.fixedDeltaTime);
			transform.rotation = facingRotation;
		}
	}
	
	// Update is called once per frame
	public void RunPlayerMoveUpdate ()
	{
		if (!isLocalPlayer)
			return;

		if (Input.GetKeyDown (KeyCode.Space) && grounded == true && !chatManager.IsChatfocused ()) {
			//GetComponent<Rigidbody>().velocity = Vector3.up * jumpPower;
			grounded = false;
			inertiaVector = GetMovementVectorBasedOnKeys();
			inertiaAngle = transform.rotation;
		}
	}
	**/
	/// <summary>
	/// Converts the keyboard input into a Vector3, which is normalized, then multiplied by the moveSpeed variable and returned
	/// </summary>
	/// <returns>The movement vector based on keys.</returns>
	private Vector3 GetMovementVectorBasedOnKeys ()
	{
		Vector3 translation = new Vector3 ();

		if (Input.GetKey (KeyCode.W) || (Input.GetMouseButton (0) && Input.GetMouseButton (1)))
			translation += Vector3.forward;
		if (Input.GetKey (KeyCode.S))
			translation += Vector3.back;
		if (Input.GetKey (KeyCode.E) || (Input.GetKey (KeyCode.D) && (Input.GetMouseButton (1) || firstPersonMode)))
			translation += Vector3.right;
		if (Input.GetKey (KeyCode.Q) || (Input.GetKey (KeyCode.A) && (Input.GetMouseButton (1) || firstPersonMode)))
			translation += Vector3.left;

		// Normalizes (sets the magnitude of the vector to 1) then multiplies by the moveSpeed
		translation = translation.normalized * moveSpeed;

		// If the movement isn't naught, flag the model to animate walking
		if (translation != Vector3.zero) {
			if (fighterAnimator != null) fighterAnimator.SetBool("Walking", true);
		} else {
			// Otherwise flag as not walking
			if (fighterAnimator != null) fighterAnimator.SetBool("Walking", false);
		}

		// These determine the move direction in one of 9 ways, then orients the fighter model accordingly
		SetMoveDirectionEnumBasedOnVector(translation);
		if(fighterModel != null) ReOrient_Fighter_Model();

		return translation;
	}

	/// <summary>
	/// This sets the moveDirectionEnum value to a direction for use in orienting the visual model
	/// </summary>
	/// <param name="translation">Translation.</param>
	void SetMoveDirectionEnumBasedOnVector (Vector3 translation)
	{
		// Strafe Left, Strafe Right, or Idle
		if (translation.z == 0) {
			// Idle
			if (translation.x == 0) {
				moveDirectionEnum = MoveDirectionEnum.Idle;
			}
			if (translation.x > 0) {
				moveDirectionEnum = MoveDirectionEnum.Right;
			}
			if (translation.x < 0) {
				moveDirectionEnum = MoveDirectionEnum.Left;
			}
		} else {
			// Going Forward
			if (translation.z > 0) {
				// Idle
				if (translation.x == 0) {
					moveDirectionEnum = MoveDirectionEnum.Forward;
				}
				if (translation.x > 0) {
					moveDirectionEnum = MoveDirectionEnum.ForwardRight;
				}
				if (translation.x < 0) {
					moveDirectionEnum = MoveDirectionEnum.ForwardLeft;
				}
			} else {
				// Going Backward
				if (translation.z < 0) {
					// Idle
					if (translation.x == 0) {
						moveDirectionEnum = MoveDirectionEnum.Back;
					}
					if (translation.x > 0) {
						moveDirectionEnum = MoveDirectionEnum.BackRight;
					}
					if (translation.x < 0) {
						moveDirectionEnum = MoveDirectionEnum.BackLeft;
					}
				}
			}
		}
	}

	/// <summary>
	/// Orients the model so that it faces the direction of movement
	/// </summary>
	void ReOrient_Fighter_Model ()
	{
		float baseRotationOffset = (20.0f);
		if (baseRotationOffset < 0)
			baseRotationOffset = 0;

		switch (moveDirectionEnum) {
		case MoveDirectionEnum.ForwardLeft:
			fighterModel.transform.localRotation = Quaternion.identity;
			fighterModel.transform.Rotate (0, baseRotationOffset, 0);
			fighterModel.transform.Rotate (0, -45, 0);
			break;
		case MoveDirectionEnum.Forward:
			fighterModel.transform.localRotation = Quaternion.identity;
			fighterModel.transform.Rotate (0, baseRotationOffset, 0);
			break;
		case MoveDirectionEnum.ForwardRight:
			fighterModel.transform.localRotation = Quaternion.identity;
			fighterModel.transform.Rotate (0, baseRotationOffset, 0);
			fighterModel.transform.Rotate (0, 45, 0);
			break;
		case MoveDirectionEnum.Left:
			fighterModel.transform.localRotation = Quaternion.identity;
			fighterModel.transform.Rotate (0, baseRotationOffset, 0);
			fighterModel.transform.Rotate (0, -90, 0);
			break;
		case MoveDirectionEnum.Idle:
			//if (GetComponent<Combat> () != null) {
			//	if (GetComponent<Combat> ().autoAttacking) {
					//fighterModel.transform.localRotation = Quaternion.identity;
					//.transform.Rotate (0, baseRotationOffset, 0);
			//	}
			//}
			break;
		case MoveDirectionEnum.Right:
				fighterModel.transform.localRotation = Quaternion.identity;
				fighterModel.transform.Rotate(0, baseRotationOffset, 0);
				fighterModel.transform.Rotate(0, 90, 0);
			break;
		case MoveDirectionEnum.BackLeft:
				fighterModel.transform.localRotation = Quaternion.identity;
				fighterModel.transform.Rotate(0, baseRotationOffset, 0);
				fighterModel.transform.Rotate(0, -135, 0);
			break;
		case MoveDirectionEnum.Back:
				fighterModel.transform.localRotation = Quaternion.identity;
				fighterModel.transform.Rotate(0, baseRotationOffset, 0);
				fighterModel.transform.Rotate(0, 180, 0);
			break;
		case MoveDirectionEnum.BackRight:
				fighterModel.transform.localRotation = Quaternion.identity;
				fighterModel.transform.Rotate(0, baseRotationOffset, 0);
				fighterModel.transform.Rotate(0, 135, 0);
			break;
			default:

			break;
		}
	}
	//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
}
