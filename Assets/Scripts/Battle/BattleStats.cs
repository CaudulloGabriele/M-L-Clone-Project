using System;

/// <summary>
/// Classe per le statistiche di battaglia dei nemici e giocatore
/// </summary>
public class BattleStats
{

    #region Variables

    private const int N_OF_STATS = 3;

    //indica quanta vita si ha al momento
    private float hp = 1;
    //indica la propria vita massima
    private float maxHp = 1;
    //indica quanto male si fa con un attacco
    private float attack = 1;
    //indica la propria velocità, che indicherà l'ordine di turno
    private float speed = 1;

    #endregion

    #region Initialization

    /// <summary>
    /// Inizializza le statistiche in base al livello. Permette inoltre di modificarle con un moltiplicatore.
    /// </summary>
    /// <param name="level"></param>
    /// <param name="statsMultiplier">
    /// indica di quanto devono essere moltiplicate le statistiche del giocatore
    /// 0 - hp
    /// 1 - attack
    /// 2 - speed
    /// </param>
    public bool InitializeStats(int level, float[] statsMultiplier = null)
    {
        //crea una variabile che indica se c'è stato un errore
        bool thereWasAnError = false;

        //riporta le statistiche al loro valore originale
        ResetVariables();
        //se non è stato passato un moltiplicatore di statistiche, lo inizializza
        if (statsMultiplier == null)
        {

            statsMultiplier = new float[N_OF_STATS];
            for (int i = 0; i < N_OF_STATS; i++) { statsMultiplier[i] = 1; }

        }
        //se il moltiplicatore di statistiche per qualche motivo ha troppi elementi, lo porta alla grandezza normale e annuncia l'errore
        if (statsMultiplier.Length != N_OF_STATS) { Array.Resize(ref statsMultiplier, N_OF_STATS); thereWasAnError = true; }

        //inizializza le statistiche, moltiplicandole per livello e moltiplicatore di statistiche
        hp *= level * statsMultiplier[0];
        maxHp = hp;
        attack *= level * statsMultiplier[1];
        speed *= level * statsMultiplier[2];

        //infine, comunica se c'è stato un errore o meno
        return thereWasAnError;

    }

    private void ResetVariables()
    {
        hp = 1;
        attack = 1;
        speed = 1;
    }

    #endregion

    #region Getter Methods

    public float GetCurrentHealth() { return hp; }
    public float GetMaxHealth() { return maxHp; }
    public float GetAttack() { return attack; }
    public float GetSpeed() { return speed; }

    #endregion

    #region Setter Methods

    public void SetCurrentHealth(float newValue) { hp = newValue; }

    #endregion

}
