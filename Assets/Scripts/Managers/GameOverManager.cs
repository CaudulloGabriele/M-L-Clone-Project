using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Manages what happens during a Game Over and after it
/// </summary>
public class GameOverManager : MonoBehaviour
{

    #region Variables

    //reference to the DataManager
    private DataManager dataManager;

    //reference to the Animator of the GameOverScreen
    private Animator anim;
    //reference to the CanvasGroup of the GameOverScreen
    private CanvasGroup canvasGroup;

    //indicates how much time to wait before showing the GameOverScreen
    [SerializeField]
    private float waitBeforeShowingGameOver = 3f;
    private int waitBeforeShowingGameOverMilliseconds;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        //gets the references of the GameOverScreen components
        anim = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        //calculates the time to wait before showing the GameOverScreen in milliseconds
        waitBeforeShowingGameOverMilliseconds = (int)(waitBeforeShowingGameOver * 1000);

    }

    private void Start()
    {
        //obtains the reference to the DataManager
        dataManager = PermanentRefs.instance.GetDataManager();

    }

    #endregion

    /// <summary>
    /// Allows to set if the game is in GameOver or not
    /// </summary>
    /// <param name="state"></param>
    /// <param name="sceneToLoad"></param>
    public async void SetGameOverState(bool state, string sceneToLoad = "")
    {
        //sets the current game state to the desired state
        GameStateManager.SetIfGameIsOver(state);

        //if the desired state is game over, waits before showing the GameOverScreen
        if (state) await Task.Delay(waitBeforeShowingGameOverMilliseconds);

        //executes the right animation based on the desired state of game over
        string triggerName = state ? "GameOver" : "GameReturn";
        anim.SetTrigger(triggerName);

        //allows to interact or not with the GameOverScreen's buttons based on the desired state of game over
        canvasGroup.blocksRaycasts = state;

        //if the desired state is not of GameOver, loads the desired scene
        if (!state) SceneChange.StaticLoadThisScene(sceneToLoad, true);

    }

    #region Button Methods

    /// <summary>
    /// Makes the player return to the last save point
    /// </summary>
    public void RetryFromLastSave()
    {
        string retrySceneName = SceneChange.GetSceneNameByIndex(dataManager.lastSaveScene);
        SetGameOverState(false, retrySceneName);

    }
    /// <summary>
    /// Makes the player return to the MainMenu
    /// </summary>
    public void ReturnToMainMenu() { SetGameOverState(false, "MainMenu"); }

    #endregion

}
