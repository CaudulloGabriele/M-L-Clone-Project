using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BulletsBehaviour : MonoBehaviour
{
    //reference to this bullet collisions manager
    [SerializeField]
    private BattleCollisionsManager collisionsManager;
    //reference to the Rigidbody2D of this bullet
    private Rigidbody2D bulletRb;

    //indicates how fast this bullet moves
    [SerializeField]
    private float speed;
    //indicates how much damage the bullet does
    [SerializeField]
    private float damage;


    private void Awake()
    {
        //obtains the reference to the Rigidbody2D of this bullet
        bulletRb = GetComponent<Rigidbody2D>();

        //updates the collisions manager damage
        SetBulletDamage(damage);

    }

    /// <summary>
    /// Shoots bullet towards a specified target position
    /// </summary>
    /// <param name="targetPos"></param>
    public async void ShootBulletToTarget(Vector2 targetPos, Vector2 startPos)
    {
        //activates the bullet
        gameObject.SetActive(true);

        //positions the bullet at the start position received
        transform.position = startPos;

        //shoots the bullet towards the target position
        transform.right = targetPos - (Vector2)transform.position;
        bulletRb.velocity = transform.right * speed;

        float lifeTime = Vector2.Distance(startPos, targetPos) / speed;

        //while the bullet has yet to expire...
        while (lifeTime > 0)
        {
            //...waits a millisecond...
            await Task.Delay(1);
            //...and slowly expires the bullet
            lifeTime -= Time.deltaTime;

        }

        //the bullet expires
        Expire();

    }

    /// <summary>
    /// The bullet expires
    /// </summary>
    private void Expire()
    {

        gameObject.SetActive(false);

    }
    /// <summary>
    /// Allows to set this bullet's damage
    /// </summary>
    /// <param name="newDmg"></param>
    public void SetBulletDamage(float newDmg)
    {
        
        damage = newDmg;

        collisionsManager.SetDamage(damage);
    
    }

}
