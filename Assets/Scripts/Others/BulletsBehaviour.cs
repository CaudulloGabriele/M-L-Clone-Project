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

        startLifeTime = lifeTime;

    }

    public async void ShootBulletToTarget(Vector2 targetPos)
    {

        gameObject.SetActive(true);

        //transform.LookAt(targetPos);
        transform.right = targetPos - (Vector2)transform.position;
        bulletRb.velocity = transform.right * speed;

        while (!HasBulletExpired())
        {

            await Task.Delay(/*System.TimeSpan.FromSeconds(Time.deltaTime)*/1);

            lifeTime -= Time.deltaTime;

        }
        //while (Vector2.Distance(target.position, transform.position) <= closeEnoughDist) await Task.Delay(1);

        Expire();

    }

    private bool HasBulletExpired() { return lifeTime <= 0; }

    private void Expire()
    {

        lifeTime = startLifeTime;

        gameObject.SetActive(false);

    }

}
