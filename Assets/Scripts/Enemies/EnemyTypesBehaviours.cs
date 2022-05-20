using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypesBehaviours : MonoBehaviour
{

    private enum EnemyTypes 
    {
    


    }

    //riferimento allo SpriteRenderer di questo nemico
    [SerializeField]
    private SpriteRenderer thisEnemySR;
    //riferimento all'istanza del battleManager
    private BattleManager battleManager;
    //indica il tipo di questo nemico
    [SerializeField]

    private int thisEnemyType = 0;


    private void Start()
    {
        //prende il riferimento all'istanza del battleManager
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
    /// Ritorna lo sprite di questo nemico
    /// </summary>
    /// <returns></returns>
    public Sprite GetEnemySprite() { return thisEnemySR.sprite; }

}
