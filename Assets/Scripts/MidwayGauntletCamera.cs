using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidwayGauntletCamera : MonoBehaviour {
	public float maxDistanceBeforeSplit = 5.0f;

	public Transform player1Transform, player2Transform, player3Transform, player4Transform;
	Quaternion startRotation = Quaternion.identity;
	float startHeight = 10.0f;
    float zOffset = -10.3f;

	public enum PlayerNumberToFollow {
		FollowPlayerOne,
		FollowPlayerTwo,
		FollowPlayerThree,
		FollowPlayerFour
	}
	public PlayerNumberToFollow playerToFollow = PlayerNumberToFollow.FollowPlayerOne;


	public GameObject cameraMask, p1Image, canvas;

	// Use this for initialization
	void Start () {
		startRotation = transform.rotation;
		startHeight = transform.position.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 newCamPosition = Vector3.zero;
        // Calculate the distance
        if (player2Transform != null)
        {
            float distance = Vector3.Distance(player1Transform.position, player2Transform.position);
            
            // If this player is within ~5~ units of the other player.
            if (distance < maxDistanceBeforeSplit)
            {
                p1Image.transform.parent = canvas.transform;

                float p1groundedY = player1Transform.GetComponent<PlayerMovement>().groundedYHeight;
                float p2groundedY = player2Transform.GetComponent<PlayerMovement>().groundedYHeight;
                Vector3 p1groundedPosition = new Vector3(player1Transform.position.x, p1groundedY, player1Transform.position.z);
                Vector3 p2groundedPosition = new Vector3(player2Transform.position.x, p2groundedY, player2Transform.position.z);

                switch (playerToFollow)
                {
                    case PlayerNumberToFollow.FollowPlayerOne:
                        newCamPosition = Vector3.MoveTowards(p1groundedPosition, p2groundedPosition, distance / 2);
                        newCamPosition.y = p1groundedY;
                        
                        transform.position = (newCamPosition + (Vector3.up * startHeight) - (Vector3.forward * 10));
                        transform.Translate(0, -(p1groundedY), 0, Space.World);

                        transform.rotation = startRotation;
                        break;

                    case PlayerNumberToFollow.FollowPlayerTwo:
                        //Calculate the halfway point between the two players
                        newCamPosition = Vector3.MoveTowards(p1groundedPosition, p2groundedPosition, distance / 2);
                        // Position the camera at that halfway point, plus an offset.
                        transform.position = (newCamPosition + (Vector3.up * startHeight) - (Vector3.forward * 10));
                        // Drop the camera a bit to make it jive perfectly with a splitting camera.
                        transform.Translate(0, -(p2groundedY), 0, Space.World);
                        // Reset the rotation to the normal gameplay angle
                        transform.rotation = startRotation;
                        break;
                }
            }
            else
            {
                float cameraZModifier = 0;
                float boogy = ((distance - maxDistanceBeforeSplit) / maxDistanceBeforeSplit);
                if (boogy > .2f) boogy = .2f;
                switch (playerToFollow)
                {
                    case PlayerNumberToFollow.FollowPlayerTwo:
                        // Positions the camera between the
                        transform.position = player1Transform.position;
                        // Offsets jump weirdness
                        transform.Translate(0, player2Transform.position.y - transform.position.y, 0, Space.World);
                        transform.LookAt(player2Transform);
                        transform.Translate(0, startHeight, maxDistanceBeforeSplit / 2);
                        transform.Translate(-(Vector3.forward * 10), Space.World);

                        cameraZModifier = (player1Transform.position.z - player2Transform.position.z) * boogy;
                        if (cameraZModifier > 4) cameraZModifier = 4;
                        if (cameraZModifier < -2) cameraZModifier = -2;
                        transform.Translate(0, -player2Transform.position.y, (cameraZModifier) * boogy, Space.World);


                        //deltaVector = Vector3.MoveTowards(player2Transform.position, player1Transform.position, maxDistanceBeforeSplit/2);
                        //transform.position += (deltaVector + (Vector3.up * startHeight));


                        p1Image.transform.parent = canvas.transform;
                        cameraMask.transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.y);
                        p1Image.transform.parent = cameraMask.transform;

                        transform.rotation = startRotation;
                        break;

                    case PlayerNumberToFollow.FollowPlayerOne:
                        transform.position = player2Transform.position;
                        //offsets jump weirdness
                        transform.Translate(0, player1Transform.position.y - transform.position.y, 0, Space.World);
                        transform.LookAt(player1Transform);
                        transform.Translate(0, startHeight, maxDistanceBeforeSplit / 2);
                        transform.Translate(-(Vector3.forward * 10), Space.World);

                        cameraZModifier = (player2Transform.position.z - player1Transform.position.z) * boogy;
                        if (cameraZModifier > 4) cameraZModifier = 4;
                        if (cameraZModifier < -2) cameraZModifier = -2;
                        transform.Translate(0, -player1Transform.position.y, (cameraZModifier) * boogy, Space.World);

                        //deltaVector = Vector3.MoveTowards(player2Transform.position, player1Transform.position, maxDistanceBeforeSplit/2);
                        //transform.position += (deltaVector + (Vector3.up * startHeight));


                        transform.rotation = startRotation;
                        break;
                }
            }
        } else
        {
            // Single player camera code
            if (player1Transform != null)
            {
                float groundedY = player1Transform.gameObject.GetComponent<PlayerMovement>().groundedYHeight;
                transform.position = player1Transform.position;
                
                transform.Translate(0, startHeight - (player1Transform.position.y - groundedY), zOffset, Space.World);
            }
        }
	}
}
