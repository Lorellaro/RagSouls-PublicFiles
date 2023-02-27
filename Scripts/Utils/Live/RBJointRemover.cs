using UnityEngine;
using Photon.Pun;

namespace MainGame.Utils
{
	public class RBJointRemover : MonoBehaviour
	{
		[SerializeField] bool destroyJointToo = false;
		[SerializeField] FixedJoint fixedJointIfApllicable;
		[SerializeField] private PhotonView view;

		private Rigidbody jointRB;

		void Start()
		{
			if(view == null)
			{
				view = transform.root.GetComponent<PhotonView>();
			}

			jointRB = GetComponent<Rigidbody>();

			//Destroy joint if this player obj is not owned by me
			if (!view.IsMine)
			{
				jointRB.isKinematic = true;
				if (destroyJointToo)
				{
					if (fixedJointIfApllicable)
					{
						Destroy(fixedJointIfApllicable);
					}
					else
					{
						Destroy(GetComponent<ConfigurableJoint>());
					}
				}
			}
		}
	}
}
