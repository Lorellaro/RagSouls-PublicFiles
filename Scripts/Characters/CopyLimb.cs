using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CopyLimb : MonoBehaviour
{
	[SerializeField] private Transform targetLimb;
	[SerializeField] bool isPlayer = false;
	[SerializeField] bool isEnemy = false;

	private ConfigurableJoint m_configurableJoint;
	private PhotonView view;
	private Rigidbody jointRB;

	Quaternion targetInitialRotation;

    // Start is called before the first frame update
    void Start()
    {
		view = transform.root.GetComponent<PhotonView>();
		if (!PhotonNetwork.IsMasterClient && isEnemy)
		{ return; }

		this.m_configurableJoint = this.GetComponent<ConfigurableJoint>();
		this.targetInitialRotation = this.targetLimb.transform.localRotation;
		jointRB = GetComponent<Rigidbody>();

		if (!view.IsMine && isPlayer)
		{
			jointRB.isKinematic = true;
			Destroy(m_configurableJoint);
		}
    }

	private void FixedUpdate()
	{
		if (!view.IsMine && isPlayer)
		{
			return;
		}

		if(!PhotonNetwork.IsMasterClient && isEnemy)
		{ return; }

		this.m_configurableJoint.targetRotation = copyRotation();
		
	}

	private Quaternion copyRotation()
	{
		return Quaternion.Inverse(this.targetLimb.localRotation) * this.targetInitialRotation;
	}
}
