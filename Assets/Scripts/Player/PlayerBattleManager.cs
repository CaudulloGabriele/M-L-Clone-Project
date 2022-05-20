using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleManager : MonoBehaviour, IUpdateData
{
    //reference to the class that manages the player's stats
    private BattleStats playerStats;
    //indicates the player's level
    [SerializeField]
    private int playerLevel = 1;
    //indicates how much to amplify the player's battle stats
    // 0 - hp
    // 1 - attack
    // 2 - speed
    [SerializeField]
    private float[] playerStatsMultipliers = new float[3];


    private void Start()
    {
        //gets the saved values
        GetSavedPlayerStats();
        //initializes the player's battle stats
        playerStats = new BattleStats();
        //calculates player's stats
        CalculatePlayerStats();

    }
    /// <summary>
    /// Gets the player's saved battle stats values
    /// </summary>
    public void GetSavedPlayerStats()
    {
        playerLevel = DataManager.savedPlayerLevel;
        playerStatsMultipliers = (float[])DataManager.savedPlayerStatsMult.Clone();

    }
    /// <summary>
    /// Calculates the player's battle stats based on the multipliers
    /// </summary>
    public void CalculatePlayerStats()
    {

        bool thereWasAnError = playerStats.InitializeStats(playerLevel, playerStatsMultipliers);


        if (thereWasAnError) { Debug.LogError("There was an error with the initialization of player's stats:"); }
        Debug.Log("Player Level: " + playerLevel);
        Debug.Log("Player HP: " + playerStats.GetCurrentHealth());
        Debug.Log("Player Attack: " + playerStats.GetAttack());
        Debug.Log("Player Speed: " + playerStats.GetSpeed());
        Debug.LogWarning("Begin Debug Log of stats multipliers");
        for (int i = 0; i < playerStatsMultipliers.Length; i++) { Debug.Log(i+") " + playerStatsMultipliers[i]); }
        Debug.LogWarning("End Debug Log of stats multipliers");
    }


    public void UpdateData()
    {

        DataManager.savedPlayerLevel = playerLevel;
        DataManager.savedPlayerStatsMult = playerStatsMultipliers;

    }

}
