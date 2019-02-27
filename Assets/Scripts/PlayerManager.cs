using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    enum GameMode
    {
        InMenus,
        OnePlayer,
        TwoPlayer
    }
    GameMode gameMode;

    // The prefab object that will be the controlled player
    public GameObject playerPrefab;
    // The default starting position for player 1
    public Vector3 player1StartPosition = new Vector3(-8, 1.5f, 0);
    // The default starting position for player 2 - just to the right of player 1
    public Vector3 player2StartPosition = new Vector3(-6, 1.5f, 0);
    // The cameras
    public MidwayGauntletCamera player1Camera, player2Camera;

    // The GameObject variables to store players 1 and 2
    GameObject player1, player2;

    private void Start()
    {
        // PlayerManager exists in the Meta Scene, and therefore starts when the game starts.
        //   - The game initially goes into the menus and shifts out from there.
        gameMode = GameMode.InMenus;
    }
    public void LoadPlayerForSinglePlayer()
    {
        // Actually creates the player objects
        player1 = (GameObject)Instantiate(playerPrefab, player1StartPosition, Quaternion.identity);
        // Assigns the PlayerController playerID for input
        player1.GetComponent<PlayerController>().SetAsPlayer1();
        // Assigns the transforms for the Midway Gauntlet Cameras' player 1 tracking
        player1Camera.player1Transform = player1.transform;
        player2Camera.player1Transform = player1.transform;
        // Flags the game mode as single player
        gameMode = GameMode.OnePlayer;
    }
    public void LoadPlayersForTwoPlayer()
    {
        player1 = (GameObject)Instantiate(playerPrefab, player1StartPosition, Quaternion.identity);
        player2 = (GameObject)Instantiate(playerPrefab, player2StartPosition, Quaternion.identity);
        // Assigns the PlayerController playerIDs for differentiating input
        player1.GetComponent<PlayerController>().SetAsPlayer1();
        player2.GetComponent<PlayerController>().SetAsPlayer2();
        // Assigns the transforms for the Midway Gauntlet Cameras' player 1 tracking
        player1Camera.player1Transform = player1.transform;
        player2Camera.player1Transform = player1.transform;
        // Assigns the transforms for the Midway Gauntlet Cameras' player 2 tracking
        player1Camera.player2Transform = player2.transform;
        player2Camera.player2Transform = player2.transform;
        // Flags the game mode as two player
        gameMode = GameMode.TwoPlayer;
    }
    public void UnloadPlayers()
    {
        if (player1 != null) Destroy(player1);
        if (player2 != null) Destroy(player2);
        gameMode = GameMode.InMenus;
    }
}
