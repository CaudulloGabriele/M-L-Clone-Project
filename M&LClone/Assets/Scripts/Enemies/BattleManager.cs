using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    #region Variables

    public const int N_TYPES = 10, //numero di tipi di nemici presenti nel gioco
        MAX_ENEMIES = 5, //numero massimo di nemici che possono essere in campo
        START_OF_BOSS_LIST = 7; //indice da cui parte la lista dei nemici boss

    //unica istanza di questo script nella scena
    public static BattleManager instance;

    //array di riferimenti ai nemici presenti nel gioco
    private EnemyTypesBehaviours[] allEnemies;
    private GameObject[] allEnemiesGO;
    //array di riferimenti di tutte le posizioni di combattimento dei nemici
    private Transform[] enemiesPositions;
    //riferimento al contenitore delle posizioni del giocatore
    private Transform playerPositionsContainer;
    //riferimento al contenitore dei nemici attualmente in combattimento
    private Transform activeEnemiesContainer;
    //riferimento al contenitore dei nemici già spawnati e sconfitti
    private Transform spawnedEnemiesContainer;
    //riferimento al giocatore...
    [SerializeField]
    private Transform mapPlayer, //...nella mappa
        battlePlayer; //...in battaglia

    //riferimento allo script di movimento della telecamera
    [SerializeField]
    private CameraFollow camFollow;
    //riferimento alla posizione in cui il giocatore deve essere all'inizio del combattimento
    private Transform playerFightPos;
    //indica quanto tempo bisogna aspettare per posizionare il giocatore alla posizione di combattimento
    [SerializeField]
    private float positionPlayerTimer = 1.5f;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        //imposta l'istanza, se non esiste già
        if (!instance) instance = this;
        //altrimenti, quest'istanza si distrugge
        else { Destroy(gameObject); return; }

        //prende i riferimenti di tutti nemici
        GetAllEnemies();
        //prende i riferimenti di tutti i contenitori
        GetAllContainers();
        //prende i riferimenti di tutte le posizioni
        GetAllPositions();


        //foreach (Transform t in enemiesPositions) Debug.LogError(t.name);
        //Debug.LogWarning("------------------------------------------------------------------------------");
        //foreach (Transform t in allEnemies) Debug.LogError(t.name);
    }

    #endregion

    #region Inizialization Methods

    /// <summary>
    /// Prende il riferimento ai nemici e li disattiva
    /// </summary>
    private void GetAllEnemies()
    {

        Transform allEnemiesContainer = transform.GetChild(1);
        allEnemies = allEnemiesContainer.GetComponentsInChildren<EnemyTypesBehaviours>();
        allEnemiesGO = new GameObject[allEnemies.Length];
        for (int i = 0; i < allEnemies.Length; i++) { allEnemiesGO[i] = allEnemies[i].gameObject; }

        allEnemiesContainer.gameObject.SetActive(false);

    }
    /// <summary>
    /// Ottiene i riferimenti ai contenitori di nemici attivi e previamente spawnati
    /// </summary>
    private void GetAllContainers()
    {

        activeEnemiesContainer = transform.GetChild(2);

        spawnedEnemiesContainer = transform.GetChild(3);

        playerPositionsContainer = transform.GetChild(4);

    }
    /// <summary>
    /// Ottiene i riferimenti alle posizioni dei nemici e del giocatore
    /// </summary>
    private void GetAllPositions()
    {
        //prende il riferimento a tutte le posizioni di battaglia per i nemici
        Transform allPositionsContainer = transform.GetChild(0);
        Transform[] allPositions = allPositionsContainer.GetComponentsInChildren<Transform>();
        //rimuove dall'array il riferimento al container delle posizioni
        foreach (Transform pos in allPositions)
        {
            //se la posizione controllata è invece il contenitore...
            if (pos == allPositionsContainer)
            {
                //...la rimuove dall'array ed esce dal ciclo
                var recipient = new Transform[allPositions.Length - 1];
                for (int i = 0; i < recipient.Length; i++) { recipient[i] = allPositions[i + 1]; }
                allPositions = recipient;

                break;

            }

        }
        //salva le posizioni nell'array statico
        enemiesPositions = allPositions;
        //ottiene il riferimento alla posizione in cui mettere il giocatore ad inizio battaglia
        playerFightPos = playerPositionsContainer.GetChild(0);

    }

    #endregion

    #region Fight Managment

    /// <summary>
    /// Fa partire la battaglia, mettendo i nemici in posizione
    /// </summary>
    /// <param name="enemiesType"></param>
    public void FightStart(int[] enemiesType)
    {
        //imposta lo stato di gioco a combattimento
        GameStateManager.SetFightingState(true);
        //crea un indice locale per la posizione in cui mettere il nemico
        int posIndex = 0;
        //cicla ogni tipo di nemico ricevuto...
        foreach (int enemyType in enemiesType)
        {
            //...e li posiziona nel campo di battaglia
            //allEnemies[enemyType].position = enemiesPositions[index].position;
            SpawnEnemy(enemyType, enemiesPositions[posIndex].position);
            //...e incrementa l'indice di posizione per il prossimo nemico
            posIndex++;

            Debug.Log("Spawn enemy type: " + enemyType);
        }
        //fa partire la coroutine che si occupa della tempistica di inizio battaglia
        StartCoroutine(ManageStartFightTiming());

    }
    /// <summary>
    /// Fa spawnare il nemico indicato nella posizione indicata, istanziandone uno nuovo se necessario
    /// </summary>
    /// <param name="enemyToSpawn"></param>
    /// <param name="enemyPos"></param>
    private void SpawnEnemy(int enemyToSpawn, Vector2 enemyPos)
    {
        //prende il riferimento al contenitore del nemico di quel tipo da spawnare
        Transform remainingSpawnedEnemies = spawnedEnemiesContainer.GetChild(enemyToSpawn);
        //se il contenitore contiene un nemico precedentemente istanziato...
        if (remainingSpawnedEnemies.childCount > 0)
        {
            //...ne prende il riferimento...
            Transform spawnableEnemy = remainingSpawnedEnemies.GetChild(0);
            //...e lo spawna nella posizione indicata
            spawnableEnemy.parent = activeEnemiesContainer;
            spawnableEnemy.position = enemyPos;

        } //altrimenti, istanzia un nuovo nemico di quel tipo
        else { Instantiate(allEnemiesGO[enemyToSpawn], enemyPos, Quaternion.identity, activeEnemiesContainer); }

    }
    /// <summary>
    /// Richiamato da un nemico sconfitto, controlla se è stata vinta la battaglia dopo aver tolto questo nemico
    /// </summary>
    /// <param name="enemyChildIndex"></param>
    /// <param name="enemyType"></param>
    public void AnEnemyWasDefeated(int enemyChildIndex, int enemyType)
    {
        //porta il nemico sconfitto nel contenitore dei nemici già spawnati
        Transform defeatedEnemy = activeEnemiesContainer.GetChild(enemyChildIndex);
        defeatedEnemy.parent = spawnedEnemiesContainer.GetChild(enemyType);
        //se non ci sono più nemici attivi, il giocatore ha vinto la battaglia
        if (activeEnemiesContainer.childCount == 0) { BattleWon(); }

    }
    /// <summary>
    /// Rende il giocatore vincitore della battaglia corrente
    /// </summary>
    private void BattleWon()
    {
        //esce dalla battaglia
        ExitBattlePhase();


        Debug.LogError("IL GIOCATORE NON PRENDE ANCORA ESPERIENZA VINCENDO LA BATTAGLIA");
        Debug.Log("Battle Won!");
    }
    /// <summary>
    /// Fa uscire il giocatore dalla battaglia
    /// </summary>
    private void ExitBattlePhase()
    {
        //riporta la telecamera a seguire il giocatore come prima della battaglia
        camFollow.ResetCameraSpeed();
        camFollow.ChangeTarget(mapPlayer);
        //imposta lo stato di gioco a non combattimento(quindi di esplorazione)
        GameStateManager.SetFightingState(false);

    }

    #endregion

    #region Timing Managment

    /// <summary>
    /// Si occupa di far partire la battaglia solo dopo l'eventuale animazione di incontro con nemico
    /// </summary>
    /// <returns></returns>
    private IEnumerator ManageStartFightTiming()
    {
        //aspetta un po' di tempo
        yield return new WaitForSeconds(positionPlayerTimer);
        //porta la telecamera alla posizione di combattimento immediatamente
        camFollow.ChangeTarget(transform);
        camFollow.ChangeCameraSpeed(99999);
        //porta il giocatore da battaglia nella posizione da battaglia
        battlePlayer.position = playerFightPos.position;

    }

    #endregion

    /// <summary>
    /// Ritorna lo sprite del nemico del tipo indicato
    /// </summary>
    /// <param name="enemyType"></param>
    /// <returns></returns>
    public Sprite GetEnemySpriteBasedOnType(int enemyType) { return allEnemies[enemyType].GetEnemySprite(); }

}
