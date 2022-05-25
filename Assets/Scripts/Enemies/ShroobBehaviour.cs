using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroobBehaviour : MonoBehaviour, IAmEnemy
{

    private EnemyTypesBehaviours etb;


    private void Awake()
    {
        Debug.Log("CREATO SHROOB");

        /*CHANGE STATS MULTIPLIERS FOR SHROOB*/

    }

    #region Interface Methods

    public void SetEnemyTypesBehaviuorsRef(EnemyTypesBehaviours newEtb) { etb = newEtb; }

    #endregion

}
