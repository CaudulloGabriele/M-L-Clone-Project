using UnityEngine;

/// <summary>
/// Manages a character's movements
/// </summary>
public class CharacterMovement : MonoBehaviour
{
    //reference to the Rigidbody2D of the character
    private Rigidbody2D rb;
    //movement speed of the character
    [SerializeField]
    private float speed = 1;


    private void Awake()
    {
        //gets the reference to the Rigidbody2D of the character
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        //stops the movement, if the character is moving
        if (rb.velocity != Vector2.zero) rb.velocity = Vector2.zero;

    }

    /// <summary>
    /// Moves the character
    /// </summary>
    /// <param name="newVelocity"></param>
    public void Move(Vector2 newVelocity)
    {
        //moves the character based on the received vector multiplied by the character's movement speed
        rb.velocity = (newVelocity * speed);

    }

}
