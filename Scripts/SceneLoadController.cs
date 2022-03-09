using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadController : MonoBehaviour
{
    private string nameOfSceneToLoad;
    private string nameOfCurrentScene;
    public string nameOfFirstMapScene;

    // Load first map scene, persistent scene is already loaded
    void Start()
    {
        LoadMapScene(nameOfFirstMapScene);
    }

    // Set callback for loading new map scene, start unloading
    public void LoadNewMapScene(string nameOfSceneToLoad)
    {
        SceneManager.sceneUnloaded += MapSceneUnloadFinished;
        this.nameOfSceneToLoad = nameOfSceneToLoad;
        SceneManager.UnloadSceneAsync(nameOfCurrentScene);
    }

    // Remove callback, call load for new map scene
    private void MapSceneUnloadFinished(Scene unloadedScene)
    {
        SceneManager.sceneUnloaded -= MapSceneUnloadFinished;
        if (!SceneManager.GetSceneByName(nameOfSceneToLoad).isLoaded)
        {
            LoadMapScene(nameOfSceneToLoad);
        }
    }

    // Set currentSceneName, load a map scene in additive mode to keep persistent scene
    private void LoadMapScene(string nameOfSceneToLoad)
    {
        nameOfCurrentScene = nameOfSceneToLoad;
        SceneManager.LoadScene(nameOfSceneToLoad, LoadSceneMode.Additive);
    }
}
