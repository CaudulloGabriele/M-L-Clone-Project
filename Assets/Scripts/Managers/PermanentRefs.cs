using UnityEngine;

/// <summary>
/// Stores and returns the reference to all the objects permanently during the game
/// </summary>
public class PermanentRefs : MonoBehaviour
{
    //only instance of this script
    public static PermanentRefs instance;

    [Header("PLAYER REFS")]

    //reference to the player's transform
    [SerializeField]
    private Transform player;
    //reference to the player's actions manager
    [SerializeField]
    private PlayerActionsManager playerActionsManager;
    //reference to the player's interactions manager
    [SerializeField]
    private PlayerInteractionsManager playerInteractionsManager;

    [Header("DATA_MANAGER REFS")]

    //reference to the DataManager
    [SerializeField]
    private DataManager dataManager;


    private void Awake()
    {

        if (instance) Destroy(gameObject);

        else instance = this;

    }

    #region Player

    /// <summary>
    /// Returns the Transform of the player
    /// </summary>
    /// <returns></returns>
    public Transform GetPlayer() { return player; }
    /// <summary>
    /// Returns the player's actions manager
    /// </summary>
    /// <returns></returns>
    public PlayerActionsManager GetPlayerActionsManager() { return playerActionsManager; }
    /// <summary>
    /// Returns the player's interactions manager
    /// </summary>
    /// <returns></returns>
    public PlayerInteractionsManager GetPlayerInteractionsManager() { return playerInteractionsManager; }

    #endregion

    #region DataManager

    /// <summary>
    /// Returns the DataManager
    /// </summary>
    /// <returns></returns>
    public DataManager GetDataManager() { return dataManager; }

    #endregion

}
