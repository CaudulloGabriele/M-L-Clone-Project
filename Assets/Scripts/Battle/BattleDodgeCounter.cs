using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Manages how an entity dodges or counters during a fight
/// </summary>
public class BattleDodgeCounter : MonoBehaviour
{

    #region Variables

    //reference to this entity's jump manager
    [SerializeField]
    private CharacterJump entityJump;

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
    private bool deflectBullets; //the entity will try to counter-attack by deflecting incoming bullets
    [SerializeField]
    private bool spikyBodyCounter; //the entity will counter-attack any physical attack

    //indicates wheter or not this entity is currently dodging or countering
    private bool isDodgeCountering = false;

    #endregion

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

        if (deflectBullets) DeflectBullets();
        if (spikyBodyCounter) SpikyBodyCounter();

    }
    /// <summary>
    /// Ends the current dodge or counter
    /// </summary>
    public void EndDodgeCounter()
    {

        isDodgeCountering = false;

    }

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

    private void DeflectBullets()
    {



    }

    private void SpikyBodyCounter()
    {



    }

    #endregion

}
