using System.Threading.Tasks;
using UnityEngine;

public class PlayerBattleManager : EntityBattleManager, IUpdateData
{

    #region Variables

    //reference to the DataManager
    private DataManager dataManager;
    //reference to the GameOverManager
    private GameOverManager gameOverManager;

    //reference to the player's battle actions manager
    [SerializeField]
    private BattleActionsManager battleActionsManager;

    #endregion

    #region MonoBehaviour Methods

    protected override async void OnEnable()
    {

        transform.parent = null;

        //waits for the updating of the stats in the Start
        await Task.Delay(1);

        base.OnEnable();

    }

    protected override void Start()
    {
        base.Start();

        //gets some permanent references
        PermanentRefs permaRefs = PermanentRefs.instance;
        dataManager = permaRefs.GetDataManager();
        gameOverManager = permaRefs.GetGameOverManager();

        //gets the saved values
        GetSavedPlayerStats();
        //initializes the player's battle stats
        entityStats = new BattleStats();

        //calculates player's stats
        //CalculatePlayerStats();

        //Debug.Log("GETTING SAVED STATS");
    }

    #endregion

    #region Player Stats Methods

    /// <summary>
    /// Gets the player's saved battle stats values
    /// </summary>
    public void GetSavedPlayerStats()
    {
        entityLevel = dataManager.savedPlayerLevel;
        entityStatsMult = (float[])dataManager.savedPlayerStatsMult.Clone();

    }
    /// <summary>
    /// Calculates the player's battle stats based on the multipliers
    /// </summary>
    /// <param name="notStartCalculation">indicates wheter or not this calculation is the starting one or another</param>
    public void CalculatePlayerStats(bool notStartCalculation)
    {
        //gets the previous health, if this is not the starting calculation
        float previousHealth = 0;
        if (notStartCalculation) previousHealth = entityStats.GetCurrentHealth();

        //calculates the player's stats
        OnEnable();

        //if this is not the starting calculation, the current health is set to the one before the calculation
        if (notStartCalculation) entityStats.SetCurrentHealth(previousHealth);


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

    #endregion

    #region EntityBattleManager Methods

    public override void StartOwnTurn()
    {
        base.StartOwnTurn();

        //resets the player's action blocks
        battleActionsManager.ResetActionBlocks();

        //hides the tip on how to dodge/counter the incoming attack(since it's the player's turn)
        battleActionsManager.ToggleDodgeCounterTip(false);

    }

    
    public override void SetEntityDodgeCounter(bool hasToDodge, bool onlyThis)
    {
        base.SetEntityDodgeCounter(hasToDodge, onlyThis);

        //shows which button to press to dodge/counter the incoming attack
        battleActionsManager.ToggleDodgeCounterTip(true, hasToDodge);


        Debug.LogError("Player can now " + (hasToDodge ? "dodge" : "counter") + " attacks -> " + onlyThis);
    }
    

    public override void ChangeHealth(float dmg)
    {
        base.ChangeHealth(dmg);

        Debug.Log("PLAYER AFTER DAMAGE: " + entityStats.GetCurrentHealth());
    }

    protected override void EntityDeath()
    {
        base.EntityDeath();

        /*MAKE SOMETHING HAPPEN WHEN PLAYER IS DEFEATED-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        //comunicates that the player died and the game is over
        gameOverManager.SetGameOverState(true);

    }

    #endregion

    #region IUpdateData Methods

    public void OnLoad() { }

    public void UpdateData()
    {

        dataManager.savedPlayerLevel = entityLevel;
        dataManager.savedPlayerStatsMult = entityStatsMult;

    }

    #endregion

}
