using UnityEngine;

/// <summary>
/// Manages the Turn-Based Combat System
/// </summary>
public class TurnBasedCombatManager : MonoBehaviour
{

    #region Variables

    //references to the player's scripts that manage his fight
    private BattleActionsManager playerBattleActionsManager;
    private PlayerBattleManager playerBattleManager;

    //array of references to all the behaviours of the enemies currently in the fight
    private IAmEnemy[] enemiesInFight;
    //array of references to all the manager of battle stats of the entities in the fight
    [SerializeField]
    private EntityBattleManager[] entitiesInBattle = new EntityBattleManager[1];

    #endregion

    #region MonoBehaviour Methods

    private void Start()
    {
        //obtains the reference to the instance of the BattleManager
        BattleManager battleManager = BattleManager.instance;

        //obtains the references to the player's scripts that manage his fight
        playerBattleActionsManager = battleManager.GetPlayerBattleActionsManager();
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
        EstablishTurnOrder();

    }
    /// <summary>
    /// Sets the turn order of the entities based on their speed
    /// </summary>
    private void EstablishTurnOrder()
    {

        /*entitiesInBattle and order based on speed*/

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
