//Si occupa del movimento del giocatore
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //riferimento al Rigidbody2D del giocatore
    private Rigidbody2D rb;
    //indica la velocit� di movimento del giocatore
    [SerializeField]
    private float speed = 1;


    private void Awake()
    {
        //ottiene il riferimento al Rigidbody2D dell'entit�
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {

        if (rb.velocity != Vector2.zero) rb.velocity = Vector2.zero;

    }

    /// <summary>
    /// Muove il giocatore
    /// </summary>
    /// <param name="newVelocity"></param>
    public void Move(Vector2 newVelocity)
    {
        //muove il giocatore, aggiungendo forza al Rigidbody del giocatore in base alla direzione ricevuta per la velocit�
        rb.velocity = (newVelocity * speed);

    }

}
