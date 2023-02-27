using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class LagCompensator : MonoBehaviourPun, IPunObservable
{
	Rigidbody _rigidbody;
	Vector3 networkPosition;
	Vector3 previousPosition;
	new PhotonView photonView;

	Quaternion networkRotation;

    public int sendRate = 30;
    public int serializationRate = 24;

	public bool teleportIfFar;
	public float teleportIfFarDistance;

	[Header("Lerping [Experimental]")]
	public float smoothPos = 5.0f;
	public float smoothRot = 5.0f;

	private void Awake()
	{
		PhotonNetwork.SendRate = sendRate;
		PhotonNetwork.SerializationRate = serializationRate;

		_rigidbody = GetComponent<Rigidbody>();
		photonView = transform.root.GetComponent<PhotonView>();
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(_rigidbody.position);
			stream.SendNext(_rigidbody.rotation);
			stream.SendNext(_rigidbody.velocity);
		}
		else
		{
			networkPosition = (Vector3)stream.ReceiveNext();
			networkRotation = (Quaternion)stream.ReceiveNext();
			_rigidbody.velocity = (Vector3)stream.ReceiveNext();

			float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
			networkPosition += (this._rigidbody.velocity * lag);
		}
	}

	public void FixedUpdate()
	{
		if (!photonView.IsMine)
		{
			/*
			GetComponent<Rigidbody>().position = Vector3.MoveTowards(GetComponent<Rigidbody>().position, networkPosition, Time.fixedDeltaTime);
			GetComponent<Rigidbody>().rotation = Quaternion.RotateTowards(GetComponent<Rigidbody>().rotation, networkRotation, Time.fixedDeltaTime * 100.0f);
			*/

			_rigidbody.position = Vector3.Lerp(_rigidbody.position, networkPosition, smoothPos * Time.fixedDeltaTime);
			_rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, networkRotation, smoothRot * Time.fixedDeltaTime);

			if(Vector3.Distance(_rigidbody.position, networkPosition) > teleportIfFarDistance)
			{
				_rigidbody.position = networkPosition;
			}
		}
	}
}
