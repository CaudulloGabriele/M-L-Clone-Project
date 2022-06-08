using System.Threading.Tasks;
using UnityEngine;

public class SoloAction : MonoBehaviour
{
    /// <summary>
    /// Delegate for SoloAction
    /// </summary>
    /// <param name="start"></param>
    /// <param name="timer"></param>
    public delegate void SoloActionDelegate(bool start, float timer = 0);
    //delegate for solo action of this entity
    private SoloActionDelegate soloAction;

    #region Variables

    //reference to the BattleActionsManager of this entity
    private BattleActionsManager battleActionsManager;
    //reference to the performer of the solo action and its components
    [SerializeField]
    private Transform performer;
    private CharacterMovement performerCharMove;
    //reference to the position in which the entity has to go to perform the solo action
    private Vector2 performingPos;

    //indicates the moving speed(only when not using CharacterMovement)
    [Tooltip("To use ONLY if there is no CharacterMovement on the performer")]
    [SerializeField]
    private float moveSpeed = 10;
    //indicates how close the entity has to be close to the position in which it's going to stop moving
    [SerializeField]
    private float closeEnoughDist = 0.1f;

    //indicates how much the anticipation attack lasts
    [SerializeField]
    private float anticipationTimer = 1;
    //private float startAnticipationTimer;


    //indicates whether or not the solo action is being performed
    private bool isPerforming = false;

    //indicates if there has to be an anticipation before attacking
    //[SerializeField]
    //private bool hasToAnticipate = false;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        //obtains the reference to the BattleActionsManager of this entity
        battleActionsManager = GetComponent<BattleActionsManager>();

        //tries to obtain the performer's components
        performerCharMove = performer.GetComponent<CharacterMovement>();

        //obtains the timers starting values
        //startAnticipationTimer = anticipationTimer;

    }

    #endregion

    #region Performation Methods

    /// <summary>
    /// Performs the solo action
    /// </summary>
    public async void PerformSoloAction()
    {
        //obtains the performer position before starting the solo action
        Vector2 prevPos = performer.position;

        //moves the performer to the performing position
        MoveToPos(performingPos);
        //waits that the performer is close enough to the performing position
        while (!IsCloseToPos(performingPos)) { await Task.Delay(1); }

        Debug.LogWarning("Finished moving to performPos");

        //performs the solo action
        soloAction(true, anticipationTimer);
        //waits for the action to end
        SetIfPerforming(true);
        while (isPerforming) { await Task.Delay(1); }

        //waits for the anticipation to end, if there has to be an anticipation
        /*
        if (hasToAnticipate)
        {
            while (!HasAnticipationEnded()) { await Task.Delay(1); }

        }
        */

        Debug.LogWarning("Finished solo action");

        //stops(or in any case, ends) the solo action
        soloAction(false);

        //moves back to the starting position
        MoveToPos(prevPos);
        //waits that the performer is close enough to the starting position
        while (!IsCloseToPos(prevPos)) { await Task.Delay(1); }
        performer.position = prevPos;

        //informs the battle actions manager that this entity has ended the action and is not performing one anymore
        battleActionsManager.SetIfPerformingAnAction(false);
        battleActionsManager.EndedAnAction();
        /*
        if (battleActionsManager)
        {
            battleActionsManager.SetIfPerformingAnAction(false);
            battleActionsManager.EndOfAction();

        }
        else { BattleManager.instance.OnTurnFinished(); }
        */

        Debug.LogWarning("Finished moving to start pos");
    }
    /// <summary>
    /// Moves the performer to a new position
    /// </summary>
    /// <param name="newPos"></param>
    private async void MoveToPos(Vector2 newPos)
    {
        //Debug.LogError("START MOVING");

        //moves the performer based on the components references
        if (performerCharMove)
        {
            //Debug.LogError("YES CHARACTER MOVEMENT");

            //the performer keeps moving close to the new position until is close enough
            while (!IsCloseToPos(newPos))
            {

                performerCharMove.Move((newPos - (Vector2)performer.position).normalized);

                await Task.Delay(1);

            }

        }
        else
        {
            //the performer keeps moving close to the new position until is close enough
            while (!IsCloseToPos(newPos))
            {

                performer.position = Vector3.MoveTowards(performer.position, newPos, moveSpeed * Time.deltaTime);

                await Task.Delay(1);

            }

            //Debug.LogError("NO CHARACTER MOVEMENT");
        }

        //Debug.LogError("END MOVING");
    }

    #endregion

    #region Check Methods

    /// <summary>
    /// Returns if the entity is close to the received position
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool IsCloseToPos(Vector2 pos)
    {

        float dist = Vector2.Distance(pos, performer.position);

        bool closeEnough = dist <= closeEnoughDist;

        return closeEnough;
    
    }

    /*
    private bool HasAnticipationEnded()
    {

        anticipationTimer -= Time.deltaTime;

        bool ended = anticipationTimer <= 0;

        if (ended) { anticipationTimer = startAnticipationTimer; }

        return ended;

    }
    */

    public void SetIfPerforming(bool state) { isPerforming = state; }

    #endregion

    /// <summary>
    /// Allows to set the solo action to perform for this entity
    /// </summary>
    /// <param name="newSoloAction"></param>
    public void SetSoloActionToPerform(SoloActionDelegate newSoloAction) { soloAction += newSoloAction; }
    /// <summary>
    /// Allows to change the performing position
    /// </summary>
    /// <param name="newPos"></param>
    public void SetPerformingPos(Vector2 newPos) { performingPos = newPos; }

}
