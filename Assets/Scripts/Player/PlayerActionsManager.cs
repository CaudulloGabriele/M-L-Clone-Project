using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerActionsManager : MonoBehaviour
{

    #region Variables

    [Header("IN EXPLORATION")]

    //reference to the player's interactions manager
    [SerializeField]
    private PlayerInteractionsManager playerInteractionsManager;

    [Header("IN BATTLE")]
    [Header("")]

    //reference to the EntityBattleManager of the player
    [SerializeField]
    private EntityBattleManager playerEbm;
    //reference to the script that manages the battle actions of the player(during battle)
    [SerializeField]
    private BattleActionsManager battleActionsManager;
    //reference to the manager of the player's SoloAction
    private SoloAction soloAction;

    //reference to the player's solo attack(damageable damage giver)
    [SerializeField]
    private GameObject playerSoloAttackGO;
    private IDamageable playerSoloAttack;

    //indicates how fast the player has to press the action button during a solo action to be too early and miss anyway
    [SerializeField]
    private float tooEarlyHit = 0.2f;
    //indicates how much weaker a missed hit is compared to a perfect hit
    [SerializeField]
    private float missedHitDiminisher = 2;

    //indicates if an action is in execution
    //private bool inAction = false;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        //obtains the reference to the player's solo attack(damageable damage giver)
        playerSoloAttack = playerSoloAttackGO.GetComponent<IDamageable>();
        playerSoloAttackGO.SetActive(false);

    }

    private void Start()
    {
        //sets the delegates for the various battle actions of the player
        SetActionsDelegates();

    }

    #endregion

    #region Actions Managment

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

            if (confirm) { playerInteractionsManager.InteractWithCloseObject(); }
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
        //if the solo action has to end...
        if (!start)
        {
            //...returns to the idle animation...
            /*RETURNS TO IDLE ANIMATION*/

            //...deactivates the player's solo attack...
            playerSoloAttackGO.SetActive(false);

            //...and nothing more
            return;

        }

        //indicates until when the player's action button press will count as a miss anyway
        float tooEarlyTimeSpan = (timeSpan - tooEarlyHit);
        //indicates if the player executed a perfect hit or not
        bool perfectHit = false;

        //while the timer hasn't ended...
        while (timeSpan > 0)
        {
            //...if the player presses the action button...
            if (Input.GetButtonDown("Action"))
            {
                //...comunicates that the player executed a perfect hit, if it is not too early...
                bool tooEarly = timeSpan > tooEarlyTimeSpan;
                if (!tooEarly) perfectHit = true;
                //...and ends the anticipation of the solo action
                timeSpan = 0;

            }
            //...otherwise, the timer continues...
            timeSpan -= Time.deltaTime;
            //...and waits 1 millisecond for the next check
            await Task.Delay(1);

        }

        //calculates the damage of the attack based on the player's timing of attack and activates the attack
        float attackDmg = playerEbm.GetEntityAttack();
        if (!perfectHit) attackDmg /= missedHitDiminisher;
        playerSoloAttack.SetDamage(attackDmg);
        playerSoloAttackGO.SetActive(true);

        //waits a bit before ending the action
        int waitBeforeEnding = (int)(BattleActionsManager.WAIT_AFTER_END_OF_ACTION * 1000);
        await Task.Delay(waitBeforeEnding);

        //the solo action ends
        soloAction.SetIfPerforming(false);


        Debug.LogWarning(perfectHit ? "Perfect Hit" : "Missed Hit");
    }

    #endregion

}
