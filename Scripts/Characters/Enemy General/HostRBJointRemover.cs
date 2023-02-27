using UnityEngine;
using Photon.Pun;

namespace MainGame.Utils
{
	public class HostRBJointRemover : MonoBehaviour
	{
		[SerializeField] bool destroyJointToo = false;
		[SerializeField] private PhotonView view;

		private Rigidbody jointRB;

		void Start()
		{
			if (view == null)
			{
				view = transform.root.GetComponent<PhotonView>();
			}

			jointRB = GetComponent<Rigidbody>();

			//Destroy joint if this player obj is not owned by host
			if (!PhotonNetwork.IsMasterClient)
			{
				jointRB.isKinematic = true;

				if (destroyJointToo)
				{
					Destroy(GetComponent<ConfigurableJoint>());
				}
			}
		}
	}
}
