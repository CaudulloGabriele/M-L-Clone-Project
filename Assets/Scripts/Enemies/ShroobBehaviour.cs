using System;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Manages a Shroob type enemy's behaviour
/// </summary>
public class ShroobBehaviour : MonoBehaviour, IAmEnemy
{

    #region Variables

    //reference to this enemy's EnemyTypesBehaviour script
    private EnemyTypesBehaviours etb;
    //reference to this enemy's SoloAction manager
    private SoloAction soloAction;

    //reference to the player's fight position
    private Vector2 playerFightPos;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        Debug.Log("CREATO SHROOB");

        /*CHANGE STATS MULTIPLIERS FOR SHROOB*/

    }

    private void Start()
    {
        //obtains the BattleManager instance
        BattleManager battleManager = BattleManager.instance;

        //sets the solo action
        soloAction = GetComponent<SoloAction>();
        soloAction.SetSoloActionToPerform(ShroobAttack);
        soloAction.SetPerformingPos(battleManager.GetCenterPos());

        //obtains the fighting position of the player
        playerFightPos = battleManager.GetPlayerFightPos();

    }

    #endregion

    /// <summary>
    /// Starts or ends the Shroob's Attack
    /// </summary>
    /// <param name="start"></param>
    /// <param name="anticipationTimer"></param>
    private async void ShroobAttack(bool start, float anticipationTimer = 0)
    {
        //if the solo action has to end, returns to the idle animation
        if (!start) { /*RETURNS TO IDLE ANIMATION*/ return; }

        //waits until the anticipation ends
        while (anticipationTimer > 0)
        {
            anticipationTimer -= Time.deltaTime;

            await Task.Delay(1);

        }

        /*SHOOTS BULLET TOWARDS PLAYER*/

        //waits a bit
        await Task.Delay(TimeSpan.FromSeconds(BattleActionsManager.WAIT_AFTER_END_OF_ACTION));

        //comunicates that the solo action has been performed
        soloAction.SetIfPerforming(false);

    }

    #region Interface Methods

    public void SetEnemyTypesBehavioursRef(EnemyTypesBehaviours newEtb) { etb = newEtb; }

    public void PerformAction()
    {

        soloAction.PerformSoloAction();

    }

    #endregion

}
