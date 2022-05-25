using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsManager : MonoBehaviour
{
    //reference to the script that manages the battle actions of the player(during battle)
    [SerializeField]
    private BattleActionsManager battleActionsManager;

    //indicates if an action is in execution
    //private bool inAction = false;


    /// <summary>
    /// Activates the right action of the player based on the game's state
    /// </summary>
    /// <param name="confirm"></param>
    /// <param name="actionOfFirstCharacter"></param>
    public void ManageActionForCharacter(bool confirm, bool actionOfFirstCharacter)
    {
        //if an action is in execution, no other action can be executed
        if (battleActionsManager.IsPerformingAnAction()) return;

        //if the player is exploring...
        if (GameStateManager.IsPlayerExploring())
        {
            Debug.Log("Exploring Action");

            if (confirm) { }
            else { }

        }
        //otherwise, if the player is fighting...
        else if (GameStateManager.IsPlayerFighting())
        {
            //...performs or cancels the currently possible battle action
            if (confirm) { battleActionsManager.PerformAnAction(); }
            else { battleActionsManager.ExitCurrentChoiceOfAction(); }

        }

    }

}
