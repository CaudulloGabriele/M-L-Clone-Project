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
    //reference to the empty where bullets will come from
    private Transform bulletSpawner;

    //reference to the player's fight position
    private Vector2 playerFightPos;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        Debug.Log("CREATO SHROOB");
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

        //shoots the bullet towards the player position, while setting its damage
        GameObject bullet = ObjectPooling.GetObjectFromPool("ShroobBullet");
        BulletsBehaviour shroobBullet = bullet.GetComponent<BulletsBehaviour>();
        shroobBullet.SetBulletDamage(etb.GetEnemyAttack());
        shroobBullet.ShootBulletToTarget(playerFightPos, bulletSpawner.position);

        //waits a bit
        await Task.Delay(TimeSpan.FromSeconds(BattleActionsManager.WAIT_AFTER_END_OF_ACTION));

        //comunicates that the solo action has been performed
        soloAction.SetIfPerforming(false);

    }

    #region Interface Methods

    /// <summary>
    /// Allows to initialize the references of this enemy
    /// </summary>
    /// <param name="newEtb"></param>
    public void InitializeEnemy(EnemyTypesBehaviours newEtb)
    {
        
        etb = newEtb;

        bulletSpawner = etb.GetEnemyAttackPosition();

    }
    /// <summary>
    /// Makes the Shroob perform a battle action
    /// </summary>
    public void PerformAction()
    {

        soloAction.PerformSoloAction();

    }
    /// <summary>
    /// Returns the Shroob's EnemyTypesBehaviuor script
    /// </summary>
    /// <returns></returns>
    public EnemyTypesBehaviours GetThisEnemyTypesBehaviour() { return etb; }

    #endregion

}
