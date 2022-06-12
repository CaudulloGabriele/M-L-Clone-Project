using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages how scenes are loaded
/// </summary>
public class SceneChange : MonoBehaviour
{

    private static readonly DataManager dataManager;
    private static DataManager DataManag
    {

        get
        {

            //if (!dataManager) PermanentRefs.instance.GetDataManager();

            return PermanentRefs.instance.GetDataManager();

        }

    }

    private static string previouslyLoadedSceneName = "StartScene";


    /// <summary>
    /// Permette di caricare una scena tramite nome
    /// </summary>
    /// <param name="staticSceneName"></param>
    public static void StaticLoadThisScene(string staticSceneName, bool additive = false)
    {
        //comunicates to the GameStateManager that a scene is being loaded
        GameStateManager.OnSceneLoad(staticSceneName == "MainMenu");

        //if the scene is being loaded additively, the previously loaded one gets unloaded
        if (additive) UnloadPreviousScene();

        //loads the desired scene
        SceneManager.LoadScene(staticSceneName, (additive ? LoadSceneMode.Additive : LoadSceneMode.Single));

        //lets the time run normally in case it was paused
        Time.timeScale = 1;

        previouslyLoadedSceneName = staticSceneName;

        //Debug.LogWarning("PREVIOUSLY LOADED SCENE NAME: " + previouslyLoadedSceneName);
        //Debug.Log("Caricata scena di nome " + staticSceneName);
    }

    /// <summary>
    /// Permette di caricare una scena tramite buildIndex
    /// </summary>
    /// <param name="staticSceneIndex"></param>
    public static void StaticLoadThisScene(int staticSceneIndex, bool additive = false)
    {

        GameStateManager.OnSceneLoad(staticSceneIndex == 1);

        if (additive) UnloadPreviousScene();

        SceneManager.LoadScene(staticSceneIndex, (additive ? LoadSceneMode.Additive : LoadSceneMode.Single));
        Time.timeScale = 1;

        previouslyLoadedSceneName = GetSceneNameByIndex(staticSceneIndex);

        //Debug.LogWarning("PREVIOUSLY LOADED SCENE NAME: " + previouslyLoadedSceneName);
        //Debug.Log("Caricata scena ad indice " + staticSceneIndex);
    }
    /// <summary>
    /// Permette di caricare una scena tramite nome
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadThisScene(string sceneName, bool additive = false) { StaticLoadThisScene(sceneName, additive); }
    /// <summary>
    /// Permette di caricare una scena tramite buildIndex
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void LoadThisScene(int sceneIndex, bool additive = false) { StaticLoadThisScene(sceneIndex, additive); }
    /// <summary>
    /// Permette di caricare la stessa scena in cui si è
    /// </summary>
    public void ReloadScene() { StaticLoadThisScene(gameObject.scene.name); }
    /// <summary>
    /// Carica una scena additivamente
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void AddNewScene(int sceneIndex) { StaticLoadThisScene(sceneIndex, true); }
    /// <summary>
    /// Toglie la scena precedentemente caricata
    /// </summary>
    private static void UnloadPreviousScene()
    {

        Scene previousScene = SceneManager.GetSceneByName(previouslyLoadedSceneName);

        if (previousScene.buildIndex == 0) return;

        SceneManager.UnloadSceneAsync(previousScene);

        Debug.Log("UNLOADED SCENE: " + previouslyLoadedSceneName + " -> " + previousScene.name);
    }
    /// <summary>
    /// Returns the name of the scene of the desired index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static string GetSceneNameByIndex(int index) { return SceneManager.GetSceneByBuildIndex(index).name; }
    /// <summary>
    /// Returns the index of the currently loaded scene
    /// </summary>
    /// <returns></returns>
    public static int GetCurrentlyLoadedSceneIndex() { return SceneManager.GetSceneByName(previouslyLoadedSceneName).buildIndex; }

    public void QuitGame()
    {
        Application.Quit();
        //Debug.Log("Esce dal gioco");
    }

}
