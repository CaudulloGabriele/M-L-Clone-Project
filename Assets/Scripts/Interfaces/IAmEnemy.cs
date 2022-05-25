/// <summary>
/// Interface for the enemies
/// </summary>
public interface IAmEnemy
{
    /// <summary>
    /// Allows to set the reference of the enemy's EnemyTypesBehaviours scripts
    /// </summary>
    /// <param name="newEtb"></param>
    void SetEnemyTypesBehavioursRef(EnemyTypesBehaviours newEtb);
    /// <summary>
    /// Makes the enemy perform an action
    /// </summary>
    void PerformAction();

}
