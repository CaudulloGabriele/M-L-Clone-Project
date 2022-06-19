using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Manages how the deflect counter works
/// </summary>
public class BulletsDeflect : MonoBehaviour
{

    #region Variables

    //reference to the collider that deflects any counterable attack it touches
    [SerializeField]
    private Collider2D deflectArea;

    //indicates after how much time, after a use, can the counter be used
    [SerializeField]
    private float deflectCooldown = 0.2f;
    //indicates for how much time the deflection lasts, when the counter is used
    [SerializeField]
    private float deflectDuration = 0.5f;
    private int deflectDurationMilliseconds;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        //sets that the deflection counter is not in effect
        SetDeflectingState(false);

        //calculates the duration of the deflection duration in milliseconds
        deflectDurationMilliseconds = (int)(deflectDuration * 1000);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if any counterable attack enters the trigger, during the deflection, the attack gets countered
        ICounterable counterableAtk = collision.GetComponentInParent<ICounterable>();
        if (counterableAtk != null) counterableAtk.CounterThis();

    }

    #endregion

    #region Deflect Methods

    /// <summary>
    /// Deflects any counterable attack for the entire duration of the counter(after which, the deflection will)
    /// </summary>
    public async void Deflect()
    {

        SetDeflectingState(true);

        await Task.Delay(deflectDurationMilliseconds);

        SetDeflectingState(false);

    }
    /// <summary>
    /// Sets wheter any counterable attack should be deflected or not
    /// </summary>
    /// <param name="state"></param>
    private void SetDeflectingState(bool state)
    {

        deflectArea.enabled = state;

    }

    #endregion

    #region Getter Methods

    /// <summary>
    /// Returns the cooldown duration of the deflection counter
    /// </summary>
    /// <returns></returns>
    public float GetDeflectCooldown() { return deflectCooldown; }
    /// <summary>
    /// Returns the duration of the deflection counter
    /// </summary>
    /// <returns></returns>
    public float GetDeflectDuration() { return deflectDuration; }

    #endregion

}
