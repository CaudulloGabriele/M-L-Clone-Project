using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsManager : MonoBehaviour
{
    //reference to the script that manages the battle actions of the player(during battle)
    [SerializeField]
    private BattleActionsManager battleActionsManager;

    //indicates if an action is in execution
    private bool inAction = false;


    /// <summary>
    /// Activates the right action of the player based on the game's state
    /// </summary>
    public void ManageActionForCharacter()
    {
        //if an action is in execution, no other action can be executed
        if (inAction) return;

        //if the player is exploring...
        if (GameStateManager.IsPlayerExploring())
        {
            Debug.Log("Exploring Action");
        }
        //otherwise, if the player is fighting...
        else if (GameStateManager.IsPlayerFighting())
        {
            //...performs the currently selected action
            battleActionsManager.PerformCurrentAction();

        }

    }

}
