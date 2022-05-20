using UnityEngine;

/// <summary>
/// Si occupa del movimento della telecamera
/// </summary>
public class CameraFollow : MonoBehaviour
{

	#region Variables

	//riferimento all'obiettivo che la telecamera deve seguire
	[SerializeField]
	private Transform followTarget;
	//indica quanto velocemente si muove la telecamera
	[SerializeField]
	private float camSpeed;
	private float startCamSpeed;
	//indica continuamente la posizione dell'obiettivo
	private Vector3 targetPos;

    #endregion


    private void Awake()
    {

		startCamSpeed = camSpeed;

    }

    void FixedUpdate()
	{
		//se si ha un obiettivo da seguire...
		if (followTarget != null)
		{
			//...si muove verso di lui a poco, senza però cambiare la posizione nell'asse Z
			FollowTarget();

		}

	}

	/// <summary>
	/// Si muove verso di lui a poco, senza però cambiare la posizione nell'asse Z
	/// </summary>
	private void FollowTarget()
    {
		targetPos = new Vector3(followTarget.position.x, followTarget.position.y, transform.position.z);
		Vector3 velocity = (targetPos - transform.position) * camSpeed;
		transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 1.0f, Time.deltaTime);

	}

	public void ChangeTarget(Transform newTarget) { followTarget = newTarget; }

	public void ChangeCameraSpeed(float newSpeed) { camSpeed = newSpeed; }

	public void ResetCameraSpeed() { camSpeed = startCamSpeed; }

}
