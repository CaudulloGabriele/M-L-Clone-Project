using UnityEngine;

/// <summary>
/// Manages collisions during a fight between fighters and attacks
/// </summary>
public class BattleCollisionsManager : MonoBehaviour, IDamageable
{
    //reference to this entity's battle manager(if any)
    [SerializeField]
    private EntityBattleManager thisEntity;

    //indicates how much damage this entity gives on contact(if it's a damage giver)
    [SerializeField]
    private float dmgToGive;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks if collided with a damageable object
        IDamageable collDamageable = collision.GetComponent<IDamageable>();
        if (collDamageable != null)
        {
            //if these two damageables are of the same type, nothing happens
            if (IsDamageGiver() == collDamageable.IsDamageGiver()) return;

            /*
            //if this is an entity that gives damage, inflicts damage to the collided entity
            if (IsDamageGiver())
            {

                collDamageable.TakeDamage(dmgToGive);

            }
            else //otherwise this is an entity that gets damaged, so this entity takes damage based on the collided entity damage
            */

            //takes damage based on the collided entity damage, if this entity takes damage
            if (!IsDamageGiver())
            {
                float dmgToReceive = collDamageable.GetDamage();
                TakeDamage(dmgToReceive);

            }

        }
        
    }


    public bool IsDamageGiver() { return dmgToGive != 0; }

    public float GetDamage() { return dmgToGive; }

    public void TakeDamage(float dmg)
    {

        if (!thisEntity) { Debug.LogError("COULDN'T TAKE DAMAGE BECAUSE THERE IS NO REFERENCE TO THIS ENTITY_BATTLE_MANAGER: " + name); return; }

        thisEntity.ChangeHealth(dmg);

    }

}
