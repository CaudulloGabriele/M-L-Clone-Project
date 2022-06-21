using UnityEngine;

public class BattleActionsManager : MonoBehaviour
{
    //indicates how much time to wait after end of an action to go ahead
    public const float WAIT_AFTER_END_OF_ACTION = 0.8f;

    /// <summary>
    /// enum that indicates the states of battle actions
    /// </summary>
    private enum ActionChoiceState { notOwnTurn = -1, choosingTypeOfAction = 0, choosingItem = 1, choosingTarget = 2 }

    #region Variables

    //tells the current state of choice of battle action
    [SerializeField]
    private ActionChoiceState currentActionChoiceState = ActionChoiceState.notOwnTurn;

    //reference to the DataManager
    private DataManager dataManager;

    [Tooltip("References to the SpriteRenderers of the action blocks in this order:\n1)Current\n2)Right\n3)Back\n4)Left")]
    [SerializeField]
    private SpriteRenderer[] actionBlocksSprites;

    [Tooltip("References to all action blocks Sprites in this order:\n1)Solo\n2)Coop\n3)Run\n4)Items")]
    [SerializeField]
    private Sprite[] actionsSprites;

    [Tooltip("References to the SpriteRenderers of the arrows")]
    [SerializeField]
    private SpriteRenderer[] sideArrowsSprites;

    //reference to the SoloAction manager script of this entity
    private SoloAction soloAction;

    //reference to the manager of this entity's dodge counterattack
    [SerializeField]
    private BattleDodgeCounter battleDodgeManager;

    //reference to the selection arrow
    [SerializeField]
    private Transform selectionArrow;
    private GameObject selectionArrowGO;

    //reference to the tip to dodge/counter the incoming attack
    [SerializeField]
    private GameObject dodgeCounterTip;
    //reference to the SpriteRenderer that shows what the player can do to neutralize the incoming attack
    [SerializeField]
    private SpriteRenderer dodgeCounterModeSR;

    //reference to the sprite to show when the player has to dodge or counter an incoming attack
    private Sprite dodgeTipSprite, 
                   counterTipSprite;

    //reference to the instance of the BattleManager
    private BattleManager battleManager;

    //index of the current action
    private int currentActionIndex = 0;
    //index of the current selected enemy
    private int currentlySelectedEnemyIndex = -1;
    //number of possible actions during battle
    private int nActionBlocks = -1;

    //indicates if an action is being performed
    private bool performingAnAction = false;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        //obtains the number of possible actions during battle
        nActionBlocks = actionBlocksSprites.Length;

        //obtains the reference to this entity's SoloAction manager script
        soloAction = GetComponent<SoloAction>();

        //obtains the reference to the gameObject of the selection arrow
        if (selectionArrow) selectionArrowGO = selectionArrow.gameObject;

