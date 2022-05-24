//Si occupa di controllare gli input del giocatore
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    //reference to the script that manages the movement of the player(while exploring)
    private CharacterMovement mapPlayerMovement;
    //reference to the script that manages the battle actions of the player(during battle)
    [SerializeField]
    private BattleActionsManager battleActionsManager;
    //reference to the script that manages the player's actions
    private PlayerActionsManager playerActionsManager;


    private void Start()
    {
        //obtains the references to the player's scripts
        mapPlayerMovement = GetComponent<CharacterMovement>();
        playerActionsManager = GetComponent<PlayerActionsManager>();

    }

    private void LateUpdate()
    {
        //if the game isn't paused, checks the player's inputs
        if (!GameStateManager.IsGamePaused()) { CheckInputs(); }
        //by pressing the pause button, the game will be paused or restored to not paused
        if (Input.GetButtonDown("Pause")) { GameStateManager.SetPauseState(!GameStateManager.IsGamePaused()); }

    }

    /// <summary>
    /// Checks the player's inputs
    /// </summary>
    private void CheckInputs()
    {
        //if the action button is pressed, executes the player action based on the state of the game
        if (Input.GetButtonDown("Action")) { playerActionsManager.ManageActionForCharacter(true, Input.GetAxisRaw("Action") > 0); }
        //if the cancel button is pressed, cancels the player action based on the state of the game
        if (Input.GetButtonDown("Cancel")) { playerActionsManager.ManageActionForCharacter(false, Input.GetAxisRaw("Cancel") > 0); }
        //if the player wants to move, it either moves him or changes the current battle action
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        MovePlayer(movement);

    }
    /// <summary>
    /// Moves the player or changes the current battle action based on the game's state
    /// </summary>
    /// <param name="movement"></param>
    private void MovePlayer(Vector2 movement)
    {
        //if the player is not in a fight, it moves the player in the overworld
        if (!GameStateManager.IsPlayerFighting()) { mapPlayerMovement.Move(movement); }
        //otherwise, it changes the current battle action selection based on the direction(only if the button was pressed this frame)
        else if (Input.GetButtonDown("Horizontal")) { battleActionsManager.ChangeSelection(movement); }

    }

}
