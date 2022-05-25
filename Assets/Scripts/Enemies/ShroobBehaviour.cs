using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroobBehaviour : MonoBehaviour, IAmEnemy
{

    private EnemyTypesBehaviours etb;


    #region Interface Methods

    public void SetEnemyTypesBehaviuorsRef(EnemyTypesBehaviours newEtb) { etb = newEtb; }

    #endregion

}
