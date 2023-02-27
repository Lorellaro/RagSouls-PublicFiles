using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpecificMoves : MonoBehaviour
{
	[SerializeField] Rigidbody ragdollRootRB;

	float forwardForce;
	float upliftForce;
	float slamDownForce;

	bool startForwardForce;
	bool startUpliftForce;
	bool startSlamDownForce;

	Transform mainCamTrans;

	private void Awake()
	{
		mainCamTrans = Camera.main.transform;
	}

	//Forward force
	public void ForwardForce(float _forwardForce)
	{
		forwardForce = _forwardForce;
		StartCoroutine(EnableForwardForce());
	}

	private IEnumerator EnableForwardForce()
	{
		startForwardForce = true;
		yield return new WaitForSeconds(0.15f);
		startForwardForce = false;
	}

	//Downforce
	public void DownForce(float _downForce)
	{
		slamDownForce = _downForce;
		StartCoroutine(EnableDownForce());
	}

	private IEnumerator EnableDownForce()
	{
		startSlamDownForce = true;
		yield return new WaitForSeconds(0.15f);
		startSlamDownForce = false;
	}

	//Uplift
	public void UpLift(float _upliftForce)
	{
		upliftForce = _upliftForce;
		StartCoroutine(EnableUpliftForce());
	}

	private IEnumerator EnableUpliftForce()
	{
		startUpliftForce = true;
		yield return new WaitForSeconds(0.15f);
		startUpliftForce = false;
	}

	private void FixedUpdate()
	{
		if (startUpliftForce)
		{
			ragdollRootRB.velocity = Vector3.up * upliftForce * Time.deltaTime;
		}

		if (startSlamDownForce)
		{
			ragdollRootRB.velocity = Vector3.down * slamDownForce * Time.deltaTime;
		}

		if (startForwardForce)
		{
			Vector3 forwardDir = new Vector3(mainCamTrans.forward.x, 0f, mainCamTrans.forward.z).normalized;
			ragdollRootRB.velocity = forwardDir * forwardForce * Time.deltaTime;
		}
	}
}
