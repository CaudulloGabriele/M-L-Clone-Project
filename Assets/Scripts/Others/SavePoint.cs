using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SavePoint : Interactable
{

    private Transform mapPlayer;


    protected override void Awake()
    {
        base.Awake();

        mapPlayer = PermanentRefs.instance.GetPlayer();

    }


    public override void Interact()
    {
        base.Interact();

        Debug.Log("INTERACTED WITH SAVE POINT");
    }

}
