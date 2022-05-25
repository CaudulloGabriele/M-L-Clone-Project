using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEncounter : MonoBehaviour
{
    //reference to the enemy's overworld sprite
    [SerializeField]
    private SpriteRenderer enemySprite;
    //reference to the BattleManager instance
    private BattleManager battleManager;

    [SerializeField]
    private bool randomized, //tells if, for this fight, the enemies have to be randomized
        isBoss; //tells if this is a boss fight

    //array of all enemies to spawn in the fight
    [SerializeField]
    private int[] enemiesType;


    private void Start()
    {
        //gets the reference to the BattleManager instance
        battleManager = BattleManager.instance;
        //sets the overworld sprite of the enemy as the one of the first enemy in the array
        enemySprite.sprite = battleManager.GetEnemySpriteBasedOnType(enemiesType[0]);

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

        /*DEACTIVATES COLLIDER*/

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

}
