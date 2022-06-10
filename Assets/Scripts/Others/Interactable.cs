using UnityEngine;

/// <summary>
/// Ereditary class for all the interactable objects
/// </summary>
public class Interactable : MonoBehaviour
{
    //reference to the player's interactions manager
    private PlayerInteractionsManager interactionsManager;

    //reference to this interactable's collider(if there is one)
    [SerializeField]
    private Collider2D interactableColl;
    //array of references to all the points to check for interaction(if any)
    [SerializeField]
    private Transform[] pointsToCheckForInteraction;


    protected virtual void Awake()
    {
        //adds itself to the list of interactables to check
        interactionsManager = PermanentRefs.instance.GetPlayerInteractionsManager();
        interactionsManager.AddToOrRemoveFromList(this, true);
        
    }

    protected virtual void OnDestroy()
    {
        //removes itself from the list of interactables to check
        interactionsManager.AddToOrRemoveFromList(this, false);

    }

    public virtual void Interact()
    {

    }

    /// <summary>
    /// Returns the closest point of this interactable object to the desired position
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public virtual Vector2 GetClosestPoint(Vector2 pos)
    {
        //local variable that will indicate the closest point of this object to the position(initialized to the transform position)
        Vector2 closestPoint = transform.position;

        //if this interactable has a collider, gets the closest point of the collider
        if (interactableColl) { closestPoint = interactableColl.bounds.ClosestPoint(pos); }
        //otherwise, if there are points to check in the array...
        else if (pointsToCheckForInteraction.Length > 0)
        {
            //...checks which of the points is the closest
            closestPoint = pointsToCheckForInteraction[0].position;
            float closestPointDist = Vector2.Distance(closestPoint, pos);

            for (int i = 1; i < pointsToCheckForInteraction.Length; i++)
            {
                Transform point = pointsToCheckForInteraction[i];

                float pointDist = Vector2.Distance(point.position, pos);
                if (pointDist > closestPointDist) continue;

                closestPoint = point.position;
                closestPointDist = pointDist;

            }

        }

        //in the end, returns the closest point found
        return closestPoint;

    }

}
