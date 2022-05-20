//Si occupa di controllare gli input del giocatore
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    //riferimento allo script che si occupa del movimento del giocatore(nella mappa)
    private CharacterMovement mapPlayerMovement;
    //riferimento allo script che si occupa del movimento del giocatore(durante una battaglia)
    [SerializeField]
    private CharacterMovement battlePlayerMovement;
    //riferimento allo script che si occupa delle azioni del giocatore
    private PlayerActionsManager playerActionsManager;


    private void Start()
    {
        //ottiene i riferimenti agli script del giocatore
        mapPlayerMovement = GetComponent<CharacterMovement>();
        playerActionsManager = GetComponent<PlayerActionsManager>();

    }

    private void LateUpdate()
    {
        //se il gioco non è in pausa, controlla gli input del giocatore
        if (!GameStateManager.IsGamePaused()) { CheckInputs(); }
        //premendo il tasto di pausa, il gioco viene messo o tolto dalla pausa in base allo stato di pausa attuale
        if (Input.GetButtonDown("Pause")) { GameStateManager.SetPauseState(!GameStateManager.IsGamePaused()); }

    }

    /// <summary>
    /// Controlla gli input del giocatore
    /// </summary>
    private void CheckInputs()
    {
        //se preme il bottone d'azione, esegue l'azione che può essere eseguita in quel momento
        if (Input.GetButtonDown("Action")) { playerActionsManager.ManageActionForCharacter(); }
        //se il giocatore vuole muoversi, lo muove
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        MovePlayer(movement);

    }
    /// <summary>
    /// Muove il giocatore che si sta controllando in base allo stato del gioco
    /// </summary>
    /// <param name="movement"></param>
    private void MovePlayer(Vector2 movement)
    {
        //se il giocatore non è in combattimento, si muove il suo personaggio nella mappa
        if (!GameStateManager.IsPlayerFighting()) { mapPlayerMovement.Move(movement); }
        //altrimenti, verrà mosso il suo personaggio nel campo di battaglia
        else { battlePlayerMovement.Move(movement); }

    }

}
