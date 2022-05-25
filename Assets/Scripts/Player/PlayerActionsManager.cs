using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerActionsManager : MonoBehaviour
{
    //reference to the script that manages the battle actions of the player(during battle)
    [SerializeField]
    private BattleActionsManager battleActionsManager;

    //reference to the manager of the player's SoloAction
    private SoloAction soloAction;

    //indicates if an action is in execution
    //private bool inAction = false;


    private void Start()
    {
        //sets the delegates for the various battle actions of the player
        SetActionsDelegates();

    }

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

            if (confirm) { /*INTERACTION WITH CLOSE OBJECT*/ }
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

    /// <summary>
    /// Sets the delegates for the various battle actions of the player
    /// </summary>
    private void SetActionsDelegates()
    {
        //SOLO ACTION
        soloAction = battleActionsManager.GetSoloActionManager();
        soloAction.SetSoloActionToPerform(PlayerSoloAction);

    }

    /// <summary>
    /// Starts or ends the player's solo action
    /// </summary>
    /// <param name="start"></param>
    /// <param name="timeSpan"></param>
    private async void PlayerSoloAction(bool start, float timeSpan = 0)
    {
        //if the solo action has to end, returns to the idle animation
        if (!start) { /*RETURNS TO IDLE ANIMATION*/ return; }

        //while the timer hasn't ended...
        while (timeSpan > 0)
        {
            //...if the player presses the action button...
            if (Input.GetButtonDown("Action"))
            {
                //...ends the anticipation of the solo action...
                soloAction.SetIfPerforming(false);

                Debug.LogWarning("Perfect Hit");

                //...and ends the solo action
                return;

            }
            //...otherwise, the timer continues...
            timeSpan -= Time.deltaTime;
            //...and waits 1 millisecond for the next check
            await Task.Delay(1);
            
        }

        soloAction.SetIfPerforming(false);

        Debug.LogWarning("Missed Hit");

    }

}
