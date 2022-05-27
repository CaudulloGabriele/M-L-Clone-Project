using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObjectBehaviour : MonoBehaviour
{

    [SerializeField]
    private int objectID;

    /*
    [SerializeField]
    private int objectIndex;

    private bool isBeingCreated = true;
    */

    /*
    private void Start()
    {

        gameObject.SetActive(false);
        
    }

    private void OnEnable()
    {
        
        if (isBeingCreated) { return; }

        Debug.LogError("ABOUT TO BE REMOVED");

        ObjectPooling.RemoveObjectFromPool(objectID, objectIndex);
        
    }
    */

    private void OnDisable()
    {

        //if (isBeingCreated) { isBeingCreated = false; return; }

        //Debug.LogError("ABOUT TO BE READDED");

        /*objectIndex = */ObjectPooling.AddObjectToPool(objectID, gameObject);

    }

    public void SetObjectID(int id) { objectID = id; }
    //private void SetObjectIndex(int index) { objectIndex = index; }

}
