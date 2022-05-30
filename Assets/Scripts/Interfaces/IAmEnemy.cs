/// <summary>
/// Interface for the enemies
/// </summary>
public interface IAmEnemy
{
    /// <summary>
    /// Allows to set the reference of the enemy's EnemyTypesBehaviours scripts
    /// </summary>
    /// <param name="newEtb"></param>
    void InitializeEnemy(EnemyTypesBehaviours newEtb);
    /// <summary>
    /// Returns this enemy's EnemyTypesBehaviuors script
    /// </summary>
    EnemyTypesBehaviours GetThisEnemyTypesBehaviour();
    /// <summary>
    /// Makes the enemy perform an action
    /// </summary>
    void PerformAction();

}
