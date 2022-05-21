using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    #region Variables

    public const int N_TYPES = 10, //number of enemy types in the game
        MAX_ENEMIES = 5, //max number of possible enemies on the field
        START_OF_BOSS_LIST = 7; //indicates the index at which the boss enemies start

    //only instance of this manager
    public static BattleManager instance;

    //arrays of references to all the enemies
    private EnemyTypesBehaviours[] allEnemies;
    private GameObject[] allEnemiesGO;
    //array of references to all enemies combat positions
    private Transform[] enemiesPositions;
    //reference to the container of the player combat positions
    private Transform playerPositionsContainer;
    //reference to the container of the currently active enemies in the current fight
    private Transform activeEnemiesContainer;
    //reference to the container of all already spawned and defeated enemies
    private Transform spawnedEnemiesContainer;
    //reference to the player...
    [SerializeField]
    private Transform mapPlayer, //...in the map
        battlePlayer; //...in combat

    //reference to the camera movement script
    [SerializeField]
    private CameraFollow camFollow;
    //reference to the player fight position during battle
    private Transform playerFightPos;
    //reference to the script that manages the battle actions of the player(during battle)
    [SerializeField]
    private BattleActionsManager battleActionsManager;

    //time to wait to position player in the fight position
    [SerializeField]
    private float positionPlayerTimer = 1.5f;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        //this becomes the only instance, if there is not another one
        if (!instance) instance = this;
        //otherwise, this instance destroys itself
        else { Destroy(gameObject); return; }

        //gets the references to all the enemies
        GetAllEnemies();
        //gets the references to all the containers
        GetAllContainers();
        //gets the references to all fight positions
        GetAllPositions();


        //foreach (Transform t in enemiesPositions) Debug.LogError(t.name);
        //Debug.LogWarning("------------------------------------------------------------------------------");
        //foreach (Transform t in allEnemies) Debug.LogError(t.name);
    }

    #endregion

    #region Inizialization Methods

    /// <summary>
    /// Gets the references to all the enemies and deactivates them
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
    /// Gets the references to all the containers of spawned and active enemies and of the player positions 
    /// </summary>
    private void GetAllContainers()
    {

        activeEnemiesContainer = transform.GetChild(2);

        spawnedEnemiesContainer = transform.GetChild(3);

        playerPositionsContainer = transform.GetChild(4);

    }
    /// <summary>
    /// Gets the references to all fight positions
    /// </summary>
    private void GetAllPositions()
    {
        //gets the references to all enemies positions
        Transform allPositionsContainer = transform.GetChild(0);
        Transform[] allPositions = allPositionsContainer.GetComponentsInChildren<Transform>();
        //removes from the array the container of enemies positions
        foreach (Transform pos in allPositions)
        {
            //if "pos" is actually the container of positions...
            if (pos == allPositionsContainer)
            {
                //...it removes it from the array and exits the cycle
                var recipient = new Transform[allPositions.Length - 1];
                for (int i = 0; i < recipient.Length; i++) { recipient[i] = allPositions[i + 1]; }
                allPositions = recipient;

                break;

            }

        }
        //saves the positions in the static array
        enemiesPositions = allPositions;
        //gets the reference to the player fight position
        playerFightPos = playerPositionsContainer.GetChild(0);

    }

    #endregion

    #region Fight Managment

    /// <summary>
    /// Starts the battle and positions the enemies to fight
    /// </summary>
    /// <param name="enemiesType"></param>
    public void FightStart(int[] enemiesType)
    {
        //sets that the game is in the fight state
        GameStateManager.SetFightingState(true);
        //creates a local index that indicates the position in which to put the enemy
        int posIndex = 0;
        //cycles all the enemies received as parameters...
        foreach (int enemyType in enemiesType)
        {
            //...and positions them on the battleground...
            //allEnemies[enemyType].position = enemiesPositions[index].position;
            SpawnEnemy(enemyType, enemiesPositions[posIndex].position);
            //...and increments the index for the next enemy position
            posIndex++;

            Debug.Log("Spawn enemy type: " + enemyType);
        }
        //starts the coroutine that manages the start fight timing
        StartCoroutine(ManageStartFightTiming());

    }
    /// <summary>
    /// Spawns the indicated enemy in the indicated position, instantiating it if necessary
    /// </summary>
    /// <param name="enemyToSpawn"></param>
    /// <param name="enemyPos"></param>
    private void SpawnEnemy(int enemyToSpawn, Vector2 enemyPos)
    {
        //gets the reference to the container of already spawned enemies of the enemy's type received
        Transform remainingSpawnedEnemies = spawnedEnemiesContainer.GetChild(enemyToSpawn);
        //if the container contains an already spawned and defeated enemy...
        if (remainingSpawnedEnemies.childCount > 0)
        {
            //...it spawns it in the indicated position...
            Transform spawnableEnemy = remainingSpawnedEnemies.GetChild(0);
            spawnableEnemy.parent = activeEnemiesContainer;
            spawnableEnemy.position = enemyPos;

        } //otherwise, it instantiates a new enemy of the received type
        else { Instantiate(allEnemiesGO[enemyToSpawn], enemyPos, Quaternion.identity, activeEnemiesContainer); }

    }
    /// <summary>
    /// Called when an enemy is defeated, checks if this was the last enemy(so that the player wins)
    /// </summary>
    /// <param name="enemyChildIndex"></param>
    /// <param name="enemyType"></param>
    public void AnEnemyWasDefeated(int enemyChildIndex, int enemyType)
    {
        //moves the defeated enemy to the container of defeated enemies
        Transform defeatedEnemy = activeEnemiesContainer.GetChild(enemyChildIndex);
        defeatedEnemy.parent = spawnedEnemiesContainer.GetChild(enemyType);
        //if there are no more active enemies, the player wins the battle
        if (GetNumberOfCurrentlyActiveEnemies() == 0) { BattleWon(); }

    }
    /// <summary>
    /// Makes the player victorious of the current battle
    /// </summary>
    private void BattleWon()
    {
        //exits the battle
        ExitBattlePhase();


        Debug.LogError("IL GIOCATORE NON PRENDE ANCORA ESPERIENZA VINCENDO LA BATTAGLIA");
        Debug.Log("Battle Won!");
    }
    /// <summary>
    /// Returns the player to the overworld map
    /// </summary>
    private void ExitBattlePhase()
    {
        //the camera follows the overworld player like before the battle
        camFollow.ResetCameraSpeed();
        camFollow.ChangeTarget(mapPlayer);
        //sets the game' state as no more in fight
        GameStateManager.SetFightingState(false);

    }

    #endregion

    #region Timing Managment

    /// <summary>
    /// Starts the battle after the animation of enemy figth
    /// </summary>
    /// <returns></returns>
    private IEnumerator ManageStartFightTiming()
    {
        //waits a bit
        yield return new WaitForSeconds(positionPlayerTimer);
        //immediately moves the camera in the position of the battleground
        camFollow.ChangeTarget(transform);
        camFollow.ChangeCameraSpeed(99999);
        //moves the battle player in the combat position
        battlePlayer.position = playerFightPos.position;
        //resets the action blocks
        battleActionsManager.ResetActionBlocks();

    }

    #endregion

    #region Getter Methods

    public int GetNumberOfCurrentlyActiveEnemies() { return activeEnemiesContainer.childCount; }

    public EnemyTypesBehaviours GetActiveEnemyAtIndex(int index)
    {

        if (index >= GetNumberOfCurrentlyActiveEnemies()) index = 0;

        return activeEnemiesContainer.GetChild(index).GetComponent<EnemyTypesBehaviours>();

    }

    /// <summary>
    /// Returns the sprite of the enemy of the desired type
    /// </summary>
    /// <param name="enemyType"></param>
    /// <returns></returns>
    public Sprite GetEnemySpriteBasedOnType(int enemyType) { return allEnemies[enemyType].GetEnemySprite(); }

    #endregion

}
