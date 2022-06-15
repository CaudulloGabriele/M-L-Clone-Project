using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBattleManager : MonoBehaviour
{

    protected BattleStats entityStats;

    //reference to the instance of the BattleManager
    protected BattleManager battleManager;

    [Header("ENTITY BATTLE MANAGER")]

    //indicates the entity's level
    [SerializeField]
    protected int entityLevel = 1;
    //indicates by how much to amplify the entity's battle stats
    [SerializeField]
    protected float[] entityStatsMult = { 1, 1, 1 };
    //indicates wheter this entity is the player or not
    [SerializeField]
    private bool isPlayer = false;

    [Header("")]

    //reference to the player's dodge and counter manager
    [SerializeField]
    protected BattleDodgeCounter battleDodgeCounter;

    //indicates wheter or not the attack that this entity is about to use can be dodged
    [SerializeField]
    protected bool canOwnAttackBeDodged = false;
    //indicates wheter or not the attack that this entity is about to use can be countered
    [SerializeField]
    protected bool canOwnAttackBeCountered = false;

    //indicates what type of dodge this entity can do
    [SerializeField]
    protected int entityDodgeType = -1;
    //indicates what type of counter this entity can do
    [SerializeField]
    protected int entityCounterType = -1;


    protected virtual void Awake()
    {

        entityStats = new BattleStats();

    }

    protected virtual void Start()
    {
        //gets the BattleManager instance
        battleManager = BattleManager.instance;

    }

    protected virtual void OnEnable()
    {

        //whenever the entity gets activated(either by instantiating or respawning) its stats go back to normal
        bool thereWasAnError = entityStats.InitializeStats(entityLevel, entityStatsMult);


        if (thereWasAnError) { Debug.LogError("There was an error with the initialization of entity's stats: " + name); }
        /*
        Debug.Log("Entity Level: " + entityLevel);
        Debug.Log("Entity HP: " + entityStats.GetCurrentHealth());
        Debug.Log("Entity Attack: " + entityStats.GetAttack());
        Debug.Log("Entity Speed: " + entityStats.GetSpeed());
        Debug.LogWarning("Begin Debug Log of stats multipliers");
        for (int i = 0; i < entityStatsMult.Length; i++) { Debug.Log(i + ") " + entityStatsMult[i]); }
        Debug.LogWarning("End Debug Log of stats multipliers");
        */

    }

    /// <summary>
    /// Allows this entity to start the turn
    /// </summary>
    public virtual void StartOwnTurn() { }

    /// <summary>
    /// Returns wheter this entity is the player or not
    /// </summary>
    /// <returns></returns>
    public bool IsThisEntityThePlayer() { return isPlayer; }

    /// <summary>
    /// Allows to give damage to this entity
    /// </summary>
    /// <param name="dmg"></param>
    public virtual void ChangeHealth(float dmg)
    {

        entityStats.SetCurrentHealth(entityStats.GetCurrentHealth() - dmg);

        if (entityStats.GetCurrentHealth() <= 0) EntityDeath();


        Debug.Log("ENTITY \"" + name + "\" TOOK DAMAGE: " + dmg + " | CURRENT HEALTH: " + entityStats.GetCurrentHealth());
    }
    /// <summary>
    /// Return this entity's attack
    /// </summary>
    /// <returns></returns>
    public float GetEntityAttack() { return entityStats.GetAttack(); }
    /// <summary>
    /// Return this entity's speed
    /// </summary>
    /// <returns></returns>
    public float GetEntitySpeed() { return entityStats.GetSpeed(); }

    /// <summary>
    /// Manages what happens when this entity is defeated
    /// </summary>
    protected virtual void EntityDeath()
    {

    }

    #region Methods For Entity DodgeCounter

    /// <summary>
    /// Allows to change what type of dodge and or counter this entity can do
    /// 
    ///    <para>
    ///         DODGES INDEXES: 0 - jump dodge | 1 - vanish dodge
    ///     </para>
    ///     <para>
    ///         COUNTERS INDEXES: 0 - deflect bullets counter | 1 - spiky body counter
    ///     </para>
    ///     
    /// </summary>
    /// <param name="hasToDodge"></param>
    /// <param name="onlyThis"></param>
    public virtual void SetEntityDodgeCounter(bool hasToDodge, bool onlyThis)
    {
        //gets the type of dodge or counter to use based on the received parameter("hasToDodge")
        int dodgeCounterType = hasToDodge ? entityDodgeType : entityCounterType;

        //if the entity has to do only this dodge or counter, removes all the previously active ones and sets only the desired one
        if (onlyThis) { battleDodgeCounter.SetSingleDodgeCounter(hasToDodge, dodgeCounterType); }
        //otherwise, the desired dodge counter gets added to the already active ones
        else { battleDodgeCounter.AddOrRemoveDodgeCounter(hasToDodge, dodgeCounterType, true); }
        
    }
    /// <summary>
    /// Makes this entity unable to dodge and or counter any attacks
    /// </summary>
    public void MakeEntityUnableToDodgeCounter() { battleDodgeCounter.RemoveAllDodgeCounters(); }

    /// <summary>
    /// Returns wheter or not the attack that this entity is about to use can be dodged
    /// </summary>
    /// <returns></returns>
    public bool AttackAboutToUseIsDodgeable() { return canOwnAttackBeDodged; }
    /// <summary>
    /// Returns wheter or not the attack that this entity is about to use can be countered
    /// </summary>
    /// <returns></returns>
    public bool AttackAboutToUseIsCounterable() { return canOwnAttackBeCountered; }

    #endregion

}
