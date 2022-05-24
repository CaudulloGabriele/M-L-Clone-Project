using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypesBehaviours : MonoBehaviour
{

    private enum EnemyTypes 
    {
        shroob = 0,
        commanderShroob = 1,
        shrooBomb = 2,
        shroobRex = 3,
        ufoShroob = 4, 
        motherUfoShroob = 5,
        fawful = 6,
        princessShroob = 7,
        elderPrincessShroob = 8,
        finalBoss = 9

    }

    //references to the SpriteRenderer of this Enemy
    [SerializeField]
    private SpriteRenderer thisEnemySR;
    //reference to the instance of the BattleManager
    private BattleManager battleManager;
    //reference to the class that manages the enemy's stats
    private BattleStats enemyStats;

    //reference to the position in which the selection arrow has to be in when selecting this enemy
    [SerializeField]
    private Transform selectionArrowPos;

    //indicates the enemy's type
    [SerializeField]
    private int thisEnemyType = 0;
    //indicates the enemy's level
    [SerializeField]
    private int thisEnemyLevel = 1;
    //indicates by how much to amplify the enemy's battle stats
    private float[] thisEnemyStatsMult = { 1, 1, 1, 1};


    private void Awake()
    {
        enemyStats = new BattleStats();

    }

    private void OnEnable()
    {
        //whenever the enemy gets activated(either by instantiating or respawning) its stats go back to normal
        enemyStats.InitializeStats(thisEnemyLevel, thisEnemyStatsMult);

        Debug.LogError("THE ENEMY STATS MULTIPLIER STILL CAN'T CHANGE ITS VALUE");
    }

    private void Start()
    {
        //gets the BattleManager instance
        battleManager = BattleManager.instance;

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F12))
        {
            battleManager.AnEnemyWasDefeated(transform.GetSiblingIndex(), thisEnemyType);
        }

    }

    /// <summary>
    /// Returns this enemy's sprite
    /// </summary>
    /// <returns></returns>
    public Sprite GetEnemySprite() { return thisEnemySR.sprite; }
    /// <summary>
    /// Returns the position in which the selection arrow has to be in when selecting this enemy
    /// </summary>
    /// <returns></returns>
    public Vector2 GetSelectionArrowPos() { return selectionArrowPos.position; }

}
