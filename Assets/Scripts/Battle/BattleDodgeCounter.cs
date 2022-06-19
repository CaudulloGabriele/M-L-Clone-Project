using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Manages how an entity dodges or counters during a fight
/// </summary>
public class BattleDodgeCounter : MonoBehaviour
{

    private const int NUMBER_OF_DODGES = 2,
                      NUMBER_OF_COUNTERS = 2;

    #region Variables

    //reference to this entity's jump manager
    [SerializeField]
    private CharacterJump entityJump;
    //reference to this entity's bullets deflector
    [SerializeField]
    private BulletsDeflect bulletsDeflector;


    //indicates wheter this entity dodges or counters automatically
    [SerializeField]
    private bool isAutomatic = false;

    //these variables indicate which dodges and counters the entity will do when about to be attacked

    [Header("DODGES")]

    [SerializeField]
    private bool jumpDodge; //the entity will try to dodge by jumping
    [SerializeField]
    private bool vanishDodge; //the entity will try to dodge by vanishing for a bit

    [Header("COUNTER-ATTACKS")]

    [SerializeField]
    private bool deflectBulletsCounter; //the entity will try to counter-attack by deflecting incoming bullets
    [SerializeField]
    private bool spikyBodyCounter; //the entity will counter-attack any physical attack


    //indicates wheter or not this entity is currently dodging or countering
    private bool isDodgeCountering = false;

    #endregion

    #region Dodge AND/OR Counter Management

    /// <summary>
    /// Starts dodging or countering
    /// </summary>
    public void StartDodgeCounter()
    {
        //if this entity is already dodging or countering, it doesn't do anything
        if (isDodgeCountering) return;

        //otherwise, starts dodging or countering
        isDodgeCountering = true;

        //DODGES

        if (jumpDodge) JumpDodge();
        if (vanishDodge) VanishDodge();


        //COUNTERS

        if (deflectBulletsCounter) DeflectBullets();
        if (spikyBodyCounter) SpikyBodyCounter();

    }
    /// <summary>
    /// Ends the current dodge or counter
    /// </summary>
    public void EndDodgeCounter()
    {

        isDodgeCountering = false;

    }

    #endregion

    #region Dodges

    /// <summary>
    /// Executes a dodge by jumping
    /// </summary>
    private async void JumpDodge()
    {
        //if this entity dodges automatically...
        if (isAutomatic)
        {

            await Task.Delay(1);

        }
        else //otherwise...
        {
            //Debug.LogError("JUMP DODGE START");

            //...the entity keeps jumping until he can't or until the action button is no longer pressed...
            bool canStillJump = true;
            while (Input.GetButton("Action") && canStillJump)
            {

                canStillJump = entityJump.Jump();

                await Task.Delay(1);


                //Debug.LogError("JUMP DODGING");
            }
            //...then, once the entity has landed from the jump...
            while (!entityJump.HasLanded())
            {

                await Task.Delay(1);


                //Debug.LogError("JUMP LANDING");
            }
            //...the dodge ends
            EndDodgeCounter();


            //Debug.LogError("JUMP DODGE END");
        }

    }

    private void VanishDodge()
    {



    }

    #endregion

    #region Counters

    /// <summary>
    /// Executes a counter that allows to deflect bullets
    /// </summary>
    private async void DeflectBullets()
    {

        //if this entity counters automatically...
        if (isAutomatic)
        {

            await Task.Delay(1);

        }
        else //otherwise...
        {
            //Debug.LogError("JUMP DODGE START");

            //...starts the deflection counter...
            bulletsDeflector.Deflect();

            //...gets the duration and cooldown of the counter...
            float deflectDuration = bulletsDeflector.GetDeflectDuration();
            float deflectCD = bulletsDeflector.GetDeflectCooldown();
            //...waits for the deflect counter to end...
            await Task.Delay((int)(deflectDuration * 1000));
            await Task.Delay((int)(deflectCD * 1000));

            //...the counter ends
            EndDodgeCounter();


            //Debug.LogError("JUMP DODGE END");
        }

    }

    private void SpikyBodyCounter()
    {



    }

    #endregion

    #region Setter Methods

    /// <summary>
    /// Allows to set the only dodge/counter the entity can do
    /// 
    ///     <para>
    ///         DODGES INDEXES: 0 - jump dodge | 1 - vanish dodge
    ///     </para>
    ///     <para>
    ///         COUNTERS INDEXES: 0 - deflect bullets counter | 1 - spiky body counter
    ///     </para>
    ///     
    /// </summary>
    /// <param name="dodge"></param>
    /// <param name="dodgeCounterType"></param>
    public void SetSingleDodgeCounter(bool dodge, int dodgeCounterType)
    {
        //removes all the active dodges and counters
        RemoveAllDodgeCounters();

        //activates the desired dodge or counter
        AddOrRemoveDodgeCounter(dodge, dodgeCounterType, true);

    }
    /// <summary>
    /// Allows to add or remove a dodge or counter that the entity can do
    /// 
    ///    <para>
    ///         DODGES INDEXES: 0 - jump dodge | 1 - vanish dodge
    ///     </para>
    ///     <para>
    ///         COUNTERS INDEXES: 0 - deflect bullets counter | 1 - spiky body counter
    ///     </para>
    ///     
    /// </summary>
    /// <param name="dodge"></param>
    /// <param name="dodgeCounterType"></param>
    /// <param name="toAdd"></param>
    public void AddOrRemoveDodgeCounter(bool dodge, int dodgeCounterType, bool toAdd)
    {
        //if it wants to add or remove a dodge...
        if (dodge)
        {
            //...adds or removes a dodge, based on the received type
            switch (dodgeCounterType)
            {

                case 0: { jumpDodge = toAdd; break; }

                case 1: { vanishDodge = toAdd; break; }

                default: { Debug.LogError("TRIED TO " + (toAdd ? "ADD" : "REMOVE") + " INCORRECT TYPE OF DODGE: " + dodgeCounterType); break; }

            }

        }
        //otherwise, it wants to add or remove a counter, so...
        else
        {
            //...adds or removes a counter, based on the received type
            switch (dodgeCounterType)
            {

                case 0: { deflectBulletsCounter = toAdd; break; }

                case 1: { spikyBodyCounter = toAdd; break; }

                default: { Debug.LogError("TRIED TO " + (toAdd ? "ADD" : "REMOVE") + " INCORRECT TYPE OF COUNTER: " + dodgeCounterType); break; }

            }

        }

    }
    /// <summary>
    /// Makes the entity unable to dodge or counter anything
    /// </summary>
    public void RemoveAllDodgeCounters()
    {

        for (int dodge = 0; dodge < NUMBER_OF_DODGES; dodge++) { AddOrRemoveDodgeCounter(true, dodge, false); }
        for (int counter = 0; counter < NUMBER_OF_COUNTERS; counter++) { AddOrRemoveDodgeCounter(false, counter, false); }

    }

    #endregion

}
