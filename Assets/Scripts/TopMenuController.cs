using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopMenuController : MonoBehaviour
{
    public GameManager GM;
    
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void StartSinglePlayer()
    {
        GM.Start1PlayerGame();
    }
    public void StartTwoPlayer()
    {
        GM.Start2PlayerGame();
    }
}
