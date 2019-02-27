using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public string topMenuScene = "Top Menu Scene";
    public string terrainScene = "Terrain Scene";

    // The menu scene load and unload functions
    public void LoadTopMenuScene()
    {
        SceneManager.LoadSceneAsync(topMenuScene, LoadSceneMode.Additive);
    }
    public void UnloadTopMenuScene()
    {
        SceneManager.UnloadSceneAsync(topMenuScene);
    }

    // Loads the base terrain that underlies the entire game world
    public void LoadTerrainScene()
    {
        // This has to be LoadScene and not LoadSceneAsync, or the player falls under the world before it loads
        SceneManager.LoadScene(terrainScene, LoadSceneMode.Additive);
    }
    public void UnloadTerrainScene()
    {
        SceneManager.UnloadSceneAsync(terrainScene);
    }

    // A meta scene function that unloads the top menus and loads the terrain scene
    public void TransitionFromTopMenuToGame()
    {
        UnloadTopMenuScene();
        LoadTerrainScene();
    }
}
