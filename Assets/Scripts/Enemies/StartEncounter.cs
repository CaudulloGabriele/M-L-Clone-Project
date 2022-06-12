using System;
using UnityEngine;

public class StartEncounter : MonoBehaviour
{
    //reference to the enemy's overworld sprite
    [SerializeField]
    private SpriteRenderer enemySprite;
    //reference to the overworld enemy collider
    [SerializeField]
    private Collider2D enemyColl;

    //reference to the BattleManager instance
    private BattleManager battleManager;

    [SerializeField]
    private bool randomized, //tells if, for this fight, the enemies have to be randomized
        isBoss; //tells if this is a boss fight

    //indicates wheter this enemy's behaviour is active or not
    private bool behaviourActive = true;

    //array of all enemies to spawn in the fight
    [SerializeField]
    private int[] enemiesType;

    //indicates after how much time the enemy will reactivate after the player run away from the fight
    [SerializeField]
    private float reactivationTimer = 1;
    private float startReactivationTimer;


    private void Start()
    {
        //gets the reference to the BattleManager instance
        battleManager = BattleManager.instance;
        //sets the overworld sprite of the enemy as the one of the first enemy in the array
        enemySprite.sprite = battleManager.GetEnemySpriteBasedOnType(enemiesType[0]);

        //obtains the start value of the reactivation timer
        startReactivationTimer = reactivationTimer;

    }

    private void FixedUpdate()
    {
        //if the player is in a fight, the overworld enemies don't move or do anything
        if (GameStateManager.IsPlayerFighting()) return;


        if (behaviourActive)
        {
            /*MOVES ANYWHERE AND SEES THE PLAYER, THERE SHOULD ALSO BE DIFFERENT MOVEMENT PATTERNS(BETTER CREATE A NEW SCRIPT THAT MANAGES OVERWORLD ENEMIES MOVEMENTS THAT IS CONNECTED TO CHARACTER_MOVEMENT)*/

            return;
        }


        //if the player won the fight, the enemy will disappear from the overworld
        if (battleManager.HasPlayerWonTheLastFight()) Disappear();
        //otherwise, if he didn't win nor lose the player run away from the fight, so...
        else if (!GameStateManager.IsGameOver())
        {
            //...the enemy will remain deactivated until the timer reaches 0
            reactivationTimer -= Time.deltaTime;

            if(reactivationTimer <= 0)
            {
                SetIfEnemyActive(true);
                reactivationTimer = startReactivationTimer;

            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if this enemy collides with the player, it starts a battle
        if (collision.CompareTag("Player")) { StartTheBattle(); }

    }

    private void OnValidate()
    {

        if (enemiesType.Length > BattleManager.MAX_ENEMIES) { Array.Resize(ref enemiesType, BattleManager.MAX_ENEMIES); }

    }

    /// <summary>
    /// Starts the battle with the enemies in the array
    /// </summary>
    private void StartTheBattle()
    {
        //randomizes enemies, if it has to
        if (randomized) { RandomizeEnemies(); }
        //starts the battle
        battleManager.FightStart(enemiesType);

        //deactivates the enemy's behaviour
        SetIfEnemyActive(false);

    }
    /// <summary>
    /// Randomizes the enemies to fight(both in type and quantity)
    /// </summary>
    private void RandomizeEnemies()
    {
        //randomizes the number of enemies to fight(min 1)
        int n_Enemies = UnityEngine.Random.Range(0, BattleManager.MAX_ENEMIES) + 1;
        enemiesType = new int[n_Enemies];
        //sets the range of possible enemies to spawn, based on wheter this is a boss fight or not
        int minRange = !isBoss ? 0 : BattleManager.START_OF_BOSS_LIST;
        int maxRange = !isBoss ? BattleManager.START_OF_BOSS_LIST : BattleManager.N_TYPES;
        //sets the new and randomized enemies
        for (int i = 0; i < n_Enemies; i++) { enemiesType[i] = UnityEngine.Random.Range(minRange, maxRange); }

    }
    /// <summary>
    /// Allows to set wheter this overworld enemy is active or not
    /// </summary>
    /// <param name="active"></param>
    private void SetIfEnemyActive(bool active)
    {
        behaviourActive = active;
        enemyColl.enabled = active;

        /*THERE SHOULD BE AN ANIMATOR THAT MANAGES THE FADEIN/FADEOUT ANIMATION TO MAKE THE PLAYER UNDERSTAND THE ENEMY IS DEACTIVATED*/

    }
    /// <summary>
    /// Makes the overworld enemy disappear
    /// </summary>
    private void Disappear()
    {

        enemySprite.color = Color.clear;

        enabled = false;

    }

}
