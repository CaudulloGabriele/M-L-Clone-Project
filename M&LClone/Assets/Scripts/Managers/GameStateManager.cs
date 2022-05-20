//Si occupa di gestire lo stato di gioco
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    //crea un enumeratore per i possibili stati di gioco
    private enum GameState { exploring = 0, fighting = 1, paused = 2 };
    //indica lo stato corrente di gameplay
    private static GameState currentState = GameState.exploring;
    //indica lo stato in cui il gioco si trovava prima di essere messo in pausa
    private static GameState beforePauseState;

    //riferimento statico al bottone per caricare i dati, da attivare solo quando ci si trova nel menù principale
    private static GameObject staticLoadSaveButton;
    private static PlayerBattleManager staticPbm;

    //riferimento al bottone per caricare i dati, da attivare solo quando ci si trova nel menù principale
    [SerializeField]
    private GameObject loadSaveButton;

    [SerializeField]
    private PlayerBattleManager pbm;


    private void Awake()
    {
        //ottiene i riferimenti statici
        staticLoadSaveButton = loadSaveButton;
        staticPbm = pbm;

        //carica la scena di gameplay, additivamente
        SceneChange.StaticLoadThisScene(1, true);

    }

    /// <summary>
    /// Inizializza varie cose quando viene caricata una scena, in base alla scena
    /// </summary>
    public static void OnSceneLoad(bool loadedMainMenu)
    {

        staticLoadSaveButton.SetActive(loadedMainMenu);

        if (!loadedMainMenu)
        {

            DataManager.instance.SaveDataAfterUpdate(DataManager.GetCurrentlyLoadedSlotName());

            staticPbm.GetSavedPlayerStats();
            staticPbm.CalculatePlayerStats();
            Debug.Log("NONNOO");
        }

    }
    /// <summary>
    /// Permette di mettere il gioco in pausa, o di farne uscire
    /// </summary>
    /// <param name="pauseState"></param>
    public static void SetPauseState(bool pauseState)
    {
        //se il gioco deve essere messo in pausa, e non lo è già...
        if (pauseState && !IsGamePaused())
        {
            //...salva lo stato in cui era il gioco prima di metterlo in pausa...
            beforePauseState = currentState;
            //...e mette il gioco in pausa
            currentState = GameState.paused;

        } //altrimenti, torna allo stato precedente alla pausa
        else { currentState = beforePauseState; }

    }
    /// <summary>
    /// Permette di impostare se il gioco è in stato di combattimento o meno
    /// </summary>
    /// <param name="isFighting"></param>
    public static void SetFightingState(bool isFighting)
    {
        
        currentState = isFighting ? GameState.fighting : GameState.exploring;

        if (isFighting) { staticPbm.CalculatePlayerStats(); Debug.Log("IS FIGHTING"); }
    
    }
    /// <summary>
    /// Comunica se in questo momento il gioco è in stato esplorativo
    /// </summary>
    /// <returns></returns>
    public static bool IsPlayerExploring() { return (currentState == GameState.exploring); }
    /// <summary>
    /// Comunica se in questo momento il gioco è in stato di combattimento
    /// </summary>
    /// <returns></returns>
    public static bool IsPlayerFighting() { return (currentState == GameState.fighting); }
    /// <summary>
    /// Comunica se in questo momento il gioco è in pausa
    /// </summary>
    /// <returns></returns>
    public static bool IsGamePaused() { return (currentState == GameState.paused); }

}
