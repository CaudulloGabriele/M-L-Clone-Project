
public class EnemyStats
{

    private static int[] enemiesLevel =
    {

        1, //shroob
        3, //commander shroob
        2, //shroob-omb
        5, //shroob rex
        5, //ufo shroob
        10, //mother ufo shroob
        7, //fawful
        15, //princess shroob
        15, //elder princess shroob
        20 //final boss

    };

    private static float[] enemiesHealthMult =
        {
            
        0.2f, //shroob
        0.3f, //commander shroob
        1f, //shroob-omb
        0.5f, //shroob rex
        0.35f, //ufo shroob
        0.7f, //mother ufo shroob
        1f, //fawful
        2f, //princess shroob
        2.5f, //elder princess shroob
        4f //final boss

        };

    private static float[] enemiesAtkMult =
        {

        1f, //shroob
        0.45f, //commander shroob
        1f, //shroob-omb
        0.25f, //shroob rex
        0.35f, //ufo shroob
        0.9f, //mother ufo shroob
        1.1f, //fawful
        2f, //princess shroob
        3.35f, //elder princess shroob
        4f //final boss

        };

    private static float[] enemiesSpeedMult =
        {

        0.2f, //shroob
        0.7f, //commander shroob
        0.1f, //shroob-omb
        1f, //shroob rex
        2f, //ufo shroob
        3f, //mother ufo shroob
        2f, //fawful
        2f, //princess shroob
        1.5f, //elder princess shroob
        3f //final boss

        };


    /// <summary>
    /// Returns the level of the desired enemy
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    public static int GetEnemyLevel(int enemy) { return enemiesLevel[enemy]; }
    /// <summary>
    /// Returns the stats multipliers of the desired enemy
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    public static float[] GetEnemyStatsMult(int enemy)
    {

        return new float[] { enemiesHealthMult[enemy], enemiesAtkMult[enemy], enemiesSpeedMult[enemy] };

    }

}
