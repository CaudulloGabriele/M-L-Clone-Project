using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypesBehaviours : EntityBattleManager
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

    [Header("")]
    [Header("ENEMY TYPES BEHAVIOUR")]

    //references to the SpriteRenderer of this Enemy
    [SerializeField]
    private SpriteRenderer thisEnemySR;
    //reference to the instance of the BattleManager
    private BattleManager battleManager;

    //reference to the position in which the selection arrow has to be in when selecting this enemy
    [SerializeField]
    private Transform selectionArrowPos;
    //reference to the position in which the player has to be to inflict damage to this enemy
    [SerializeField]
    private Transform damagePointPos;
    //reference to the position in which the enemy's attacks will come from
    [SerializeField]
    private Transform enemyAttackPos;

    //reference to this enemy's behaviour
    private IAmEnemy thisEnemyBehaviour = null;

    //indicates the enemy's type
    [SerializeField]
    private int thisEnemyType = 0;

    #endregion

    #region MonoBehaviour Methods

    protected override void Awake()
    {
        base.Awake();

        //adds the appropriate behaviour to this enemy
        GetBehaviourBasedOnType();

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

        /*
        if (Input.GetKeyDown(KeyCode.F11))
        {

            if (thisEnemyBehaviour == null) { Debug.LogError("THERE IS NO BEHAVIOUR IN ENEMY: " + name + " | TYPE = " + thisEnemyType); return; }

            thisEnemyBehaviour.PerformAction();

        }
        */

    }

    #endregion

    /// <summary>
    /// Allows to give damage to this enemy
    /// </summary>
    /// <param name="dmg"></param>
    public void ChangeHealth(float dmg)
    {

        entityStats.SetCurrentHealth(entityStats.GetCurrentHealth() - dmg);

        if (entityStats.GetCurrentHealth() <= 0) battleManager.AnEnemyWasDefeated(GetEnemyIndex(), thisEnemyType);

    }
    /// <summary>
    /// Adds the behaviour of the enemy based on its type
    /// </summary>
    private void GetBehaviourBasedOnType()
    {

        //if the enemy doesn't already have a behaviour, gets and adds the behaviour based on type
        IAmEnemy behaviour = GetComponent<IAmEnemy>();
        if (behaviour == null)
        {
            switch (thisEnemyType)
            {
                //SHROOB
                case (int)EnemyTypes.shroob: { behaviour = gameObject.AddComponent<ShroobBehaviour>(); break; }
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

        }

        //if a behaviour was added, gives itself as reference to the enemy's behaviour
        if (behaviour != null)
        {
            thisEnemyBehaviour = behaviour;
            thisEnemyBehaviour.InitializeEnemy(this);

            entityLevel = EnemyStats.GetEnemyLevel(thisEnemyType);
            entityStatsMult = EnemyStats.GetEnemyStatsMult(thisEnemyType);

        }

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
    /// <summary>
    /// Returns the point from which the enemy's attacks come from
    /// </summary>
    /// <returns></returns>
    public Transform GetEnemyAttackPosition() { return enemyAttackPos; }
    /// <summary>
    /// Returns the index of this enemy
    /// </summary>
    /// <returns></returns>
    public int GetEnemyIndex() { return transform.GetSiblingIndex(); }

    /// <summary>
    /// Return this enemy's attack
    /// </summary>
    /// <returns></returns>
    public float GetEnemyAttack() { return entityStats.GetAttack(); }

    #endregion

}
