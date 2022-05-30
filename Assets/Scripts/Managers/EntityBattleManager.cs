using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBattleManager : MonoBehaviour
{

    protected BattleStats entityStats;

    [Header("ENTITY BATTLE MANAGER")]

    //indicates the entity's level
    [SerializeField]
    protected int entityLevel = 1;
    //indicates by how much to amplify the entity's battle stats
    [SerializeField]
    protected float[] entityStatsMult = { 1, 1, 1 };

    protected virtual void Awake()
    {

        entityStats = new BattleStats();

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

}
