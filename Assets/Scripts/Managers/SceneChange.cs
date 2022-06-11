using System.Threading.Tasks;
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


    /// <summary>
    /// Permette di caricare una scena tramite nome
    /// </summary>
    /// <param name="staticSceneName"></param>
    public async static void StaticLoadThisScene(string staticSceneName, bool additive = false)
    {

        UnloadMainMenu();

        

        await Task.Delay(1);

        GameStateManager.OnSceneLoad(staticSceneName == "MainMenu");


        SceneManager.LoadScene(staticSceneName, (additive ? LoadSceneMode.Additive : LoadSceneMode.Single));
        Time.timeScale = 1;

        int sceneIndex = SceneManager.GetSceneByName(staticSceneName).buildIndex;

        if (sceneIndex > 1) DataManag.lastSaveScene = sceneIndex;


        //Debug.Log("Caricata scena di nome " + staticSceneName);
    }

    /// <summary>
    /// Permette di caricare una scena tramite buildIndex
    /// </summary>
    /// <param name="staticSceneIndex"></param>
    public async static void StaticLoadThisScene(int staticSceneIndex, bool additive = false)
    {

        UnloadMainMenu();

        

        await Task.Delay(1);
        GameStateManager.OnSceneLoad(staticSceneIndex == 1);

        SceneManager.LoadScene(staticSceneIndex, (additive ? LoadSceneMode.Additive : LoadSceneMode.Single));
        Time.timeScale = 1;

        if (staticSceneIndex > 1) DataManag.lastSaveScene = staticSceneIndex;

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
