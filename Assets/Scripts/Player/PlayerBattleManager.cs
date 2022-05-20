using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleManager : MonoBehaviour, IUpdateData
{
    //riferimento alla classe che tiene conto delle statistiche del giocatore
    private BattleStats playerStats;
    //indica il livello del giocatore
    [SerializeField]
    private int playerLevel = 1;
    //indica di quanto devono essere moltiplicate le statistiche del giocatore
    // 0 - hp
    // 1 - attack
    // 2 - speed
    [SerializeField]
    private float[] playerStatsMultipliers = new float[3];


    private void Start()
    {
        //ottiene i valori salvati
        GetSavedPlayerStats();
        //inizializza le statistiche del giocatore in base al livello e moltiplicatore salvato
        playerStats = new BattleStats();
        //calcola le statistiche del giocatore
        CalculatePlayerStats();

    }
    /// <summary>
    /// Ottiene i valori salvati riguardanti le statistiche del giocatore
    /// </summary>
    public void GetSavedPlayerStats()
    {
        playerLevel = DataManager.savedPlayerLevel;
        playerStatsMultipliers = (float[])DataManager.savedPlayerStatsMult.Clone();

    }
    /// <summary>
    /// Calcola le statistiche del giocatore in base ai moltiplicatori
    /// </summary>
    public void CalculatePlayerStats()
    {

        bool thereWasAnError = playerStats.InitializeStats(playerLevel, playerStatsMultipliers);


        if (thereWasAnError) { Debug.LogError("C'è stato un errore con l'inizializzazione delle statistiche del giocatore"); }
        Debug.Log("Player Level: " + playerLevel);
        Debug.Log("Player HP: " + playerStats.GetCurrentHealth());
        Debug.Log("Player Attack: " + playerStats.GetAttack());
        Debug.Log("Player Speed: " + playerStats.GetSpeed());
        Debug.Log("Inizio Debug Log per moltiplicatori di statistiche");
        for (int i = 0; i < playerStatsMultipliers.Length; i++) { Debug.Log(i+") " + playerStatsMultipliers[i]); }
        Debug.Log("Fine Debug Log per moltiplicatori di statistiche");
    }


    public void UpdateData()
    {

        DataManager.savedPlayerLevel = playerLevel;
        DataManager.savedPlayerStatsMult = playerStatsMultipliers;

    }

}
