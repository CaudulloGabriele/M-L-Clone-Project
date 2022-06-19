using UnityEngine;

/// <summary>
/// Manages a character's movements
/// </summary>
public class CharacterMovement : MonoBehaviour
{

    #region Variables

    //reference to the Rigidbody2D of the character
    private Rigidbody2D rb;

    //movement speed of the character
    [SerializeField]
    private float speed = 1;
    //start value of the gravity scale
    private float startGravityScale = 1;

    //indicates wheter or not this character moves by changing the velocity immediately or by adding force
    [SerializeField]
    private bool movesWithAddedForces = false;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        //gets the reference to the Rigidbody2D of the character and its starting values
        rb = GetComponent<Rigidbody2D>();

        startGravityScale = rb.gravityScale;

    }

    private void Update()
    {

        if (movesWithAddedForces) return;

        //stops the movement, if the character is moving
        if (rb.velocity != Vector2.zero) rb.velocity = Vector2.zero;

    }

    #endregion

    #region Movement Methods

    /// <summary>
    /// Moves the character based on the received vector multiplied by the character's movement speed
    /// </summary>
    /// <param name="newVelocity"></param>
    public void Move(Vector2 newVelocity)
    {

        rb.velocity = (newVelocity * speed);

        movesWithAddedForces = false;
        rb.gravityScale = 0;

    }
    /// <summary>
    /// Moves the character by adding a force to its Rigidbody2D
    /// </summary>
    /// <param name="forceToAdd"></param>
    public void MoveByAddingForce(Vector2 forceToAdd)
    {

        rb.AddForce(forceToAdd, ForceMode2D.Force);

        movesWithAddedForces = true;
        rb.gravityScale = startGravityScale;

    }

    #endregion

    #region Getter Methdos

    /// <summary>
    /// Returns wheter this character is touching ground or not
    /// </summary>
    /// <returns></returns>
    public bool IsCharacterGrounded() { return rb.velocity == Vector2.zero; }
    /// <summary>
    /// Returns the character's current velocity
    /// </summary>
    /// <returns></returns>
    public Vector2 GetCurrentVelocity() { return rb.velocity; }

    #endregion

}
