using UnityEngine;

/// <summary>
/// Manages a character's jump
/// </summary>
public class CharacterJump : MonoBehaviour
{

    #region Variables

    //reference to this character's movement script
    [SerializeField]
    private CharacterMovement characterMovement;

    //indicates how much force to add when jumping
    private Vector2 jumpForceToAdd;
    [SerializeField]
    private float jumpForce = 1;

    //indicates how much stronger should the jump start
    [SerializeField]
    private float jumpStartForce = 1;
    //indicates how fast the jump is when not instant
    [SerializeField]
    private float forceAddingRatio = 0.1f;
    //indicates how high can the character jump
    [SerializeField]
    private float maxJumpCapacity = 10;

    //indicates wheter this character's jump should be instantaneous or prolonged in time
    [SerializeField]
    private bool instaJump = false;
    //indicates wheter or not the character is already jumping
    private bool isAlreadyJumping = false;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        //calculates what force to add everytime the character needs to jump
        jumpForceToAdd = new Vector2(0, jumpForce);

        if (!instaJump) jumpForceToAdd.y *= forceAddingRatio;
        
    }

    #endregion

    /// <summary>
    /// Makes the character jump
    /// </summary>
    /// <returns></returns>
    public bool Jump()
    {
        
        Vector2 forceToAdd = jumpForceToAdd;
        if (!isAlreadyJumping) forceToAdd.y *= jumpStartForce;
        characterMovement.MoveByAddingForce(forceToAdd);

        //comunicates that the character is jumping
        isAlreadyJumping = true;

        //returns wheter or not this character can still jump or not
        return CanStillJump();

    }

    #region Getter Methods

    /// <summary>
    /// Returns wheter or not the character can still jump or if he reached the maximum jump capacity
    /// </summary>
    /// <returns></returns>
    private bool CanStillJump()
    {

        bool canStillJump = characterMovement.GetCurrentVelocity().y < maxJumpCapacity;

        return canStillJump;

    }
    /// <summary>
    /// Returns wheter or not the character landed from a jump
    /// </summary>
    /// <returns></returns>
    public bool HasLanded()
    {

        bool landed = characterMovement.IsCharacterGrounded();

        //if landed, comunicates that the character is no longer jumping
        if (landed) isAlreadyJumping = false;

        return landed;

    }

    #endregion

}
