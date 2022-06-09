/// <summary>
/// Interface for all the objects who have to give or take damage
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Returns wheter this damageable object is a damage giver or a damage taker
    /// </summary>
    /// <returns></returns>
    bool IsDamageGiver();
    /// <summary>
    /// Allows to make this damageable take damage
    /// </summary>
    /// <param name="dmg"></param>
    void TakeDamage(float dmg);
    /// <summary>
    /// Returns the damage this damageable gives(if any)
    /// </summary>
    /// <returns></returns>
    float GetDamage();

}
