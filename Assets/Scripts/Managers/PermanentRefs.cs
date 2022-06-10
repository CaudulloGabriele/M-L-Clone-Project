using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PermanentRefs : MonoBehaviour
{
    //only instance of this script
    public static PermanentRefs instance;

    //reference to the player's transform
    [SerializeField]
    private Transform player;
    //reference to the player's actions manager
    [SerializeField]
    private PlayerActionsManager playerActionsManager;
    //reference to the player's interactions manager
    [SerializeField]
    private PlayerInteractionsManager playerInteractionsManager;


    private void Awake()
    {

        if (instance) Destroy(gameObject);

        else instance = this;

    }

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

}
