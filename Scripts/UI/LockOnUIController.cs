using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnUIController : MonoBehaviour
{
	[SerializeField] Transform followTarget;

    // Update is called once per frame
    void Update()
    {
		transform.position = followTarget.position;
    }

	public void SetFollowTarget(Transform _followTarget)
	{
		followTarget = _followTarget;
	}
}
