using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloAction : MonoBehaviour
{

    [SerializeField]
    private Transform performer;

    private Vector2 performingPos;

    //indicates if there has to be an anticipation before attacking
    [SerializeField]
    private bool hasToAnticipate = false;


    public void PerformSoloAction()
    {

        performer.position = performingPos;

    }

    public void SetPerformingPos(Vector2 newPos) { performingPos = newPos; }

}
