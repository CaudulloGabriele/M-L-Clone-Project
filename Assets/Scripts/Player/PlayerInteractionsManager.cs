using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerInteractionsManager : MonoBehaviour
{

    #region Variables

    //list of all the interactable objects currently in the scene
    private List<Interactable> allCurrentInteractables = new List<Interactable>();
    //reference to the currently closest interactable
    //[SerializeField]
    private Interactable closestInteractable;

    //indicates how much time has to pass between checks
    [SerializeField]
    private float delayInCheck = 1f;
    private int delayInCheckMillisecond;
    //indicates how close an interactable has to be to be interacted with
    [SerializeField]
    private float checkRadius = 1f;

    private bool canCheck = true, //indicates wheter or not the check for interactables has to start in this frame
                 canInteract = true; //indicates wheter or not the player can currently interact with objects

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        //calculates the delay between checks in milliseconds
        delayInCheckMillisecond = (int)(delayInCheck * 1000);
        
    }

    private async void FixedUpdate()
    {
        //if the check doesn't have to start in this frame, it does nothing
        if (!canCheck) return;

        //otherwise...

        //...checks for close interactables in this frame...
        CheckForCloseInteractables();
        canCheck = false;

        //...waits for the end of the delay...
        await Task.Delay(delayInCheckMillisecond);

        //...comunicates that the next frame a new check has to start
        canCheck = true;

    }

    private void OnDestroy()
    {
        //when the player is being destroyed(when the game is closed), it can't interact with anything(avoiding errors)
        SetIfCanInteract(false);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, checkRadius);

        foreach (Interactable interactable in allCurrentInteractables)
        {

            float dist = Vector2.Distance(transform.position, interactable.GetClosestPoint(transform.position));
            Gizmos.color = (dist > checkRadius) ? Color.cyan : Color.yellow;

            Gizmos.DrawLine(transform.position, interactable.GetClosestPoint(transform.position));
        }

    }

    #endregion

    #region Interactables Management Methods

    /// <summary>
    /// Allows to add to or remove an interactable from the list of interactables
    /// </summary>
    /// <param name="interactable"></param>
    /// <param name="add"></param>
    public void AddToOrRemoveFromList(Interactable interactable, bool add)
    {

        if (add) allCurrentInteractables.Add(interactable);

        else allCurrentInteractables.Remove(interactable);


        //Debug.LogError((add ? "ADDED" : "REMOVED") + " FROM LIST AN INTERACTABLE -> " + allCurrentInteractables.Count);
    }
    /// <summary>
    /// Checks if any interactable is close enough and sets the closest as interactable
    /// </summary>
    private void CheckForCloseInteractables()
    {
        //if the player can't interact, it doesn't check the interactables
        if (!canInteract) return;

        //obtains the interactables that are close enough to be interacted with
        List<Interactable> closeInteractables = new List<Interactable>();
        List<float> closeInteractablesDist = new List<float>();
        foreach (Interactable interactable in allCurrentInteractables)
        {
            //if close enough, it gets added to the list of close interactables
            float dist = Vector2.Distance(transform.position, interactable.GetClosestPoint(transform.position));
            if (dist < checkRadius)
            {
                closeInteractables.Add(interactable);
                closeInteractablesDist.Add(dist);

            }

        }

        //if there is no close enough interactable, the check ends here
        int n_CloseInteractables = closeInteractables.Count;
        if (n_CloseInteractables == 0)
        {
            closestInteractable = null;
            return;

        }

        //otherwise...

        //confronts which of the interactables is closest
        Interactable closestInteractableChecked = closeInteractables[0];
        float closestDist = closeInteractablesDist[0];
        for (int i = 1; i < n_CloseInteractables; i++)
        {
            //if the cycled interactable is closer then the currently closest interactable...
            float otherInteractableDist = closeInteractablesDist[i];
            if (otherInteractableDist < closestDist)
            {
                //...sets the cycled interactable as the closest one
                closestInteractableChecked = closeInteractables[i];
                closestDist = otherInteractableDist;

            }

        }

        //at the end of the check, sets the closest interactable
        closestInteractable = closestInteractableChecked;

    }
    /// <summary>
    /// Allows to interact with the closest interactable object(if any)
    /// </summary>
    public void InteractWithCloseObject()
    {

        if (closestInteractable == null) return;

        closestInteractable.Interact();

    }

    #endregion

    /// <summary>
    /// Allows to set wheter the player can interact or not
    /// </summary>
    /// <param name="can"></param>
    public void SetIfCanInteract(bool can) { canInteract = can; }

}
