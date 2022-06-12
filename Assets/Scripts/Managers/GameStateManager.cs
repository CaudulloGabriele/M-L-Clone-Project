using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Manages the game's states
/// </summary>
public class GameStateManager : MonoBehaviour
{
    //enum for all the possible states of the game
    private enum GameState { exploring = 0, fighting = 1, paused = 2, gameOver = 3 };
    //tells the current state of the game
    private static GameState currentState = GameState.exploring;
    //tells the state in which the game was before getting paused
    private static GameState beforePauseState;

    //static reference to the load button in the save menu, to activate only if the currently loaded scene is the MainMenu
    private static GameObject staticLoadSaveButton;
    //static reference to the player battle manager
    private static PlayerBattleManager staticPbm;

    private static DataManager dataManager;

    //reference to the load button in the save menu, to activate only if the currently loaded scene is the MainMenu
    [SerializeField]
    private GameObject loadSaveButton;
    //reference to the player's position
    private static Transform player;
    //reference to the player battle manager
    [SerializeField]
    private PlayerBattleManager pbm;

    public static bool isLoadingSave = false;


    private void Awake()
    {
        //gets the static references
        staticLoadSaveButton = loadSaveButton;
        staticPbm = pbm;

        //additively loads the MainMenu scene
        SceneChange.StaticLoadThisScene(1, true);

    }

    private void Start()
    {
        //gets the reference to the player's position
        player = PermanentRefs.instance.GetPlayer();

        dataManager = PermanentRefs.instance.GetDataManager();

    }

    /// <summary>
    /// Initializes various things when a scene is loaded, based on the scene
    /// </summary>
    public async static void OnSceneLoad(bool loadedMainMenu)
    {
        //activates or deactivates the load button based on wheter the loaded scene is the MainMenu or not
        staticLoadSaveButton.SetActive(loadedMainMenu);
        //if this is not the MainMenu...
        if (!loadedMainMenu)
        {
            //...calculates the player stats
            staticPbm.GetSavedPlayerStats();
            staticPbm.CalculatePlayerStats(true);

        }

        //if a save file is being loaded...
        if (isLoadingSave)
        {
            //...the player is positioned
            player.position = new Vector2(dataManager.savedPlayerPos[0], dataManager.savedPlayerPos[1]);
            isLoadingSave = false;

        }

        //updates the list of data updaters in the scene(after waiting for the scene to fully load)
        await Task.Delay(10);
        DataManager.UpdateListOfDataUpdaters();

    }
    /// <summary>
    /// Allows to pause or resume the game
    /// </summary>
    /// <param name="pauseState"></param>
    public static void SetPauseState(bool pauseState)
    {
        //if the game has to be paused, and is not already...
        if (pauseState && !IsGamePaused())
        {
            //...it saves the previous state before pausing...
            beforePauseState = currentState;
            //...and pauses the game
            currentState = GameState.paused;
            Time.timeScale = 0;

        } //otherwise, sets the game in the state it was before pausing
        else { currentState = beforePauseState; Time.timeScale = 1; }

    }
    /// <summary>
    /// Allows to set if the game is in fight state or not
    /// </summary>
    /// <param name="isFighting"></param>
    public static void SetFightingState(bool isFighting)
    {
        
        currentState = isFighting ? GameState.fighting : GameState.exploring;

        if (isFighting) { staticPbm.CalculatePlayerStats(true); Debug.Log("IS FIGHTING"); }
    
    }

    public static void SetIfGameIsOver(bool state)
    {

        GameState newState = state ? GameState.gameOver : GameState.exploring;
        currentState = newState;

    }
    /// <summary>
    /// Returns true if the state of the game is set to "exploring"
    /// </summary>
    /// <returns></returns>
    public static bool IsPlayerExploring() { return (currentState == GameState.exploring); }
    /// <summary>
    /// Returns true if the state of the game is set to "fighting"
    /// </summary>
    /// <returns></returns>
    public static bool IsPlayerFighting() { return (currentState == GameState.fighting); }
    /// <summary>
    /// Returns true if the game is paused
    /// </summary>
    /// <returns></returns>
    public static bool IsGamePaused() { return (currentState == GameState.paused); }
    /// <summary>
    /// Returns true if the player is in GameOver
    /// </summary>
    /// <returns></returns>
    public static bool IsGameOver() { return (currentState == GameState.gameOver); }

}
