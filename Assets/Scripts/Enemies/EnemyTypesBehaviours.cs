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

    #region Variables

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
    //reference to the position in which the player has to be to inflict damage to this enemy
    [SerializeField]
    private Transform damagePointPos;

    //reference to this enemy's behaviour
    private IAmEnemy thisEnemyBehaviour = null;

    //indicates the enemy's type
    [SerializeField]
    private int thisEnemyType = 0;
    //indicates the enemy's level
    [SerializeField]
    private int thisEnemyLevel = 1;
    //indicates by how much to amplify the enemy's battle stats
    private float[] thisEnemyStatsMult = { 1, 1, 1 };

    #endregion

    #region MonoBehaviour Methods

    public void Awake()
    {

        enemyStats = new BattleStats();

        IAmEnemy behaviour = GetComponent<IAmEnemy>();
        if (behaviour != null)
        {
            thisEnemyBehaviour = behaviour;

            return;
        }

        GetBehaviourBasedOnType();

    }

    private void OnEnable()
    {
        //whenever the enemy gets activated(either by instantiating or respawning) its stats go back to normal
        bool thereWasAnError = enemyStats.InitializeStats(thisEnemyLevel, thisEnemyStatsMult);


        Debug.LogError("THE ENEMY STATS MULTIPLIER STILL CAN'T CHANGE ITS VALUE");
        
        
        if (thereWasAnError) { Debug.LogError("There was an error with the initialization of enemy's stats: " + name); }
        /*
        Debug.Log("Enemy Level: " + thisEnemyLevel);
        Debug.Log("Enemy HP: " + enemyStats.GetCurrentHealth());
        Debug.Log("Enemy Attack: " + enemyStats.GetAttack());
        Debug.Log("Enemy Speed: " + enemyStats.GetSpeed());
        Debug.LogWarning("Begin Debug Log of stats multipliers");
        for (int i = 0; i < thisEnemyStatsMult.Length; i++) { Debug.Log(i + ") " + thisEnemyStatsMult[i]); }
        Debug.LogWarning("End Debug Log of stats multipliers");
        */
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
            ChangeHealth(999);
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {

            if (thisEnemyBehaviour == null) { Debug.LogError("THERE IS NO BEHAVIOUR IN ENEMY: " + name + " | TYPE = " + thisEnemyType); return; }

            thisEnemyBehaviour.PerformAction();

        }

    }

    #endregion

    /// <summary>
    /// Allows to give damage to this enemy
    /// </summary>
    /// <param name="dmg"></param>
    public void ChangeHealth(float dmg)
    {

        enemyStats.SetCurrentHealth(enemyStats.GetCurrentHealth() - dmg);

        if (enemyStats.GetCurrentHealth() <= 0) battleManager.AnEnemyWasDefeated(transform.GetSiblingIndex(), thisEnemyType);

    }
    /// <summary>
    /// Adds the behaviour of the enemy based on its type
    /// </summary>
    private void GetBehaviourBasedOnType()
    {

        switch (thisEnemyType)
        {
            //SHROOB
            case (int)EnemyTypes.shroob: { thisEnemyBehaviour = gameObject.AddComponent<ShroobBehaviour>(); break; }
            //COMMANDER SHROOB
            case (int)EnemyTypes.commanderShroob: { Debug.LogError("THERE STILL ISN'T A BEHAVIOUR FOR THIS ENEMY TYPE: " + thisEnemyType); break; }
            //SHROOB-BOMB
            case (int)EnemyTypes.shrooBomb: { Debug.LogError("THERE STILL ISN'T A BEHAVIOUR FOR THIS ENEMY TYPE: " + thisEnemyType); break; }
            //SHROOB REX
            case (int)EnemyTypes.shroobRex: { Debug.LogError("THERE STILL ISN'T A BEHAVIOUR FOR THIS ENEMY TYPE: " + thisEnemyType); break; }
            //UFO SHROOB
            case (int)EnemyTypes.ufoShroob: { Debug.LogError("THERE STILL ISN'T A BEHAVIOUR FOR THIS ENEMY TYPE: " + thisEnemyType); break; }
            //MOTHER UFO SHROOB
            case (int)EnemyTypes.motherUfoShroob: { Debug.LogError("THERE STILL ISN'T A BEHAVIOUR FOR THIS ENEMY TYPE: " + thisEnemyType); break; }
            //FAWFUL
            case (int)EnemyTypes.fawful: { Debug.LogError("THERE STILL ISN'T A BEHAVIOUR FOR THIS ENEMY TYPE: " + thisEnemyType); break; }
            //PRINCESS SHROOB
            case (int)EnemyTypes.princessShroob: { Debug.LogError("THERE STILL ISN'T A BEHAVIOUR FOR THIS ENEMY TYPE: " + thisEnemyType); break; }
            //ELDER PRINCESS SHROOB
            case (int)EnemyTypes.elderPrincessShroob: { Debug.LogError("THERE STILL ISN'T A BEHAVIOUR FOR THIS ENEMY TYPE: " + thisEnemyType); break; }
            //FINAL BOSS
            case (int)EnemyTypes.finalBoss: { Debug.LogError("THERE STILL ISN'T A BEHAVIOUR FOR THIS ENEMY TYPE: " + thisEnemyType); break; }

        }

        //gives itself as reference to the enemy's behaviour
        if (thisEnemyBehaviour != null) thisEnemyBehaviour.SetEnemyTypesBehavioursRef(this);

    }

    #region Getter Methods

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
    /// <summary>
    /// Returns the position in which the player has to be to inflict damage to this enemy
    /// </summary>
    /// <returns></returns>
    public Vector2 GetDamagePointPos() { return damagePointPos.position; }

    #endregion

}