        //hides the tip on how to dodge or counter the incoming attack
        ToggleDodgeCounterTip(false);

    }

    private void Start()
    {
        //gets the reference to the instance of the BattleManager
        battleManager = BattleManager.instance;

        //gets the reference to the DataManager
        dataManager = PermanentRefs.instance.GetDataManager();

        //updates the tips for dodging/countering, based on what the player has equipped
        UpdateDodgeCounterTips(dataManager.savedPlayerDodge, dataManager.savedPlayerCounter);

    }

    #endregion

    #region Action Perform

    /// <summary>
    /// Performs an action based on the current state of choice of action
    /// </summary>
    public void PerformAnAction()
    {
        //based on the current choice state, does something different
        switch (currentActionChoiceState)
        {
            //NOT OWN TURN
            case ActionChoiceState.notOwnTurn:
                {
                    //starts a dodge, since it's not own turn(if an enemy is attacking)
                    if (TurnBasedCombatManager.IsCurrentlyAnEnemyTurn()) battleDodgeManager.StartDodgeCounter();

                    break;

                }
            //CHOOSING TYPE OF ACTION
            case ActionChoiceState.choosingTypeOfAction:
                {

                    Debug.LogError("STILL DOESN'T GO TO ITEM SELECTION WHEN PERFORMING ACTION: " + SelectedActionIsBasicAttack());

                    //if the solo attack block was selected...
                    if (SelectedActionIsBasicAttack())
                    {
                        //...selects the first active enemy...
                        ChangeSelectedEnemy(Vector2.zero, true);
                        //...indicates that the player is selecting which enemy to attack
                        SetIsChoosingTarget();

                    }
                    else if (SelectedActionIsItemUse())
                    {

                        Debug.LogError("STILL DOESN'T GO TO ITEM SELECTION WHEN PERFORMING ACTION");
                    }
                    else if (SelectedActionIsRunAway())
                    {

                        Debug.LogError("NOTHING HAPPENS FOR RUN-AWAY SELECTION WHEN PERFORMING ACTION");
                    }
                    else if (SelectedActionIsCoopAttack())
                    {

                        Debug.LogError("NOTHING HAPPENS FOR CO-OP ACTION SELECTION WHEN PERFORMING ACTION");
                    }

                    break;

                }
            //CHOOSING ITEM
            case ActionChoiceState.choosingItem:
                {

                    Debug.LogError("STILL DOESN'T EXIT FROM ITEM SELECTION MENU WHEN PERFORMING ACTION");

                    //NEEDS TO GO TO STATE "choosingEnemyOrPlayer" AND CLOSE ITEM MENU

                    break;

                }
            //CHOOSING TARGET
            case ActionChoiceState.choosingTarget:
                {
                    //performs the selected action
                    PerformCurrentAction();

                    break;

                }
            //ERROR
            default: break;

        }

    }
    /// <summary>
    /// Cancels the current action
    /// </summary>
    public void ExitCurrentChoiceOfAction()
    {
        //based on the current choice state, does something different
        switch (currentActionChoiceState)
        {
            //NOT OWN TURN
            case ActionChoiceState.notOwnTurn:
                {


                    break;

                }
            //CHOOSING TYPE OF ACTION
            case ActionChoiceState.choosingTypeOfAction:
                {

                    Debug.LogError("DOESN'T NEED TO GO BACK WHEN CANCELLING");

                    /*
                    if (SelectedActionIsBasicAttack())
                    {

                        ChangeSelectedEnemy(Vector2.zero, true);

                        currentActionChoiceState = ActionChoiceState.choosingEnemyOrPlayer;

                    }
                    */

                    break;

                }
            //CHOOSING ITEM
            case ActionChoiceState.choosingItem:
                {

                    Debug.LogError("STILL DOESN'T EXIT FROM ITEM SELECTION MENU WHEN CANCELLING");

                    SetIsChoosingAction();

                    break;

                }
            //CHOOSING TARGET
            case ActionChoiceState.choosingTarget:
                {
                    //returns to the action blocks selection state
                    SetIsChoosingAction();
                    //hides the selection arrow
                    PositionSelectionArrow(Vector2.zero, true);

                    break;

                }
            //ERROR
            default: break;

        }

    }
    /// <summary>
    /// Performs the current action
    /// </summary>
    public void PerformCurrentAction()
    {
        //comunicates that an action is being performed
        SetIfPerformingAnAction(true);
        //hides the action blocks
        SetActionBlocksVisibility(false);
        //hides the selection arrow
        PositionSelectionArrow(Vector2.zero, true);

        switch (currentActionIndex)
        {
            //SOLO ACTION
            case 0:
                {
                    //sets the selected enemy's damage point position as the performing position of the solo action of this entity, if any where selected
                    if (currentlySelectedEnemyIndex != -1)
                    {
                        Vector2 soloActionPerformPos = battleManager.GetActiveEnemyAtIndex(currentlySelectedEnemyIndex).GetDamagePointPos();
                        soloAction.SetPerformingPos(soloActionPerformPos);

                    }
                    //performs the solo action
                    soloAction.PerformSoloAction();

                    Debug.Log("Solo Action");

                    break;

                }
            //CO-OP ACTION
            case 1:
                {

                    Debug.Log("Co-op Action");

                    break;

                }
            //RUN ACTION
            case 2:
                {

                    Debug.Log("Run Action");

                    break;

                }
            //ITEMS ACTION
            case 3:
                {

                    Debug.Log("Items Action");

                    break;

                }
            //ERROR
            default:
                {

                    Debug.LogError("Error: the current action index is wrong! -> " + currentActionIndex);

                    break;

                }

        }

    }
    /// <summary>
    /// Manages what happens when an action has ended
    /// </summary>
    public void EndedAnAction()
    {

        SetIsNotOwnTurn();

        battleManager.OnTurnFinished();
    
    }

    #endregion

    #region Selection Managment

    /// <summary>
    /// Allows to change the current selection(be it of an action, item or enemy)
    /// </summary>
    /// <param name="selection"></param>
    public void ChangeSelection(Vector2 selection)
    {
        //if an action is being chosen, rotates the action blocks in the desired direction
        if (IsChoosingAction()) RotateActionBlocks(selection.x > 0);
        //otherwise, if an item is being chosen, moves the selection to the desired item
        else if (IsChoosingItem()) { /*CAMBIA SELEZIONE NEL MENU' DEGLI OGGETTI*/ }
        //otherwise, an enemy is being chosen so selects the next enemy based on the desired direction
        else { ChangeSelectedEnemy(selection); }
    }
    /// <summary>
    /// Changes the currently selected enemy
    /// </summary>
    /// <param name="selection"></param>
    /// <param name="firstSelection"></param>
    private void ChangeSelectedEnemy(Vector2 selection, bool firstSelection = false)
    {
        //if this is the first selection, selects the first active enemy
        if (firstSelection) currentlySelectedEnemyIndex = 0;
        //otherwise...
        else
        {
            //...selects the next enemy based on selection...
            currentlySelectedEnemyIndex += (selection.x > 0) ? 1 : -1;

            //...and makes sure the index doesn't go out of the expected range
            int currentlyActiveEnemiesAmount = battleManager.GetNumberOfCurrentlyActiveEnemies();

            if (currentlySelectedEnemyIndex >= currentlyActiveEnemiesAmount) currentlySelectedEnemyIndex = 0;

            else if (currentlySelectedEnemyIndex < 0) currentlySelectedEnemyIndex = currentlyActiveEnemiesAmount - 1;

        }

        //positions the arrow over the selected enemy
        Vector2 overEnemyPos = battleManager.GetActiveEnemyAtIndex(currentlySelectedEnemyIndex).GetSelectionArrowPos();
        PositionSelectionArrow(overEnemyPos);

    }

    #endregion

    #region "HUD" Management

    /// <summary>
    /// Resets the action blocks state
    /// </summary>
    public void ResetActionBlocks()
    {
        //shows the action blocks
        SetActionBlocksVisibility(true);
        //sets that this entity is currently choosing an action
        SetIsChoosingAction();
        //sets the first action block as selected
        currentActionIndex = -1;
        RotateActionBlocks(true);

    }
    /// <summary>
    /// Allows to show or hide this entity's action blocks
    /// </summary>
    /// <param name="show"></param>
    public void SetActionBlocksVisibility(bool show)
    {

        Color visibilityColor = show ? Color.white : Color.clear;
        foreach (SpriteRenderer actionBlock in actionBlocksSprites) { actionBlock.color = visibilityColor; }
        foreach (SpriteRenderer arrow in sideArrowsSprites) { arrow.color = visibilityColor; }

    }
    /// <summary>
    /// Rotates the action blocks right or left based on the received parameter
    /// </summary>
    /// <param name="right"></param>
    private void RotateActionBlocks(bool right)
    {
        //increments or decrements the index of the current action based on the parameter
        currentActionIndex += right ? 1 : -1;
        //corrects the index if is out of the array range
        if (currentActionIndex >= nActionBlocks) currentActionIndex -= nActionBlocks;
        else if (currentActionIndex < 0) currentActionIndex += nActionBlocks;
        //cycles each action block and changes its sprite based on where its rotating
        for (int i = 0; i < nActionBlocks; i++)
        {

            int actionIndex = i + currentActionIndex;

            if (actionIndex >= nActionBlocks) actionIndex -= nActionBlocks;

            actionBlocksSprites[i].sprite = actionsSprites[actionIndex];

        }

    }
    /// <summary>
    /// Allows to change the position of the selection arrow or hide it
    /// </summary>
    /// <param name="newPos"></param>
    private void PositionSelectionArrow(Vector2 newPos, bool hide = false)
    {
        //if there is no reference to the selection arrow, returns nothing
        if (!selectionArrow) return;

        selectionArrowGO.SetActive(!hide);

        if (hide) return;

        selectionArrow.position = newPos;

    }
    /// <summary>
    /// Allows to show or hide the tip for how to dodge or counter an incoming attack
    /// </summary>
    /// <param name="toggle"></param>
    /// <param name="hasToDodge"></param>
    public void ToggleDodgeCounterTip(bool toggle, bool hasToDodge = true)
    {
        //if there is no reference to the dodge/counter tip, nothing happens
        if (!dodgeCounterTip) return;

        //toggles the dodge/counter tip
        dodgeCounterTip.SetActive(toggle);

        //if hidden, the sprite doesn't get changed
        if (!toggle) return;

        //changes the sprite to show how to neutralize the incoming attack
        dodgeCounterModeSR.sprite = hasToDodge ? dodgeTipSprite : counterTipSprite;


    }
    /// <summary>
    /// Allows to change the tips for dodging and countering
    /// </summary>
    /// <param name="dodgeIndex"></param>
    /// <param name="counterIndex"></param>
    public void UpdateDodgeCounterTips(int dodgeIndex, int counterIndex)
    {
        dodgeTipSprite = BattleHUD.GetDodgeCounterSpriteByIndex(true, dodgeIndex);
        counterTipSprite = BattleHUD.GetDodgeCounterSpriteByIndex(false, counterIndex);

    }

    #endregion



    #region Getter Methods for Current Action

    /// <summary>
    /// Returns true if this entity is currently performing a solo action
    /// </summary>
    /// <returns></returns>
    public bool SelectedActionIsBasicAttack() { return currentActionIndex == 0; }
    /// <summary>
    /// Returns true if this entity is currently performing a co-op action
    /// </summary>
    /// <returns></returns>
    public bool SelectedActionIsCoopAttack() { return currentActionIndex == 1; }
    /// <summary>
    /// Returns true if this entity is currently running away
    /// </summary>
    /// <returns></returns>
    public bool SelectedActionIsRunAway() { return currentActionIndex == 2; }
    /// <summary>
    /// Returns true if this entity is using an item
    /// </summary>
    /// <returns></returns>
    public bool SelectedActionIsItemUse() { return currentActionIndex == 3; }

    #endregion

    #region Action Performing

    /// <summary>
    /// Returns wheter or not an action is currently being performed
    /// </summary>
    /// <returns></returns>
    public bool IsPerformingAnAction() { return performingAnAction; }
    /// <summary>
    /// Allows to set if there is currently an action being performed or not
    /// </summary>
    /// <param name="performing"></param>
    public void SetIfPerformingAnAction(bool performing) { performingAnAction = performing; }

    #endregion

    #region Getter Methods for Current Action Choice State

    /// <summary>
    /// Returns true if the current turn is not of this entity
    /// </summary>
    /// <returns></returns>
    private bool IsNotOwnTurn() { return currentActionChoiceState == ActionChoiceState.notOwnTurn; }
    /// <summary>
    /// Returns true if this entity is currently choosing an action to perform
    /// </summary>
    /// <returns></returns>
    private bool IsChoosingAction() { return currentActionChoiceState == ActionChoiceState.choosingTypeOfAction; }
    /// <summary>
    /// Returns true if this entity is choosing an item to use
    /// </summary>
    /// <returns></returns>
    private bool IsChoosingItem() { return currentActionChoiceState == ActionChoiceState.choosingItem; }
    /// <summary>
    /// Returns true if this entity is choosing a target for its decided action
    /// </summary>
    /// <returns></returns>
    private bool IsChoosingTarget() { return currentActionChoiceState == ActionChoiceState.choosingTarget; }

    #endregion

    #region Setter Methods for Current Action Choice State

    /// <summary>
    /// Sets that the current turn is not the one of this entity
    /// </summary>
    private void SetIsNotOwnTurn() { currentActionChoiceState = ActionChoiceState.notOwnTurn; }
    /// <summary>
    /// Sets that this entity is choosing an action to perform
    /// </summary>
    private void SetIsChoosingAction() { currentActionChoiceState = ActionChoiceState.choosingTypeOfAction; }
    /// <summary>
    /// Sets that this entity is choosing an item to use
    /// </summary>
    private void SetIsChoosingItem() { currentActionChoiceState = ActionChoiceState.choosingItem; }
    /// <summary>
    /// Sets that this entity is choosing a target for the decided action
    /// </summary>
    private void SetIsChoosingTarget() { currentActionChoiceState = ActionChoiceState.choosingTarget; }

    #endregion

    #region Getter Methods for Actions Scripts

    /// <summary>
    /// Returns this entity's SoloAction manager
    /// </summary>
    /// <returns></returns>
    public SoloAction GetSoloActionManager() { return soloAction; }

    #endregion

}
