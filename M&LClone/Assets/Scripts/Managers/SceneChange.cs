//Script per bottoni che devono portare ad una nuova scena
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    /// <summary>
    /// Permette di caricare una scena tramite nome
    /// </summary>
    /// <param name="staticSceneName"></param>
    public static void StaticLoadThisScene(string staticSceneName, bool additive = false)
    {

        UnloadMainMenu();

        SceneManager.LoadScene(staticSceneName, (additive ? LoadSceneMode.Additive : LoadSceneMode.Single));
        Time.timeScale = 1;

        GameStateManager.OnSceneLoad(staticSceneName == "MainMenu");

        int sceneIndex = SceneManager.GetSceneByName(staticSceneName).buildIndex;

        if (sceneIndex > 1) DataManager.lastSaveScene = sceneIndex;

        //Debug.Log("Caricata scena di nome " + staticSceneName);
    }

    /// <summary>
    /// Permette di caricare una scena tramite buildIndex
    /// </summary>
    /// <param name="staticSceneIndex"></param>
    public static void StaticLoadThisScene(int staticSceneIndex, bool additive = false)
    {

        UnloadMainMenu();

        SceneManager.LoadScene(staticSceneIndex, (additive ? LoadSceneMode.Additive : LoadSceneMode.Single));
        Time.timeScale = 1;

        GameStateManager.OnSceneLoad(staticSceneIndex == 1);

        if (staticSceneIndex > 1) DataManager.lastSaveScene = staticSceneIndex;

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
    /// Toglie la scena del menù principale
    /// </summary>
    private static void UnloadMainMenu()
    {

        Scene mainMenuScene = SceneManager.GetSceneByName("MainMenu");
        if (mainMenuScene.buildIndex == 1) SceneManager.UnloadSceneAsync(mainMenuScene);

    }

    public void QuitGame()
    {
        Application.Quit();
        //Debug.Log("Esce dal gioco");
    }

}
