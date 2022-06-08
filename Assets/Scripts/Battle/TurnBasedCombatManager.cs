using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Manages the Turn-Based Combat System
/// </summary>
public class TurnBasedCombatManager : MonoBehaviour
{

    #region Variables

    //references to the player's scripts that manage his fight
    private PlayerBattleManager playerBattleManager;

    //array of references to all the behaviours of the enemies currently in the fight
    private IAmEnemy[] enemiesInFight;
    //array of references to all the manager of battle stats of the entities in the fight
    [SerializeField]
    private EntityBattleManager[] entitiesInBattle = new EntityBattleManager[1];

    [SerializeField]
    private int[] orderedTurnsOfEntities;

    //indicates the current turn in the list
    private int currentTurn = -1;
    //indicates how much time to wait before going to the next turn
    [SerializeField]
    private float waitForTurnChange = 0.2f;

    #endregion

    #region MonoBehaviour Methods

    private void Start()
    {
        //obtains the reference to the instance of the BattleManager
        BattleManager battleManager = BattleManager.instance;

        //obtains the references to the player's scripts that manage their fight
        playerBattleManager = battleManager.GetPlayerBattleManager();

        //sets the player's battle manager as the first entity in the array
        entitiesInBattle[0] = playerBattleManager;

    }

    #endregion

    #region Combat Turns Management

    /// <summary>
    /// Begins the turn-based combat, by first establishing the order of the entities in the fight
    /// </summary>
    /// <param name="enemies"></param>
    public void BeginTurnBasedCombat(Transform[] enemies)
    {
        //adds every enemy in the array of entities in combat
        int nEnemies = enemies.Length;
        enemiesInFight = new IAmEnemy[nEnemies];
        for (int i = 0; i < nEnemies; i++)
        {
            IAmEnemy enemyBehaviour = enemies[i].GetComponent<IAmEnemy>();
            enemiesInFight[i] = enemyBehaviour;

            //if the enemy has no behaviour, throws an error and doesn't add the enemy in the turn system
            if(enemyBehaviour == null) { Debug.LogError("The enemy of index " + i + " has no behaviour!"); continue; }

            AddOrRemoveEnemyInCombat(true, enemyBehaviour.GetThisEnemyTypesBehaviour());

        }

        //sets the turn order of the entities based on their speed
        EstablishTurnOrder(true);

    }
    /// <summary>
    /// Sets the turn order of the entities based on their speed
    /// </summary>
    public void EstablishTurnOrder(bool fightBegun)
    {
        
        int numberOfEntities = entitiesInBattle.Length;

        //initializes the array of turns order
        orderedTurnsOfEntities = new int[numberOfEntities];
        for (int fighterIndex = 0; fighterIndex < numberOfEntities; fighterIndex++) { orderedTurnsOfEntities[fighterIndex] = fighterIndex; }

        //cycles each entity in battle and orders them in order of who is faster
        for (int i = 0; i < numberOfEntities; i++)
        {

            EntityBattleManager iEntity = entitiesInBattle[i];

            for (int j = i + 1; j < numberOfEntities; j++)
            {

                EntityBattleManager jEntity = entitiesInBattle[j];

                if (iEntity.GetEntitySpeed() <= jEntity.GetEntitySpeed())
                {
                    orderedTurnsOfEntities[i] = j;
                    orderedTurnsOfEntities[j] = i;
                    /*
                    EntityBattleManager temp = entitiesInBattle[i];
                    entitiesInBattle[i] = jEntity;
                    entitiesInBattle[j] = temp;
                    */

                }

            }

        }

        /*
        Debug.LogWarning("ORDERED TURNS:");
        foreach (int entity in orderedTurnsOfEntities) { Debug.Log("\n " + entity); }
        Debug.LogWarning("\nEND-----------------------------");
        */

        //se i turni sono stati cambiati durante la battaglia, non fa iniziare un nuovo turno
        if (!fightBegun) return;

        //starts the first turn of the fight
        currentTurn = -1;
        StartNewTurn();
        
    }
    /// <summary>
    /// Starts the next turn
    /// </summary>
    public async void StartNewTurn()
    {
        //goes to the next turn
        currentTurn++;
        if (currentTurn >= entitiesInBattle.Length) { currentTurn = 0; }

        //waits a bit
        await Task.Delay((int)(waitForTurnChange * 1000));

        int fighterIndex = orderedTurnsOfEntities[currentTurn];
        entitiesInBattle[fighterIndex].StartOwnTurn();

    }

    #endregion

    #region Arrays Management

    /// <summary>
    /// Allows to add or remove an enemy from the ones currently in the fight who need to have a turn
    /// </summary>
    /// <param name="add"></param>
    /// <param name="newEnemy"></param>
    public void AddOrRemoveEnemyInCombat(bool add, EnemyTypesBehaviours newEnemy)
    {
        //if the enemy has no behaviour, throws an error and doesn't do anything
        if (newEnemy.GetComponent<IAmEnemy>() == null) { Debug.LogError("The enemy  " + newEnemy.name + " has no behaviour!"); return; }

        //calculates the new length of the array of entities based on wheter an enemy is being added or removed
        int newLength = entitiesInBattle.Length + (add ? 1 : -1);

        //creates a recipient for the previous elements in the array of entities
        EntityBattleManager[] recipient = new EntityBattleManager[newLength - (add ? 0 : -1)];

        //Debug.LogWarning("RECIPIENT: " + recipient.Length + " | ENTITIES = " + entitiesInBattle.Length);

        for (int j = 0; j < entitiesInBattle.Length; j++) { recipient[j] = entitiesInBattle[j]; }
        
        //changes the length of the array of entities and sets the player battle manager as the first element
        entitiesInBattle = new EntityBattleManager[newLength];
        entitiesInBattle[0] = recipient[0];

        //creates an offset in case the enemy to be removed has to be skipped
        int offset = 0;

        //cycles through all the enemies and adds them to the list(adding or skipping over the received enemy)
        for (int i = 1; i < newLength; i++)
        {
            //if the enemy has to be removed...
            if (!add)
            {
                //...if the cycled enemy is the enemy to be removed, it gets skipped
                EnemyTypesBehaviours etb = recipient[i].gameObject.GetComponent<EnemyTypesBehaviours>();
                //Debug.LogError("ETB: " + etb + " | GAMEOBJECT: " + recipient[i].gameObject.name);
                if (etb.GetEnemyIndex() == newEnemy.GetEnemyIndex()) offset += 1;

            } //otherwise, if the enemy has to be added and the index is the correct one, adds it to the recipient
            else if (i == newEnemy.GetEnemyIndex() + 1) { recipient[i] = newEnemy; }

            //Debug.LogWarning("NEW LENGTH: " + newLength + " | i = " + i + " | offset = " + offset);

            //adds the recipient value in the "i" index in the array of entities in the fight
            entitiesInBattle[i] = recipient[i + offset];

        }

    }

    #endregion

}
