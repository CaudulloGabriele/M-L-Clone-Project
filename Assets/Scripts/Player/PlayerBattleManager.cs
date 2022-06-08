using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerBattleManager : EntityBattleManager, IUpdateData
{

    [SerializeField]
    private BattleActionsManager battleActionsManager;


    protected override async void OnEnable()
    {
        //waits for the updating of the stats in the Start
        await Task.Delay(1);

        base.OnEnable();

    }

    private void Start()
    {
        //gets the saved values
        GetSavedPlayerStats();
        //initializes the player's battle stats
        entityStats = new BattleStats();

        //calculates player's stats
        //CalculatePlayerStats();

        //Debug.Log("GETTING SAVED STATS");
    }
    /// <summary>
    /// Gets the player's saved battle stats values
    /// </summary>
    public void GetSavedPlayerStats()
    {
        entityLevel = DataManager.savedPlayerLevel;
        entityStatsMult = (float[])DataManager.savedPlayerStatsMult.Clone();

    }
    /// <summary>
    /// Calculates the player's battle stats based on the multipliers
    /// </summary>
    public void CalculatePlayerStats()
    {

        OnEnable();

        /*
        bool thereWasAnError = entityStats.InitializeStats(entityLevel, entityStatsMult);


        if (thereWasAnError) { Debug.LogError("There was an error with the initialization of player's stats:"); }
        Debug.Log("Player Level: " + entityLevel);
        Debug.Log("Player HP: " + entityStats.GetCurrentHealth());
        Debug.Log("Player Attack: " + entityStats.GetAttack());
        Debug.Log("Player Speed: " + entityStats.GetSpeed());
        Debug.LogWarning("Begin Debug Log of stats multipliers");
        for (int i = 0; i < entityStatsMult.Length; i++) { Debug.Log(i+") " + entityStatsMult[i]); }
        Debug.LogWarning("End Debug Log of stats multipliers");
        */
    }

    public override void StartOwnTurn()
    {
        base.StartOwnTurn();

        battleActionsManager.ResetActionBlocks();

    }

    public void UpdateData()
    {

        DataManager.savedPlayerLevel = entityLevel;
        DataManager.savedPlayerStatsMult = entityStatsMult;

    }

}
