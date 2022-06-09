using UnityEngine;

/// <summary>
/// Makes the object rotate in the desired axises
/// </summary>
public class RotationBehaviour : MonoBehaviour
{
    //indicates the speed of rotation to apply on each axis
    [SerializeField]
    private Vector3 rotationSpeedOnAxis;


    private void FixedUpdate()
    {
        //continues to rotate
        transform.Rotate(rotationSpeedOnAxis);

    }

}
