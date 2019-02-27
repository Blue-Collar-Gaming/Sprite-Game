using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SceneController sceneController;
    public PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        sceneController.LoadTopMenuScene();
    }

    public void Start1PlayerGame()
    {
        sceneController.TransitionFromTopMenuToGame();
        playerManager.LoadPlayerForSinglePlayer();
    }
    public void Start2PlayerGame()
    {
        sceneController.TransitionFromTopMenuToGame();
        playerManager.LoadPlayersForTwoPlayer();
    }
}
