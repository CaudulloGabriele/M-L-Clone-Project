using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BulletsBehaviour : MonoBehaviour
{
    //reference to the Rigidbody2D of this bullet
    private Rigidbody2D bulletRb;

    //indicates how fast this bullet moves
    [SerializeField]
    private float speed;
    //indicates when the bullet expires after being shot
    [SerializeField]
    private float lifeTime = 1;
    private float startLifeTime;


    private void Awake()
    {
        //obtains the reference to the Rigidbody2D of this bullet
        bulletRb = GetComponent<Rigidbody2D>();

        //obtains the start value of the bullet's lifetime
        startLifeTime = lifeTime;

    }

    /// <summary>
    /// Shoots bullet towards a specified target position
    /// </summary>
    /// <param name="targetPos"></param>
    public async void ShootBulletToTarget(Vector2 targetPos)
    {
        //activates the bullet
        gameObject.SetActive(true);

        //shoots the bullet towards the target position
        transform.right = targetPos - (Vector2)transform.position;
        bulletRb.velocity = transform.right * speed;

        //while the bullet has yet to expire...
        while (!HasBulletExpired())
        {
            //...waits a millisecond...
            await Task.Delay(1);
            //...and slowly expires the bullet
            lifeTime -= Time.deltaTime;

        }
        //while (Vector2.Distance(target.position, transform.position) <= closeEnoughDist) await Task.Delay(1);

        //the bullet expires
        Expire();

    }

    /// <summary>
    /// Returns if the bullet has expired or not
    /// </summary>
    /// <returns></returns>
    private bool HasBulletExpired() { return lifeTime <= 0; }

    /// <summary>
    /// The bullet expires
    /// </summary>
    private void Expire()
    {

        gameObject.SetActive(false);

        lifeTime = startLifeTime;

    }

}
