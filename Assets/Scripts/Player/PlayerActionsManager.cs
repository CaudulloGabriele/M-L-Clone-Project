using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsManager : MonoBehaviour
{
    //indica se è già in esecuzione un'azione
    private bool inAction = false;


    public void ManageActionForCharacter()
    {

        if (!inAction)
        {

            if (GameStateManager.IsPlayerExploring())
            {
                Debug.Log("Exploring Action");
            }
            else if (GameStateManager.IsPlayerFighting())
            {
                Debug.Log("Fighting Action");
            }

        }

    }

}
