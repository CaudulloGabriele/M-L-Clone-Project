using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{

    /// <summary>
    /// Class containing the specifics of an object to pool
    /// </summary>
    [System.Serializable]
    private class PoolingObject
    {
        //indentifier for pooling object
        [SerializeField]
        private string objName;
        //prefab of the object to pool
        public GameObject objToPool;

        /// <summary>
        /// Returns the identifier of the pooling object
        /// </summary>
        /// <returns></returns>
        public string GetPoolingObjectIdentifier() { return objName; }
        /// <summary>
        /// Returns the object to pool
        /// </summary>
        /// <returns></returns>
        public GameObject GetPoolingObject() { return objToPool; }

    }

    #region Variables

    //only instance of the ObjectPooling
    private static ObjectPooling instance;

    //array of references of all the objects to pool
    [SerializeField]
    private PoolingObject[] poolingObjects;
    //hashtable of the object to pool
    private Hashtable hashPoolingObjects = new Hashtable();

    //list containing all already spawned and available objects in the pool(for each type of PoolingObject)
    private List<List<GameObject>> availableObjectsInPool = new List<List<GameObject>>();

    [SerializeField]
    List<GameObject> shroobBullets;
    //list containing all containers of available objects in pool
    private Transform[] containersOfAvailableObjectsInPool;
    //array that indicates the number of currently available objects in the pool(for each type of PoolingObject)
    [SerializeField]
    private int[] numberOfAvailableObjectsInPool;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        //if an instance of ObjectPooling already exists, this instance is destroyed
        if (instance) { Destroy(gameObject); return; }

        //otherwise, this becomes the new instance...
        instance = this;
        //...and initializes itself
        InitializeObjectPooling();


        //Debug.Log("HASHTABLE GIVES: " + hashPoolingObjects["ShroobBullet"]);
        //Debug.Log("WITH HASH GET OBJECT: " + GetObjectByName("ShroobBullet"));
    }

    private void FixedUpdate()
    {

        shroobBullets = availableObjectsInPool[0];
        
    }

    private async void OnDestroy()
    {
        //if this was the sole existing instance, sets the instance as null
        await Task.Delay(1);
        if (instance == this) instance = null;

    }

    #endregion

    #region Instance Methods

    /// <summary>
    /// Initializes the object pooling
    /// </summary>
    private void InitializeObjectPooling()
    {
        //obtains the number of objects to pool
        int numberOfObjToPool = poolingObjects.Length;

        //initializes all the arrays
        numberOfAvailableObjectsInPool = new int[numberOfObjToPool];
        containersOfAvailableObjectsInPool = new Transform[numberOfObjToPool];

        //cycles every object to pool
        for (int i = 0; i < numberOfObjToPool; i++)
        {
            PoolingObject poolObj = poolingObjects[i];

            /*
             * creates an hashtable for every object to pool
             * the hashtable will have as key the object identifier and as value the index of the current cycle
             */
            hashPoolingObjects.Add(poolObj.GetPoolingObjectIdentifier(), i);

            //creates a container for every object to pool
            Transform container = new GameObject("Container Of " + poolObj.GetPoolingObjectIdentifier()).transform;
            container.parent = transform;
            containersOfAvailableObjectsInPool[i] = container;
            //container.gameObject.SetActive(false);

            //spawns at least one object to pool each
            availableObjectsInPool.Add(new List<GameObject>());
            //SpawnFromPool(i);

        }


        //Debug.LogError("COUNT: " + availableObjectsInPool.Count + " | " + hashPoolingObjects.Count + " | " + numberOfAvailableObjectsInPool.Length);
    }

    /// <summary>
    /// Returns the desired object from the pool
    /// </summary>
    /// <param name="objToSpawn"></param>
    /// <returns></returns>
    public static GameObject GetObjectFromPool(string objToSpawn) { return instance.GetObjectByName(objToSpawn); }

    /// <summary>
    /// Allows to add an object to the pool
    /// </summary>
    /// <param name="id"></param>
    /// <param name="obj"></param>
    public static int AddObjectToPool(int id, GameObject obj)
    {

        int n = instance.AddToPool(id, obj);

        return n;

    }
    /// <summary>
    /// Allows to remove an object from the pool
    /// </summary>
    /// <param name="id"></param>
    /// <param name="index"></param>
    public static void RemoveObjectFromPool(int id, int index) { instance.RemoveFromPool(id, index); }

    #endregion

    #region Pooling Methods

    /// <summary>
    /// Returns a pooling object by name from the pool
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private GameObject GetObjectByName(string name)
    {
        //obtains the index for the object to pool
        int index = (int)hashPoolingObjects[name];
        //obtains the desired object from the pool
        GameObject obj = SpawnFromPool(index);

        //returns the desired object from the pool
        return obj;

    }
    /// <summary>
    /// Returns an object of the desired type from the pool(wheter by instantiating it or getting an available one)
    /// </summary>
    /// <param name="objectID"></param>
    /// <returns></returns>
    private GameObject SpawnFromPool(int objectID)
    {
        //local variable that will contain the desired object
        GameObject poolObj;

        //if there is no available object of the desired type...
        if (!IsObjectAvailable(objectID, 0))
        {
            //...instantiates a new object of the desired type...
            GameObject newObj = Instantiate(poolingObjects[objectID].GetPoolingObject(), containersOfAvailableObjectsInPool[objectID]);
            //...and gives it the behaviour of a poolable object...
            PooledObjectBehaviour p = newObj.AddComponent<PooledObjectBehaviour>();
            p.SetObjectID(objectID);

            //...and adds it to the pool
            AddToPool(objectID, newObj);

        }

        //...gets the reference of the first available object...
        poolObj = availableObjectsInPool[objectID][0];
        //...and removes it from the pool
        RemoveFromPool(objectID, 0);

        //Debug.LogWarning("OBJECT RETURNED: " + poolObj);

        //return the desired object
        return poolObj;

    }

    /// <summary>
    /// Adds an object to the pool
    /// </summary>
    /// <param name="objectID"></param>
    /// <param name="objectToAdd"></param>
    private int AddToPool(int objectID, GameObject objectToAdd)
    {
        //Debug.LogWarning("OBJECT TO ADD: " + objectToAdd + " AT INDEX: " + objectID);
        //Debug.LogWarning("OBJECTS AVAILABLE COUNT: " + availableObjectsInPool.Count);


        numberOfAvailableObjectsInPool[objectID]++;

        availableObjectsInPool[objectID].Add(objectToAdd);

        //objectToAdd.transform.parent = containersOfAvailableObjectsInPool[objectID];


        return numberOfAvailableObjectsInPool[objectID];

    }
    /// <summary>
    /// Removes an object from the pool
    /// </summary>
    /// <param name="objectID"></param>
    /// <param name="objectIndex"></param>
    private void RemoveFromPool(int objectID, int objectIndex)
    {
        //Debug.LogWarning("INDEX OF OBJECT TO REMOVE: " + objectID);
        //Debug.LogWarning("OBJECTS AVAILABLE COUNT: " + availableObjectsInPool.Count);

        if (!IsObjectAvailable(objectID, objectIndex)) return;

        numberOfAvailableObjectsInPool[objectID]--;

        availableObjectsInPool[objectID].RemoveAt(objectIndex);

    }

    /// <summary>
    /// Returns whetere the desired object is available or not
    /// </summary>
    /// <param name="objectID"></param>
    /// <param name="objectIndex"></param>
    /// <returns></returns>
    private bool IsObjectAvailable(int objectID, int objectIndex)
    {

        if (numberOfAvailableObjectsInPool[objectID] > 0)
        {

            if (availableObjectsInPool[objectID][objectIndex] != null) return true;

        }

        return false;
    
    }

    #endregion

}
