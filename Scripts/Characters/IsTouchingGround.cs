using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IsTouchingGround : MonoBehaviour
{
	private float timeSinceLeftFloor = 0f;
	private float fireDistance = 0.0000001f;
	private bool isTouchingGround = false;

	PhotonView view;

	// Start is called before the first frame update
	void Start()
	{
		view = transform.root.GetComponent<PhotonView>();
	}

	private void Update()
	{
		if (view.IsMine)
		{
			if (!isTouchingGround)
			{
				timeSinceLeftFloor += Time.deltaTime;
			}

			//raycast down
			//set timesinceleftfloor 0
			//set istouchingground = true

			RaycastHit hit;
			if (Physics.Raycast(transform.position, -Vector3.up, out hit, fireDistance, LayerMask.NameToLayer("Ground")))
			{
				if (Vector3.Distance(transform.position, hit.point) < fireDistance)
				{
					//timeSinceLeftFloor = 0;
					//isTouchingGround = true;
				}
			}
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (!view.IsMine) { return; }

		if (collision.gameObject.CompareTag("Ground"))
		{
			isTouchingGround = true;
			timeSinceLeftFloor = 0;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (!view.IsMine) { return; }

		if (collision.gameObject.CompareTag("Ground"))
		{
			isTouchingGround = false;
		}
	}

	public float getTimeSinceLeftFloor()
	{
		return timeSinceLeftFloor;
	}

	public bool getIsTouchingGround()
	{
		return isTouchingGround;
	}
}
