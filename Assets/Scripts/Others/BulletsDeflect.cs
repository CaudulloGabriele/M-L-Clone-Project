using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BulletsDeflect : MonoBehaviour
{

    [SerializeField]
    private Collider2D deflectArea;


    [SerializeField]
    private float deflectCooldown = 0.2f;

    [SerializeField]
    private float deflectDuration = 0.5f;
    private int deflectDurationMilliseconds;

    /*
    [SerializeField]
    private float timeBetweenChecks = 0.25f;
    private int timeBetweenChecksMilliseconds;


    private bool deflecting = false;
    */

    private void Awake()
    {

        SetDeflectingState(false);

        deflectDurationMilliseconds = (int)(deflectDuration * 1000);
        //timeBetweenChecksMilliseconds = (int)(timeBetweenChecks * 1000);

    }

    /*
    private void OnDrawGizmos()
    {
        


    }
    */

    
    private void OnTriggerEnter2D(Collider2D collision)
    {

        BulletsBehaviour bullet = collision.GetComponentInParent<BulletsBehaviour>();
        if (bullet) bullet.DeflectThisBullet();

    }
    

    public async void Deflect()
    {

        /*DEFLECTS ANY BULLETS WHILE THE DEFLECT IS ACTIVE AND WAITS FOR THE COOLDOWN*/

        SetDeflectingState(true);

        //DeflectAnyIncomingBullets();

        await Task.Delay(deflectDurationMilliseconds);

        SetDeflectingState(false);

    }

    /*
    private async void DeflectAnyIncomingBullets()
    {

        while (deflecting)
        {

            Collider2D[]
            Physics2D.BoxCastNonAlloc(transform.position, , , );

            await Task.Delay(timeBetweenChecksMilliseconds);

        }

    }
    */
    private void SetDeflectingState(bool state)
    {

        //deflecting = state;

        deflectArea.enabled = state;

    }

    public float GetDeflectCooldown() { return deflectCooldown; }

    public float GetDeflectDuration() { return deflectDuration; }

}
