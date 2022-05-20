using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionsManager : MonoBehaviour
{
    /// <summary>
    /// enum that indicates the states of battle actions
    /// </summary>
    private enum ActionChoiceState { choosingTypeOfAction = 0, choosingItem = 1, choosingEnemyOrPlayer = 2 }

    //tells the current state of choice of battle action
    private ActionChoiceState currentActionChoiceState = ActionChoiceState.choosingTypeOfAction;

    [Tooltip("References to the SpriteRenderers of the action blocks in this order:\n1)Current\n2)Right\n3)Back\n4)Left")]
    [SerializeField]
    private SpriteRenderer[] actionBlocksSprites;

    [Tooltip("References to all action blocks Sprites in this order:\n1)Solo\n2)Coop\n3)Run\n4)Items")]
    [SerializeField]
    private Sprite[] actionsSprites;

    //reference to the instance of the BattleManager
    private BattleManager battleManager;

    //index of the current action
    private int currentActionIndex = 0;
    //index of the current selected enemy
    private int currentlySelectedEnemyIndex = -1;
    //number of possible actions during battle
    private int nActionBlocks = -1;


    private void Awake()
    {
        //obtains the number of possible actions during battle
        nActionBlocks = actionBlocksSprites.Length;

    }

    private void Start()
    {
        //gets the reference to the instance of the BattleManager
        battleManager = BattleManager.instance;

    }

    public void ChangeSelection(Vector2 selection)
    {

        if (IsChoosingAction()) RotateActionBlocks(selection.x > 0);
        else if (IsChoosingItem()) { /*CAMBIA SELEZIONE NEL MENU' DEGLI OGGETTI*/ }
        else { ChangeSelectedEnemy(selection); }
    }
    /// <summary>
    /// Resets the action blocks state
    /// </summary>
    public void ResetActionBlocks()
    {

        currentActionChoiceState = ActionChoiceState.choosingTypeOfAction;

        currentActionIndex = -1;

        RotateActionBlocks(true);

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
        if (currentActionIndex > nActionBlocks) currentActionIndex -= nActionBlocks;
        else if (currentActionIndex < 0) currentActionIndex += nActionBlocks;
        //cycles each action block and changes its sprite based on where its rotating
        for (int i = 0; i < nActionBlocks; i++)
        {

            int actionIndex = i + currentActionIndex;

            if (actionIndex >= nActionBlocks) actionIndex -= nActionBlocks;

            actionBlocksSprites[i].sprite = actionsSprites[actionIndex];

        }

    }

    private void ChangeSelectedEnemy(Vector2 selection, bool firstSelection = false)
    {

        /*SELECTS ENEMY BASED ON SELECTION VECTOR, MOVES AN ARROW IN THE ENEMY'S POSITION(GOTTEN FROM BATTLEMANAGER)*/

    }

    public void PerformAnAction()
    {

        switch (currentActionChoiceState)
        {
            //CHOOSING TYPE OF ACTION
            case ActionChoiceState.choosingTypeOfAction:
                {

                    Debug.LogError("STILL DOESN'T GO TO ENEMY OR ITEM SELECTION");

                    if (SelectedActionIsBasicAttack())
                    {

                        ChangeSelectedEnemy(Vector2.zero, true);

                        currentActionChoiceState = ActionChoiceState.choosingEnemyOrPlayer;

                    }

                    break;

                }
            //CHOOSING ITEM
            case ActionChoiceState.choosingItem:
                {

                    Debug.LogError("STILL DOESN'T EXIT FROM ITEM SELECTION MENU");

                    break;

                }
            //CHOOSING ENEMY OR PLAYER
            case ActionChoiceState.choosingEnemyOrPlayer:
                {

                    PerformCurrentAction();

                    break;

                }
            //ERROR
            default: break;

        }

    }

    public void PerformCurrentAction()
    {

        switch (currentActionIndex)
        {
            //SOLO ACTION
            case 0:
                {

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

    private bool SelectedActionIsBasicAttack() { return currentActionIndex == 0; }
    private bool SelectedActionIsCoopAttack() { return currentActionIndex == 1; }
    private bool SelectedActionIsRunAway() { return currentActionIndex == 2; }
    private bool SelectedActionIsItemUse() { return currentActionIndex == 3; }

    private bool IsChoosingAction() { return currentActionChoiceState == ActionChoiceState.choosingTypeOfAction; }
    private bool IsChoosingItem() { return currentActionChoiceState == ActionChoiceState.choosingItem; }
    private bool IsChoosingEnemyOrPlayer() { return currentActionChoiceState == ActionChoiceState.choosingEnemyOrPlayer; }

}
