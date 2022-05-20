using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionsManager : MonoBehaviour
{

    [Tooltip("References to the SpriteRenderers of the action blocks in this order:\n1)Current\n2)Right\n3)Back\n4)Left")]
    [SerializeField]
    private SpriteRenderer[] actionBlocksSprites;

    [Tooltip("References to all action blocks Sprites in this order:\n1)Solo\n2)Coop\n3)Run\n4)Items")]
    [SerializeField]
    private Sprite[] actionsSprites;

    //index of the current action
    private int currentActionIndex = 0;
    //number of possible actions during battle
    private int nActionBlocks = -1;


    private void Awake()
    {
        //obtains the number of possible actions during battle
        nActionBlocks = actionBlocksSprites.Length;

    }

    /// <summary>
    /// Resets the action blocks state(the Sprite they have)
    /// </summary>
    public void ResetActionBlocks()
    {

        currentActionIndex = -1;

        RotateActionBlocks(true);

    }
    /// <summary>
    /// Rotates the action blocks right or left based on the received parameter
    /// </summary>
    /// <param name="right"></param>
    public void RotateActionBlocks(bool right)
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

}
