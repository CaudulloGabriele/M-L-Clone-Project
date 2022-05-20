using UnityEngine;

/// <summary>
/// Si occupa del movimento della telecamera
/// </summary>
public class CameraFollow : MonoBehaviour
{

	#region Variables

	//reference to the target the camera has to follow
	[SerializeField]
	private Transform followTarget;
	//camera's movement speed
	[SerializeField]
	private float camSpeed;
	private float startCamSpeed;
	//indicates the target's position continuosly
	private Vector3 targetPos;

    #endregion


    private void Awake()
    {
		//sets the camera initial speed
		startCamSpeed = camSpeed;

    }

    void FixedUpdate()
	{
		//if there is a terget to follow...
		if (followTarget != null)
		{
			//...the camera goes towards it, without changing the Z axis position
			FollowTarget();

		}

	}

	/// <summary>
	/// The camera goes towards the target, without changing the Z axis position
	/// </summary>
	private void FollowTarget()
    {
		targetPos = new Vector3(followTarget.position.x, followTarget.position.y, transform.position.z);
		Vector3 velocity = (targetPos - transform.position) * camSpeed;
		transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 1.0f, Time.deltaTime);

	}
	/// <summary>
	/// Allows to change the camera's target
	/// </summary>
	/// <param name="newTarget"></param>
	public void ChangeTarget(Transform newTarget) { followTarget = newTarget; }
	/// <summary>
	/// Allows to change the camera's movement speed
	/// </summary>
	/// <param name="newSpeed"></param>
	public void ChangeCameraSpeed(float newSpeed) { camSpeed = newSpeed; }
	/// <summary>
	/// Allows to return the camera's movement speed to the initial value
	/// </summary>
	public void ResetCameraSpeed() { camSpeed = startCamSpeed; }

}
