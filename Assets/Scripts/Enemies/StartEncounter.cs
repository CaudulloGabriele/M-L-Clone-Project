using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEncounter : MonoBehaviour
{
    //riferimento allo SpriteRenderer del nemico nell'OverWorld
    [SerializeField]
    private SpriteRenderer enemySprite;
    //riferimento all'istanza del battleManager
    private BattleManager battleManager;

    [SerializeField]
    private bool randomized, //indica se i nemici devono essere randomizzati per questo incontro
        isBoss; //indica se questo è un incontro con un boss

    //array di tutti i tipi di nemici nell'incontro
    [SerializeField]
    private int[] enemiesType;


    private void Start()
    {
        //prende il riferimento all'istanza del battleManager
        battleManager = BattleManager.instance;
        //imposta lo sprite del nemico in OverWorld a quello del primo nemico nella lista di nemici
        enemySprite.sprite = battleManager.GetEnemySpriteBasedOnType(enemiesType[0]);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se si collide con il giocatore, comincia la battaglia
        if (collision.CompareTag("Player")) { StartTheBattle(); }

    }

    private void OnValidate()
    {

        if (enemiesType.Length > BattleManager.MAX_ENEMIES) { Array.Resize(ref enemiesType, BattleManager.MAX_ENEMIES); }

    }

    /// <summary>
    /// Fa partire la battaglia contro i nemici stabili
    /// </summary>
    private void StartTheBattle()
    {
        //se bisogna randomizzare i nemici, li randomizza
        if (randomized) { RandomizeEnemies(); }
        //fa cominciare la battaglia
        battleManager.FightStart(enemiesType);

    }
    /// <summary>
    /// Randomizza i nemici da affrontare(sia in numero che in tipo)
    /// </summary>
    private void RandomizeEnemies()
    {
        //randomizza il numero di nemici presenti nell'incontro(minimo 1)
        int n_Enemies = UnityEngine.Random.Range(0, BattleManager.MAX_ENEMIES) + 1;
        enemiesType = new int[n_Enemies];
        //calcola il range di nemici randomizzabili, in base a se questo è un incontro con un boss o meno
        int minRange = !isBoss ? 0 : BattleManager.START_OF_BOSS_LIST;
        int maxRange = !isBoss ? BattleManager.START_OF_BOSS_LIST : BattleManager.N_TYPES;
        //infine, imposta i nuovi e randomizzati nemici da affrontare
        for (int i = 0; i < n_Enemies; i++) { enemiesType[i] = UnityEngine.Random.Range(minRange, maxRange); }

    }

}
