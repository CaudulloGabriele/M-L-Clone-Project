using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypesBehaviours : MonoBehaviour
{

    private enum EnemyTypes 
    {
    


    }

    //references to the SpriteRenderer of this Enemy
    [SerializeField]
    private SpriteRenderer thisEnemySR;
    //reference to the instance of the BattleManager
    private BattleManager battleManager;
    //indicates the enemy's type
    [SerializeField]
    private int thisEnemyType = 0;


    private void Start()
    {
        //gets the BattleManager instance
        battleManager = BattleManager.instance;

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F12))
        {
            battleManager.AnEnemyWasDefeated(transform.GetSiblingIndex(), thisEnemyType);
        }

    }

    /// <summary>
    /// Returns this enemy's sprite
    /// </summary>
    /// <returns></returns>
    public Sprite GetEnemySprite() { return thisEnemySR.sprite; }

}
